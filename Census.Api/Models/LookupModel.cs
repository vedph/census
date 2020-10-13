using System.ComponentModel.DataAnnotations;

namespace Census.Api.Models
{
    /// <summary>
    /// Lookup data model.
    /// </summary>
    public sealed class LookupModel
    {
        /// <summary>
        /// Gets or sets the table identifier.
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// Gets or sets the filter to be matched.
        /// </summary>
        [MaxLength(50)]
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the max count of items to be returned.
        /// </summary>
        [Range(0, 100)]
        public int Limit { get; set; }
    }
}
