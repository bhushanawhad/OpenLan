using System;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Portal.Web;
using Site.Library;
using Xrm;

namespace Site.Controls
{
	public partial class EventCalendar : PortalUserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Page.IsPostBack)
			{
				return;
			}

			DateTime startingDate;

			if (!DateTime.TryParse(Request["date"], out startingDate))
			{
				startingDate = DateTime.UtcNow;
			}

			var firstDayOfMonth = new DateTime(startingDate.Year, startingDate.Month, 1);

			// hook up the calendar to the events for the starting date (now)

			var events =
				from @event in XrmContext.CampaignSet
				where @event.TypeCode.GetValueOrDefault() == 3 && @event.MSA_PublishEventDetailsonWeb.GetValueOrDefault() && (@event.MSA_StartDateTime >= firstDayOfMonth && @event.MSA_StartDateTime <= firstDayOfMonth.AddMonths(1))
				select @event;

			CalendarRepeater.DataSource = events;
			CalendarRepeater.DataBind();

			// highlight the calendar days starting from the first of the current month
			HighlightCalendarDates(firstDayOfMonth);
			EventsCalendar.VisibleDate = startingDate;
		}

		protected void HighlightCalendarDates(DateTime startingDate)
		{
			var dates =
				from @event in XrmContext.CampaignSet
				where @event.TypeCode.GetValueOrDefault() == 3 && @event.MSA_PublishEventDetailsonWeb.GetValueOrDefault()
				select @event.MSA_StartDateTime;

			foreach (var date in dates)
			{
				if (date != null) EventsCalendar.SelectedDates.Add((DateTime)date);
			}
		}

		protected void MonthChanged(object sender, MonthChangedEventArgs args)
		{
			var url = new UrlBuilder(ServiceContext.GetUrl(Entity));
			url.QueryString.Add("date", args.NewDate.ToString("MM/yyyy"));

			Response.Redirect(url.PathWithQueryString);
		}

		protected void EventsCalendar_DayRender(object sender, DayRenderEventArgs e)
		{
			e.Day.IsSelectable = false;
		}

		protected string FormatEventDate(Campaign campaign)
		{
			return campaign == null ? string.Empty : ContentUtility.FormatEventDate(XrmContext, campaign, campaign.MSA_StartDateTime);
		}
	}
}
