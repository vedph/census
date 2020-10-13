namespace Census.Core
{
    public sealed class LookupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public LookupItem()
        {
        }

        public LookupItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return $"#{Id} {Name}";
        }
    }
}
