using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LendingTreeLib.Common
{
    public class Utf8StringWriter : StringWriter
    {
        public Utf8StringWriter()
        {
        }

        public Utf8StringWriter(StringBuilder sb) : base()
        {
        }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}
