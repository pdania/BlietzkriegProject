namespace UI.Templates
{
    public class ScheduleInput
    {
        public string idFrom { get; set; }
        public string idTo { get; set; }
        public int amount { get; set; }
        public int period { get; set; }

        public ScheduleInput(string idFrom, string idTo, int amount, int period)
        {
            this.idFrom = idFrom;
            this.idTo = idTo;
            this.amount = amount;
            this.period = period;
        }
    }
}