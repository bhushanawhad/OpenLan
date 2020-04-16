using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Data.SqlClient;

namespace MarkCaseAsBilled
{
   public class BillCaseActivity : CodeActivity
    {
       string IncidentLogicalName = "incident";

        StringBuilder log = new StringBuilder();
        string firedEntityLogicalName = string.Empty;
        string messageName = string.Empty;
       protected override void Execute(CodeActivityContext context)
       {
           try
           {
               log = new StringBuilder(string.Empty);

               IWorkflowContext workFlowContext = context.GetExtension<IWorkflowContext>();

               if(workFlowContext.PrimaryEntityName != IncidentLogicalName)
               {
                   log.AppendLine("Primary Entity is not Incident :" + workFlowContext.PrimaryEntityName);
                   return;
               }

               Guid incidentId = workFlowContext.PrimaryEntityId;

               log.AppendLine("Got the case id :"+incidentId.ToString());

               Boolean isBilled = this.Billed.Get(context);

               int istrue = isBilled == true ? 1 : 0;

               string connectionString = GetConnectionString(@"OPENLAN-DYNAMIC\MSSQLSERVER2014", "OpenlanCrm_MSCRM", "bhushan", "bsa1319");

               log.AppendLine("Connection String :"+ connectionString);
               //Update Case in DB
               SqlCommand cmd = new SqlCommand();

               cmd.CommandText = "Update Incident Set openlan_IsBilled = " + istrue + " Where IncidentId = " + "'" + incidentId.ToString() + "'";

               log.AppendLine("Sql Command Text :" +cmd.CommandText);

               using (SqlConnection sqlConn = new SqlConnection(connectionString))
               {
                   if (cmd != null)
                   {
                       sqlConn.Open();
                       cmd.Connection = sqlConn;
                       cmd.ExecuteNonQuery();
                       sqlConn.Dispose();
                   }
               }
           }
           catch (Exception ex)
           {
               CreateLog(ex.Message);
               throw new InvalidPluginExecutionException(log.ToString());
           }

       }
       [Input("Billed Status")]
       [RequiredArgument]
       public InArgument<Boolean> Billed { get; set; }

       public static string GetConnectionString(string serverName, string dataBasename, string userName, string password)
       {
           string connectionString = string.Empty;

           connectionString = "Server=" + serverName + ";Database=" + dataBasename + ";User Id=" + userName + ";Password=" + password + ";"; //"Data Source=ServerName;Initial Catalog=DatabaseName;User ID=UserName;Password=Password" ;
           
           return connectionString;
       }
       public void CreateLog(string message)
       {
           log.AppendLine(message);
       }
    }
}
