using System;
using System.Linq;
using Microsoft.Xrm.Client;
using Site.Library;
using Xrm;

namespace Site.Pages.Events
{
	public partial class EventListing : PortalPage
	{
		protected IQueryable<Campaign> Campaigns
		{
			get
			{
				return
					from c in XrmContext.CampaignSet
					where c.TypeCode.Value == 3
						&& c.MSA_PublishEventDetailsonWeb == true
						&& c.StatusCode.Value >= 200000
						&& c.StatusCode.Value <= 200002
					select c;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Page.IsPostBack) return;

			var now = DateTime.UtcNow.Floor(RoundTo.Minute);
			EventsRepeater.DataSource = Campaigns.Where(c => c.MSA_FeaturedEvent == true && c.MSA_StartDateTime > now);
			EventsRepeater.DataBind();
		}

		protected void Menu_Click(object sender, EventArgs e)
		{
			var now = DateTime.UtcNow.Floor(RoundTo.Minute);

			switch (EventListFilter.SelectedValue)
			{
				case "RecentlyAdded":
					EventsRepeater.DataSource = Campaigns.Where(c => c.CreatedOn > now.AddDays(-7));
					break;
				case "Upcoming":
					EventsRepeater.DataSource = Campaigns.Where(c => c.MSA_StartDateTime > now);
					break;
				case "Past":
					EventsRepeater.DataSource = Campaigns.Where(c => c.MSA_StartDateTime < now);
					break;
				default:
					EventsRepeater.DataSource = Campaigns.Where(c => c.MSA_FeaturedEvent == true && c.MSA_StartDateTime > now);
					break;
			}

			EventsRepeater.DataBind();
		}

		protected string FormatEventDate(Campaign campaign)
		{
			return campaign == null ? string.Empty : ContentUtility.FormatEventDate(XrmContext, campaign, campaign.MSA_StartDateTime);
		}
	}
}
