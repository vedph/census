namespace Census.Ef
{
    public sealed class EfActProfession
    {
        public int ActId { get; set; }
        public int ProfessionId { get; set; }
        public bool Unsure { get; set; }

        public EfAct Act { get; set; }
        public EfProfession Profession { get; set; }

        public override string ToString()
        {
            return $"act {ActId}-profession {ProfessionId}{(Unsure ? "?" : "")}";
        }

    }
}
