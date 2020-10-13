namespace Census.Core
{
    public class Book
    {
        public int Id { get; set; }
        public LookupItem Archive { get; set; }
        public LookupItem Type { get; set; }
        public LookupItem Subtype { get; set; }
        public LookupItem WritePlace { get; set; }
        public LookupItem Writer { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Incipit { get; set; }
        public short StartYear { get; set; }
        public short EndYear { get; set; }
        public string Edition { get; set; }
        public string Note { get; set; }
        public string File { get; set; }
    }
}
