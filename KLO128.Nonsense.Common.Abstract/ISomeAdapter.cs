using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLO128.Nonsense.Common.Abstract.Models;

namespace KLO128.Nonsense.Common.Abstract
{
    public interface ISomeAdapter
    {
        public List<Token> LoadTokens(string code);
    }
}
