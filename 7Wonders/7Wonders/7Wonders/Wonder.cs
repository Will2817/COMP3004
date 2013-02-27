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
    public class Wonder
    {
        protected string name;
        protected char side;
        protected Visual image;

        public Wonder(Game1 theGame,JObject _json)
        {
            name = (string)_json["name"];
            side = (char)_json["side"];
            image = new Visual(theGame, new Vector2(0, 0), 0, 0, (string)_json["image"]);
        }

        public string getName()
        {
            return name;
        }

        public char getSide()
        {
            return side;
        }

        public Visual getVisual()
        {
            return image;
        }
    }
}
