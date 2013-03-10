using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders
{
    public class Deck
    {
        // List of cards for all Ages + guilds
        public static List<Card> age1;
        public static List<Card> age2;
        public static List<Card> age3;
        public static List<Card> guilds;

        // Random Variables
        private static Random rnd = new Random();
        private int r;

        public Deck(JObject _json, int numPlayers)
        {

            age1 = new List<Card>();
            age2 = new List<Card>();
            age3 = new List<Card>();
            guilds = new List<Card>();

            // Loops through the cards.json array and gets the cards
            // one by one and check to see if it meets the number of players
            // and adds it in, but a special cause is taken into account for
            // Guild cards, where it will always be inserted into age3 randomly
            foreach (JObject c in (JArray)_json["cards"])
            {
                // Adding the appropriate cards for the number of players
                // to the decks by AGE
                if ((int)c["players"] <= numPlayers)
                {
                    Card card = new Card(c);

                    switch((int)c["age"])
                    {
                        case 1:
                            age1.Add(card);
                            break;
                        case 2:
                            age2.Add(card);
                            break;
                        case 3:
                            if ((int)c["players"] == 0)
                            {
                                guilds.Add(card);
                            }
                            else
                            {
                                age3.Add(card);
                            }
                            break;
                    }
                }
            }

            addGuilds(numPlayers);
        }

        // Adds the required number of Guild Cards into the Age 3 Deck
        public void addGuilds(int numPlayers)
        {
            // Randomly selecting numPlayers + 2 Guild Cards
            for (int i = 0; i < numPlayers + 2; i++)
            {
                // Randomly selecting from guild list
                // and placing it within the Age 3 deck
                r = rnd.Next(guilds.Count());           

                // Printing out the Guild cards to check if it is random
                //Console.WriteLine("Name: " + guilds[r].name);
                //Console.WriteLine("Image: " + guilds[r].image);

                age3.Add(guilds[r]);
                guilds.RemoveAt(r);
            }
        }

        // Deals 7 random cards to a player 
        // depending on the age number it will determine the age
        // and then removes it from the deck
        public List<Card> dealCards(int age)
        {
            List<Card> hand = new List<Card>();

            switch(age)
            {
                case 1:
                    for (int i = 0; i < 7; i++)
                    {
                        r = rnd.Next(age1.Count());

                        hand.Add(age1[r]);
                        age1.RemoveAt(r);
                    }
                    break;

                case 2:
                    for (int i = 0; i < 7; i++)
                    {
                        r = rnd.Next(age2.Count());

                        hand.Add(age3[r]);
                        age2.RemoveAt(r);
                    }
                    break;

                case 3:
                    for (int i = 0; i < 7; i++)
                    {
                        r = rnd.Next(age3.Count());

                        hand.Add(age3[r]);
                        age3.RemoveAt(r);
                    }
                    break;
            }
            return hand;
        }
    }
}
