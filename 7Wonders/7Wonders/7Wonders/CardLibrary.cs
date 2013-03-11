using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;


namespace _7Wonders
{
    public class CardLibrary
    {
        Dictionary<string, Card> cards;

        public CardLibrary()
        {
            JObject _json = JObject.Parse(File.ReadAllText("Content/Json/cards.json"));
            cards = new Dictionary<string, Card>();
            foreach (JObject c in (JArray)_json["cards"])
            {
                cards.Add((string)c["name"] + "_" + c["age"] + "_" + c["players"], new Card(c));
            }

        }

        public Card getCard(string cardKey)
        {
            if (cards.ContainsKey(cardKey)) return cards[cardKey];
            return null;
        }

    }
}
