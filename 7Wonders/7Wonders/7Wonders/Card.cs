using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json.Linq;

namespace _7Wonders
{
    using Game_Cards;


    public class Card
    {
        // Auto-Implementing Properites of Cards
        public string name          { get; set; }
        public string image         { get; set; }
        public int players          { get; set; }
        public int age              { get; set; }
        public string guild         { get; set; }
        public CardColour colour    { get; set; }

        public Cost cost            { get; set; }
        public List<Effect> effects { get; set; }
        public List<string> chains  { get; set; }        
        

        // Card Constructure
        public Card(JObject _json)
        {
            // Initializing Variables
            name = (string)_json["name"];
            image = (string)_json["image"];
            players = (int)_json["players"];
            age = (int)_json["age"];
            guild = (string)_json["guild"];
            colour = getGuildType(guild);
            chains = new List<string>();

            // Adding in chains to the chains List of Strings
            foreach (string s in (JArray)_json["chains"])
            {
                if (s != null)
                    chains.Add(s);
            }

            // Adding the effects for the cards into a List of Effects
            foreach (JObject e in (JArray)_json["effects"])
            {

            }

            printCardInfo();

                
            
            //chains = new (JArray))_json["chains"].List<string>();
            //foreach (J _chain in (JArray)_json["chains"])
            {
              //  Console.WriteLine(_chain);
            }
        }

        // Gets the full name of the Guild - used for display
        public CardColour getGuildType(string _colour)
        {
            CardColour c = 0;
            switch (_colour)
            {
                case "brown":
                    c = CardColour.BROWN;
                    break;

                case "gray":
                    c = CardColour.GRAY;
                    break;

                case "purple":
                    c = CardColour.PURPLE;
                    break;

                case "blue":
                    c = CardColour.BLUE;
                    break;

                case "red":
                    c = CardColour.RED;
                    break;

                case "yellow":
                    c = CardColour.YELLOW;
                    break;

                case "green":
                    c = CardColour.GREEN;
                    break;

                default:
                    Console.WriteLine("Error: Invalid Guild Colour, getGuildType(" + colour + ")");
                    break;
            }
            return c;
        }

        public void printCardInfo()
        {
            Console.WriteLine("Name: " + name);
            Console.WriteLine("Image:" + image);
            Console.WriteLine("Players: " + players);
            Console.WriteLine("Age: " + age);
            Console.WriteLine("Guild: " + guild + "\t Colour: " +  colour);
            Console.Write("ChainCount: " + chains.Count() + " Chains: ");
            for (int i = 0; i < chains.Count(); i++)
            {
                Console.Write("[" + chains[i] + "] ");
            }
            Console.WriteLine("\n");
        }
    }
}
