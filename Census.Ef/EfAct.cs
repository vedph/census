using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfAct
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int TypeId { get; set; }
        public int SubtypeId { get; set; }
        public int? FamilyId { get; set; }
        public int? CompanyId { get; set; }
        public int? PlaceId { get; set; }

        [MaxLength(200)]
        public string Label { get; set; }

        [MaxLength(1000)]
        public string Note { get; set; }

        public EfBook Book { get; set; }
        public EfActType Type { get; set; }
        public EfActSubtype Subtype { get; set; }
        public EfFamily Family { get; set; }
        public EfCompany Company { get; set; }
        public EfPlace Place { get; set; }

        public override string ToString()
        {
            return $"#{Id} {Label}";
        }
    }
}
