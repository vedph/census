﻿using System.ComponentModel.DataAnnotations;

namespace Census.Ef
{
    public sealed class EfCompany
    {
        public int Id { get; set; }
        public int? PreviousId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public EfCompany Previous { get; set; }

        public override string ToString()
        {
            return $"#{Id}: {Name}";
        }
    }
}
