using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfBook
    {
        public int Id { get; set; }
        public int ArchiveId { get; set; }
        public int TypeId { get; set; }
        public int SubtypeId { get; set; }
        public int? WritePlaceId { get; set; }
        public int? WriterId { get; set; }

        [MaxLength(100)]
        public string Location { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(3000)]
        public string Incipit { get; set; }
        public short StartYear { get; set; }
        public short EndYear { get; set; }

        [MaxLength(500)]
        public string Edition { get; set; }

        [MaxLength(3000)]
        public string Note { get; set; }

        [MaxLength(50)]
        public string File { get; set; }

        public EfArchive Archive { get; set; }
        public EfBookType Type { get; set; }
        public EfBookSubtype Subtype { get; set; }
        public EfPlace WritePlace { get; set; }
        public EfPerson Writer { get; set; }

        public override string ToString()
        {
            return $"#{Id} {Location}";
        }
    }
}
