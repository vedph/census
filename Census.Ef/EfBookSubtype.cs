using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfBookSubtype
    {
        public int Id { get; set; }
        public int BookTypeId { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Namex { get; set; }

        public EfBookType BookType { get; set; }

        public override string ToString()
        {
            return $"#{Id}: {Name}";
        }
    }
}
