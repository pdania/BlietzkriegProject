using System;

namespace UI.Templates
{
    public class Account
    {
        private readonly string _showInCombobox;
        public string Type { get; set; }
        public string CardNumber { get; set; }
        public int Balance { get; set; }
        public string Status { get; set; }
        public double Percent { get; set; }
        public int? Rate { get; set; }
        public int? Limit { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string ShowInCombobox => CardNumber+"\t"+Type+"\t"+Balance;
    }
}