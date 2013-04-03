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

        public Dictionary<Resource, int> cost;
        public List<Effect> effects { get; set; }
        public List<string> chains  { get; set; }        
        

        // Card Constructor
        public Card(JObject _json)
        {
            // Initializing Variables
            name = (string)_json["name"];
            image = (string)_json["image"];
            players = (int)_json["players"];
            age = (int)_json["age"];
            guild = (string)_json["guild"];
            colour = parseColour(guild);

            cost = new Dictionary<Resource, int>();
            cost = (new Cost((JObject)_json["cost"])).getCost(); // Setting up the cost

            effects = new List<Effect>();
            chains = new List<string>();

            // Adding the effects for the cards into a List of Effects
            foreach (JObject e in (JArray)_json["effects"])
            {
                Effect effect = new Effect(e);
                effects.Add(effect);
            }

            // Adding in chains to the chains List of Strings
            foreach (string s in (JArray)_json["chains"])
            {
                if (s != null)
                    chains.Add(s);
            }

            // Used to output the variables of Card
            //printCardInfo();
        }

        // Gets the full name of the Guild - used for display
        public CardColour parseColour(string _colour)
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
                    Console.WriteLine("Error: Invalid Guild Colour, getGuildType(" + _colour + ")");
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
            Console.WriteLine("\nEffects:");
            for (int i = 0; i < effects.Count; i++)
                effects[i].PrintEffect();

            Console.WriteLine();

            
        }

        // Returns a string of the image ID
        public string getImageId()
        {
            return name + "_" + age + "_" + players;
        }
    }
}
