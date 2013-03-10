using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders
{
    class Deck
    {

        public List<Card> age1;
        public List<Card> age2;
        public List<Card> age3;
        public List<Card> guilds;

        public Deck(JObject _json, int player)
        {
            foreach (JObject c in (JArray)_json["cards"])
            {
                //Just going to add all the cards into Age1 for testing
                age1.Add(new Card(c));

                // Make algorithm for adding cards for all decks,
                // along with the guild cards in randomly

                /*
                if ((int)c["players"] == player)
                {
                    Console.WriteLine("Name: " + (string)c["name"]);
                    Console.WriteLine("Image: " + (string)c["image"]);
                    Console.WriteLine("Player: " + (int)c["players"]);           
                }*/
            }
        }

        
    }
}
