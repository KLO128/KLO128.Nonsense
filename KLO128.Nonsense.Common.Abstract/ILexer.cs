using KLO128.Nonsense.Common.Abstract.Models;

namespace KLO128.Nonsense.Common.Abstract
{
    public interface ILexer<TCodeType> : ILexer where TCodeType : ICodeType
    {
    }

    public interface ILexer
    {
        List<Token> GetTokens(string code);
    }
}
