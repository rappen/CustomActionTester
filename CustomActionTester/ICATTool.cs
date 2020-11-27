using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace Rappen.XTB.CAT
{
    public interface ICATTool
    {
        string Name { get; }
        string Target { get; }
        string MessageIdentifierColumn { get; }
        string ParameterIdentifierColumn { get; }
        QueryExpression GetActionQuery(Guid solutionid);
        QueryExpression GetInputQuery(Guid actionid);
        QueryExpression GetOutputQuery(Guid actionid);
        void PreProcessParams(EntityCollection records, IEnumerable<EntityMetadataProxy> entities);
    }
}
