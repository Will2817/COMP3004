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
        // Declaring Variables
        private List<Dictionary<Resource, int>> stageCosts;
        private List<List<Effect>> stageEffects;
        private int stageNum;
        public int stagesBuilt { get; set; }

        // I don't think we need the stages antmore
        List<Structure> stages;
        Resource initialResource;



        public Side(JObject j)
        {
            // Initializing Variables
            stageCosts = new List<Dictionary<Resource, int>>();
            stageEffects = new List<List<Effect>>();
            stagesBuilt = 0;
            stageNum = 0;

            // foreach loop - going through the JArray of stages
            // and placing the stageCosts and stageEffects appropriately
            foreach (JObject stage in (JArray)j["stages"])
            {
                stageCosts.Add(new Cost((JObject)stage["cost"]).getCost());

                // Creating a temporary list for effects
                // for that particular stage
                List<Effect> e = new List<Effect>();

                foreach (JObject _j in (JArray)stage["effects"])
                {
                    Effect effect = new Effect(_j);
                    e.Add(effect);
                }

                stageEffects.Add(e);
            }

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

            stageNum = stageCosts.Count;
        }

        public Resource getIntialResource()
        {
            return initialResource;
        }

        // Returns the number of stages in the particular Wonder
        public int getStageNum() { return stageNum; }

        // Returns a Dictionary of Costs for that particular stage
        public Dictionary<Resource, int> getStageCost(int _stageNum)
        {
            return stageCosts[_stageNum];
        }

        // Returns a List of Effects for that particular stage
        public List<Effect> getStageEffects(int _stageNum)
        {
            return stageEffects[_stageNum];
        }

        // Returns a List of the Dictionary Costs for the Wonder
        public List<Dictionary<Resource, int>> getAllStageCosts()
        {
            return stageCosts;
        }

        // Returns a List of Effect Lists for the Wonder
        public List<List<Effect>> getAllStageEffects()
        {
            return stageEffects;
        }

        // Print out the Effects of the Wonder
        public void printWonderEffects()
        {
            Console.WriteLine("Stage#: " + stages);
            for (int i = 0; i < stageEffects.Count; i++)
            {
                Console.WriteLine("\tStage " + (i + 1));
                for (int j = 0; j < stageEffects[i].Count; j++)
                    stageEffects[i][j].PrintEffect();
            }
        }
    }
}
