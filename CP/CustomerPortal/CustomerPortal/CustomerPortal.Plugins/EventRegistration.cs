using System;
using System.Reflection;
using CustomerPortal.Plugins.Diagnostics;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace CustomerPortal.Plugins
{
	/// <summary>
	/// A simple callout plug-in class which updates various campaign attributes
	/// when a campaign response is created or updated. The plug-in must
	/// be registered to execute after (post-event) a campaignresponse is created
	/// or updated.
	/// </summary>
	public class EventRegistration : IPlugin
	{
		// These are default values in case you don't supply the plugin with parameters.
		private int _normalEventStatus = 2; // Campaign Status = Launched
		private int _waitlistEventStatus = 200001; // Campaign Status = Waitlist Only
		private int _soldOutEventStatus = 200002; // Campaign Status = Sold Out

		private int _registeredEventResponse = 200000; // Campaign Response = Registered
		private int _cancelledEventResponse = 200001; // Campaign Response = Cancelled
		private int _waitlistEventResponse = 200002; // Campaign Response = Waitlist

		public void Execute(IServiceProvider serviceProvider)
		{
			Tracing.WorkflowInformation(GetType().FullName, MethodBase.GetCurrentMethod().Name, "Begin");

			IPluginExecutionContext executionContext;
			IOrganizationService service;

			if (!TryGetExecutionContextAndService(serviceProvider, out executionContext, out service))
			{
				return;
			}

			Entity campaignResponse;

			// Get the target entity, and verify that it represents a campaignresponse.
			if (!TryGetTarget(executionContext, "campaignresponse", out campaignResponse))
			{
				throw new InvalidPluginExecutionException(OperationStatus.Canceled, "Unable to retrieve target campaignresponse.");
			}

			try
			{
				// We only want to do processing if this request involves a response code from the Campaign Response.
				if (!campaignResponse.Attributes.Contains("responsecode"))
				{
					Tracing.WorkflowError(typeof (EventRegistration).FullName, MethodBase.GetCurrentMethod().Name, "No response code, exiting.");

					return;
				}

				EntityReference campaignReference;
				int preResponseCode;

				if (!TryGetCampaignReference(executionContext, service, campaignResponse, out preResponseCode, out campaignReference))
				{
					throw new InvalidOperationException("Unable to retrieve campaign reference.");
				}

				// Now, let's retrieve the campaign (event) record that this campaignresponse (registration) belongs to.
				var campaignRetrieveRequest = new RetrieveRequest
				{
					Target = campaignReference,
					ColumnSet = new ColumnSet(new[]
					{
						"name",
						"msa_registrationcount",
						"statuscode",
						"msa_maximumeventcapacity",
						"msa_waitliststartingpoint",
						"msa_waitlistthisevent",
						"msa_manageregistrationcount"
					})
				};

				var campaignRetrieveResponse = (RetrieveResponse)service.Execute(campaignRetrieveRequest);

				var campaign = campaignRetrieveResponse.Entity;

				if (campaign == null)
				{
					throw new InvalidOperationException("Unable to retrieve campaign.");
				}

				var msa_manageregistrationcount = campaign.GetAttributeValue<bool?>("msa_manageregistrationcount").GetValueOrDefault(false);
				var msa_registrationcount = campaign.GetAttributeValue<int?>("msa_registrationcount").GetValueOrDefault(0);
				var msa_maximumeventcapacity = campaign.GetAttributeValue<int?>("msa_maximumeventcapacity").GetValueOrDefault(0);
				var msa_waitlistthisevent = campaign.GetAttributeValue<bool?>("msa_waitlistthisevent").GetValueOrDefault(false);
				var msa_waitliststartingpoint = campaign.GetAttributeValue<int?>("msa_waitliststartingpoint").GetValueOrDefault(0);

				var campaignResponseCode = campaignResponse.GetAttributeValue<OptionSetValue>("responsecode");

				// If this is a new registration or new waitlist registration then add 1 to the registration count.
				if (executionContext.MessageName == "Create" && (campaignResponseCode.Value == _registeredEventResponse || campaignResponseCode.Value == _waitlistEventResponse))
				{
					msa_registrationcount = msa_registrationcount + 1;
				}

				if (executionContext.MessageName == "Update")
				{
					// If the previous response was a waitlist then we DON'T want to add to the registration count.
					if ((campaignResponseCode.Value == _registeredEventResponse || campaignResponseCode.Value == _waitlistEventResponse) && (preResponseCode != _waitlistEventResponse && preResponseCode != _registeredEventResponse))
					{
						msa_registrationcount = msa_registrationcount + 1;
					}

					// If this is a cancellation and the previous response code was registered or waitlist then
					// subtract 1 from the registration count.
					if (campaignResponseCode.Value == _cancelledEventResponse && msa_registrationcount > 0 && (preResponseCode == _registeredEventResponse || preResponseCode == _waitlistEventResponse))
					{
						msa_registrationcount = msa_registrationcount - 1;
					}
				}

				var updateCampaign = new Entity(campaign.LogicalName);

				updateCampaign["campaignid"] = campaign.Id;
				updateCampaign["msa_registrationcount"] = msa_registrationcount;

				service.Update(updateCampaign);

				//Now we'll update the event status based on the registration count
				var setStateRequest = new SetStateRequest
				{
					EntityMoniker = campaign.ToEntityReference(),
					State = new OptionSetValue(0),
					Status = new OptionSetValue(_normalEventStatus)
				};

				// We will default the Event status to Open (i.e. not Sold Out and not Waitlisted).

				// If this is a waitlist managed event then check these figures.
				if (msa_waitlistthisevent)
				{
					// if the registration count is greater than or equal to the waitlist starting point then set
					// the event status to Waitlisted.
					if (msa_registrationcount >= msa_waitliststartingpoint)
					{
						setStateRequest.Status = new OptionSetValue(_waitlistEventStatus);
					}
				}

				// If the event has set up to manage the registration count.
				if (msa_manageregistrationcount)
				{
					// If the registration count is >= the maximum capacity then set the status Sold Out.
					if (msa_registrationcount >= msa_maximumeventcapacity)
					{
						setStateRequest.Status = new OptionSetValue(_soldOutEventStatus);
					}
				}
					
				service.Execute(setStateRequest);
			}
			catch (Exception e)
			{
				Tracing.WorkflowError(typeof (EventRegistration).FullName, MethodBase.GetCurrentMethod().Name, "{0}", e);

				throw new InvalidPluginExecutionException(OperationStatus.Failed, e.Message);
			}
			finally
			{
				Tracing.WorkflowInformation(GetType().FullName, MethodBase.GetCurrentMethod().Name, "End");
			}
		}

		private static bool TryGetCampaignReference(IExecutionContext executionContext, IOrganizationService service, Entity campaignResponse, out int preResponseCode, out EntityReference campaign)
		{
			Tracing.WorkflowInformation(typeof (EventRegistration).FullName, "TryGetCampaignReference", "MessageName={0}", executionContext.MessageName);

			preResponseCode = 0;
			campaign = null;

			// Grab the campaign (event) ID that this response (registration) relates to.
			if (executionContext.MessageName == "Create")
			{
				campaign = campaignResponse.GetAttributeValue<EntityReference>("regardingobjectid");
			}

			// This is an update transaction if the campaign response contains a property called activityid.
			// We are interested if the campaignresponse response code has changed.
			//
			// If the responsecode has changed from registered to cancelled then we need to decrement the
			// registration count for the campaign (event). If the responsecode changes from cancelled to
			// registered then we need to increment the registration count.
			if (campaignResponse.Attributes.Contains("activityid"))
			{
				if (executionContext.PreEntityImages.Contains("CampaignResponseImage"))
				{
					var preCampaignResponse = executionContext.PreEntityImages["CampaignResponseImage"];

					if (preCampaignResponse != null)
					{
						Tracing.WorkflowInformation(typeof (EventRegistration).FullName, MethodBase.GetCurrentMethod().Name, "CampaignResponseImage PreEntityImage found.");

						var preCampaignResponseCodeOption = preCampaignResponse.GetAttributeValue<OptionSetValue>("responsecode");

						if (preCampaignResponseCodeOption != null)
						{
							preResponseCode = preCampaignResponseCodeOption.Value;
						}
					}
				}
				else
				{
					Tracing.WorkflowInformation(typeof (EventRegistration).FullName, MethodBase.GetCurrentMethod().Name, "CampaignResponseImage PreEntityImage not found.");
				}

				var campaignResponseId = campaignResponse.GetAttributeValue<Guid>("activityid");

				// If this is an update transaction then we need to go and retrieve the related campaign (event) ID.
				var campaignResponseRetrieveRequest = new RetrieveRequest
				{
					Target = new EntityReference("campaignresponse", campaignResponseId),
					ColumnSet = new ColumnSet(new[] {"regardingobjectid"}),
				};

				var campaignResponseRetrieveResponse = (RetrieveResponse)service.Execute(campaignResponseRetrieveRequest);

				campaign = campaignResponseRetrieveResponse.Entity.GetAttributeValue<EntityReference>("regardingobjectid");
			}

			return campaign != null;
		}

		private static bool TryGetExecutionContextAndService(IServiceProvider serviceProvider, out IPluginExecutionContext context, out IOrganizationService service)
		{
			var serviceFactory = serviceProvider.GetService(typeof(IOrganizationServiceFactory)) as IOrganizationServiceFactory;

			context = serviceProvider.GetService(typeof(IPluginExecutionContext)) as IPluginExecutionContext;

			if (serviceFactory == null || context == null)
			{
				service = null;

				return false;
			}

			service = serviceFactory.CreateOrganizationService(context.UserId);

			return true;
		}

		private static bool TryGetTarget(IExecutionContext context, string entityLogicalName, out Entity entity)
		{
			entity = null;

			if (!context.InputParameters.Contains("Target"))
			{
				return false;
			}

			entity = context.InputParameters["Target"] as Entity;

			return entity != null && entity.LogicalName == entityLogicalName;
		}
	}
}
