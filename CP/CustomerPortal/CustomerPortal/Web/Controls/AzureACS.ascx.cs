using System;
using System.Collections.Generic;
using System.Web.UI;
using Microsoft.Xrm.Portal.IdentityModel.Configuration;
using Microsoft.Xrm.Portal.IdentityModel.Web.Modules;

namespace Site.Controls
{
	public partial class AzureAcs : UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			var settings = FederationCrmConfigurationManager.GetUserRegistrationSettings();
			var invitationCodeKey = settings.InvitationCodeKey ?? "invitation";
			var returnUrlKey = settings.ReturnUrlKey ?? "returnurl";
			var liveIdTokenKey = settings.LiveIdTokenKey ?? "live-id-token";

			var context = new Dictionary<string, string>
				{
					{returnUrlKey, Request[returnUrlKey]},
				};

			if (!string.IsNullOrWhiteSpace(Request[invitationCodeKey]))
			{
				context.Add(invitationCodeKey, Request[invitationCodeKey]);
			}

			if (!string.IsNullOrWhiteSpace(Request[liveIdTokenKey]))
			{
				context.Add(liveIdTokenKey, Request[liveIdTokenKey]);
			}

			var fam = new CrmFederationAuthenticationModule(Context);

			var manager = ScriptManager.GetCurrent(Page);

			if (manager == null)
			{
				return;
			}

			var script = new ScriptReference("~/js/acs.js");

			manager.Scripts.Add(script);

			var reference = new ScriptReference(fam.GetHomeRealmDiscoveryMetadataFeedUrl(callback: "HandleSigninPage", context: context));

			manager.Scripts.Add(reference);
		}
	}
}