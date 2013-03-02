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
        protected string name;
        protected Visual image;
        protected int players;
        protected int age;
        protected string guild;
        protected Structure cost;
        protected Structure effect;

        public Card(Game1 theGame, JObject _json)
        {
            name = (string)_json["name"];
            //image = new Visual ??
            players = (int)_json["players"];
            age = (int)_json["age"];
            guild = (string)_json["guild"];
        }

        public string getName()
        {
            return name;
        }

        public Visual getVisual()
        {
            return image;
        }

        public int getPlayers()
        {
            return players;
        }

        public int getAge()
        {
            return age;
        }

        public string getGuild()
        {
            return guild;
        }

        
    }
}
