namespace UI.Templates
{
    public class MoneyFrom
    {
        public string IdFrom { get; set; }
        public int Amount { get; set; }

        public MoneyFrom(string idFrom, int amount)
        {
            IdFrom = idFrom;
            Amount = amount;
        }
    }
}