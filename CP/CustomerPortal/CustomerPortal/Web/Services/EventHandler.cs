using System;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Xrm.Portal.Configuration;
using Xrm;

namespace Site.Services
{
	public class EventHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			Guid id;

			if (!Guid.TryParse(context.Request.QueryString["id"], out id))
			{
				NotFound(context.Response, string.Format(@"Query string parameter ""id"" with value ""{0}"" is not a valid event ID.", context.Request.QueryString["id"]));

				return;
			}

			var serviceContext = (XrmServiceContext)PortalCrmConfigurationManager.CreateServiceContext();

			if (string.Equals(context.Request.QueryString["type"], "registration", StringComparison.OrdinalIgnoreCase))
			{
				var campaign = serviceContext.CampaignSet.FirstOrDefault(c => c.CampaignId == id);

				if (campaign == null)
				{
					NotFound(context.Response, string.Format(@"Campaign with ID ""{0}"" not found.", id));

					return;
				}

				var vevent = new VEvent
				{
					Created = campaign.CreatedOn,
					Start = campaign.MSA_StartDateTime,
					End = campaign.MSA_EndDateTime,
					Summary = campaign.MSA_EventName,
					Description = campaign.MSA_EventDetails,
					Location = string.Format("{0} {1} {2} {3} {4}", campaign.MSA_Street1, campaign.MSA_City, campaign.MSA_StateProvince, campaign.MSA_ZipPostalCode, campaign.MSA_CountryRegion),
					Organizer = campaign.MSA_EventContact,
					Url = campaign.MSA_EventBrochureURL,
				};

				context.Response.ContentType = "text/calendar";

				context.Response.Write(vevent.ToString());

				return;
			}

			if (string.Equals(context.Request.QueryString["type"], "service", StringComparison.OrdinalIgnoreCase))
			{
				var scheduledActivity = serviceContext.ServiceAppointmentSet.FirstOrDefault(s => s.ActivityId == id);

				if (scheduledActivity == null)
				{
					NotFound(context.Response, string.Format(@"Service appointment with ID ""{0}"" not found.", id));

					return;
				}

				var vevent = new VEvent
				{
					Created = scheduledActivity.CreatedOn,
					Summary = scheduledActivity.service_service_appointments.Name,
					Start = scheduledActivity.ScheduledStart,
					End = scheduledActivity.ScheduledEnd
				};

				context.Response.ContentType = "text/calendar";

				context.Response.Write(vevent.ToString());

				return;
			}

			NotFound(context.Response, string.Format(@"Specified type ""{0}"" is not a recognized event type.", context.Request.QueryString["type"]));
		}

		private static void NotFound(HttpResponse response, string message)
		{
			response.StatusCode = 404;
			response.ContentType = "text/plain";
			response.Write(message);
			response.End();
		}

		private class VEvent
		{
			public DateTime? Created { get; set; }

			public string Description { get; set; }

			public DateTime? End { get; set; }

			public string Location { get; set; }

			public string Organizer { get; set; }

			public DateTime? Start { get; set; }

			public string Summary { get; set; }

			public string Url { get; set; }

			public override string ToString()
			{
				var vevent = new StringBuilder();

				vevent.Append("BEGIN:VCALENDAR\r\n");
				vevent.Append("VERSION:1.0\r\n");
				vevent.Append("BEGIN:VEVENT\r\n");

				AppendDateField(vevent, "DCREATED", Created);
				AppendDateField(vevent, "DTSTART", Start);
				AppendDateField(vevent, "DTEND", End);

				AppendField(vevent, "SUMMARY", Summary);
				AppendField(vevent, "DESCRIPTION", Description);
				AppendField(vevent, "LOCATION", Location);
				AppendField(vevent, "ORGANIZER", Organizer);
				AppendField(vevent, "URL", Url);

				vevent.Append("END:VEVENT\r\n");
				vevent.Append("END:VCALENDAR\r\n");

				return vevent.ToString();
			}

			private static void AppendField(StringBuilder vevent, string name, string value)
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}

				vevent.AppendFormat("{0}: {1}\r\n", name, value);
			}

			private static void AppendDateField(StringBuilder vevent, string name, DateTime? value)
			{
				if (value == null)
				{
					return;
				}

				AppendField(vevent, name, value.Value.ToUniversalTime().ToString("yyyyMMddTHHmmssZ"));
			}
		}
	}
}