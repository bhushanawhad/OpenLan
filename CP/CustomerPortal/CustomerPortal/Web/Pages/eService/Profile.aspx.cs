using System;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Messages;
using Microsoft.Xrm.Portal.Access;
using Microsoft.Xrm.Portal.Cms;
using Xrm;

namespace Site.Pages.eService
{
	public partial class Profile : PortalPage
	{
		public Adx_accountaccess AccountPermission
		{
			get { return ServiceContext.GetAccountAccessByContact(Contact) as Adx_accountaccess; }
		}

		protected bool AccountReadAccess
		{
			get { return (AccountPermission != null && AccountPermission.Adx_Read.GetValueOrDefault()) || (ContactPermission != null && ContactPermission.Adx_Scope.Value == 2 && ContactPermission.Adx_Read.GetValueOrDefault()); }
		}

		protected bool AccountWriteAccess
		{
			get { return (AccountPermission != null && AccountPermission.Adx_Write.GetValueOrDefault()) || (ContactPermission != null && ContactPermission.Adx_Scope.Value == 2 && ContactPermission.Adx_Write.GetValueOrDefault()); }
		}

		public Adx_contactaccess ContactPermission
		{
			get { return ServiceContext.GetContactAccessByContact(Contact) as Adx_contactaccess; }
		}

		private void BindTimeZoneDropDown()
		{
			TimeZoneSelection.DataSource = XrmContext.TimeZoneDefinitionSet.OrderBy(t => t.StandardName);
			TimeZoneSelection.DataTextField = "standardname";
			TimeZoneSelection.DataValueField = "timezonecode";
			TimeZoneSelection.DataBind();

			TimeZoneSelection.Items.Insert(0, new ListItem("Please Select One..."));

			if (Contact.Adx_TimeZone.HasValue)
			{
				TimeZoneSelection.SelectedValue = Contact.Adx_TimeZone.ToString();
			}
		}

		public bool IsListChecked(object listoption)
		{
			var list = (List)listoption;

			if (Request.IsAuthenticated)
			{
				return Contact != null
					&& Contact.listcontact_association
					.Any(l => l.ListId == list.ListId);
			}

			return false;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack) return;

			// check if this is a profile edit, or a new signup
			if (Request.IsAuthenticated)
			{
				//bind timezone options
				BindTimeZoneDropDown();

				// the user is already authenticated, so this is a profile edit.
				if (Portal != null)
				{
					if (Contact == null) return;

					PopulateAccountForm(Contact.contact_customer_accounts);

					PopulateContactForm(Contact);
				}

				SubmitButton.Text = "Update Account";

				if (!AccountInformation.Visible && !SelfInformation.Visible)
				{
					SubmitButton.Text = "You do not have permissions to read your account information!";
					SubmitButton.Enabled = false;
				}
				else if ((AccountName.ReadOnly && (!SelfInformation.Visible || FirstName.ReadOnly)) || (FirstName.ReadOnly && (!AccountInformation.Visible || AccountName.ReadOnly)))
				{
					SubmitButton.Visible = false;
				}
			}
			else
			{
				// the user is not signed in
				var loginPage = ServiceContext.GetPageBySiteMarkerName(Website, "Login");

				Response.Redirect(ServiceContext.GetUrl(loginPage));
			}

			if (Website == null) return;

			MarketingList.DataSource = Website.adx_website_list;
			MarketingList.DataBind();
		}

		private void PopulateAccountForm(Account account)
		{
			if (account == null || !AccountReadAccess)
			{
				AccountInformation.Visible = false;
				return;
			}

			AccountName.Text = account.Name;
			AccountWebsite.Text = account.WebSiteURL;
			AccountAddress1.Text = account.Address1_Line1;
			AccountAddress2.Text = account.Address1_Line2;
			AccountAddress3.Text = account.Address1_Line3;
			AccountCity.Text = account.Address1_City;
			AccountProvince.Text = account.Address1_StateOrProvince;
			AccountPostalCode.Text = account.Address1_PostalCode;
			AccountCountry.Text = account.Address1_Country;

			if (!AccountWriteAccess)
			{
				AccountName.Enabled = false;
				AccountWebsite.Enabled = false;
				AccountAddress1.Enabled = false;
				AccountAddress2.Enabled = false;
				AccountAddress3.Enabled = false;
				AccountCity.Enabled = false;
				AccountProvince.Enabled = false;
				AccountPostalCode.Enabled = false;
				AccountCountry.Enabled = false;
			}
		}

		protected void PopulateContactForm(Contact contact)
		{
			if (ContactPermission == null || !ContactPermission.Adx_Read.GetValueOrDefault())
			{
				SelfInformation.Visible = false;
				return;
			}

			FirstName.Text = contact.FirstName;
			LastName.Text = contact.LastName;
			EmailAddress.Text = contact.EMailAddress1;
			PhoneNumber.Text = contact.Telephone1;
			JobTitle.Text = contact.JobTitle;
			ContactAddressLine1.Text = contact.Address1_Line1;
			ContactAddressLine2.Text = contact.Address1_Line2;
			ContactAddressLine3.Text = contact.Address1_Line3;
			ContactCity.Text = contact.Address1_City;
			ContactState.Text = contact.Address1_StateOrProvince;
			ContactZip.Text = contact.Address1_PostalCode;
			ContactCountry.Text = contact.Address1_Country;
			marketEmail.Checked = !(bool)contact.DoNotEMail;
			marketFax.Checked = !(bool)contact.DoNotFax;
			marketPhone.Checked = !(bool)contact.DoNotPhone;
			marketMail.Checked = !(bool)contact.DoNotPostalMail;



			if (!ContactPermission.Adx_Write.GetValueOrDefault())
			{
				FirstName.Enabled = false;
				LastName.Enabled = false;
				EmailAddress.Enabled = false;
				PhoneNumber.Enabled = false;
				JobTitle.Enabled = false;
				ContactAddressLine1.Enabled = false;
				ContactAddressLine2.Enabled = false;
				ContactAddressLine3.Enabled = false;
				ContactCity.Enabled = false;
				ContactState.Enabled = false;
				ContactZip.Enabled = false;
				ContactCountry.Enabled = false;
				TimeZoneSelection.Enabled = false;
				marketEmail.Enabled = false;
				marketFax.Enabled = false;
				marketPhone.Enabled = false;
				marketMail.Enabled = false;
			}
		}

		protected void SubmitButton_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid) return;

			if (!Request.IsAuthenticated) return;

			if (AccountInformation.Visible)
			{
				var contactAccount = Contact.contact_customer_accounts;

				SaveAccountDetails(XrmContext.AccountSet.FirstOrDefault(a => a.AccountId == contactAccount.AccountId));
			}

			if (SelfInformation.Visible)
			{
				var contact = XrmContext.MergeClone(Contact);

				SetContactDetails(contact);

				XrmContext.UpdateObject(contact);

				ManageLists(XrmContext, contact);

				XrmContext.SaveChanges();
			}

			var snippet = RegistrationPanel.FindControl("ProfileUpdatedMsg");
			if (snippet != null)
			{
				snippet.Visible = true;
			}

		}

		private void SaveAccountDetails(Account account)
		{
			if (account == null) return;

			account.Name = AccountName.Text;
			account.WebSiteURL = AccountWebsite.Text;
			account.Address1_Line1 = AccountAddress1.Text;
			account.Address1_Line2 = AccountAddress2.Text;
			account.Address1_Line3 = AccountAddress3.Text;
			account.Address1_City = AccountCity.Text;
			account.Address1_StateOrProvince = AccountProvince.Text;
			account.Address1_PostalCode = AccountPostalCode.Text;
			account.Address1_Country = AccountCountry.Text;

			XrmContext.UpdateObject(account);
			XrmContext.SaveChanges();
		}

		public Contact SetContactDetails(Contact contact)
		{
			int tz;
			var timeZone = int.TryParse(TimeZoneSelection.SelectedValue, out tz) ? tz as int? : null;

			contact.FirstName = FirstName.Text;
			contact.LastName = LastName.Text;
			contact.EMailAddress1 = EmailAddress.Text;
			contact.Telephone1 = PhoneNumber.Text;
			contact.JobTitle = JobTitle.Text;
			contact.Address1_Line1 = ContactAddressLine1.Text;
			contact.Address1_Line2 = ContactAddressLine2.Text;
			contact.Address1_Line3 = ContactAddressLine3.Text;
			contact.Address1_City = ContactCity.Text;
			contact.Address1_StateOrProvince = ContactState.Text;
			contact.Address1_PostalCode = ContactZip.Text;
			contact.Address1_Country = ContactCountry.Text;
			contact.Adx_TimeZone = timeZone;
			contact.DoNotEMail = !marketEmail.Checked;
			contact.DoNotBulkEMail = !marketEmail.Checked;
			contact.DoNotFax = !marketFax.Checked;
			contact.DoNotPhone = !marketPhone.Checked;
			contact.DoNotPostalMail = !marketMail.Checked;

			return contact;
		}

		public void ManageLists(XrmServiceContext context, Contact contact)
		{
			foreach (RepeaterItem item in MarketingList.Items)
			{
				if (item != null)
				{
					RepeaterItem repeaterItem = item;

					var ml = context.ListSet.First(m => m.ListId == new Guid(((HiddenField)(repeaterItem.FindControl("ListID"))).Value));

					var listCheckBox = (CheckBox)item.FindControl("ListCheckbox");

					var contactLists = contact.listcontact_association.ToList();

					var inList = contactLists.Any(list => list.ListId == ml.ListId);

					if (listCheckBox.Checked && !inList)
					{
						context.AddMemberList(ml.ListId.Value, contact.ContactId.Value);
					}
					else if (!listCheckBox.Checked && inList)
					{
						context.RemoveMemberList(ml.ListId.Value, contact.ContactId.Value);
					}
				}
			}
		}
	}
}
