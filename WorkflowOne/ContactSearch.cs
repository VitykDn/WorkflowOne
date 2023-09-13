using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Query;

namespace WorkflowOne
{
    public class ContactSearch : CodeActivity
    {
        [Input("Parameter 1")]
        public InArgument<string> Parameter1 { get; set; }

        [Input("Field Name for Parameter 1")]
        public InArgument<string> FieldName1 { get; set; }

        [Input("Parameter 2")]
        public InArgument<string> Parameter2 { get; set; }

        [Input("Field Name for Parameter 2")]
        public InArgument<string> FieldName2 { get; set; }

        [Output("Status")]
        public OutArgument<int> Status { get; set; }

        [Output("Contact Link")]
        public OutArgument<string> ContactLink { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();

            IOrganizationService service = serviceFactory.CreateOrganizationService(null);

            string parameter1 = Parameter1.Get(context);
            string fieldName1 = FieldName1.Get(context);
            string parameter2 = Parameter2.Get(context);
            string fieldName2 = FieldName2.Get(context);

            QueryExpression query = new QueryExpression("contact");
            query.Criteria.AddCondition(fieldName1, ConditionOperator.Equal, parameter1);
            query.Criteria.AddCondition(fieldName2, ConditionOperator.Equal, parameter2);

            EntityCollection result = service.RetrieveMultiple(query);

            if (result.Entities.Count == 1)
            {
                Status.Set(context, 1);
                ContactLink.Set(context, result.Entities[0].Id.ToString());
            }
            else if (result.Entities.Count == 0)
            {
                Status.Set(context, 2);
            }
            else
            {
                Status.Set(context, 3);
            }
        }
    }
}
