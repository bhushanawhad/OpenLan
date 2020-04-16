using System;
using System.Linq;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Sdk;
using Site.Library;
using Xrm;

namespace Site.Pages.Events
{
	public partial class Registration : PortalPage
	{
		protected Campaign Campaign
		{
			get { return XrmContext.CampaignSet.First(c => c.CampaignId == new Guid(Request.QueryString["campaignID"])); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			AddAsirraValidation();

			if (Request.QueryString["campaignID"] == null || Campaign.MSA_StartDateTime < DateTime.Now)
			{
				var page = ServiceContext.GetPageBySiteMarkerName(Website, "Event Listings");

				Response.Redirect(ServiceContext.GetUrl(page));
			}

			EventName.Text = string.IsNullOrEmpty(Campaign.MSA_EventName) ? Campaign.Name : Campaign.MSA_EventName;
			EventTopic.Text = Campaign.MSA_EventTopic;
			Address.Text = string.Format("{0}, {1}, {2}",
				Campaign.MSA_City,
				Campaign.MSA_StateProvince,
				Campaign.MSA_CountryRegion);
			Cost.Text = string.Format("Cost: ${0}", Campaign.MSA_CostofAdmission ?? "0");

			var brochureUrl = Campaign.MSA_EventBrochureURL;

			EventBrochure.NavigateUrl = brochureUrl;
			EventBrochure.Visible = !string.IsNullOrEmpty(brochureUrl);

			EventContact.Text = GetContactInfo();

			EventDateTime.Text = ContentUtility.FormatEventDateRange(XrmContext, Campaign);

			EventLocation.Text = "Event Address:<br />{0}<br /> {1}, {2}, {3} {4}".FormatWith(
				Campaign.MSA_Street1,
				Campaign.MSA_City,
				Campaign.MSA_StateProvince,
				Campaign.MSA_CountryRegion,
				MapUrl());
		}

		protected string GetContactInfo()
		{
			var contactInfo = Campaign.MSA_EventContact;

			if (contactInfo != null)
			{
				if (contactInfo.ToLower().Contains("http") || contactInfo.ToLower().Contains("www"))
				{
					return "<a href='{0}'>{1}</a>".FormatWith(contactInfo, contactInfo);
				}

				if (contactInfo.Contains("@"))
				{
					return "<a href='mailto:{0}'>{1}</a>".FormatWith(contactInfo, contactInfo);
				}

				return "<br/>{0}".FormatWith(contactInfo);
			}

			return null;
		}

		protected string MapUrl()
		{
			return Campaign.MSA_MappingUrl == null ? string.Empty : "<a href='<{0}'>(Map) </a>".FormatWith(Campaign.MSA_MappingUrl);
		}

		protected void Register_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid) return;

			// Determine if the user is already in the CRM as either a lead or customer
			var existingContact = XrmContext.ContactSet.FirstOrDefault(c => c.EMailAddress1 == EMail.Text);
			var existingLead = XrmContext.LeadSet.FirstOrDefault(l => l.EMailAddress1 == EMail.Text);
			
			var eventParticipant = new ActivityParty();

			if (existingContact != null)
			{
			    eventParticipant.contact_activity_parties = existingContact;
			    eventParticipant.PartyId = new EntityReference(Contact.EntityLogicalName, existingContact.ContactId.GetValueOrDefault());
			}
			else if (existingLead != null)
			{
			    eventParticipant.lead_activity_parties = existingLead;
			    eventParticipant.PartyId = new EntityReference(Lead.EntityLogicalName, existingLead.Id);
			}
			else
			{ 
			    // No Contact or Lead previously exsits in the CRM
			    // Create the registrant as a lead in the CRM
			    var newLead = new Lead
			    {
			        Subject = string.Format("{0}, {1}: {2}", LastName.Text, FirstName.Text, Campaign.Name),
			        FirstName = FirstName.Text,
			        LastName = LastName.Text,
			        CompanyName = CompanyName.Text,
			        LeadQualityCode = 2,
			        Telephone1 = Phone.Text,
			        EMailAddress1 = EMail.Text,
			        Address1_Line1 = Address1.Text,
			        Address1_Line2 = Address2.Text,
			        Address1_Line3 = Address3.Text,
			        Address1_StateOrProvince = State.Text,
			        Address1_PostalCode = PostalCode.Text,
			        Address1_Country = Country.Text
			    };

			    XrmContext.AddObject(newLead);
			    XrmContext.SaveChanges();

			    existingLead = XrmContext.LeadSet.FirstOrDefault(l => l.EMailAddress1 == EMail.Text);
			    if (existingLead != null)
			    {
			        eventParticipant.lead_activity_parties = existingLead;
			        eventParticipant.PartyId = new EntityReference(Lead.EntityLogicalName, existingLead.Id);
			    }
			}

			// Register user for event by creating a marketing response
			var registration = new CampaignResponse
			{ 
			    Customer = new[] {eventParticipant},
			    RegardingObjectId = Campaign.ToEntityReference(),
			    ResponseCode = 200000,
			    Subject = string.Format("{0}, {1}: {2}", LastName.Text, FirstName.Text, Campaign.Name),
			    ChannelTypeCode = 200000,
			    ReceivedOn = DateTime.Now,
			    FirstName = FirstName.Text,
			    LastName = LastName.Text,
			    MSA_StreetAddress1 = Address1.Text,
			    MSA_StreetAddress2 = Address2.Text,
			    MSA_StreetAddress3 = Address3.Text,
			    MSA_City = City.Text,
			    MSA_State = State.Text,
			    MSA_PostalCode = PostalCode.Text,
			    MSA_Country = Country.Text,
			    CompanyName = CompanyName.Text,
			    MSA_JobTitle = JobTitle.Text,
			    Telephone = Phone.Text,
			    EMailAddress = EMail.Text,
			    Description = Notes.Text,
			    MSA_PreferredMethodofCommunication = CommunicationMethod.SelectedIndex
			};

			XrmContext.AddObject(registration);
			XrmContext.SaveChanges();

			//Show Confirmation
			RegForm.Visible = false;
			ConfirmationMsg.Visible = true;
			EventExportLink.NavigateUrl = string.Format("/Event.axd?type=registration&id={0}", Campaign.Id);
		}

		private void AddAsirraValidation()
		{
			if (IsPostBack && !Asirra.ValidateAsirraChallenge(Request["Asirra_Ticket"]))
			{
				// If the Asirra ticket is not valid, stop the submission.
				Response.Redirect(Request.RawUrl);
			}

			SubmitButton.OnClientClick = string.Format(@"if (!validateAsirraChallenge(""{0}"")) return false;", SubmitButton.ClientID);
		}
	}
}
