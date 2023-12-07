namespace KLO128.Nonsense.Common.Abstract.Models
{
    public enum TokenTypeExtended
    {
        NONE = 0,
        /// <summary>
        /// "byte", nameof(Byte), "sbyte", nameof(SByte), "short", nameof(Int16), "ushort", nameof(UInt16), "int", nameof(Int32), "uint", nameof(UInt32), "long", nameof(Int64), "ulong", nameof(Int64), "float", nameof(Single), "double", nameof(Double), "decimal", nameof(Decimal)
        /// </summary>
        NumberType = 1,
        /// <summary>
        /// "&&" "||" "==" "!=" "&lt;" "&gt;" "&lt=;" "&gt;="
        /// </summary>
        BoolOperator = 2,
        /// <summary>
        /// "&" "|" "^"
        /// </summary>
        LogicalOperator = 3,
        /// <summary>
        /// "*", "+", "-"
        /// </summary>
        IntegerOperator = 4,
        /// <summary>
        /// "/", "%"
        /// </summary>
        FloatOperator = 5,
        /// <summary>
        /// "=&gt;"
        /// </summary>
        LambdaOperator = 6,
        /// <summary>
        /// return
        /// </summary>
        Return = 7,
        /// <summary>
        /// "=", "+=", "-=", "/=", "*=", "%=", "&=", "|=", "^=", "++", "--"
        /// </summary>
        AssignmentOperator = 8
    }
}
