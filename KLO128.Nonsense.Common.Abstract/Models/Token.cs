namespace KLO128.Nonsense.Common.Abstract.Models
{
    public class Token
    {
        public string Text { get; set; } = null!;

        public TokenType TokenType { get; set; }

        public TokenTypeExtended ExtendedType { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public int PrintableIndex { get; set; }

        public IdentifierCoreName? PrintableIdef { get; set; }

        public bool IsInDolarString { get; set; }

        public override string ToString()
        {
            return $"{Text} --> {TokenType}";
        }
    }
}
