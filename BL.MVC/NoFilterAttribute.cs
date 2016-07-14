using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NoFilterAttribute:Attribute
    {
        public NoFilterAttribute() { }
    }
}
