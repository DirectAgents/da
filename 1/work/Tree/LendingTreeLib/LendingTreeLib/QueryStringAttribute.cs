using System;

namespace LendingTreeLib
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class QueryStringAttribute : Attribute
    {
        public QueryStringAttribute()
        {
        }

        public QueryStringAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public QueryStringAttribute(string parameterName, string defaultValue)
        {
            ParameterName = parameterName;
            DefaultValue = defaultValue;
        }

        public string ParameterName { get; set; }
        public string DefaultValue { get; set; }
    }
}
