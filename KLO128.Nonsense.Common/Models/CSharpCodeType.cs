using KLO128.Nonsense.Common.Abstract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLO128.Nonsense.Common.Models
{
    internal class CSharpCodeType : ICodeType
    {
        private string SomePrivateProperty { get; set; } = string.Empty;

        public CSharpCodeType(string somePrivateProperty)
        {
            SomePrivateProperty = somePrivateProperty;
        }

    }
}
