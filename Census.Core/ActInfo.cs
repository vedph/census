namespace Census.Core
{
    public class ActInfo
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int SubtypeId { get; set; }
        public string SubtypeName { get; set; }
        public int FamilyId { get; set; }
        public string FamilyName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string Label { get; set; }
        public int ArchiveId { get; set; }
        public string ArchiveName { get; set; }
        public int BookId { get; set; }
        public string BookLocation { get; set; }
        public string BookDescription { get; set; }
        public short BookStartYear { get; set; }
        public short BookEndYear { get; set; }
        public string BookFile { get; set; }

        public override string ToString()
        {
            return $"#{Id} {Label} {BookStartYear}-{BookEndYear}";
        }
    }
}
