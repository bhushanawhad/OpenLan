using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Security.Application;
using Microsoft.Xrm.Portal.IdentityModel.Web.Modules;

namespace Site.MasterPages
{
	public partial class Default : MasterPage
	{
		protected void Page_Init(object sender, EventArgs e)
		{
			if (Page.User.Identity.IsAuthenticated)
			{
				Page.ViewStateUserKey = Page.User.Identity.Name;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			var federatedLoginLink = (HyperLink)LoginViewControl.FindControl("LoginLink");

			if (federatedLoginLink != null)
			{
				federatedLoginLink.NavigateUrl = "~/login?ReturnUrl=" + AntiXss.UrlEncode(Request.Path);
			}
		}
	}
}
