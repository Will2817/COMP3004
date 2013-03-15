using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Game_Cards
{
    public class Effect
    {
        // Auto-Implementing Properites of Effect
        // from cards
        public string type          { get; set; }
        public int amount           { get; set; }
        public List<string> list    { get; set; }
        public string from          { get; set; }
        public string basis         { get; set; }

        public Effect(JObject _json)
        {
            type = null;
            amount = -1;
            list = null;
            from = null;
            basis = null;

            foreach (KeyValuePair<string, JToken> token in (JObject)_json)
            {
                switch (token.Key)
                {
                    case "type":
                        type = (string)token.Value;
                        break;
                    case "amount":
                        amount = (int)token.Value;
                        break;
                    case "list":
                        list = new List<string>();
                        foreach (string s in (JArray)_json["list"])
                        {
                            if (s != null)
                                list.Add(s);
                        }
                        break;
                    case "from":
                        from = (string)token.Value;
                        break;
                    case "basis":
                        basis = (string)token.Value;
                        break;
                    default:
                        Console.WriteLine("Error: Effect Token " + token.Key + " not RECOGNIZED!");
                        break;
                }
            }

            // Used for Testing the Effect parameters
            PrintEffect();
        }

        public void PrintEffect()
        {
            if (type != null)
                Console.WriteLine("\tType:\t" + type);
            // Because of type int?
            Console.WriteLine("\tAmount:\t" + amount);

            if (list != null)
            {
                Console.Write("\tList:\t");
                for (int i = 0; i < list.Count(); i++)
                    Console.Write(list[i] + "\t");
            }
            else
                Console.WriteLine("\tList:\t NULL");

            if (from != null)
                Console.WriteLine("\tFrom:\t" + from);
            else
                Console.WriteLine("\tFrom:\t NULL");

            if (basis != null)
                Console.WriteLine("\tBasis:\t" + basis);
            else
                Console.WriteLine("\tBasis:\t NULL\n");

        }
    }
}
