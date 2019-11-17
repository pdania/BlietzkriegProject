using System;

namespace UI.Templates
{
    public class ScheduleTranferDto
    {
        
        public int TranferId { get; set; }
        public string CardNumberFrom { get; set; }
        public string CardNumberTo { get; set; }
        public int? Amount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int Period { get; set; }

        public ScheduleTranferDto(int tranferId, string cardNumberFrom, string cardNumberTo, int? amount, DateTime? transactionDate, int period)
        {
            TranferId = tranferId;
            CardNumberFrom = cardNumberFrom;
            CardNumberTo = cardNumberTo;
            Amount = amount;
            TransactionDate = transactionDate;
            Period = period;
        }
    }
}