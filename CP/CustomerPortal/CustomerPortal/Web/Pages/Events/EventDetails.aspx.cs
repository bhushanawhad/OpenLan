using System;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Xrm.Portal.Cms;
using Site.Library;
using Xrm;

namespace Site.Pages.Events
{
	public partial class EventDetails : PortalPage
	{
		protected Campaign Campaign
		{
			get { return XrmContext.CampaignSet.First(c => c.CampaignId == new Guid(Request.QueryString["campaignID"])); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack) return;
			
			if (Request.QueryString["campaignID"] == null)
			{
				var page = ServiceContext.GetPageBySiteMarkerName(Website, "Event Listings");

				Response.Redirect(ServiceContext.GetUrl(page));
			}

			Details.DataSource = new[] { Campaign };
			Details.DataBind();

			var registrationButton = (Button)Details.FindControl("RegisterButton");
			var message = (Label)Details.FindControl("Message");

			if (Campaign.MSA_StartDateTime > DateTime.Now)
			{
				switch (Campaign.StatusCode.Value)
				{
					case 200001:
						registrationButton.Text = "Register for this Event";
						message.Text = "*Event is waitlisted";
						break;
					case 200002:
						registrationButton.Visible = false;
						message.Text = "This event is Sold Out.";
						break;
					default:
						registrationButton.Text = "Register for this Event";
						break;
				}
			}
			else
			{
				registrationButton.Visible = false;
				message.Text = "*Event occurs in the past";
			}
		}

		protected string ContactInfo()
		{
			var contactInfo = Campaign.MSA_EventContact;

			if (string.IsNullOrWhiteSpace(contactInfo))
			{
				return string.Empty;
			}

			if (contactInfo.ToLower().Contains("http") || contactInfo.ToLower().Contains("www"))
			{
				return string.Format("Contact Information: <a href='{0}'>{0}</a>", contactInfo);
			}
				
			if (contactInfo.Contains("@"))
			{
				return string.Format("Contact Information: <a href='mailto:{0}'>{0}</a>", contactInfo);
			}
				
			return string.Format("Contact Information: {0}", contactInfo);
		}

		protected void RegisterButton_Click(object sender, EventArgs e)
		{
			var page = ServiceContext.GetPageBySiteMarkerName(Website, "Event Registration");

			Response.Redirect(string.Format("{0}?campaignID={1}", ServiceContext.GetUrl(page), Request.QueryString["campaignID"]));
		}

		protected string FormatEventDateRange(Campaign campaign)
		{
			return ContentUtility.FormatEventDateRange(XrmContext, campaign);
		}
	}
}
