using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Rappen.XTB.CAT
{
    class CATTool : ICATTool
    {
        public string Name => $"{Target} Tester";

        public string Target => "Custom Action";

        public Bitmap Logo16 => Properties.Resources.CAT_logo_16;

        public Bitmap Logo24 => Properties.Resources.CAT_logo_24;

        public Icon Icon16 => Properties.Resources.CAT_icon;

        public Image LogoAbout => Properties.Resources.CAT_about;

        public Columns Columns => new Columns
        {
            APIName = "name",
            APIUniqueName = "uniquename",
            APIMessageName = "M.name",
            ParamName = "name",
            ParamUniqueName = "name"
        };

        public void AddSolutionFilter(QueryExpression qx)
        {
            var solcomp = qx.AddLink(Solutioncomponent.EntityName, Solutioncomponent.SolutionId, Solution.PrimaryKey);
            solcomp.Columns.AddColumns(Solutioncomponent.ComponentType);
            var wf = solcomp.AddLink(Workflow.EntityName, Solutioncomponent.ObjectId, Workflow.PrimaryKey);
            wf.LinkCriteria.AddCondition(Workflow.Category, ConditionOperator.Equal, (int)Workflow.Category_OptionSet.Action);
            wf.LinkCriteria.AddCondition(Workflow.Type, ConditionOperator.Equal, (int)Workflow.Type_OptionSet.Definition);
            wf.LinkCriteria.AddCondition(Workflow.ComponentState, ConditionOperator.Equal, (int)Workflow.ComponentState_OptionSet.Published);
            wf.LinkCriteria.AddCondition(Workflow.StatusCode, ConditionOperator.Equal, (int)Workflow.StatusCode_OptionSet.Activated);
        }

        public Customapi.BindingType_OptionSet BindingType(Entity ca)
        {
            return Customapi.BindingType_OptionSet.Global;
        }

        public QueryExpression GetActionQuery(Guid solutionid)
        {
            var qx = new QueryExpression("workflow");
            qx.ColumnSet.AddColumns("name", "uniquename", "createdby", "primaryentity", "scope", "mode", "ismanaged", "iscustomizable", "istransacted", "iscustomprocessingstepallowedforotherpublishers", "inputparameters", "description");
            qx.AddOrder("ismanaged", OrderType.Descending);
            qx.AddOrder("name", OrderType.Ascending);
            qx.Criteria.AddCondition("category", ConditionOperator.Equal, 3);
            qx.Criteria.AddCondition("type", ConditionOperator.Equal, 1);
            qx.Criteria.AddCondition("componentstate", ConditionOperator.Equal, 0);
            qx.Criteria.AddCondition("statuscode", ConditionOperator.Equal, 2);
            var qxsdk = qx.AddLink("sdkmessage", "sdkmessageid", "sdkmessageid", JoinOperator.LeftOuter);
            qxsdk.EntityAlias = "M";
            qxsdk.Columns.AddColumns("name", "workflowsdkstepenabled");
            if (!solutionid.Equals(Guid.Empty))
            {
                var solcomp = qx.AddLink("solutioncomponent", "workflowid", "objectid");
                solcomp.LinkCriteria.AddCondition("solutionid", ConditionOperator.Equal, solutionid);
            }
            return qx;
        }

        public QueryExpression GetInputQuery(Guid actionid)
        {
            var qx = new QueryExpression("sdkmessagerequestfield");
            qx.Distinct = true;
            qx.ColumnSet.AddColumns("name", "position", "parameterbindinginformation", "optional", "parser", "fieldmask");
            qx.AddOrder("position", OrderType.Ascending);
            var req = qx.AddLink("sdkmessagerequest", "sdkmessagerequestid", "sdkmessagerequestid");
            var pair = req.AddLink("sdkmessagepair", "sdkmessagepairid", "sdkmessagepairid");
            var msg = pair.AddLink("sdkmessage", "sdkmessageid", "sdkmessageid");
            var wf = msg.AddLink("workflow", "sdkmessageid", "sdkmessageid");
            wf.LinkCriteria.AddCondition("workflowid", ConditionOperator.Equal, actionid);
            return qx;
        }

        public QueryExpression GetOutputQuery(Guid actionid)
        {
            var qx = new QueryExpression("sdkmessageresponsefield");
            qx.Distinct = true;
            qx.ColumnSet.AddColumns("name", "position", "parameterbindinginformation", "formatter", "publicname");
            qx.AddOrder("position", OrderType.Ascending);
            var resp = qx.AddLink("sdkmessageresponse", "sdkmessageresponseid", "sdkmessageresponseid");
            var req = resp.AddLink("sdkmessagerequest", "sdkmessagerequestid", "sdkmessagerequestid");
            var pair = req.AddLink("sdkmessagepair", "sdkmessagepairid", "sdkmessagepairid");
            var msg = pair.AddLink("sdkmessage", "sdkmessageid", "sdkmessageid");
            var wf = msg.AddLink("workflow", "sdkmessageid", "sdkmessageid");
            wf.LinkCriteria.AddCondition("workflowid", ConditionOperator.Equal, actionid);
            return qx;
        }

        public void PreProcessParams(EntityCollection records, IEnumerable<EntityMetadataProxy> entities)
        {
            foreach (var record in records.Entities.Where(e => !e.Contains("type")))
            {
                var attribute = record.Contains("parser") ? "parser" : "formatter";
                if (record.TryGetAttributeValue(attribute, out string parser))
                {
                    parser = parser.Split(',')[0];
                    while (parser.Contains("."))
                    {
                        parser = parser.Substring(parser.IndexOf('.') + 1);
                    }
                    record["type"] = parser;
                }
            }
            var otcrecords = records.Entities.Where(r => r.Contains("parameterbindinginformation"));
            if (otcrecords.Count() > 0)
            {
                var siblingrecords = new List<Entity>();
                foreach (var otcrecord in otcrecords)
                {
                    var siblingrecord = records.Entities.FirstOrDefault(r => r["name"].ToString() == otcrecord["name"].ToString() && !r.Contains("parameterbindinginformation"));
                    if (siblingrecord == null)
                    {
                        continue;
                    }
                    var binding = otcrecord["parameterbindinginformation"].ToString();
                    var otcstr = binding.Replace("OTC:", "").Trim();
                    if (int.TryParse(otcstr, out int otc))
                    {
                        if (entities.FirstOrDefault(e => e.Metadata.ObjectTypeCode == otc) is EntityMetadataProxy meta)
                        {
                            otcrecord["entity"] = meta;
                        }
                    }
                    siblingrecords.Add(siblingrecord);
                }
                siblingrecords.ForEach(s => records.Entities.Remove(s));
            }
            records.Entities.Where(e => !e.Contains("isoptional") && e.Contains("optional")).ToList().ForEach(e => e["isoptional"] = e["optional"]);
        }
    }
}
