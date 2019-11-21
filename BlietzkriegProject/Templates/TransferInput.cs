namespace UI.Templates
{

      public class TranferInput
        {
            public string IdFrom { get; set; }
            public string IdTo { get; set; }
            public int Amount { get; set; }

            public TranferInput(string idFrom, string idTo, int amount)
            {
                IdFrom = idFrom;
                IdTo = idTo;
                Amount = amount;
            }
        }
    
}