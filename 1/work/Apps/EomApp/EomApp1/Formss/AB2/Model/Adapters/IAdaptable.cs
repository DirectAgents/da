using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Formss.AB2.Model.Adapters
{
    public interface IAdaptable<TFrom>
    {
        TFrom From { get; }
        object Adapted { get; }
    }
}
