using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Portal.Cms;
using Microsoft.Xrm.Portal.Core;
using Microsoft.Xrm.Sdk.Client;

namespace Site.Pages.eService
{
	public partial class KbArticle : PortalPage
	{
		private Xrm.KbArticle _article;

		protected Xrm.KbArticle Article
		{
			get { return _article ?? (_article = XrmContext.KbArticleSet.First(article => article.KbArticleId == new Guid(Request["id"]))); }
		}

		protected string ArticleContent
		{
			get
			{
				var tableRegex = new Regex("<table(.*?)</table>", RegexOptions.Singleline);
				var tableMatch = tableRegex.Match(Article.Content);

				return tableMatch.Success
					? tableMatch.Value
					: string.Empty;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			RedirectToLoginIfAnonymous();

			CrmPageCopy.InnerHtml = ArticleContent;
		}

		protected void SendKBArticle_Click(object sender, EventArgs e)
		{
			var from = ServiceContext.GetSiteSettingValueByName(Website, "smtp/from");

			var mail = new MailMessage
			{
				From = new MailAddress(from),
				Subject = Article.Title,
				Body = ArticleContent.Replace("style=\"", "style=\"line-height: normal;").Replace("style='", "style='line-height: normal;"),
				IsBodyHtml = true
			};

			mail.To.Add(toEmailAddresses.Text);

			var smtpServer = ServiceContext.GetSiteSettingValueByName(Website, "smtp/server");
			var smtpUserName = ServiceContext.GetSiteSettingValueByName(Website, "smtp/userName");
			var smtpPassword = ServiceContext.GetSiteSettingValueByName(Website, "smtp/password");

			var smtp = new SmtpClient(smtpServer) { Credentials = new NetworkCredential(smtpUserName, smtpPassword) };
			
			smtp.Send(mail);

			var noteSubject = "Note created on " + DateTime.Now + " by " + Contact.FullName;

			var contact = XrmContext.MergeClone(Contact);
			XrmContext.AddNoteAndSave(contact, noteSubject, "Knowledge Base Article titled: " + Article.Title + ", was emailed to " + toEmailAddresses.Text + " on " + DateTime.Now);
			
			toEmailAddresses.Text = "";
			
			EmailSentMessage.Text = "Your email has been sent.";
		}
	}
}
