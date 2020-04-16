using System;
using Microsoft.Xrm.Portal;
using Microsoft.Xrm.Portal.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Xrm;

namespace Site.Library
{
	public class PortalClass
	{
		private readonly Lazy<XrmServiceContext> _xrmContext;

		public PortalClass()
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

		protected XrmServiceContext CreateXrmServiceContext(MergeOption? mergeOption = null)
		{
			var context = PortalCrmConfigurationManager.CreateServiceContext(PortalName) as XrmServiceContext;
			if (context != null && mergeOption != null) context.MergeOption = mergeOption.Value;
			return context;
		}
	}
}