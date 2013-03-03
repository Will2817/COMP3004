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
        // Protected Variables
        protected string name;
        protected Visual image;
        protected int players;
        protected int age;
        protected string colour;
        protected Structure structure;
        protected List<string> chains;

        // Card Constructure
        public Card(Game1 theGame, JObject _json)
        {
            // Initializing Variables
            name = (string)_json["name"];
            image = new Visual(theGame, Vector2.Zero, 0, 0, (string)_json["image"]);
            players = (int)_json["players"];
            age = (int)_json["age"];
            colour = (string)_json["guild"];
            
            // Need to go into chains and copy each value
        }

        // Accessors
        public string getName()         {   return name; }
        public Visual getVisual()       {   return image; }
        public int getPlayers()         {   return players; }
        public int getAge()             {   return age; }
        public string getColour()       {   return colour; }
        public List<string> getCost()   { return structure.getCosts(); }
        public List<string> getEffects(){ return structure.getEffects(); }

        // Gets the full name of the Guild - used for display
        public string getGuildType(string colour)
        {
            string c = colour;

            switch (colour)
            {
                case "brown": 
                    c = "Raw Materials Structure";
                    break;
                case "gray": 
                    c = "Manufactured Goods Structure";
                    break;
                case "purple":
                    c = "Guilds Structure";
                    break;
                case "blue":
                    c = "Civilian Structure";
                    break;
                case "red":
                    c = "Military Structure";
                    break;
                case "yellow":
                    c = "Commercial Structure";
                    break;
                case "green":
                    c = "Scientific Structure";
                    break;
                default:
                    Console.WriteLine("Error: Invalid Guild Colour, getGuildType(" + c + ")");
                    break;
            }
            return c;
        }        
    }
}
