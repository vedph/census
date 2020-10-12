using Fusi.Tools.Data;
using System.Collections.Generic;

namespace Census.MySql
{
    public sealed class ActFilter : PagingOptions
    {
        public int ArchiveId { get; set; }
        public int BookId { get; set; }
        public short BookYearMin { get; set; }
        public short BookYearMax { get; set; }
        public string Description { get; set; }
        public int ActTypeId { get; set; }
        public int FamilyId { get; set; }
        public int CompanyId { get; set; }
        public int PlaceId { get; set; }
        public string Label { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> ProfessionIds { get; set; }
        public List<int> PartnerIds { get; set; }

        public ActFilter()
        {
            CategoryIds = new List<int>();
            ProfessionIds = new List<int>();
            PartnerIds = new List<int>();
        }

        public bool IsEmpty()
        {
            return ArchiveId == 0
                && BookId == 0
                && BookYearMin == 0 && BookYearMax == 0
                && string.IsNullOrEmpty(Description)
                && ActTypeId == 0
                && FamilyId == 0
                && CompanyId == 0
                && PlaceId == 0
                && string.IsNullOrEmpty(Label)
                && (CategoryIds == null || CategoryIds.Count == 0)
                && (ProfessionIds == null || ProfessionIds.Count == 0)
                && (PartnerIds == null || PartnerIds.Count == 0);
        }
    }
}
