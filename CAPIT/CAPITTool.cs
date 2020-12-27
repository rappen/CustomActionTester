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

        public Bitmap Logo16 => Properties.Resources.CAPIT_logo_16;

        public Bitmap Logo24 => Properties.Resources.CAPIT_logo_24;

        public Icon Icon16 => Properties.Resources.CAPIT_icon;

        public Image LogoAbout => Properties.Resources.CAPIT_about;

        public Columns Columns => new Columns
        {
            APIName = Customapi.PrimaryName,
            APIUniqueName = Customapi.UniqueName,
            APIMessageName = Customapi.UniqueName,
            APIScope = Customapi.BindingType,
            APIBoundEntity = Customapi.BoundEntityLogicalName,
            ParamName = Customapirequestparameter.PrimaryName,
            ParamUniqueName = Customapirequestparameter.UniqueName
        };

        public void AddSolutionFilter(QueryExpression qx)
        {
            var solcomp = qx.AddLink(Solutioncomponent.EntityName, Solutioncomponent.SolutionId, Solution.PrimaryKey);
            var custapi = solcomp.AddLink(Customapi.EntityName, Solutioncomponent.ObjectId, Customapi.PrimaryKey);
            custapi.LinkCriteria.AddCondition(Customapi.StatusCode, ConditionOperator.Equal, (int)Customapi.StatusCode_OptionSet.Active);
        }

        public Customapi.BindingType_OptionSet BindingType(Entity ca)
        {
            if (!ca.TryGetAttributeValue(Customapi.BindingType, out OptionSetValue type))
            {
                return Customapi.BindingType_OptionSet.Global;
            }
            return (Customapi.BindingType_OptionSet)type.Value;
        }

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

        public string GetActionUrlPath(Guid actionid) => $"/main.aspx?pagetype=entityrecord&etn=customapi&id={actionid}";
        
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
