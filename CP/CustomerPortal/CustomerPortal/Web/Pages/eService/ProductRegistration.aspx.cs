using System;
using System.Linq;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Portal.Web.UI.WebControls;

namespace Site.Pages.eService
{
	public partial class ProductRegistration : PortalPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			RedirectToLoginIfAnonymous();

			if (Page.IsPostBack)
			{
				return;
			}

			var products = XrmContext.ProductSet.Select(p => p.Name).ToList().OrderBy(p => p);

			if (!products.Any())
			{
				ProductRegistrationFormPanel.Visible = false;
				NoProductsMessage.Visible = true;

				return;
			}

			Product.DataSource = products;
			Product.DataBind();
		}

		protected void OnItemInserting(object sender, CrmEntityFormViewInsertingEventArgs e)
		{
			var product = XrmContext.ProductSet.First(p => p.Name == Product.SelectedValue);

			var serialNumbers = XrmContext.MSA_productregistrationSet
				.Where(pr => pr.MSA_SerialNumber == e.Values["msa_serialnumber"].ToString())
				.ToList();

			if (serialNumbers.Any())
			{
				RegistrationMessage.CssClass = "RegistrationFailure";
				RegistrationMessage.Text = ServiceContext.GetSnippetValueByName(Website, "ProductRegistration/RegistrationFailure");
				e.Cancel = true;

				RegForm.Visible = false;

				return;
			}

			e.Values["msa_customercontactid"] = Contact.ToEntityReference();

			var account = Contact.contact_customer_accounts;

			if (account != null)
			{
				e.Values["msa_customeraccountid"] = account.ToEntityReference();
			}

			e.Values["msa_registeredproductid"] = product.ToEntityReference();
			e.Values["adx_createdbyusername"] = Contact.FullName;
			e.Values["adx_createdbyipaddress"] = Request.UserHostAddress;
		}

		protected void OnItemInserted(object sender, CrmEntityFormViewInsertedEventArgs e)
		{
			RegistrationMessage.CssClass = "RegistrationSuccess";
			RegistrationMessage.Text = ServiceContext.GetSnippetValueByName(Website, "ProductRegistration/RegistrationSuccess");

			RegForm.Visible = false;
		}
	}
}
