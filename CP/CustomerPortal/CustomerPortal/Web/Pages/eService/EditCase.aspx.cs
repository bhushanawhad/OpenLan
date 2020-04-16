using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Xrm.Client.Messages;
using Microsoft.Xrm.Portal.Access;
using Microsoft.Xrm.Portal.Core;
using Microsoft.Xrm.Sdk;
using Site.Library;
using Xrm;
using Microsoft.Xrm.Portal.Web.UI;
using System.Collections.Generic;
using Microsoft.Xrm.Portal.Data;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace Site.Pages.eService
{
	public partial class EditCase : PortalPage
	{
		private Incident _case;

		public Incident Case
		{
			get
			{
				if (_case != null)
				{
					return _case;
				}

				Guid caseId;

				if (!Guid.TryParse(Request["CaseID"], out caseId))
				{
					return null;
				}

				_case = XrmContext.GetCase(caseId) as Incident;

				XrmContext.Detach(_case);

				return _case;
			}
			set
			{
				if (value != null)
				{
					try
					{
						_case = value;
					}
					catch
					{
						throw new InvalidCastException();
					}
				}
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			//RedirectToLoginIfAnonymous();

			if (IsPostBack) return;

			var parameter = new Parameter("Filter", DbType.String, WebAnnotationPrefix);

			CrmNoteSource.WhereParameters.Add(parameter);

			if (Case == null)
			{
				actionResultMessage.Text = "Case not found";
				CaseInfoPanel.Visible = false;
				CaseInfoPanelNotes.Visible = false;
				EditPanel.Visible = false;
				return;
			}

			TitleLabel.Text = Case.Title;
			CaseNumberLabel.Text = " Case Number: " + Case.TicketNumber;
			StatusReasonLabel.Text = Enum.GetName(typeof(Enums.IncidentState), Case.StateCode.GetValueOrDefault());
            string Description = Case.Description ;
            if (Description != null)
            {
                Description = Description.TrimStart(); Description = Description.TrimEnd();
                if (Description.Length > 1000)
                {
                    Description += Description.Substring(1, 999);
                }

                descriptionTextBox.Text = Description;
            }
			XrmContext.Attach(Case);

			if (Case.subject_incidents != null)
			{
				SubjectLabel.Text = "| Subject: " + Case.subject_incidents.Title;
			}

			XrmContext.Detach(Case);
			CaseTypeLabel.Text = "| Case Type: " + GetCaseTypeCodeOptionLabelByValue(Case.CaseTypeCode.GetValueOrDefault());
			CreatedOn.Text = Case.CreatedOn.GetValueOrDefault().ToLocalTime().ToString();
			LastModifiedOn.Text = Case.ModifiedOn.GetValueOrDefault().ToLocalTime().ToString();

			if (Case.KbArticleId != null)
			{
				KB.Text = "Please see the following KB article: <a href=\"/kb/kb-article?id=" + Case.KbArticleId + "\">" + Case.kbarticle_incidents.Title + "</a><br/>";
			}

			var access = ServiceContext.GetCaseAccessByContact(Contact) as Adx_caseaccess;

			if (access == null || !access.Adx_Write.GetValueOrDefault())
			{
				UpdateFields.Visible = false;
				UpdateButton.Visible = false;
			}

			if (Case.StateCode != (int)Enums.IncidentState.Active)
			{
				EditPanel.Enabled = false;
				UpdateButton.Visible = false;
				CancelCaseButton.Visible = false;
				CloseCasePanel.Visible = false;
				ReopenCase.Visible = true;
			}

            ////Get Activities for cases.
            var result = new List<Entity>();

            var activityPointer =
                from c in XrmContext.CreateQuery("activitypointer")
                where c.GetAttributeValue<Guid?>("regardingobjectid") == Case.Id
                select c;
            
            result.AddRange(activityPointer);
            if (result.Count > 0)
            {
                foreach (Entity ent in result)
                {
                    if (!ent.Contains("actualdurationminutes"))
                    {
                        ent.Attributes.Add("actualdurationminutes", "0");
                    }
                    
                    if (ent.Attributes["activitytypecode"].ToString().Trim() == "email")
                    {
                        if (ent.Contains("description"))
                        {
                            string emailDescription = ent.Attributes["description"].ToString();
                            emailDescription = StripHtml(emailDescription);

                            if (emailDescription.Length > 1000)
                            {
                                emailDescription = emailDescription.Substring(0, 1999);
                            }
                            ent.Attributes["description"] = emailDescription;
                        }
                    }
                    if (!ent.Contains("description"))
                    {
                        ent.Attributes.Add("description", "");
                    }
                }
                ActivityList.DataKeyNames = new[] { "activityid" };
                ActivityList.DataSource = result.ToDataTable(XrmContext);
                //ActivityList.ColumnsGenerator = new CrmSavedQueryColumnsGenerator("Activities Web View");
                ActivityList.DataBind();
            }
		}

		protected void UpdateButton_Click(object sender, EventArgs e)
		{
			if (Case == null) throw new NullReferenceException("Case must not be null. Update failed.");

			Case.Adx_ModifiedByUsername = Contact.FullName;
			Case.Adx_ModifiedByIPAddress = Request.UserHostAddress;

			XrmContext.Attach(Case);
			XrmContext.UpdateObject(Case);
			XrmContext.SaveChanges();
			XrmContext.Detach(Case);

			var contactName = Contact.FullName;
			var noteSubject = "Note created on " + DateTime.Now + " by " + contactName;

			if (!string.IsNullOrEmpty(NewNote.Text) || (Attachment.PostedFile != null && Attachment.PostedFile.ContentLength > 0))
			{
				XrmContext.AddNoteAndSave(Case, noteSubject, "*WEB* " + NewNote.Text, Attachment.PostedFile);
			}

			if (System.Web.SiteMap.CurrentNode == null) return;

			Response.Redirect(Request.RawUrl);
		}

		protected void CancelCaseButton_Click(object sender, EventArgs e)
		{
			CloseCasePanel.Visible = true;
			CancelCaseButton.Visible = false;
		}

		protected void ResolveButton_Click(object sender, EventArgs e)
		{
			EditPanel.Enabled = false;

			CloseRelatedActivities();

			Case.CustomerSatisfactionCode = int.Parse(Satisfaction.SelectedValue);

			XrmContext.Attach(Case);
			XrmContext.UpdateObject(Case);
			XrmContext.SaveChanges();
			XrmContext.SetCaseStatusAndSave(Case, "Resolved", Resolution.Text);
			XrmContext.Detach(Case);

			Response.Redirect(Request.RawUrl);
		}

		protected void CloseRelatedActivities()
		{
			XrmContext.Attach(Case);
			var activities = Case.Incident_ActivityPointers;
			XrmContext.Detach(Case);

			if (activities == null)
			{
				return;
			}

			foreach (ActivityPointer a in activities)
			{
				if (a.StateCode == (int)Enums.ActivityPointerState.Open || a.StateCode == (int)Enums.ActivityPointerState.Scheduled)
				{
					var activityGuid = a.ActivityId;
					var activityTypeCode = a.ActivityTypeCode;

					switch (activityTypeCode)
					{
						case "phonecall":
							var phonecall = XrmContext.PhoneCallSet.Where(pc => pc.ActivityId == activityGuid).FirstOrDefault();
							SetStateCanceled(phonecall);
							break;

						case "task":
							var task = XrmContext.TaskSet.Where(t => t.ActivityId == activityGuid).FirstOrDefault();
							SetStateCanceled(task);
							break;

						case "fax":
							var fax = XrmContext.FaxSet.Where(f => f.ActivityId == activityGuid).FirstOrDefault();
							SetStateCanceled(fax);
							break;

						case "email":
							var email = XrmContext.EmailSet.Where(e => e.ActivityId == activityGuid).FirstOrDefault();
							SetStateCanceled(email);
							break;

						case "letter":
							var letter = XrmContext.LetterSet.Where(l => l.ActivityId == activityGuid).FirstOrDefault();
							SetStateCanceled(letter);
							break;

						case "appointment":
							var appointment = XrmContext.AppointmentSet.Where(ap => ap.ActivityId == activityGuid).FirstOrDefault();
							SetStateCanceled(appointment);
							break;

						case "serviceappointment":
							var serviceAct = XrmContext.ServiceAppointmentSet.Where(s => s.ActivityId == activityGuid).FirstOrDefault();
							SetStateCanceled(serviceAct);
							break;

						default:
							break;
					}
				}
			}

			return;
		}

		protected void ReopenButton_Click(object sender, EventArgs e)
		{
			XrmContext.SetCaseStatusAndSave(Case, "Active", null);

			XrmContext.Detach(Case);

			Response.Redirect(Request.RawUrl);
		}

		protected static string FormatNote(object noteText)
		{
			if (noteText != null)
			{
				var formatedNote = noteText.ToString().Replace("\n", "<br />\n");
				return formatedNote.Replace("*WEB* ", "");
			}
			return string.Empty;
		}

		private void SetStateCanceled(Entity entity)
		{
			XrmContext.SetState((int)Enums.IncidentState.Canceled, -1, entity);
		}

		protected string GetCaseTypeCodeOptionLabelByValue(int value)
		{
			return new EntityOptionSet().GetOptionSetLabelByValue(Case.LogicalName, "casetypecode", value);
		}

        protected void ActivityList_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow || e.Row.Cells.Count < 1)
            {
                return;
            }
            
            //e.Row.Cells[0].Text = string.Format(@"<a href=""{0}"">{1}</a>",
            //    ActivityUrl(ActivityList.DataKeys[e.Row.RowIndex].Value),
            //    e.Row.Cells[0].Text);

            foreach (TableCell cell in e.Row.Cells)
            {
                DateTime cellAsDateTime;

                if (DateTime.TryParse(cell.Text, out cellAsDateTime))
                {
                    cell.Text = cellAsDateTime.ToLocalTime().ToString("g");
                }
            }
        }

        private static readonly Regex _tags_ = new Regex(@"<[^>]+?>", RegexOptions.Multiline | RegexOptions.Compiled);

        //add characters that are should not be removed to this regex
        private static readonly Regex _notOkCharacter_ = new Regex(@"[^\w;&#@.:/\\?=|%!() -]", RegexOptions.Compiled);

        public static String StripHtml(String html)
        {
            html = HttpUtility.UrlDecode(html);
            html = HttpUtility.HtmlDecode(html);

            html = RemoveTag(html, "<!--", "-->");
            html = RemoveTag(html, "<script", "</script>");
            html = RemoveTag(html, "<style", "</style>");

            //replace matches of these regexes with space
            html = _tags_.Replace(html, " ");
            html = _notOkCharacter_.Replace(html, " ");
            html = SingleSpacedTrim(html);

            return html;
        }

        private static String RemoveTag(String html, String startTag, String endTag)
        {
            Boolean bAgain;
            do
            {
                bAgain = false;
                Int32 startTagPos = html.IndexOf(startTag, 0, StringComparison.CurrentCultureIgnoreCase);
                if (startTagPos < 0)
                    continue;
                Int32 endTagPos = html.IndexOf(endTag, startTagPos + 1, StringComparison.CurrentCultureIgnoreCase);
                if (endTagPos <= startTagPos)
                    continue;
                html = html.Remove(startTagPos, endTagPos - startTagPos + endTag.Length);
                bAgain = true;
            } while (bAgain);
            return html;
        }

        private static String SingleSpacedTrim(String inString)
        {
            StringBuilder sb = new StringBuilder();
            Boolean inBlanks = false;
            foreach (Char c in inString)
            {
                switch (c)
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case ' ':
                        if (!inBlanks)
                        {
                            inBlanks = true;
                            sb.Append(' ');
                        }
                        continue;
                    default:
                        inBlanks = false;
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString().Trim();
        }

        //protected string ActivityUrl(object id)
        //{
        //    var page = ServiceContext.GetPageBySiteMarkerName(Website, "View Activity");
            
        //    var url = new UrlBuilder(ServiceContext.GetUrl(page));

        //    url.QueryString.Set("ActivityID", id.ToString());

        //    return url.PathWithQueryString;
        //}
	}
}
