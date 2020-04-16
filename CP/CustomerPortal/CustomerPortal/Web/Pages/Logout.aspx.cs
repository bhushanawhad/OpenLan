using System;
using System.Linq;
using Microsoft.Xrm.Portal.Access;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Portal.Core;
using Microsoft.Xrm.Portal.Web;
using Xrm;
using System.Web.UI.WebControls;
using Site.Library;
using System.Web.Security;
using Microsoft.Xrm.Portal.Configuration;

namespace Site.Pages
{
    public partial class Logout : PortalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();

            var portal = PortalCrmConfigurationManager.CreatePortalContext();
            var website = (Adx_website)portal.Website;
            var page = (Adx_webpage)portal.ServiceContext.GetPageBySiteMarkerName(portal.Website, "Home");
            Response.Redirect(page.Adx_PartialUrl);
        }
    }
}