﻿using System;
using Microsoft.Xrm.Portal.IdentityModel.Web;

namespace Site.Pages
{
	public partial class Error : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			LoadError();
		}

		private void LoadError()
		{
			var error = Request.GetSignInResponseError();

			if (error != null)
			{
				FederationError.Visible = true;

				ErrorDetails.DataSource = new[] { error };
				ErrorDetails.DataBind();

				ErrorList.DataSource = error.Errors;
				ErrorList.DataBind();
			} else
			{
				FederationError.Visible = false;
			}
		}
	}
}