using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ParamType = Rappen.XTB.CAT.Customapirequestparameter.Type_OptionSet;

namespace Rappen.XTB.CAT
{
    public class CATRequest
    {
        private OrganizationRequest request;
        private OrganizationResponse response;

        public ExecutionInfo Execution { get; set; }
        public string Name { get; set; }
        public EntityReference Target { get; set; }
        public List<CATParameter> Parameters { get; set; }
        public List<CATParameter> Responses { get; set; }

        internal CATRequest()
        { }

        public CATRequest(OrganizationRequest request)
        {
            Execution = new ExecutionInfo();
            Request = request;
        }

        public override string ToString() => Execution?.RunTime.ToString("HH:mm:ss") + " " + Name;

        [XmlIgnore]
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
            private set
            {
                request = value;
                Name = request.RequestName;
                Parameters = new List<CATParameter>();
                if (request.Parameters != null)
                {
                    if (request.Parameters.ContainsKey("Target"))
                    {
                        Target = request["Target"] as EntityReference;
                    }
                    request.Parameters.Where(p => p.Key != "Target" && p.Value != null).ToList().ForEach(p =>
                            Parameters.Add(new CATParameter
                            {
                                Name = p.Key,
                                Type = ValueToParamType(p.Value) ?? ParamType.String,
                                Value = ValueToString(p.Value)
                            }));
                }
            }
        }

        [XmlIgnore]
        public OrganizationResponse Response
        {
            private get
            {
                return response;
            }
            set
            {
                response = value;
                Responses = new List<CATParameter>();
                response.Results.Where(p => p.Value != null).ToList().ForEach(p =>
                       Responses.Add(new CATParameter
                       {
                           Name = p.Key,
                           Type = ValueToParamType(p.Value) ?? ParamType.String,
                           Value = ValueToString(p.Value)
                       }));
            }
        }

        public static ParamType? ValueToParamType(object value)
        {
            if (value == null) return null;
            if (value is string) return ParamType.String;
            if (value is bool) return ParamType.Boolean;
            if (value is DateTime) return ParamType.DateTime;
            if (value is decimal) return ParamType.Decimal;
            if (value is double) return ParamType.Decimal;
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
            if (value is string[] strings) return $"[\n  \"" + string.Join($"\",\n  \"", strings) + "\"\n]";
            if (value is Money valuemon) return valuemon.Value.ToString();
            if (value is OptionSetValue valueosv) return valueosv.Value.ToString();
            if (value is Entity valueent) return $"{valueent.LogicalName}:{valueent.Id}";
            if (value is EntityReference valueer) return $"{valueer.LogicalName}:{valueer.Id}";
            return value?.ToString();
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
        public string Environment { get; set; }
        public string Solution { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }

    public class CATParameter
    {
        public ParamType Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Name} = {Value}";
        }
    }
}