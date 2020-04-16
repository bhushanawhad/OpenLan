using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRenewal
{
    public static class CrmFields
    {
        //Declare Service fields
        public static readonly string ServiceLogicalName = "contract";
        public static readonly string Code = "openlan_code";
        public static readonly string Quantity = "openlan_quantity";
        public static readonly string StartDate = "activeon";
        public static readonly string EndDate = "expireson";
        public static readonly string CustomerLookUp = "customerid";
        public static readonly string Duration = "billingfrequencycode";
        public static readonly string Recurring = "openlan_recurring";
        public static readonly string UpdateContractLine = "openlan_updatecontractline";
        public static readonly string RenewDate = "openlan_renewdate";
        public static readonly string FirstReminderDate = "openlan_firstreminderdate";
        public static readonly string SecondReminderDate = "openlan_secondreminderdate";
        public static readonly string FirstReminderDuration = "openlan_firstreminderduration";
        public static readonly string SecondReminderDuration = "openlan_secondreminderduration";

        //Declare Contact Fields
        public static readonly string ContactLogicalName = "contact";

        //Declare Common fields
        public static readonly string StateCode = "statecode";
        public static readonly string CreateMessage = "Create";
        public static readonly string UpdateMessage = "Update";
        public static readonly string RunOnDemandMessage = "ExecuteWorkflow";

        public enum StateCodeValue
        {
            Active = 0,
            Inactive = 1
        }

        public enum DurationValues
        {
            Monthly = 1,
            Bimonthly = 2,
            Quarterly = 3,
            Semiannually = 4,
            Annually = 5,
        }
    }
}
