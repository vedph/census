using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfCategory
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"#{Id}: {Name}";
        }
    }
}
