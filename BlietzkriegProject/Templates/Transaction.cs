using System;

namespace UI.Templates
{
    public class Transaction
    {
        public string From { get; set; }
        public string To { get; set; }
        public int? Amount { get; set; }
        public DateTime? Date { get; set;}
    }
}