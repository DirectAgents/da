using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LendingTreeLib
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class QueryStringParameterAttribute : Attribute
    {
        public QueryStringParameterAttribute()
        {
        }

        public QueryStringParameterAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public QueryStringParameterAttribute(string parameterName, string defaultValue)
        {
            ParameterName = parameterName;
            DefaultValue = defaultValue;
        }

        public string ParameterName { get; set; }
        public string DefaultValue { get; set; }
    }
}
