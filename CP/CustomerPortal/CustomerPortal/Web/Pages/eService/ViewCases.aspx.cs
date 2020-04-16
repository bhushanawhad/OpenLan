using System;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Xrm.Portal.Access;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Portal.Data;
using Microsoft.Xrm.Portal.Web;
using Microsoft.Xrm.Portal.Web.UI;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Site.Library;
using Xrm;

namespace Site.Pages.eService
{
	public partial class ViewCases : PortalPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			RedirectToLoginIfAnonymous();

			if (!IsPostBack)
			{
				HideControlsBasedOnAccess(ServiceContext, Contact);
			}

			var cases = new CaseAccess().CasesForCurrentUser.Cast<Incident>();

			var status = StatusDropDown.Text;

			var casesByStatus = string.Equals(status, "Active", StringComparison.InvariantCulture)
				? cases.Where(c => c.StateCode == (int)Enums.IncidentState.Active)
				: cases.Where(c => c.StateCode != (int)Enums.IncidentState.Active);

			var casesByCustomer = string.Equals(CustomerFilter.Text, "My", StringComparison.InvariantCulture)
				? casesByStatus.Where(c => c.CustomerId.Id == Contact.ContactId)
				: string.Equals(CustomerFilter.Text, "My Company's", StringComparison.InvariantCulture)
					? casesByStatus.Where(c => c.CustomerId.Id != Contact.ContactId)
					: casesByStatus;

			if (casesByCustomer.Count() == 0)
			{
				return;
			}

			CaseList.DataKeyNames = new[] { "incidentid" };
			CaseList.DataSource = casesByCustomer.ToDataTable(XrmContext);
			CaseList.ColumnsGenerator = new CrmSavedQueryColumnsGenerator("Cases Web View");
			CaseList.DataBind();
		}

		protected void CaseList_OnRowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.DataRow || e.Row.Cells.Count < 1)
			{
				return;
			}

			e.Row.Cells[0].Text = string.Format(@"<a href=""{0}"">{1}</a>",
				CaseUrl(CaseList.DataKeys[e.Row.RowIndex].Value),
				e.Row.Cells[0].Text);

			foreach (TableCell cell in e.Row.Cells)
			{
				DateTime cellAsDateTime;

				if (DateTime.TryParse(cell.Text, out cellAsDateTime))
				{
					cell.Text = cellAsDateTime.ToLocalTime().ToString("g");
				}
			}
		}

		protected string CaseUrl(object id)
		{
			var page = ServiceContext.GetPageBySiteMarkerName(Website, "Edit Case");

			var url = new UrlBuilder(ServiceContext.GetUrl(page));

			url.QueryString.Set("CaseID", id.ToString());

			return url.PathWithQueryString;
		}

		private void HideControlsBasedOnAccess(OrganizationServiceContext context, Entity contact)
		{
			var access = context.GetCaseAccessByContact(contact) as Adx_caseaccess;

			if (access == null || !access.Adx_Create.GetValueOrDefault())
			{
				CreateCaseLink.Visible = false;
			}

			if (access == null || access.Adx_Scope.Value != (int)Enums.Adx_caseaccess.ScopeOption.Account)
			{
				CustomerFilter.Visible = false;
			}
		}
	}
}
