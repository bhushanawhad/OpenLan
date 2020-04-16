using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Client.Messages;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Sdk;
using Site.Library;
using Xrm;

namespace Site.Pages.eService
{
	public partial class ScheduleService : PortalPage
	{
		private readonly Lazy<Service> _service;

		protected Service Service
		{
			get { return _service.Value; }
		}

		public ScheduleService()
		{
			_service = new Lazy<Service>(() => XrmContext.ServiceSet.First(s => s.ServiceId == new Guid(ServiceType.SelectedValue)));
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			RedirectToLoginIfAnonymous();

			if (IsPostBack)
			{
				return;
			}

			StartDate.SelectedDate = DateTime.Today;
			EndDate.SelectedDate = DateTime.Today.AddDays(7);

			var services = XrmContext.ServiceSet.Where(s => s.IsSchedulable == true && s.Description.Contains("*WEB*")).ToList();

			if (!services.Any())
			{
				SearchPanel.Visible = false;
				NoServicesMessage.Visible = true;

				return;
			}

			BindServicesDropDown(services);
			BindTimeZoneDropDown();
			BindTimeDropDowns();
		}

		protected void AvailableTimes_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.DataRow) return;

			e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackClientHyperlink(AvailableTimes, "Select$" + e.Row.RowIndex));
			e.Row.Attributes.Add("onmouseover", string.Format("this.style.cursor='pointer'"));
		}

		protected void AvailableTimes_SelectedIndexChanged(object sender, EventArgs e)
		{
			ScheduleServiceButton.Enabled = true;
		}

		protected void FindTimes_Click(object sender, EventArgs args)
		{
			var startTimeInMinutesFromMidnight = int.Parse(StartTime.SelectedValue);

			var startDate = StartDate.SelectedDate.AddMinutes(startTimeInMinutesFromMidnight);

			var endTimeInMinutesFromMidnight = int.Parse(EndTime.SelectedValue);

			var endDate = EndDate.SelectedDate.AddMinutes(endTimeInMinutesFromMidnight);

			if (!SelectedDatesAndTimesAreValid(startDate, endDate, startTimeInMinutesFromMidnight, endTimeInMinutesFromMidnight))
			{
				return;
			}

			// Add the timezone selected to the CRM Contact for next time.
			var contact = XrmContext.ContactSet.FirstOrDefault(c => c.ContactId == Contact.Id);

			contact.Adx_TimeZone = int.Parse(TimeZoneSelection.SelectedValue);

			XrmContext.UpdateObject(contact);

			XrmContext.SaveChanges();

			var usersMinutesFromGmt = GetUsersMinutesFromGmt(contact.Adx_TimeZone, XrmContext);

			var appointmentRequest = new AppointmentRequest
			{
				AnchorOffset = Service.AnchorOffset.GetValueOrDefault(),
				Direction = SearchDirection.Forward,
				Duration = Service.Duration.GetValueOrDefault(60),
				NumberOfResults = 10,
				RecurrenceDuration = endTimeInMinutesFromMidnight - startTimeInMinutesFromMidnight,
				RecurrenceTimeZoneCode = contact.Adx_TimeZone.GetValueOrDefault(),
				SearchRecurrenceRule = "FREQ=DAILY;INTERVAL=1",
				SearchRecurrenceStart = new DateTime(startDate.AddMinutes(usersMinutesFromGmt * -1).Ticks, DateTimeKind.Utc),
				SearchWindowEnd = new DateTime(endDate.AddMinutes(usersMinutesFromGmt * -1).Ticks, DateTimeKind.Utc),
				ServiceId = Service.ServiceId.GetValueOrDefault()
			};

			var service = XrmContext;

			var searchRequest = new OrganizationRequest("Search");
			searchRequest.Parameters["AppointmentRequest"] = appointmentRequest;

			var searchResults = (SearchResults)service.Execute(searchRequest).Results["SearchResults"];

			var schedules = searchResults.Proposals.Select(proposal => new
			{
				ScheduledStart = proposal.Start.GetValueOrDefault().ToUniversalTime().AddMinutes(usersMinutesFromGmt),
				ScheduledStartUniversalTime = proposal.Start.GetValueOrDefault().ToUniversalTime(),
				ScheduledEnd = proposal.End.GetValueOrDefault().ToUniversalTime().AddMinutes(usersMinutesFromGmt),
				ScheduledEndUniversalTime = proposal.End.GetValueOrDefault().ToUniversalTime(),
				AvailableResource = proposal.ProposalParties.First().ResourceId
			});

			AvailableTimes.DataSource = schedules;
			AvailableTimes.DataBind();

			SearchPanel.Visible = false;
			ResultsDisplay.Visible = true;
			ScheduleServiceButton.Enabled = false;
		}

		protected void ScheduleService_Click(object sender, EventArgs e)
		{
			var availableResourceId = (Guid)AvailableTimes.SelectedDataKey.Values["AvailableResource"];
			var availableResource = XrmContext.ResourceSet.First(r => r.ResourceId == availableResourceId);
			var selectedStart = (DateTime)AvailableTimes.SelectedDataKey.Values["ScheduledStartUniversalTime"];
			var selectedEnd = (DateTime)AvailableTimes.SelectedDataKey.Values["ScheduledEndUniversalTime"];

			var appointment = new ServiceAppointment
			{
				ServiceId = Service.ToEntityReference(),
				Subject = "Web Service Scheduler: " + ServiceType.SelectedItem,
				ScheduledStart = selectedStart,
				ScheduledEnd = selectedEnd,
				Resources = new[] { new ActivityParty { PartyId = new EntityReference(availableResource.ObjectTypeCode, availableResource.Id) } },
				Customers = new[] { new ActivityParty { PartyId = Contact.ToEntityReference() } }
			};

			XrmContext.AddObject(appointment);
			XrmContext.SaveChanges();
			XrmContext.SetState((int)Enums.ServiceAppointmentState.Scheduled, 4, appointment);

			var page = ServiceContext.GetPageBySiteMarkerName(Website, "Service Details");

			Response.Redirect(string.Format("{0}?serviceid={1}", ServiceContext.GetUrl(page), appointment.Id));
		}

		private void BindServicesDropDown(IEnumerable<Service> services)
		{
			ServiceType.DataSource = services;
			ServiceType.DataTextField = "name";
			ServiceType.DataValueField = "serviceid";
			ServiceType.DataBind();
		}

		private void BindTimeDropDowns()
		{
			for (var t = DateTime.MinValue; t < DateTime.MinValue.AddDays(1); t = t.AddMinutes(30))
			{
				StartTime.Items.Add(new ListItem(t.ToString("h:mm tt"), t.Subtract(DateTime.MinValue).TotalMinutes.ToString()));
				EndTime.Items.Add(new ListItem(t.ToString("h:mm tt"), t.Subtract(DateTime.MinValue).TotalMinutes.ToString()));
			}

			StartTime.Text = "540"; // 9 AM
			EndTime.Text = "1020"; // 5 PM
		}

		private void BindTimeZoneDropDown()
		{
			TimeZoneSelection.DataSource = XrmContext.TimeZoneDefinitionSet.OrderByDescending(t => t.UserInterfaceName);
			TimeZoneSelection.DataTextField = "userinterfacename";
			TimeZoneSelection.DataValueField = "timezonecode";
			TimeZoneSelection.DataBind();

			TimeZoneSelection.Items.Insert(0, new ListItem("Please Select One..."));

			if (Contact.Adx_TimeZone.HasValue)
			{
				TimeZoneSelection.SelectedValue = Contact.Adx_TimeZone.ToString();
			}
		}

		private static int GetUsersMinutesFromGmt(int? timeZoneCode, XrmServiceContext crmContext)
		{
			var definition = crmContext.TimeZoneDefinitionSet.First(timeZone => timeZone.TimeZoneCode == timeZoneCode);

			if (definition == null)
			{
				return 0;
			}

			var rule = definition.lk_timezonerule_timezonedefinitionid;

			return (rule == null || !rule.Any()) ? 0 : rule.First().Bias.GetValueOrDefault() * -1;
		}

		private bool SelectedDatesAndTimesAreValid(DateTime startDate, DateTime endDate, int startTimeInMinutesFromMidnight, int endTimeInMinutesFromMidnight)
		{
			if (startDate.Date < DateTime.Now.Date)
			{
				ErrorLabel.Text = "Please select a date range that is not in the past.";
				ErrorLabel.Visible = true;
				return false;
			}

			if (startDate > endDate)
			{
				ErrorLabel.Text = "Please select an end date that is after the start date.";
				ErrorLabel.Visible = true;
				return false;
			}

			if (TimeZoneSelection.SelectedIndex < 1)
			{
				ErrorLabel.Text = "Please select your time zone.";
				ErrorLabel.Visible = true;
				return false;
			}

			if (startTimeInMinutesFromMidnight >= endTimeInMinutesFromMidnight)
			{
				ErrorLabel.Text = "Please select an end time that is later than the start time.";
				ErrorLabel.Visible = true;
				return false;
			}

			// Start date and end dates are acceptable. Hide error message.
			ErrorLabel.Visible = false;
			return true;
		}
	}
}
