using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xrm;
using Microsoft.Xrm.Portal;
using Microsoft.Xrm.Portal.Configuration;
using Microsoft.Xrm.Portal.Access;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Portal.Core;
using System.Web.Security;

namespace Site.Pages
{

    public partial class ChangePassword :PortalPage
    {
        string userName = string.Empty;
        string currentPassword = string.Empty;

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
                            .FirstOrDefault(c => c.Adx_username == userName
                                && (c.Adx_password == currentPassword));

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
            userName = Request.QueryString["UserName"];
            currentPassword = Request.QueryString["Password"];
            userNameTextBox.Text = userName;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (userName == passwordTextBox.Text)
            {
                Message1.Visible = true;
                Message1.Text = "Old password and new password must be different.  Please try again.";
            }
            else
            {
                Message1.Visible = false;
                LoginContact.Adx_password = passwordTextBox.Text;
                LoginContact.Adx_changepasswordatnextlogon = false;
                XrmContext.Attach(LoginContact);
                XrmContext.UpdateObject(LoginContact);
                XrmContext.SaveChanges();
                XrmContext.Detach(LoginContact);
                Response.Redirect("login", true);
            }
        }

        //protected void ChangePassword1_ChangingPassword(object sender, LoginCancelEventArgs e)
        //{
        //    if (ChangePassword1.CurrentPassword.ToString() == ChangePassword1.NewPassword.ToString())
        //    {
        //        Message1.Visible = true;
        //        Message1.Text = "Old password and new password must be different.  Please try again.";
        //        e.Cancel = true;
        //    }
        //    else
        //    {
        //        //This line prevents the error showing up after a first failed attempt.
        //        Message1.Visible = false;
        //        LoginContact.Adx_password = ChangePassword1.NewPassword;
        //        LoginContact.Adx_changepasswordatnextlogon = false;
        //        XrmContext.Attach(LoginContact);
        //        XrmContext.UpdateObject(LoginContact);
        //        XrmContext.SaveChanges();
        //        XrmContext.Detach(LoginContact);
        //    }
        //}

        //protected void ChangePassword1_ChangedPassword(object sender, EventArgs e)
        //{
            
        //    //ChangePassword1.SuccessPageUrl
        //    //e.Authenticated = true;
        //    FormsAuthentication.RedirectFromLoginPage(ChangePassword1.UserName, true);
        //}
    }

}