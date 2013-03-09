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

        public Side(JObject j)
        {
            stages = new List<Structure>();
            foreach (JObject stage in j["stages"])
            {
                //stages.Add(new Structure((JObject)stage["cost"], (JObject)stage["effects"]));
            }
        }
    }
}
