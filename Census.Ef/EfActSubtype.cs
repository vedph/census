using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfActSubtype
    {
        public int Id { get; set; }
        public int ActTypeId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Namex { get; set; }

        public EfActType ActType { get; set; }

        public override string ToString()
        {
            return $"#{Id}: {Name}";
        }
    }
}
