using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk.Client;

namespace Site.Library
{
	public class CaseAccess : PortalClass
	{
		public IEnumerable<Entity> CasesForCurrentUser
		{
			get
			{
				var emptyResult = new List<Entity>();

				var currentUser = Contact;

				if (currentUser == null) return emptyResult;

				var access = GetCaseAccessByContact(ServiceContext, currentUser);

				if (access == null || !access.GetAttributeValue<bool?>("adx_read").GetValueOrDefault()) return emptyResult;

				var parentCustomerId = currentUser.GetAttributeValue<Guid?>("parentcustomerid");

				return ((access.GetAttributeValue<int?>("adx_scope") == (int)Enums.Adx_caseaccess.ScopeOption.Account) && (parentCustomerId != null))
					? GetCasesByCustomer(ServiceContext, parentCustomerId)
					: GetCasesByCustomer(ServiceContext, currentUser.GetAttributeValue<Guid?>("contactid"));
			}
		}

		public Entity GetCaseAccessForCurrentUser()
		{
			return GetCaseAccessByContact(ServiceContext, Contact);
		}

		public Entity GetCaseAccessByContact(OrganizationServiceContext context, Entity contact)
		{
			contact.AssertEntityName("contact");

			if (contact == null)
			{
				return null;
			}

			var currentContact = context.MergeClone(contact);

			if (currentContact == null)
			{
				return null;
			}

			var parentCustomerAccount = currentContact.GetRelatedEntity(context, "contact_customer_accounts");

			IEnumerable<Entity> findCaseAccess;

			if (parentCustomerAccount == null) //contact is not associated with a parent account record
			{
				findCaseAccess =
					from aa in context.CreateQuery("adx_caseaccess").ToList()
					let c = aa.GetRelatedEntity(context, "adx_contact_caseaccess")
					where c != null && c.GetAttributeValue<Guid>("contactid") == contact.GetAttributeValue<Guid>("contactid")
					select aa;
			}
			else
			{
				findCaseAccess =
					 from aa in context.CreateQuery("adx_caseaccess").ToList()
					 let c = aa.GetRelatedEntity(context, "adx_contact_caseaccess")
					 let a = aa.GetRelatedEntity(context, "adx_account_caseaccess")
					 where c != null && c.GetAttributeValue<Guid>("contactid") == contact.GetAttributeValue<Guid>("contactid")
						 && a != null && a.GetAttributeValue<Guid>("accountid") == parentCustomerAccount.GetAttributeValue<Guid>("accountid")
					 select aa;
			}

			return findCaseAccess.FirstOrDefault();
		}

		public IEnumerable<Entity> GetCasesByCustomer(OrganizationServiceContext context, Guid? customerId)
		{
			var result = new List<Entity>();

			var findCases =
				from c in context.CreateQuery("incident")
				where c.GetAttributeValue<Guid?>("customerid") == customerId
				select c;

			result.AddRange(findCases);

			var findAccounts =
				from a in context.CreateQuery("account")
				where a.GetAttributeValue<Guid?>("accountid") == customerId
				select a;

			var account = findAccounts.FirstOrDefault();

			if (account == null) return result;

			var contacts = account.GetRelatedEntities(context, "contact_customer_accounts");

			foreach (var contact in contacts)
			{
				result.AddRange(GetCasesByCustomer(context, contact.GetAttributeValue<Guid?>("contactid")));
			}

			return result;
		}
	}
}
