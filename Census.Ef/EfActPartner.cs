namespace Census.Ef
{
    public sealed class EfActPartner
    {
        public int ActId { get; set; }
        public int PartnerId { get; set; }
        public EfAct Act { get; set; }
        public EfPerson Partner { get; set; }

        public override string ToString()
        {
            return $"act {ActId}-person {PartnerId}";
        }
    }
}
