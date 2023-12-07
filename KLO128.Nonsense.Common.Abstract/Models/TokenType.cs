namespace KLO128.Nonsense.Common.Abstract.Models
{
    public enum TokenType
    {
        General = 0,
        Number = 1,
        Identifier = 2,
        Dot = 3,
        PrimitiveTypeName = 4,
        // NewKeyWord = 5,
        PrivacyModifier = 6,
        OtherKeyWords = 7,
        NameSpace = 8,
        Comment = 9,
        DocComment = 10,
        NullOperator = 11,
        ExpressionSeparator = 12,
        StringOrChar = 13,
        BlockCommand = 14,
        ObjectType = 15,
        IndexerStart = 16,
        IndexerEnd = 17,
        Parenthesis = 32,
        OpeningParenthesis = 32 | 128,
        ClosingParenthesis = 32 | 256,
        BlockBracket = 64,
        OpeningBlockBracket = 64 | 128,
        ClosingBlockBracket = 64 | 256,
        Pragma = 64 | 512,
    }
}
