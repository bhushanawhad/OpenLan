using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openlan
{
    public static class Incident
    {
        //Declare Incident fields
        public static readonly string IncidentLogicalName = "incident";
        public static readonly string IncidentPrimaryId = "incidentid";
        public static readonly string CaseNumber = "openlan_casenumber";
        public static readonly string TicketNumber = "ticketnumber";
        public static readonly string Timespent = "openlan_durationbilled";
        public static readonly string CaseUrl = "openlan_caseurl";
        public static readonly string StateCode = "statecode";
        public static readonly string StatusCode = "statuscode";


        public enum ResolveBySLAStatusCode
        {
            InProgress = 1,
            NearingNoncompliance = 2,
            Succeeded = 3,
            Noncompliant = 4
        }
        public enum StatusCodeValue
        {
            InProgress = 1,
            OnHold = 2,
            ProblemSolved = 5
        }

        public enum CaseTypeValue
        {
            ServiceRequest = 1,
            Problem = 2,
            MaintenanceTask = 100000000,
            OpportunityTask = 100000001,
            Alerts = 100000002,
            Incident = 100000003
        }
    }
}
