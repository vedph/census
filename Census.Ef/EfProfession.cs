﻿using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfProfession
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Namex { get; set; }

        public override string ToString()
        {
            return $"#{Id}: {Name}";
        }
    }
}
