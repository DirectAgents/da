using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Formss.AB2.Model.Adapters
{
    public interface IMapper
    {
        void MapTo<TTo>(TTo to);
    }
}
