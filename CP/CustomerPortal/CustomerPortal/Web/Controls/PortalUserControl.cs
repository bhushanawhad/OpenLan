using System;
using Microsoft.Xrm.Portal;
using Microsoft.Xrm.Portal.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Xrm;

namespace Site.Controls
{
	public class PortalUserControl : System.Web.UI.UserControl
	{
		private readonly Lazy<XrmServiceContext> _xrmContext;

		public PortalUserControl()
		{
			_xrmContext = new Lazy<XrmServiceContext>(() => CreateXrmServiceContext());
		}

		public XrmServiceContext XrmContext
		{
			get { return _xrmContext.Value; }
		}

		public string PortalName { get; set; }

		public IPortalContext Portal
		{
			get { return PortalCrmConfigurationManager.CreatePortalContext(PortalName); }
		}

		public XrmServiceContext ServiceContext
		{
			get { return Portal.ServiceContext as XrmServiceContext; }
		}

		public Adx_website Website
		{
			get { return Portal.Website as Adx_website; }
		}

		public Contact Contact
		{
			get { return Portal.User as Contact; }
		}

		public Entity Entity
		{
			get { return Portal.Entity; }
		}

		protected XrmServiceContext CreateXrmServiceContext(MergeOption? mergeOption = null)
		{
			var context = PortalCrmConfigurationManager.CreateServiceContext(PortalName) as XrmServiceContext;
			if (context != null && mergeOption != null) context.MergeOption = mergeOption.Value;
			return context;
		}
	}
}