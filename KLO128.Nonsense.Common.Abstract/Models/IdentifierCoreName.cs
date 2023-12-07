namespace KLO128.Nonsense.Common.Abstract.Models
{
    public class IdentifierCoreName
    {
        public string Name { get; set; } = null!;

        public string? NewName { get; set; }

        public IdentifierCoreName(string Name, string NewName)
        {
            this.Name = Name;
            this.NewName = NewName;
        }

        public IdentifierCoreName()
        {
            Name = string.Empty;
            NewName = null;
        }

        public override bool Equals(object? obj)
        {
            return obj is IdentifierCoreName idef && idef.Name == Name;
        }

        public override int GetHashCode()
        {
            return -1;
        }

        public override string ToString()
        {
            return $"Idef: {Name}";
        }
    }
}
