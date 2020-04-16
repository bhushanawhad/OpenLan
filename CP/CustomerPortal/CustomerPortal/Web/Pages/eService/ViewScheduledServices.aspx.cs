using System;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Xrm.Client.Messages;
using Microsoft.Xrm.Sdk.Client;
using Site.Library;
using Xrm;

namespace Site.Pages.eService
{
	public partial class ViewScheduledServices : PortalPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			RedirectToLoginIfAnonymous();

			if (Page.IsPostBack) return;

			if (Contact == null) return;

			if (Contact.Adx_TimeZone == null) return;

			var usersMinutesFromGmt = GetUsersMinutesFromGmt(Contact.Adx_TimeZone, ServiceContext);

			var appointments =
				from serviceActivity in ServiceContext.ServiceAppointmentSet.ToList()
				from customer in serviceActivity.Customers
				let partyLookup = customer.PartyId
				where partyLookup != null && partyLookup.Id == Contact.ContactId && serviceActivity.ScheduledStart > DateTime.UtcNow && serviceActivity.StateCode == (int)Enums.ServiceAppointmentState.Scheduled
				orderby serviceActivity.ScheduledStart.GetValueOrDefault() ascending
				select new
				{
					scheduledStart = serviceActivity.ScheduledStart.GetValueOrDefault().ToUniversalTime().AddMinutes(usersMinutesFromGmt),
					scheduledEnd = serviceActivity.ScheduledEnd.GetValueOrDefault().ToUniversalTime().AddMinutes(usersMinutesFromGmt),
					serviceType = serviceActivity.service_service_appointments.Name,
					dateBooked = serviceActivity.CreatedOn.GetValueOrDefault().ToUniversalTime().AddMinutes(usersMinutesFromGmt),
					serviceId = serviceActivity.ActivityId
				};

			BookedAppointments.DataSource = appointments;
			BookedAppointments.DataBind();
		}

		private static int GetUsersMinutesFromGmt(int? timeZoneCode, XrmServiceContext crmContext)
		{
			var definition = crmContext.TimeZoneDefinitionSet.First(timeZone => timeZone.TimeZoneCode == timeZoneCode);

			if (definition == null)
			{
				return 0;
			}

			var rule = definition.lk_timezonerule_timezonedefinitionid;

			return rule == null ? 0 : rule.First().Bias.GetValueOrDefault() * -1;
		}

		protected void BookedAppointments_OnRowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandArgument == null)
			{
				return;
			}

			if (string.Equals(e.CommandName, "Cancel", StringComparison.InvariantCulture))
			{
				var serviceId = new Guid(e.CommandArgument.ToString());
				CancelService(serviceId);
			}
		}

		protected void CancelService(Guid activityID)
		{
			var appointment =
				from serviceActivity in XrmContext.ServiceAppointmentSet.ToList()
				where serviceActivity.ActivityId == activityID
				select serviceActivity;
			var serviceApp = appointment.First();

			XrmContext.SetState((int)Enums.ServiceAppointmentState.Canceled, -1, serviceApp);

			Response.Redirect(Request.RawUrl);
		}

		protected string GetEventExportUrl(object serviceId)
		{
			return serviceId == null ? string.Empty : string.Format("/Event.axd?type=service&id={0}", serviceId);
		}
	}
}
