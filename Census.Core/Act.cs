using System;
using System.Collections.Generic;
using System.Text;

namespace Census.Core
{
    public class Act
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public LookupItem Type { get; set; }
        public LookupItem Subtype { get; set; }
        public LookupItem Company { get; set; }
        public LookupItem Family { get; set; }
        public LookupItem Place { get; set; }
        public string Label { get; set; }
        public string Note { get; set; }
        public List<LookupItem> Categories { get; set; }
        public List<LookupItem> Professions { get; set; }
        public List<LookupItem> Partners { get; set; }

        public Act()
        {
            Categories = new List<LookupItem>();
            Professions = new List<LookupItem>();
            Partners = new List<LookupItem>();
        }
    }
}
