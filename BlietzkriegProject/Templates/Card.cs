namespace UI.Templates
{
    public class Card
    {
        public string UserCardNumber { get;private set; }
        public string CardPIN { get; private set; }

        public Card(string userCardNumber, string cardPin)
        {
            UserCardNumber = userCardNumber;
            CardPIN = cardPin;
        }
    }
}