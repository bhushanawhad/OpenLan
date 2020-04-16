using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace ServiceRenewal
{
   public class ServiceRenewaDate : CodeActivity
    {
       IOrganizationService _crmService;
        StringBuilder log = new StringBuilder();
        Guid serviceEntityId ;
        string messageName = string.Empty;

       protected override void Execute(CodeActivityContext context)
       {
           int duration = 0;
           int firstReminderDuration = 0;
           int secondReminderDuration = 0;
           bool isRecurring = false;

           string startDatevalue = string.Empty;
           string endDatevalue = string.Empty;
           string renewalDatevalue = string.Empty;
           string firstReminderDatevalue = string.Empty;
           string secondReminderDatevalue = string.Empty;

           DateTime endDate;
           DateTime renewalDate;
           DateTime firstReminderDate;
           DateTime secondReminderDate;

           log = new StringBuilder(string.Empty);

           IWorkflowContext workFlowContext = context.GetExtension<IWorkflowContext>();
           IOrganizationServiceFactory factory = context.GetExtension<IOrganizationServiceFactory>();
           _crmService = factory.CreateOrganizationService(null);

           serviceEntityId = workFlowContext.PrimaryEntityId;

           Entity serviceEntity = _crmService.Retrieve(CrmFields.ServiceLogicalName, serviceEntityId, new ColumnSet(true));

           if (serviceEntity.Contains(CrmFields.Recurring))
           {
               isRecurring = serviceEntity.GetAttributeValue<Boolean>(CrmFields.Recurring);
           }

           if (serviceEntity.Contains(CrmFields.Duration))
           {
               duration = serviceEntity.GetAttributeValue<OptionSetValue>(CrmFields.Duration).Value;
           }

           if (serviceEntity.Contains(CrmFields.FirstReminderDuration))
           {
               firstReminderDuration = serviceEntity.GetAttributeValue<Int32>(CrmFields.FirstReminderDuration);
           }

           if (serviceEntity.Contains(CrmFields.SecondReminderDuration))
           {
               secondReminderDuration = serviceEntity.GetAttributeValue<Int32>(CrmFields.SecondReminderDuration);
           }

           if (serviceEntity.Contains(CrmFields.StartDate))
           {
               startDatevalue = serviceEntity.GetAttributeValue<DateTime>(CrmFields.StartDate).ToString();
           }
           if (serviceEntity.Contains(CrmFields.EndDate))
           {
               endDatevalue = serviceEntity.GetAttributeValue<DateTime>(CrmFields.EndDate).ToString();
           }

           if (serviceEntity.Contains(CrmFields.RenewDate))
           {
               renewalDatevalue = serviceEntity.GetAttributeValue<DateTime>(CrmFields.RenewDate).ToString();
           }

           //if (serviceEntity.Contains(CrmFields.FirstReminderDate))
           //{
           //    firstReminderDatevalue = serviceEntity.GetAttributeValue<DateTime>(CrmFields.FirstReminderDate).ToString();
           //}

           //if (serviceEntity.Contains(CrmFields.SecondReminderDate))
           //{
           //    secondReminderDatevalue = serviceEntity.GetAttributeValue<DateTime>(CrmFields.SecondReminderDate).ToString();
           //}

           //Set renewal date 
           //if (!string.IsNullOrEmpty(startDatevalue))
           //{
           //    switch (duration)
           //    {
           //        case  (int)CrmFields.DurationValues.Monthly :
           //            renewalDate = DateTime.Parse(startDatevalue).AddMonths(1);
           //            break;

           //        case (int)CrmFields.DurationValues.Bimonthly:
           //            renewalDate = DateTime.Parse(startDatevalue).AddMonths(2);
           //            break;

           //        case (int)CrmFields.DurationValues.Quarterly:
           //            renewalDate = DateTime.Parse(startDatevalue).AddMonths(3);
           //            break;

           //        case (int)CrmFields.DurationValues.Semiannually:
           //            renewalDate = DateTime.Parse(startDatevalue).AddMonths(6);
           //            break;

           //        case (int)CrmFields.DurationValues.Annually:
           //            renewalDate = DateTime.Parse(startDatevalue).AddMonths(12);
           //            break;
           //    }
           //}
           //else
           //{
           //    renewalDate = DateTime.Parse(renewalDatevalue).AddMonths(duration);
           //    //renewalDate = DateTime.Parse(renewalDatevalue).AddHours(duration);
           //}
           ////throw new NotImplementedException();

           //Set First Reminder Date
           endDate = DateTime.Parse(endDatevalue);
           if (firstReminderDuration != 0)
           {
               firstReminderDate = endDate.AddDays(-(firstReminderDuration));
               //firstReminderDate = renewalDate.AddMinutes(-(firstReminderDuration));
               serviceEntity[CrmFields.FirstReminderDate] = firstReminderDate;
               //FirstReminderDate.Set(context, firstReminderDate);
           }
           //Set Second Reminder Date
           if (secondReminderDuration != 0)
           {
               secondReminderDate = endDate.AddDays(-(secondReminderDuration));
               //secondReminderDate = renewalDate.AddMinutes(-(secondReminderDuration));
               serviceEntity[CrmFields.SecondReminderDate] = secondReminderDate;
               //SecondReminderDate.Set(context, secondReminderDate);
           }
           //serviceEntity[CrmFields.RenewDate] = renewalDate;

           //this.RenewalDate.Set(context, renewalDate);
         
           _crmService.Update(serviceEntity);
           
       }

       //[Output("Renewal Date output")]
       //public InOutArgument<DateTime> RenewalDate { get; set; }

       //[Output("First Reminder Date output")]
       //public InOutArgument<DateTime> FirstReminderDate { get; set; }

       //[Output("Second Reminder Date output")]
       //public InOutArgument<DateTime> SecondReminderDate { get; set; }
    }
}
