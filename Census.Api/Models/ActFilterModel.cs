using Census.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Census.Api.Models
{
    /// <summary>
    /// Filter for acts.
    /// </summary>
    public sealed class ActFilterModel
    {
        /// <summary>
        /// The page number (1-N).
        /// </summary>
        [Range(0, int.MaxValue)]
        public int PageNumber { get; set; }

        /// <summary>
        /// The size of the page (1-100).
        /// </summary>
        [Range(1, 100)]
        public int PageSize { get; set; }

        /// <summary>
        /// The archive identifier.
        /// </summary>
        public int? ArchiveId { get; set; }

        /// <summary>
        /// The book identifier.
        /// </summary>
        public int? BookId { get; set; }

        /// <summary>
        /// The minimum book's start year.
        /// </summary>
        public short? BookYearMin { get; set; }

        /// <summary>
        /// The maximum book's start year.
        /// </summary>
        public short? BookYearMax { get; set; }

        /// <summary>
        /// Any portion of the book's description.
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// The act's type identifier.
        /// </summary>
        public int? ActTypeId { get; set; }

        /// <summary>
        /// The act's family identifier.
        /// </summary>
        public int? FamilyId { get; set; }

        /// <summary>
        /// The act's company identifier.
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// The act's place identifier.
        /// </summary>
        public int? PlaceId { get; set; }

        /// <summary>
        /// Any portion of the act's label.
        /// </summary>
        [MaxLength(100)]
        public string Label { get; set; }

        /// <summary>
        /// Zero or more category IDs, separated by comma.
        /// </summary>
        [MaxLength(100)]
        public string CategoryIds { get; set; }

        /// <summary>
        /// Zero or more profession IDs, separated by comma.
        /// </summary>
        [MaxLength(100)]
        public string ProfessionIds { get; set; }

        /// <summary>
        /// Zero or more partner IDs, separated by comma.
        /// </summary>
        [MaxLength(100)]
        public string PartnerIds { get; set; }

        private static List<int> ParseIds(string text)
        {
            List<int> ids = new List<int>();
            if (string.IsNullOrEmpty(text)) return ids;

            foreach (string token in text.Split(
                ',', StringSplitOptions.RemoveEmptyEntries))
            {
                if (int.TryParse(token, out int id)) ids.Add(id);
            }
            return ids;
        }

        /// <summary>
        /// Gets the core filter from this model.
        /// </summary>
        /// <returns>Filter.</returns>
        public ActFilter GetFilter(ITextFilter filter)
        {
            return new ActFilter
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                ArchiveId = ArchiveId.GetValueOrDefault(),
                BookId = BookId.GetValueOrDefault(),
                BookYearMin = BookYearMin.GetValueOrDefault(),
                BookYearMax = BookYearMax.GetValueOrDefault(),
                Description = filter.Apply(Description),
                ActTypeId = ActTypeId.GetValueOrDefault(),
                FamilyId = FamilyId.GetValueOrDefault(),
                CompanyId = CompanyId.GetValueOrDefault(),
                PlaceId = PlaceId.GetValueOrDefault(),
                Label = filter.Apply(Label),
                CategoryIds = ParseIds(CategoryIds),
                ProfessionIds = ParseIds(ProfessionIds),
                PartnerIds = ParseIds(PartnerIds)
            };
        }
    }
}
