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
        protected string guild;
        protected Structure structure;

        // Card Constructure
        public Card(Game1 theGame, JObject _json)
        {
            // Initializing Variables
            name = (string)_json["name"];
            //image = new Visual();
            players = (int)_json["players"];
            age = (int)_json["age"];
            guild = (string)_json["guild"];
        }

        // Name Accessor
        public string getName()
        {
            return name;
        }

        // Image Accessor
        public Visual getVisual()
        {
            return image;
        }

        // Number Of Players Accessor
        public int getPlayers()
        {
            return players;
        }

        // Age Accessor
        public int getAge()
        {
            return age;
        }

        public string getGuild()
        {
            return guild;
        }

        public List<string> getCost()
        {
            return structure.getCosts();
        }

        public List<string> getEffects()
        {
            return structure.getEffects();
        }
        
    }
}
