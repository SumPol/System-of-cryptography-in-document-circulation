using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsApplication
{
    class MyClass
    {
        public static string sValue
        {
            get { return svalue; }
            set { svalue = value; if (SomeEvent != null) SomeEvent(null, EventArgs.Empty); }
        }

        static string svalue;

        public static event EventHandler SomeEvent;
    }
}
