using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Game_Cards
{
    public class Effect
    {
        public enum BasisType { NONE, BROWN, GRAY, YELLOW, GREEN, RED, DEFEAT, PURPLE, BLUE, WONDER };
        public enum FromType { NONE, PLAYER, NEIGHBOURS, ALL };
        public enum TypeType { NONE, CLAY, STONE, WOOD, ORE, GLASS, PAPYRUS, LOOM, COIN, RCHOICE, RCOSTEAST, RCOSTWEST,
                                      MCOST, VICTORY, ARMY, TABLET, COMPASS, GEAR, SCHOICE, GUILD, FREEBUILD, DISCARD, LASTCARD };
        // Auto-Implementing Properites of Effect
        // from cards
        public TypeType type          { get; set; }
        public int amount           { get; set; }
        public List<TypeType> list    { get; set; }
        public FromType from          { get; set; }
        public BasisType basis         { get; set; }

        public Effect(JObject _json)
        {
            type = TypeType.NONE;
            amount = -1;
            list = null;
            from = FromType.NONE;
            basis = BasisType.NONE;

            foreach (KeyValuePair<string, JToken> token in (JObject)_json)
            {
                switch (token.Key)
                {
                    case "type":
                        type = parseType((string)token.Value);
                        break;
                    case "amount":
                        amount = (int)token.Value;
                        break;
                    case "list":
                        list = new List<TypeType>();
                        foreach (string s in (JArray)_json["list"])
                        {
                            if (s != null)
                                list.Add(parseType(s));
                        }
                        break;
                    case "from":
                        from = parseFrom((string)token.Value);
                        break;
                    case "basis":
                        basis = parseBasis((string)token.Value);
                        break;
                    default:
                        Console.WriteLine("Error: Effect Token " + token.Key + " not RECOGNIZED!");
                        break;
                }
            }

            // Used for Testing the Effect parameters
            //PrintEffect();
        }

        public void PrintEffect()
        {
            Console.WriteLine("\tType:\t" + type.ToString());
            // Because of type int?
            Console.WriteLine("\tAmount:\t" + amount);

            if (list != null)
            {
                Console.Write("\tList:\t");
                for (int i = 0; i < list.Count(); i++)
                    Console.Write(list[i].ToString() + "\t");
            }
            else
                Console.WriteLine("\tList:\t NULL");

                Console.WriteLine("\tFrom:\t" + from.ToString());

                Console.WriteLine("\tBasis:\t" + basis.ToString());

        }

        private TypeType parseType(string t)
        {
            switch (t)
            {
                case "c": return TypeType.CLAY;
                case "s": return TypeType.STONE;
                case "w": return TypeType.WOOD;
                case "o": return TypeType.ORE;
                case "g": return TypeType.GLASS;
                case "p": return TypeType.PAPYRUS;
                case "l": return TypeType.LOOM;
                case "coin": return TypeType.COIN;
                case "rchoice": return TypeType.RCHOICE;;
                case "rcostEast": return TypeType.RCOSTEAST;
                case "rcostWest": return TypeType.RCOSTWEST;
                case "mcostBoth": return TypeType.MCOST;
                case "victory": return TypeType.VICTORY;
                case "army": return TypeType.ARMY;
                case "tab": return TypeType.TABLET;
                case "comp": return TypeType.COMPASS;
                case "gear": return TypeType.GEAR;
                case "guildCopy": return TypeType.GUILD;
                case "freeBuild": return TypeType.FREEBUILD;
                case "discard": return TypeType.DISCARD;
                case "lastcard": return TypeType.LASTCARD;
                default: return TypeType.NONE;
            }
        }

        private BasisType parseBasis(string b)
        {
            switch (b)
            {
                case "brown": return BasisType.BROWN;
                case "gray": return BasisType.GRAY;
                case "yellow": return BasisType.YELLOW;
                case "green": return BasisType.GREEN;
                case "red": return BasisType.RED;
                case "defeat": return BasisType.DEFEAT;
                case "purple": return BasisType.PURPLE;
                case "blue": return BasisType.BLUE;
                case "wonderstages": return BasisType.WONDER;
                default: return BasisType.NONE;
            }
        }

        private FromType parseFrom(string f)
        {
            switch (f)
            {
                case "player": return FromType.PLAYER;
                case "neighbors": return FromType.NEIGHBOURS;
                case "all": return FromType.ALL;
                default: return FromType.NONE;
            }
        }
    }
}
