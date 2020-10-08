using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfPerson
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"#{Id}: {Name}";
        }
    }
}
