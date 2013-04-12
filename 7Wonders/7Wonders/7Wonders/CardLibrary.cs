using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;


namespace _7Wonders
{
    // This is a Libarbary of Cards where when parse to strings
    // it will keep a dictionary of all cards via it's parsed string along with it s Card object
    public class CardLibrary
    {
        public static Dictionary<string, Card> cards;

        public static void init()
        {
            JObject _json = JObject.Parse(File.ReadAllText("Content/Json/cards.json"));
            cards = new Dictionary<string, Card>();
            foreach (JObject c in (JArray)_json["cards"])
            {
                cards.Add((string)c["name"] + "_" + c["age"] + "_" + c["players"], new Card(c));
            }

        }

        public static Card getCard(string cardKey)
        {
            if (cardKey == null) return null;
            if (cards.ContainsKey(cardKey)) return cards[cardKey];
            return null;
        }

        public static Card getCardWithName(string cardName)
        {
            foreach (Card c in cards.Values)
                if (c.name == cardName) return c;
            return null;
        }

        public static List<Card> getCardList(int players)
        {
            List<Card> cardList = new List<Card>();
            foreach (Card c in cards.Values)
                if (c.players <= players) cardList.Add(c);
            cardList.Sort((x, y) => x.age.CompareTo(y.age));
            return cardList;
        }

    }
}
