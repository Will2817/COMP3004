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
        protected Side sideA;
        protected Side sideB;
        protected Side activeSide;
        protected Visual image;

        public Wonder(Game1 theGame,JObject _json)
        {
            name = (string)_json["name"];
            sideA = new Side(theGame, (JObject)_json["a"]);
            sideB = new Side(theGame, (JObject)_json["b"]);
            activeSide = sideA;
            image = new Visual(theGame, new Vector2(0, 0), 0, 0, activeSide.getTexture());
        }

        public string getName()
        {
            return name;
        }

        public Side getSide()
        {
            return activeSide;
        }

        public void setSideA()
        {
            activeSide = sideA;
            image.setTexture(activeSide.getTexture());
        }

        public void setSideB()
        {
            activeSide = sideB;
            image.setTexture(activeSide.getTexture());
        }

        public Visual getVisual()
        {
            return image;
        }
    }
}
