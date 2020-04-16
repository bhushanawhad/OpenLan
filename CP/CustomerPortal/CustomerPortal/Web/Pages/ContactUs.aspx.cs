using System;
using System.Web.UI.WebControls;
using Microsoft.Xrm.Portal.Web.UI.WebControls;
using Site.Library;

namespace Site.Pages
{
	public partial class ContactUs : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			AddAsirraValidation();
		}

		protected void OnItemInserted(object sender, CrmEntityFormViewInsertedEventArgs e)
		{
			ContactForm.Visible = false;
			ConfirmationMessage.Visible = true;
		}

		private void AddAsirraValidation()
		{
			if (IsPostBack && !Asirra.ValidateAsirraChallenge(Request["Asirra_Ticket"]))
			{
				// If the Asirra ticket is not valid, stop the submission.
				Response.Redirect(Request.RawUrl);
			}

			var submitButton = (Button)ContactForm.FindControl("FormView").FindControl("SubmitButton");

			submitButton.OnClientClick = string.Format(@"if (!validateAsirraChallenge(""{0}"")) return false;", submitButton.ClientID);
		}
	}
}
