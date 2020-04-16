using System;
using System.Linq;
using Microsoft.Xrm.Portal.Access;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Portal.Core;
using Xrm;

namespace Site.Pages.eService
{
	public partial class CreateCase : PortalPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			RedirectToLoginIfAnonymous();

			if (IsPostBack) return;
		}

		protected void CreateButton_Click(object sender, EventArgs e)
		{
			var access = ServiceContext.GetCaseAccessByContact(Contact) as Adx_caseaccess;

			if (access == null || !access.Adx_Create.GetValueOrDefault())
			{
				return; // no permission to create a case
			}

			PriorityCode.SelectedValue = PriorityCode.Items.FindByText(ServiceContext.GetSiteSettingValueByName(Website, "case/prioritycode")).Value;

			var subject = XrmContext.SubjectSet.First(s => s.Title == ServiceContext.GetSiteSettingValueByName(Website, "case/subject"));

			var incident = new Incident
			{
				Title = TitleTextBox.Text,
				PriorityCode = int.Parse(PriorityCode.SelectedValue),
				CaseTypeCode = int.Parse(CaseType.SelectedValue),
				SubjectId = subject.ToEntityReference(),
				CustomerId = Contact.ToEntityReference(),
				Adx_CreatedByUsername = Contact.FullName,
				Adx_CreatedByIPAddress = Request.UserHostAddress,
			};

			XrmContext.AddObject(incident);

			XrmContext.SaveChanges();

			var noteSubject = "Note created on " + DateTime.Now + " by " + Contact.FullName;

			XrmContext.AddNoteAndSave(incident, noteSubject, WebAnnotationPrefix + " " + Description.Text, Attachment.PostedFile);

			var page = ServiceContext.GetPageBySiteMarkerName(Website, "Cases");

			Response.Redirect(ServiceContext.GetUrl(page));
		}
	}
}
