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
    public class Card
    {
        // Auto-Implementing Properites of Cards
        public string name { get; set; }
        public Visual image { get; set; }
        public int players { get; set; }
        public int age { get; set; }
        public CardColour colour { get; set; }
        public Structure structure { get; set; }
        public List<string> chains { get; set; }

        // Card Constructure
        public Card(Game1 theGame, JObject _json)
        {
            // Initializing Variables
            name = (string)_json["name"];
            image = new Visual(theGame, Vector2.Zero, 0, 0, (string)_json["image"]);
            players = (int)_json["players"];
            age = (int)_json["age"];
            colour = getGuildType((string)_json["guild"]);


        }

        // Gets the full name of the Guild - used for display
        public CardColour getGuildType(string _colour)
        {
            CardColour c = 0;
            switch (_colour)
            {
                case "brown":
                    c = CardColour.BROWN;
                    Console.WriteLine("brown card");
                    break;
                case "gray":
                    c = CardColour.GRAY;
                    Console.WriteLine("gray card");
                    break;
                case "purple":
                    c = CardColour.PURPLE;
                    Console.WriteLine("purple card");
                    break;
                case "blue":
                    c = CardColour.BLUE;
                    Console.WriteLine("blue card");
                    break;
                case "red":
                    c = CardColour.RED;
                    Console.WriteLine("red card");
                    break;
                case "yellow":
                    c = CardColour.YELLOW;
                    Console.WriteLine("yellow card");
                    break;
                case "green":
                    c = CardColour.GREEN;
                    Console.WriteLine("green card");
                    break;
                default:
                    Console.WriteLine("Error: Invalid Guild Colour, getGuildType(" + colour + ")");
                    break;
            }
            return c;
        }
    }
}
