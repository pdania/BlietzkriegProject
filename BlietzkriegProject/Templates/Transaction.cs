using System;

namespace UI.Templates
{
    public class Transaction
    {
        public string From { get; private set; }
        public string To { get; private set; }
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