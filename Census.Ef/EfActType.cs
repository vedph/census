using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfActType
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public List<EfActSubtype> ActSubtypes { get; set; }

        public override string ToString()
        {
            return $"#{Id}: {Name}";
        }
    }
}
