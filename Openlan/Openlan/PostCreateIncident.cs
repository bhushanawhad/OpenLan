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
   public class PostCreateIncident : IPlugin
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
                if (context.MessageName == PluginAttributes.CreateMessage && context.Stage == (int)PluginAttributes.Stage.PostOperation)
                {
                    if (context.InputParameters.Contains(PluginAttributes.Target) && context.InputParameters[PluginAttributes.Target] is Entity)
                    {
                        Entity incident = _service.Retrieve(Incident.IncidentLogicalName, context.PrimaryEntityId, new ColumnSet(new string[] { Incident.TicketNumber }));

                        log.AppendLine("Got Incident");

                        if (incident.LogicalName != Incident.IncidentLogicalName)
                        {
                            return;
                        }

                        if (incident.Contains(Incident.TicketNumber))
                        {
                            String ticketNumber = incident.GetAttributeValue<string>(Incident.TicketNumber);

                            String[] splitArray = ticketNumber.Split('-');

                            incident[Incident.CaseNumber] = splitArray[1];

                            _service.Update(incident);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(string.Format("Message: {0} Log:{1}", ex.Message, log.ToString()));
            }
        }
    }
}
