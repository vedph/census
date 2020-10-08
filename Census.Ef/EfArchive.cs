using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfArchive
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public List<EfBook> Books { get; set; }

        public override string ToString()
        {
            return $"#{Id}: {Name}";
        }
    }
}
