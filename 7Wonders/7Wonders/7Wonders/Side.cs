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

    public class Side
    {
        int numStages { get; set; }
        Stage []  stages;

        public Side(JObject _json)
        {
            numStages = ((JArray)_json["stages"]).Count();

            /*foreach (JObject stage in (JArray)_json["stages"])
            {
                stages[]
            }*/
            Console.WriteLine("Number of Stages = " + numStages);
        }
    }
}
