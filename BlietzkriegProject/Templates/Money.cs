namespace UI.Templates
{
    public class Money
    {
        public string IdTo { get; set; }
        public int Amount { get; set; }

        public Money(string idTo, int amount)
        {
            IdTo = idTo;
            Amount = amount;
        }
    }
}