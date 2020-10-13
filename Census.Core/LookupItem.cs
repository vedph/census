namespace Census.Core
{
    public sealed class LookupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"#{Id} {Name}";
        }
    }
}
