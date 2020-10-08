using CsvHelper.Configuration.Attributes;

namespace Census.Import
{
    public sealed class CsvRecord
    {
        [Index(0)]
        public int Id { get; set; }

        [Index(1)]
        public string Label { get; set; }

        [Index(2)]
        public string Family { get; set; }

        [Index(3)]
        public string Profession { get; set; }

        [Index(4)]
        public string Place { get; set; }

        [Index(5)]
        public string Category { get; set; }

        [Index(6)]
        public string ActType { get; set; }

        [Index(7)]
        public string ActSubtype { get; set; }

        [Index(8)]
        public string Company { get; set; }

        [Index(9)]
        public string Partner1 { get; set; }

        [Index(10)]
        public string Partner2 { get; set; }

        [Index(11)]
        public string PartnerEtc { get; set; }

        [Index(12)]
        public string PrevCompany { get; set; }

        [Index(13)]
        public string Archive { get; set; }

        [Index(14)]
        public string Location { get; set; }

        [Index(15)]
        public string BookDescription { get; set; }

        [Index(16)]
        public string BookPlace { get; set; }

        [Index(17)]
        public string BookWriter { get; set; }

        [Index(18)]
        public string BookType { get; set; }

        [Index(19)]
        public string BookSubtype { get; set; }

        [Index(20)]
        public string BookIncipit { get; set; }

        [Index(21)]
        public string BookStartYear { get; set; }

        [Index(22)]
        public string BookEndYear { get; set; }

        [Index(23)]
        public string BookFile { get; set; }

        [Index(24)]
        public string BookEdition { get; set; }

        [Index(25)]
        public string BookNote { get; set; }

        public override string ToString()
        {
            return $"#{Id}: {Label}";
        }
    }
}
