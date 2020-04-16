using System;
using System.Linq;
using Microsoft.Xrm.Portal.Cms;
using Xrm;

namespace Site.Pages.eService
{
	public partial class ServiceDetails : PortalPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(Request["serviceid"]))
			{
				var page = ServiceContext.GetPageBySiteMarkerName(Website, "View Scheduled Services");

				Response.Redirect(ServiceContext.GetUrl(page));
			}

			var scheduledActivity = XrmContext.ServiceAppointmentSet.First(s => s.ActivityId == new Guid(Request["serviceid"]));
			var userTimeZone = Contact.Adx_TimeZone;
			var timeZone = XrmContext.TimeZoneDefinitionSet.First(t => t.TimeZoneCode == userTimeZone);
			var usersMinutesFromGmt = GetUsersMinutesFromGmt(userTimeZone, XrmContext);

			serviceType.Text = scheduledActivity.service_service_appointments.Name;
			startTime.Text = string.Format("{0} ({1})", scheduledActivity.ScheduledStart.GetValueOrDefault().AddMinutes(usersMinutesFromGmt), timeZone.StandardName);
			endTime.Text = string.Format("{0} ({1})", scheduledActivity.ScheduledEnd.GetValueOrDefault().AddMinutes(usersMinutesFromGmt), timeZone.StandardName);

			ExportLink.NavigateUrl = string.Format("/Event.axd?type=service&id={0}", scheduledActivity.ActivityId);
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
	}
}