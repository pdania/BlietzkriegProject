namespace UI.Templates
{
    public class MoneyTo
    {
        public string IdTo { get; set; }
        public int Amount { get; set; }

        public MoneyTo(string idTo, int amount)
        {
            IdTo = idTo;
            Amount = amount;
        }
    }
}