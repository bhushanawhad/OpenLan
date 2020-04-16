using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using System.Web.Services.Protocols;

namespace Openlan
{
   public class PreCreateIncidentresolution : IPlugin
    {
       IOrganizationService _service;
       StringBuilder log = new StringBuilder();
       public void Execute(IServiceProvider serviceProvider)
       {
           try
           {
               log.Clear();

               IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
               IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

               _service = factory.CreateOrganizationService(context.InitiatingUserId);

               log.AppendLine("CRM Service Created");

               // Check for plugin message
               if (context.MessageName == PluginAttributes.CreateMessage && context.Stage == (int)PluginAttributes.Stage.PreOperation)
               {
                   UpdateCaseResolutionTime(ref context);
               }
           }
           catch (Exception ex)
           {
               throw new InvalidPluginExecutionException(string.Format("Message: {0} Log:{1}", ex.Message, log.ToString()));
           }
       }

       private void UpdateCaseResolutionTime(ref IPluginExecutionContext context)
       {
           try
           {
               log.AppendLine("UpdateCaseResolutionTime");

               if (context.InputParameters.Contains(PluginAttributes.Target) && context.InputParameters[PluginAttributes.Target] is Entity)
               {
                   Entity incidentResolution = context.InputParameters[PluginAttributes.Target] as Entity;

                   log.AppendLine("Got Incident Resolution");

                   if (incidentResolution.LogicalName != IncidentResolution.IncidentResolutionLogicalName)
                   {
                       return;
                   }

                   if (incidentResolution.Contains(IncidentResolution.IncidentId))
                   {
                       EntityReference incidentRef = incidentResolution.GetAttributeValue<EntityReference>(IncidentResolution.IncidentId);

                       log.AppendLine("Got IncidentReference");

                       int timeSpent = 0; int actualDuration = 0;
                       if (incidentResolution.Contains(IncidentResolution.Timespent))
                       {
                           timeSpent = incidentResolution.GetAttributeValue<Int32>(IncidentResolution.Timespent);

                           log.AppendLine("Time Spent :" + timeSpent);
                       }

                       if (incidentResolution.Contains(IncidentResolution.ActualDurationMinutes))
                       {
                           actualDuration = incidentResolution.GetAttributeValue<Int32>(IncidentResolution.ActualDurationMinutes);

                           log.AppendLine("Actual Duration :" + actualDuration);
                       }

                       Entity incident = new Entity { Id = incidentRef.Id, LogicalName = incidentRef.LogicalName };//_service.Retrieve(Incident.IncidentLogicalName, incidentRef.Id, new ColumnSet(new string[] { Incident.ResolveByKpiId, Incident.ResolvedBy }));
                       
                       log.AppendLine("Update Incident with Resolution time.");

                       Decimal duration = Convert.ToDecimal(timeSpent);

                       incident[Incident.Timespent] = duration/60;

                       _service.Update(incident);
                       
                       log.AppendLine("Incident Updated.");
                   }
               }
           }
           catch (SoapException ex)
           {
               throw new InvalidPluginExecutionException(string.Format("Message: {0} Log:{1}", ex.InnerException.Message, log.ToString()));
           }
           catch (Exception ex)
           {
               throw new InvalidPluginExecutionException(string.Format("Message: {0} Log:{1}", ex.Message, log.ToString()));
           }

       }
    }
}
