using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Crm.Sdk.Messages;


namespace ServiceRenewal
{
  public  class RenewContract : CodeActivity
    {
        IOrganizationService _crmService;

        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext workFlowContext = context.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory factory = context.GetExtension<IOrganizationServiceFactory>();
            _crmService = factory.CreateOrganizationService(null);

            Guid contractId = workFlowContext.PrimaryEntityId;

            RenewContractRequest contractReq = new RenewContractRequest();
            contractReq.ContractId = contractId;
            contractReq.IncludeCanceledLines = false;
            contractReq.Status = 1;

            RenewContractResponse contractResp = (RenewContractResponse)_crmService.Execute(contractReq);
            if (contractResp != null)
            {
                Entity contract = contractResp.Entity;
                if (contract.Contains(CrmFields.UpdateContractLine))
                {
                    Boolean updateContractAfterRenewal = contract.GetAttributeValue<bool>(CrmFields.UpdateContractLine);
                    if(updateContractAfterRenewal != true)
                    {
                        SetStateRequest req = new SetStateRequest();
                        req.EntityMoniker = contract.ToEntityReference();
                        req.State = new OptionSetValue(1);
                        req.Status = new OptionSetValue(2);
                        _crmService.Execute(req);
                    }
                }
               
            }

        }
    }
}
