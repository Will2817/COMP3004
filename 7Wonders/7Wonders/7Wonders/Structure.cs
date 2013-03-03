using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders
{
    // Structure Class
    public class Structure
    {
        // Variables
        protected List<string> costs;
        protected List<string> effects;
        protected List<string> chains;
        protected List<string> previous;

        // Constructor
        public Structure(JObject _costs, JObject _effects)
        {
            costs = new List<string>();
            effects = new List<string>();
            chains = new List<string>();
            previous = new List<string>();
            
            foreach (JProperty p in _costs.Properties())
            {
                switch (p.Name)
                {
                    case "c":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        break;
                    case "g":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        break;
                    case "l":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        break;
                    case "o":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        break;
                    case "p":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        break;
                    case "s":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        break;
                    case "w":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        break;
                    default:
                        Console.WriteLine("Error: No such cost " + p.Name);
                        break;
                }

                // Add to the list
                costs.Add(p.Name + ":" + p.Value);
            }

            foreach (JProperty p in _effects.Properties())
            {
                switch (p.Name)
                {
                    //Case - Resource : Type : Value
                    case "c": // Clay
                        Console.WriteLine("resource:" + p.Name + ":" + p.Value);
                        effects.Add("resource:" + p.Name + ":" + p.Value);
                        break;
                    case "g": // Glass
                        Console.WriteLine("resource:" + p.Name + ":" + p.Value);
                        effects.Add("resource:" + p.Name + ":" + p.Value);
                        break;
                    case "l": // Loom
                        Console.WriteLine("resource:" + p.Name + ":" + p.Value);
                        effects.Add("resource:" + p.Name + ":" + p.Value);
                        break;
                    case "o": // Ore
                        Console.WriteLine("resource:" + p.Name + ":" + p.Value);
                        effects.Add("resource:" + p.Name + ":" + p.Value);
                        break;
                    case "p": // Papyrus
                        Console.WriteLine("resource:" + p.Name + ":" + p.Value);
                        effects.Add("resource:" + p.Name + ":" + p.Value);
                        break;
                    case "s": // Stone
                        Console.WriteLine("resource:" + p.Name + ":" + p.Value);
                        effects.Add("resource:" + p.Name + ":" + p.Value);
                        break;
                    case "w": // Wood
                        Console.WriteLine("resource:" + p.Name + ":" + p.Value);
                        effects.Add("resource:" + p.Name + ":" + p.Value);
                        break;

                    // Case - Resource Choice: c1 : c2 : ...
                    case "rchoice":
                        string temp = "rchoice:";
                        foreach (JArray s in p["rchoice"])
                            temp += ":" + s;

                        Console.WriteLine(temp);
                        effects.Add(temp);
                        break;

                    // Case - Army : Value
                    case "army":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        effects.Add(p.Name + ":" + p.Value);
                        break;

                    // Case - Victory Points : Determinant : Value
                    // NOT DONE
                    case "victory":
                       
                        break;

                    // Case - Coin : Determinant : Value
                    // NOT DONE
                    case "coin":

                        break;
                    
                    // Case - Science: 't'ablet, 'c'ompass, 'g'ear
                    case "science":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        effects.Add(p.Name + ":" + p.Value);
                        break;

                    default:
                        Console.WriteLine("Error: No such effect " + p.Name);
                        break;
                }
            }
        }

        public List<string> getCosts()
        {
            return costs;
        }

        public List<string> getEffects()
        {
            return effects;
        }

        public List<string> getChains()
        {
            return chains;
        }

        public List<string> getPrevious()
        {
            return previous;
        }
        
    }
}
