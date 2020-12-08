using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Rappen.XTB.CAT;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Rappen.XTB.CAPIT
{
    class CAPITTool : ICATTool
    {
        public string Name => $"{Target} Tester";

        public string Target => "Custom API";

        public string MessageIdentifierColumn => Customapi.UniqueName;

        public string ParameterIdentifierColumn => Customapirequestparameter.UniqueName;

        public Bitmap Logo16 => Properties.Resources.CAPIT_logo_16;

        public Bitmap Logo24 => Properties.Resources.CAPIT_logo_24;

        public Icon Icon16 => Properties.Resources.CAPIT_icon;

        public Image LogoAbout => Properties.Resources.CAPIT_about;

        public QueryExpression GetActionQuery(Guid solutionid)
        {
            var qx = new QueryExpression(Customapi.EntityName);
            qx.ColumnSet.AddColumns(
                Customapi.UniqueName,
                Customapi.PrimaryName,
                Customapi.DisplayName,
                Customapi.Description,
                Customapi.CreatedBy,
                Customapi.Isfunction,
                Customapi.IsPrivate,
                Customapi.ExecuteprivilegeName,
                Customapi.AllowedcustomProcessingStepType,
                Customapi.BoundEntityLogicalName,
                Customapi.BindingType);
            qx.AddOrder(Customapi.PrimaryName, OrderType.Ascending);
            if (!solutionid.Equals(Guid.Empty))
            {
                var solcomp = qx.AddLink(Solutioncomponent.EntityName, Customapi.PrimaryKey, Solutioncomponent.ObjectId);
                solcomp.LinkCriteria.AddCondition(Solutioncomponent.SolutionId, ConditionOperator.Equal, solutionid);
            }
            return qx;
        }

        public QueryExpression GetInputQuery(Guid actionid)
        {
            var qx = new QueryExpression(Customapirequestparameter.EntityName);
            qx.ColumnSet.AddColumns(
                Customapirequestparameter.UniqueName,
                Customapirequestparameter.PrimaryName,
                Customapirequestparameter.DisplayName,
                Customapirequestparameter.Description,
                Customapirequestparameter.Isoptional,
                Customapirequestparameter.Type,
                Customapirequestparameter.LogicalEntityName);
            qx.AddOrder(Customapirequestparameter.Isoptional, OrderType.Ascending);
            qx.AddOrder(Customapirequestparameter.PrimaryName, OrderType.Ascending);
            qx.Criteria.AddCondition(Customapirequestparameter.CustomapiId, ConditionOperator.Equal, actionid);
            return qx;
        }

        public QueryExpression GetOutputQuery(Guid actionid)
        {
            var qx = new QueryExpression(Customapiresponseproperty.EntityName);
            qx.ColumnSet.AddColumns(
                Customapiresponseproperty.UniqueName,
                Customapiresponseproperty.PrimaryName,
                Customapiresponseproperty.DisplayName,
                Customapiresponseproperty.Description,
                Customapiresponseproperty.Type,
                Customapiresponseproperty.LogicalEntityName);
            qx.AddOrder(Customapiresponseproperty.PrimaryName, OrderType.Ascending);
            qx.Criteria.AddCondition(Customapirequestparameter.CustomapiId, ConditionOperator.Equal, actionid);
            return qx;
        }

        public void PreProcessParams(EntityCollection records, IEnumerable<EntityMetadataProxy> entities)
        {
            records.Entities.Where(e => e.Contains(Customapiresponseproperty.LogicalEntityName)).ToList().ForEach(e => e["entity"] =
                entities.FirstOrDefault(em => em.Metadata.LogicalName == e[Customapiresponseproperty.LogicalEntityName].ToString()));
        }
    }
}
