using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Openlan
{
    public class CreateCaseLink : IPlugin
    {
        IOrganizationService _service;
        int Active = 0;
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            _service = factory.CreateOrganizationService(null);

            // Check for plugin message
            if (context.MessageName == PluginAttributes.CreateMessage && context.Stage == (int)PluginAttributes.Stage.PreOperation)
            {
                if (context.InputParameters.Contains(PluginAttributes.Target) && context.InputParameters[PluginAttributes.Target] is Entity)
                {
                    string caseLink = string.Empty;
                    string caseId = string.Empty;

                    Entity caseEntity = context.InputParameters[PluginAttributes.Target] as Entity;
                    caseId = caseEntity.Id.ToString();

                    QueryExpression query = new QueryExpression(Portal.PortalLogicalName);
                    query.ColumnSet = new ColumnSet(new string[] { Portal.PortalUrl });
                    query.Criteria.AddCondition(Portal.StateCode, ConditionOperator.Equal, Active);
                    EntityCollection coll = _service.RetrieveMultiple(query);
                    if (coll.Entities.Count > 0)
                    {
                        Entity website = coll.Entities[0];
                        if (website.Contains(Portal.PortalUrl))
                        {
                            caseLink = website[Portal.PortalUrl].ToString();
                        }
                    }

                    caseLink = caseLink + "/cases/edit?CaseID=";
                    
                    caseId = caseId.Replace("{",""); caseId = caseId.Replace("}","");
                    
                    caseLink = caseLink + caseId;
                    //caseLink = "<a href=" + caseLink + ">";
                    caseLink = "<a href=" + caseLink + ">Case Link</a>";

                    caseEntity.Attributes.Add(Incident.CaseUrl, caseLink);
                }
            }
        }
    }
}
