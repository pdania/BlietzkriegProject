using System;

namespace Client_ServerConnector.Templates
{
    public class ScheduleTranferDto
    {
        
        public int TranferId { get; set; }
        public string CardNumberFrom { get; set; }
        public string CardNumberTo { get; set; }
        public int? Amount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int Period { get; set; }

    }
}