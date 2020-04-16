using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Security.Application;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Portal;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Portal.Configuration;
using Microsoft.Xrm.Portal.Web;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Xrm;

namespace Site.Pages
{
	public class PortalPage : System.Web.UI.Page
	{
		public const string WebAnnotationPrefix = "*WEB*";

		private readonly Lazy<XrmServiceContext> _xrmContext;

		public PortalPage()
		{
			_xrmContext = new Lazy<XrmServiceContext>(() => CreateXrmServiceContext());
		}

		/// <summary>
		/// A general use <see cref="OrganizationServiceContext"/> for managing entities on the page.
		/// </summary>
		public XrmServiceContext XrmContext
		{
			get { return _xrmContext.Value; }
		}

		/// <summary>
		/// The selected portal configuration name.
		/// </summary>
		public string PortalName { get; set; }

		/// <summary>
		/// The current <see cref="IPortalContext"/> instance.
		/// </summary>
		public IPortalContext Portal
		{
			get { return PortalCrmConfigurationManager.CreatePortalContext(PortalName); }
		}

		/// <summary>
		/// The <see cref="OrganizationServiceContext"/> that is associated with the current <see cref="IPortalContext"/> and used to manage its entities.
		/// </summary>
		/// <remarks>
		/// This <see cref="OrganizationServiceContext"/> instance should be used when querying against the Website, User, or Entity properties.
		/// </remarks>
		public XrmServiceContext ServiceContext
		{
			get { return Portal.ServiceContext as XrmServiceContext; }
		}

		/// <summary>
		/// The current <see cref="Adx_website"/>.
		/// </summary>
		public Adx_website Website
		{
			get { return Portal.Website as Adx_website; }
		}

		/// <summary>
		/// The current <see cref="Contact"/>.
		/// </summary>
		public Contact Contact
		{
			get { return Portal.User as Contact; }
		}

		/// <summary>
		/// The <see cref="Entity"/> representing the current page.
		/// </summary>
		public Entity Entity
		{
			get { return Portal.Entity; }
		}

		protected void RedirectToLoginIfAnonymous()
		{
			if (!Request.IsAuthenticated)
			{
				var loginPage = ServiceContext.GetPageBySiteMarkerName(Website, "Login");

				Response.Redirect(ServiceContext.GetUrl(loginPage));
			}
		}

		protected XrmServiceContext CreateXrmServiceContext(MergeOption? mergeOption = null)
		{
			var context = PortalCrmConfigurationManager.CreateServiceContext(PortalName) as XrmServiceContext;
			if (context != null && mergeOption != null) context.MergeOption = mergeOption.Value;
			return context;
		}

		protected virtual void LinqDataSourceSelecting(object sender, LinqDataSourceSelectEventArgs e)
		{
			e.Arguments.RetrieveTotalRowCount = false;
		}

		private readonly IDictionary<int, string> _campaignStatusLabelCache = new Dictionary<int, string>();

		protected string GetCampaignStatusLabel(object dataItem)
		{
			var campaign = dataItem as Campaign;

			if (campaign == null || campaign.StatusCode == null)
			{
				return string.Empty;
			}

			string cachedLabel;

			if (_campaignStatusLabelCache.TryGetValue(campaign.StatusCode.Value, out cachedLabel))
			{
				return cachedLabel;
			}

			var response = (RetrieveAttributeResponse)ServiceContext.Execute(new RetrieveAttributeRequest
			{
				EntityLogicalName = campaign.LogicalName, LogicalName = "statuscode"
			});

			var statusCodeMetadata = response.AttributeMetadata as StatusAttributeMetadata;

			if (statusCodeMetadata == null)
			{
				return string.Empty;
			}

			var option = statusCodeMetadata.OptionSet.Options.FirstOrDefault(o => o.Value == campaign.StatusCode);

			if (option == null)
			{
				return string.Empty;
			}

			var label = option.Label.UserLocalizedLabel.Label;

			if (option.Value.HasValue)
			{
				_campaignStatusLabelCache[option.Value.Value] = label;
			}

			return label;
		}

		protected UrlBuilder GetUrlForRequiredSiteMarker(string siteMarkerName)
		{
			var page = ServiceContext.GetPageBySiteMarkerName(Website, siteMarkerName);

			if (page == null)
			{
				throw new Exception("Please contact your System Administrator. Required Site Marker '{0}' is missing.".FormatWith(siteMarkerName));
			}

			var path = ServiceContext.GetUrl(page);

			if (path == null)
			{
				throw new Exception("Please contact your System Administrator. Unable to build URL for Site Marker '{0}'.".FormatWith(siteMarkerName));
			}

			return new UrlBuilder(path);
		}
	}
}