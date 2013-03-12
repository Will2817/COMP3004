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
    public class Side
    {
        List<Structure> stages;
        Resource initialResource;

        public Side(JObject j)
        {
            switch ((string)j["effects"][0]["type"])
            {
                case "c":
                    initialResource = Resource.CLAY;
                    break;
                case "g":
                    initialResource = Resource.GLASS;
                    break;
                case "l":
                    initialResource = Resource.LOOM;
                    break;
                case "o":
                    initialResource = Resource.ORE;
                    break;
                case "p":
                    initialResource = Resource.PAPYRUS;
                    break;
                case "s":
                    initialResource = Resource.STONE;
                    break;
                case "w":
                    initialResource = Resource.WOOD;
                    break;
            }
            stages = new List<Structure>();
            foreach (JObject stage in j["stages"])
            {
                //stages.Add(new Structure((JObject)stage["cost"], (JObject)stage["effects"]));
            }
        }

        public Resource getIntialResource()
        {
            return initialResource;
        }
    }
}
