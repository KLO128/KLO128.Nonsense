using System.Text;
using KLO128.Nonsense.Common.Models;
using KLO128.Nonsense.Common.Abstract;
using KLO128.Nonsense.Common.Abstract.Models;

namespace KLO128.Nonsense.Common
{
    public class CSharpLexer : ILexer<CSharpCodeType>
    {
        private static string[] PrimitiveTypeNames { get; } = new string[] { "object", "var", "void", "string", nameof(String), "bool", nameof(Boolean), "byte", nameof(Byte), "sbyte", nameof(SByte), "short", nameof(Int16), "ushort", nameof(UInt16), "int", nameof(Int32), "uint", nameof(UInt32), "long", nameof(Int64), "ulong", nameof(UInt64), "float", nameof(Single), "double", nameof(Double), "decimal", nameof(Decimal), nameof(DateTime), nameof(TimeSpan), nameof(DateTimeOffset) };

        private static string[] ObjectTypes { get; } = new string[] { "class", "interface", "enum", "struct", "record", "delegate" };

        private static string[] Modifiers { get; } = new string[] { "public", "protected", "private", "internal" };

        private static string[] OtherKeyWords { get; } = new string[] { "static", "readonly", "new", "base", "const", /*"value",*/ "get", "set", "true", "false", "null", "default", "case", "this", "continue", "break", "goto", "is", "as", "not", "in", "ref", "out", "return",/*"where",*/ "throw", "abstract", "virtual", "override", "global", "operator", "implicit", "explicit", "event", "params", "async", "await" };

        private static string[] BlockCommands { get; } = new string[] { "do", "while", "for", "switch", "if", "else", "foreach", "using", "try", "catch", "finally" };

        private static string[] NullOperators { get; } = new string[] { "!", "?", "??" };

        private static string[] ExpressionSeparators { get; } = new string[] { ",", ":", "::", ";", "=>", "==", "!=", ">", "<", ">=", "<=", "*", "+", "-", "/", "%", "&&", "||", "&", "|", "^", "=", "+=", "-=", "/=", "*=", "%=", "&=", "|=", "^=", "++", "--", "??=" };

        private static string[] NumberTypes { get; } = new string[] { "byte", nameof(Byte), "sbyte", nameof(SByte), "short", nameof(Int16), "ushort", nameof(UInt16), "int", nameof(Int32), "uint", nameof(UInt32), "long", nameof(Int64), "ulong", nameof(Int64), "float", nameof(Single), "double", nameof(Double), "decimal", nameof(Decimal) };

        private static string[] BoolOperators { get; } = new string[] { "||", "&&", "==", "!=", ">", "<", ">=", "<=" };

        private static string[] LogicalOperators { get; } = new string[] { "&", "|", "^" };

        private static string[] IntegerOperators { get; } = new string[] { "*", "+", "-" };

        private static string[] FloatOperators { get; } = new string[] { "/", "%" };

        private static string[] AssignmentOperators { get; } = new string[] { "=", "+=", "-=", "/=", "*=", "%=", "&=", "|=", "^=", "++", "--" };

        private static char PragmaOperator = '#';

        private int Line
        { get; set; }

        private int Column { get; set; }

        public List<Token> GetTokens(string code)
        {
            var ret = new List<Token>();
            var context = new StringBuilder();
            var isInCharContext = false;
            var DolarStringContext = new Stack<StringContext>();
            var lastChar = default(char);
            var preLastChar = default(char);
            Line = 1;
            Column = 1;

            DolarStringContext.Push(new StringContext
            {
                IsAtSignString = false,
                IsDolarString = false,
                IsInNonDolarStringContext = false
            });

            for (int i = 0; i < code.Length; i++)
            {
                var ch = code[i];

                AdvanceLineColumn(ch);

                if (isInCharContext)
                {
                    context.Append(ch);

                    if (ch == '\'' && (lastChar != '\\' || preLastChar != '\''))
                    {
                        isInCharContext = false;
                        AddToken(ret, context);
                    }
                }
                else if (!IsInStringContext(DolarStringContext.Peek()) && code.ElementAtOrDefault(i + 1) is char nextCh && (ch == PragmaOperator || ch == '/' && (nextCh == '/' || nextCh == '*')))
                {
                    AddToken(ret, context);
                    context.Append(ch);
                    i++;
                    char separatorFirst;
                    char separatorSecond;

                    if (nextCh == '*')
                    {
                        separatorFirst = '*';
                        separatorSecond = '/';
                    }
                    else // if (nextCh == '/')
                    {
                        separatorFirst = '\r';
                        separatorSecond = '\n';
                    }

                    do
                    {
                        AdvanceLineColumn(nextCh);
                        context.Append(nextCh);
                        i++;

                        nextCh = code.ElementAtOrDefault(i);
                    } while (i < code.Length && nextCh != separatorFirst && code.ElementAtOrDefault(i + 1) != separatorSecond);

                    var nextNextChar = code.ElementAtOrDefault(i + 1);

                    context.Append(nextCh);
                    AdvanceLineColumn(nextCh);
                    context.Append(code.ElementAtOrDefault(i + 1));
                    AdvanceLineColumn(nextNextChar);
                    i++;

                    AddToken(ret, context);
                }
                else if (!IsInStringContext(DolarStringContext.Peek()) && (ch == '@' || ch == '$'))
                {
                    if (lastChar != '@' && lastChar != '$')
                    {
                        AddToken(ret, context);
                    }

                    context.Append(ch);
                }
                else if (ch == '"' && (!DolarStringContext.Peek().IsAtSignString && lastChar != '\\' || DolarStringContext.Peek().IsAtSignString && code.ElementAtOrDefault(i + 1) != '"' && (lastChar != '"' || lastChar == '"' && preLastChar == '"' && (context.Length > 4 || context.ToString() is string str && str != "@$\"" && str != "$@\"" && str != "$\"" && !str.StartsWith("@$\"\"") && !str.StartsWith("$\"\"") && str != "@\"" && !str.StartsWith("@\"")))))
                {
                    if (IsInStringContext(DolarStringContext.Peek()))
                    {
                        context.Append(ch);
                        AddToken(ret, context, true);
                        DolarStringContext.Pop();
                    }
                    else
                    {
                        if (lastChar != '@' && lastChar != '$')
                        {
                            AddToken(ret, context);
                        }

                        context.Append(ch);

                        DolarStringContext.Push(new StringContext
                        {
                            IsAtSignString = preLastChar == '@' || lastChar == '@',
                            IsDolarString = preLastChar == '$' || lastChar == '$',
                            IsInNonDolarStringContext = preLastChar != '$' && lastChar != '$'
                        });
                    }
                }
                else if (DolarStringContext.Count > 1 && (!IsInStringContext(DolarStringContext.Peek()) || DolarStringContext.Peek().IsDolarString) && ch == '{' && (code.ElementAtOrDefault(i + 1) != '{' || !DolarStringContext.Peek().IsDolarString))
                {
                    AddToken(ret, context, true);
                    context.Append(ch);
                    AddToken(ret, context);

                    DolarStringContext.Push(new StringContext
                    {
                        IsAtSignString = false,
                        IsDolarString = false,
                        IsInNonDolarStringContext = false
                    });
                }
                else if (DolarStringContext.Count > 1 && ch == '}' && !DolarStringContext.Peek().IsInNonDolarStringContext && (lastChar != '}' || !DolarStringContext.Peek().IsDolarString))
                {
                    AddToken(ret, context);
                    context.Append(ch);
                    AddToken(ret, context);
                    var pop = DolarStringContext.Pop();

                    if (DolarStringContext.Count == 1)
                    {
                        DolarStringContext.Push(pop);
                    }
                }
                else if (IsInStringContext(DolarStringContext.Peek()))
                {
                    context.Append(ch);
                }
                else if (!isInCharContext && ch == '\'')
                {
                    AddToken(ret, context);

                    context.Append(ch);

                    isInCharContext = true;
                }
                else if (IsIdefPart(ch))
                {
                    if (IsIdefPart(lastChar))
                    {
                        context.Append(ch);
                    }
                    else
                    {
                        if (lastChar != '\'')
                        {
                            AddToken(ret, context);
                        }

                        context.Append(ch);
                    }
                }
                else
                {
                    if (context.Length > 0 && ch != '\'' && ch != '"' && !AppendHasSense(context, ch, ret.LastOrDefault()))
                    {
                        AddToken(ret, context);
                    }

                    if (!char.IsWhiteSpace(ch) || isInCharContext)
                    {
                        context.Append(ch);
                    }
                }

                preLastChar = lastChar;
                lastChar = ch;
            }

            if (context.Length > 0)
            {
                AddToken(ret, context);
            }

            return ret;
        }

        private void AdvanceLineColumn(char ch)
        {
            if (ch == '\r')
            {
                Line++;
                Column = 1;
            }
            else if (ch != '\n')
            {
                Column++;
            }
        }

        private bool IsInStringContext(StringContext context)
        {
            return context.IsInNonDolarStringContext || context.IsDolarString;
        }

        private int IsIdefOrNumber(string context)
        {
            var ret = 1;

            for (int i = 0; i < context.Length; i++)
            {
                var ch = context[i];

                if (i == 0 && char.IsDigit(ch))
                {
                    ret = -1;
                }
                else if (!IsIdefPart(ch))
                {
                    if (ret == -1 && ch == '.')
                    {
                        continue;
                    }

                    return 0;
                }
            }

            return ret;
        }

        private bool IsIdefPart(char ch)
        {
            return char.IsLetterOrDigit(ch) || ch == '_';
        }

        private bool AppendHasSense(StringBuilder context, char ch, Token? last)
        {
            if (char.IsWhiteSpace(ch))
            {
                return false;
            }
            else
            {
                return GetTokenType(string.Concat(context.ToString(), ch), last) != TokenType.General;
            }
        }

        private TokenType GetTokenType(string str, Token? last, bool forceIsString = false)
        {
            var tokenType = TokenType.General;

            if (str.StartsWith("///"))
            {
                tokenType = TokenType.DocComment;
            }
            else if (str.StartsWith("//") || str.StartsWith("/*"))
            {
                tokenType = TokenType.Comment;
            }
            else if (str.StartsWith("#"))
            {
                tokenType = TokenType.Pragma;
            }
            if (forceIsString || str.StartsWith(@"@""") || str.StartsWith(@"@$""") || str.StartsWith(@"$@""") || str.StartsWith(@"$""") || str.StartsWith("\"") || str.StartsWith("'"))
            {
                tokenType = TokenType.StringOrChar;
            }
            else if (str == ".")
            {
                tokenType = TokenType.Dot;
            }
            else if (str == "[")
            {
                tokenType = TokenType.IndexerStart;
            }
            else if (str == "]")
            {
                tokenType = TokenType.IndexerEnd;
            }
            else if (str == "namespace")
            {
                tokenType = TokenType.NameSpace;
            }
            else if (ObjectTypes.Contains(str))
            {
                tokenType = TokenType.ObjectType;
            }
            else if (Modifiers.Contains(str))
            {
                tokenType = TokenType.PrivacyModifier;
            }
            else if (OtherKeyWords.Contains(str))
            {
                tokenType = TokenType.OtherKeyWords;
            }
            else if (last != null && last.TokenType == TokenType.OtherKeyWords && last.Text == "operator" && BoolOperators.Contains(str))
            {
                tokenType = TokenType.Identifier;
            }
            else if (BlockCommands.Contains(str))
            {
                tokenType = TokenType.BlockCommand;
            }
            else if (NullOperators.Contains(str))
            {
                tokenType = TokenType.NullOperator;
            }
            else if (ExpressionSeparators.Contains(str))
            {
                tokenType = TokenType.ExpressionSeparator;
            }
            else if (str == "(")
            {
                tokenType = TokenType.OpeningParenthesis;
            }
            else if (str == ")")
            {
                tokenType = TokenType.ClosingParenthesis;
            }
            else if (str == "{")
            {
                tokenType = TokenType.OpeningBlockBracket;
            }
            else if (str == "}")
            {
                tokenType = TokenType.ClosingBlockBracket;
            }
            else if (IsIdefOrNumber(str) == -1)
            {
                tokenType = TokenType.Number;
            }
            else if (PrimitiveTypeNames.Contains(str))
            {
                tokenType = TokenType.PrimitiveTypeName;
            }
            else if (IsIdefOrNumber(str) == 1)
            {
                tokenType = TokenType.Identifier;
            }

            return tokenType;
        }

        private TokenTypeExtended GetExtendedTokenType(string tokenText, TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.PrimitiveTypeName:
                    if (NumberTypes.Contains(tokenText))
                    {
                        return TokenTypeExtended.NumberType;
                    }
                    break;
                case TokenType.OtherKeyWords:
                    if (tokenText == "return")
                    {
                        return TokenTypeExtended.Return;
                    }
                    break;
                case TokenType.ExpressionSeparator:
                    if (tokenText == "=>")
                    {
                        return TokenTypeExtended.LambdaOperator;
                    }
                    else if (BoolOperators.Contains(tokenText))
                    {
                        return TokenTypeExtended.BoolOperator;
                    }
                    else if (LogicalOperators.Contains(tokenText))
                    {
                        return TokenTypeExtended.LogicalOperator;
                    }
                    else if (IntegerOperators.Contains(tokenText))
                    {
                        return TokenTypeExtended.IntegerOperator;
                    }
                    else if (FloatOperators.Contains(tokenText))
                    {
                        return TokenTypeExtended.FloatOperator;
                    }
                    else if (AssignmentOperators.Contains(tokenText))
                    {
                        return TokenTypeExtended.AssignmentOperator;
                    }
                    break;
            }

            return TokenTypeExtended.NONE;
        }

        private void AddToken(List<Token> ret, StringBuilder context, bool forceIsString = false)
        {
            var str = context.ToString();
            if (!string.IsNullOrWhiteSpace(str) || forceIsString)
            {
                var tokenType = GetTokenType(str, ret.LastOrDefault(), forceIsString);

                ret.Add(new Token
                {
                    Text = str,
                    TokenType = tokenType,
                    ExtendedType = GetExtendedTokenType(str, tokenType),
                    Line = Line,
                    Column = Column
                });
            }

            context.Clear();
        }

        private class StringContext
        {
            public bool IsAtSignString { get; set; }

            public bool IsDolarString { get; set; }

            public bool IsInNonDolarStringContext { get; set; }
        }
    }
}
