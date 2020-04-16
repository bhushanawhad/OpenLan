using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Portal.IdentityModel.Configuration;
using Microsoft.Xrm.Portal.IdentityModel.Web.Modules;
using Site.Library;

namespace Site.Pages
{
	public partial class ConfirmInvite : PortalPage
	{
		protected IUserRegistrationSettings RegistrationSettings
		{
			get { return FederationCrmConfigurationManager.GetUserRegistrationSettings(PortalName); }
		}

		protected string ReturnUrlKey
		{
			get { return RegistrationSettings.ReturnUrlKey ?? "returnurl"; }
		}

		protected string InvitationCodeKey
		{
			get { return RegistrationSettings.InvitationCodeKey ?? "invitation"; }
		}

		protected string ChallengeAnswerKey
		{
			get { return RegistrationSettings.ChallengeAnswerKey ?? "answer"; }
		}

		protected string ResultCodeKey
		{
			get { return RegistrationSettings.ResultCodeKey ?? "result-code"; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Page.IsPostBack)
			{
				return;
			}

			if (!RegistrationSettings.Enabled)
			{
				Registration.Visible = false;
				OpenRegistration.Visible = false;

				return;
			}

			var unregistered = Request[ResultCodeKey] == "unregistered";

			if (unregistered)
			{
				UnregisteredMessage.Visible = true;
				PageContent.Visible = false;
				Registration.Visible = false;
			}

			ConfirmMessage.Visible = Request[ResultCodeKey] == "confirm";
			InactiveMessage.Visible = Request[ResultCodeKey] == "inactive";
			InvalidMessage.Visible = Request[ResultCodeKey] == "invalid-invitation-code";
			OpenRegistration.Visible = !RegistrationSettings.RequiresInvitation;

			// check if an invitation code is provided

			var invitation = Request[InvitationCodeKey];

			InvitationCode.Text = invitation;

			var question = GetChallengeQuestion(invitation);

			if (!string.IsNullOrWhiteSpace(question))
			{
				// go directly to the challenge question/answer

				EnableChallenge(question);
			}

			// pass on the sign-in post if it exists

			var fam = new CrmFederationAuthenticationModule(Context);

			if (fam.CanReadSignInResponse(Request, true))
			{
				fam.AddSignInResponseParametersToForm(Context, Form);
			}
			else
			{
				const string continueMessage = "Continue to identity provider";
				SubmitInvitationCodeButton.Text = continueMessage;
				SubmitChallengeAnswerButton.Text = continueMessage;
				SubmitOpenRegistrationButton.Text = continueMessage;
			}
		}

		protected void InvitationCodeCustomValidator_ServerValidate(object sender, ServerValidateEventArgs args)
		{
			// check that the invitation code exists

			var invitationCode = InvitationCode.Text;

			var question = GetChallengeQuestion(invitationCode);

			if (!string.IsNullOrWhiteSpace(question))
			{
				// display the challenge question

				EnableChallenge(question);
				args.IsValid = true;
				return;
			}

			if (!RegistrationSettings.RequiresChallengeAnswer)
			{
				// challenge answer is optional and this account does not have a question

				var returnUrl = Request[ReturnUrlKey];
				var fam = new CrmFederationAuthenticationModule(Context);

				if (fam.CanReadSignInResponse(Request, true))
				{
					// this is already a sign-in post, authenticate the user

					fam.TryHandleSignInResponse(Context, returnUrl, invitationCode, null, RegistrationSettings);
				}
				else
				{
					// redirect to get the sign-in token

					fam.RedirectToSignIn(Context, returnUrl, invitationCode, null, RegistrationSettings);
				}

				args.IsValid = true;
				return;
			}

			args.IsValid = false;
		}

		protected void ChallengeAnswerCustomValidator_ServerValidate(object sender, ServerValidateEventArgs args)
		{
			// check both the invitation code and challenge answer

			var invitationCode = InvitationCode.Text;
			var answer = ChallengeAnswer.Text;

			if (!string.IsNullOrWhiteSpace(invitationCode) && !string.IsNullOrWhiteSpace(answer))
			{
				var now = DateTime.UtcNow.Round(RoundTo.Hour);

				var find = XrmContext.ContactSet
					.Where(c => (c.Adx_InvitationCodeExpiryDate == null || c.Adx_InvitationCodeExpiryDate > now)
						&& c.Adx_InvitationCode == invitationCode
						&& c.Adx_passwordanswer == answer);

				if (find.AsEnumerable().Any())
				{
					args.IsValid = true;
					return;
				}
			}

			args.IsValid = false;
		}

		protected void SubmitChallengeAnswerButton_Click(object sender, EventArgs args)
		{
			if (!Page.IsValid) return;

			var invitation = InvitationCode.Text;
			var answer = ChallengeAnswer.Text;
			var returnUrl = Request[ReturnUrlKey];

			var fam = new CrmFederationAuthenticationModule(Context);

			if (fam.CanReadSignInResponse(Request, true))
			{
				// this is already a sign-in post, authenticate the user

				fam.TryHandleSignInResponse(Context, returnUrl, invitation, answer, RegistrationSettings);
			}
			else
			{
				// redirect to get the sign-in token

				fam.RedirectToSignIn(Context, returnUrl, invitation, answer, RegistrationSettings);
			}
		}

		protected void SubmitOpenRegistrationButton_Click(object sender, EventArgs args)
		{
			if (!Page.IsValid) return;

			var attributes = new Dictionary<string, string>
			{
				{ "firstname", ContentUtility.HtmlEncode(FirstName.Text) },
				{ "lastname", ContentUtility.HtmlEncode(LastName.Text) },
				{ "emailaddress1", ContentUtility.HtmlEncode(Email.Text) },
			};

			var returnUrl = Request[ReturnUrlKey];

			if (!string.IsNullOrWhiteSpace(returnUrl))
			{
				attributes.Add(ReturnUrlKey, returnUrl);
			}

			var fam = new CrmFederationAuthenticationModule(Context);

			if (fam.CanReadSignInResponse(Request, true))
			{
				// this is already a sign-in post, authenticate the user

				fam.TryHandleSignInResponse(Context, attributes, RegistrationSettings);
			}
			else
			{
				// redirect to get the sign-in token

				fam.RedirectToSignIn(Context, attributes);
			}
		}

		private string GetChallengeQuestion(string invitationCode)
		{
			if (!string.IsNullOrWhiteSpace(invitationCode))
			{
				// get the challenge question

				var question = XrmContext.ContactSet
					.Where(c => c.Adx_InvitationCode == invitationCode)
					.Select(c => c.Adx_passwordquestion)
					.FirstOrDefault();

				return question;
			}

			return null;
		}

		private void EnableChallenge(string question)
		{
			// enable the challenge form

			InvitationCode.Enabled = false;
			SubmitInvitationCodeButton.Visible = false;

			Challenge.Visible = true;
			ChallengeQuestion.Text = question;
		}
	}
}