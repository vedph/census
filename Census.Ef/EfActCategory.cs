namespace Census.Ef
{
    public sealed class EfActCategory
    {
        public int ActId { get; set; }
        public int CategoryId { get; set; }
        public bool Unsure { get; set; }

        public EfAct Act { get; set; }
        public EfCategory Category { get; set; }

        public override string ToString()
        {
            return $"act {ActId}-category {CategoryId}{(Unsure ? "?" : "")}";
        }
    }
}
