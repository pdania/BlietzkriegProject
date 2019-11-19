using System;

namespace UI.Templates
{
    public class Transaction
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Amount { get; private set; }
        public DateTime Time { get; private set;}

        public Transaction(string @from, string to, int amount, DateTime time)
        {
            From = @from;
            To = to;
            Amount = amount;
            Time = time;
        }
    }
}