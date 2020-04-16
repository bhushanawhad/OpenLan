using System;
using System.Web;
using Microsoft.IdentityModel.Web;
using Microsoft.Xrm.Portal.IdentityModel;

namespace Site
{
	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			FederatedAuthentication.ServiceConfigurationCreated += Extensions.OnServiceConfigurationCreated;
		}
	}
}