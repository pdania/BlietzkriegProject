using System;

namespace Client_ServerConnector.Templates
{
    public class Account
    {

        public string CardNumber { get; set; }
        public int Balance { get; set; }
        public string Status { get; set; }
        public double Percent { get; set; }
        public int? Rate { get; set; }
        public int? Limit { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}