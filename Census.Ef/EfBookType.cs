using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfBookType
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public List<EfBookSubtype> BookSubtypes { get; set; }

        public override string ToString()
        {
            return $"#{Id}: {Name}";
        }
    }
}
