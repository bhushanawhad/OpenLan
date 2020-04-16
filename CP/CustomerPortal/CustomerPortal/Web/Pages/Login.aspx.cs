using System;
using System.Web.Security;
using Xrm;
using System.Linq;
using Microsoft.Xrm.Portal;
using Microsoft.Xrm.Portal.Configuration;
using Microsoft.Xrm.Portal.Access;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Portal.Core;

namespace Site.Pages
{
    public partial class Login : PortalPage
    {

        private Contact _loginContact;
        protected Contact LoginContact
        {
            get
            {
                try
                {
                    if (_loginContact != null)
                    {
                        return _loginContact;
                    }

                    _loginContact = XrmContext.ContactSet
                            .FirstOrDefault(c => c.Adx_username == Login1.UserName
                                && (c.Adx_password == Login1.Password));

                    XrmContext.Detach(_loginContact);
                    return _loginContact;
                    
                }
                catch (System.Exception ex)
                {
                    return null;
                }
            }
        
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((User != null && User.Identity != null) && User.Identity.IsAuthenticated)
            {
                var redirectUrl = !string.IsNullOrEmpty(Request.QueryString["ReturnUrl"])
                    ? Request["ReturnUrl"]
                    : !string.IsNullOrEmpty(Request.QueryString["URL"])
                        ? Request["URL"]
                        : "/";

                Response.Redirect(redirectUrl);
            }
        }

        protected void Login1_Authenticate(object sender, System.Web.UI.WebControls.AuthenticateEventArgs e)
        {
            if (LoginContact == null)
            {
                e.Authenticated = false;
            }
            else
            {
                if (LoginContact.Adx_username == Login1.UserName)
                {
                    if (LoginContact.Adx_changepasswordatnextlogon.Value)
                    {
                        var portal = PortalCrmConfigurationManager.CreatePortalContext();
                        var website = (Adx_website)portal.Website;
                        var page = (Adx_webpage)portal.ServiceContext.GetPageBySiteMarkerName(portal.Website, "ChangePassword");

                        string redirectURL = page.Adx_PartialUrl + "?UserName=" + Server.UrlEncode(Login1.UserName) + 
                            "&Password=" + Server.UrlEncode(Login1.Password);
                        Response.Redirect(redirectURL);
                    }
                    else
                    {
                        LoginContact.Adx_LastSuccessfulLogon = DateTime.Now;

                        XrmContext.Attach(LoginContact);
                        XrmContext.UpdateObject(LoginContact);
                        XrmContext.SaveChanges();
                        XrmContext.Detach(LoginContact);

                        e.Authenticated = true;
                        FormsAuthentication.RedirectFromLoginPage(Login1.UserName, true);
                    }
                }
                else
                {
                    e.Authenticated = false;
                }
            }

        }
    }
}
