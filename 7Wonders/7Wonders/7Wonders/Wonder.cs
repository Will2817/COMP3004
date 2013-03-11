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
        protected bool usingA;//true=A,false=B
        protected bool inUse;

        // Cost and Effect of the Wonder Board


        public Wonder(JObject _json)
        {
            Console.WriteLine((string)_json["name"]);
            name = (string)_json["name"];
            sideA = new Side((JObject)_json["a"]);
            sideB = new Side((JObject)_json["b"]);
            usingA = true;
            inUse = false;
            Console.WriteLine();
        }

        public string getName()
        {
            return name;
        }

        public Side getSide()
        {
            if (usingA) return sideA;
            else return sideB;
        }

        public string getSideName()
        {
            if (usingA) return "A";
            return "B";
        }

        public void setSideA()
        {
            usingA = true;
        }

        public void setSideB()
        {
            usingA = false;
        }

        public void setInUse(bool inUse)
        {
            this.inUse = inUse;
        }

        public bool isInUse()
        {
            return inUse;
        }

        public JObject toJObject()
        {
            return new JObject(
                    new JProperty("name", name),
                    new JProperty("activeSide", usingA));
        }

        public string getImageName()
        {
            if (usingA) return name + "_A";
            return name + "_B";
        }
    }
}
