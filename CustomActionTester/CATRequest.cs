using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using ParamType = Rappen.XTB.CAT.Customapirequestparameter.Type_OptionSet;

namespace Rappen.XTB.CAT
{
    public class CATRequest
    {
        public ExecutionInfo Execution { get; set; }
        public string Name { get; set; }
        public EntityReference Target { get; set; }
        public List<CATRequestParameter> Parameters { get; set; }

        internal CATRequest()
        { }

        public CATRequest(OrganizationRequest request)
        {
            Execution = new ExecutionInfo();
            Name = request.RequestName;
            if (request.Parameters != null)
            {
                if (request.Parameters.ContainsKey("Target"))
                {
                    Target = request["Target"] as EntityReference;
                }
                foreach (var parameter in request.Parameters)
                {
                    if (parameter.Key != "Target")
                    {
                        if (Parameters == null)
                        {
                            Parameters = new List<CATRequestParameter>();
                        }
                        Parameters.Add(new CATRequestParameter
                        {
                            Name = parameter.Key,
                            Type = ValueToParamType(parameter.Value),
                            Value = ValueToString(parameter.Value)
                        });
                    }
                }
            }
        }

        public override string ToString() => Execution?.RunTime.ToString("HH:mm:ss") + " " + Name;

        private ParamType ValueToParamType(object value)
        {
            if (value is string) return ParamType.String;
            if (value is bool) return ParamType.Boolean;
            if (value is DateTime) return ParamType.DateTime;
            if (value is decimal) return ParamType.Decimal;
            if (value is Entity) return ParamType.Entity;
            if (value is EntityCollection) return ParamType.EntityCollection;
            if (value is EntityReference) return ParamType.EntityReference;
            if (value is float) return ParamType.Float;
            if (value is int) return ParamType.Integer;
            if (value is Money) return ParamType.Money;
            if (value is OptionSetValue) return ParamType.Picklist;
            if (value is string) return ParamType.String;
            if (value is string[]) return ParamType.StringArray;
            if (value is Guid) return ParamType.GuId;
            throw new Exception($"Incorrect value type: {value.GetType()} value: {value}");
        }

        public OrganizationRequest Request
        {
            get
            {
                var request = new OrganizationRequest(Name);
                if (Target != null)
                {
                    request["Target"] = Target;
                }
                Parameters.ForEach(p => request.Parameters.Add(p.Name, StringToParamValue(p.Type, p.Value)));
                return request;
            }
        }

        public static ParamType StringToParamType(string type)
        {
            type = type
                .Replace("OptionSetValue", "Picklist")
                .Replace("Int32", "Integer")
                .Replace("Int64", "Integer")
                .Replace("Double", "Float");
            if (Enum.TryParse(type, out ParamType paramtype))
            {
                return paramtype;
            }
            throw new Exception($"Incorrect type: {type}");
        }

        public static string ValueToString(object value)
        {
            if (value is OptionSetValue valueosv) return valueosv.Value.ToString();
            if (value is EntityReference valueer) return $"{valueer.LogicalName}:{valueer.Id}";
            else return value.ToString();
        }

        public static object StringToParamValue(ParamType type, string value)
        {
            switch (type)
            {
                case ParamType.String:
                    return value;

                case ParamType.Integer:
                    return Convert.ToInt64(value);

                case ParamType.Float:
                    return Convert.ToSingle(value);

                case ParamType.Decimal:
                    return Convert.ToDecimal(value);

                case ParamType.Money:
                    return new Money(Convert.ToDecimal(value));

                case ParamType.GuId:
                    return Guid.Parse(value);

                case ParamType.DateTime:
                    return Convert.ToDateTime(value);

                case ParamType.Picklist:
                    return new OptionSetValue(Convert.ToInt16(value));

                case ParamType.EntityReference:
                    var entrefstr = value.Split(':');
                    return new EntityReference(entrefstr[0], Guid.Parse(entrefstr[1]));

                case ParamType.Entity:
                case ParamType.EntityCollection:
                case ParamType.StringArray:
                    throw new Exception($"{type} not supported");

                default:
                    throw new Exception($"Unknown type: {type}");
            }
        }
    }

    public class ExecutionInfo
    {
        public DateTime RunTime { get; set; } = DateTime.Now;
        public long Duration { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }

    public class CATRequestParameter
    {
        public ParamType Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}