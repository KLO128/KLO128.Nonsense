using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLO128.Nonsense.Common.Abstract;
using KLO128.Nonsense.Common.Abstract.Models;
using KLO128.Nonsense.Common.Models;

namespace KLO128.Nonsense.Common
{
    public abstract class BaseSomeAdapter : ISomeAdapter
    {
        //protected Dictionary<string List<Token>> TokensToSkip { get; }

        private ILexer Lexer { get; }

        internal CSharpCodeType? CodeTypeProp { get; }

        protected BaseSomeAdapter(ILexer lexer/*, Dictionary<string List<Token>> tokensToSkip*/)
        {
            Lexer = lexer;
            // TokensToSkip = tokensToSkip;
            CodeTypeProp = new CSharpCodeType("something");
        }

        public virtual List<Token> LoadTokens(string code)
        {
            if (CodeTypeProp == null)
            {
                return new List<Token>();
            }

            return Lexer.GetTokens(code);
        }
    }
}
