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
        public Structure(JObject _costs, JObject _effects, JObject _chains, JObject _previous)
        {
            costs = new List<string>();
            effects = new List<string>();
            chains = new List<string>();
            previous = new List<string>();
            
            foreach (JProperty p in _costs.Properties())
            {
                // I don't think we need this switch statement at all
                // This may come handy for outputting information
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
                    case "coin":
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
                        string rchoice = p.Name;
                        foreach (JArray c in p)
                            rchoice += ":" + c;

                        Console.WriteLine(rchoice);
                        effects.Add(rchoice);
                        break;

                    // Case - Science choice : tablet : compass : gear
                    case "schoice":
                        string schoice = p.Name;
                        foreach (JArray c in p)
                            schoice += ":" + c;                         

                        Console.WriteLine(schoice);
                        effects.Add(schoice);
                        break;
                    // Case - Army : Value
                    case "army":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        effects.Add(p.Name + ":" + p.Value);
                        break;

                    // Case - Victory Points : Determinant : Value
                    // NOT DONE
                    case "victory":
                        foreach (JProperty s in p)
                        {
                            Console.WriteLine(p.Name + ":" + s.Name + ":" + s.Value);
                            effects.Add(p.Name + ":" + s.Name + ":" + s.Value);

                        }
                        break;

                    // Case - Coin : Determinant : Value
                    // NOT DONE
                    case "coin":

                        foreach (JProperty c in p)
                        {
                            Console.WriteLine(p.Name + ":" + c.Name + ":" + c.Value);
                            effects.Add(p.Name + ":" + c.Name + ":" + c.Value);
                        }

                        
                        break;
                    
                    // Case - Science: 't'ablet, 'c'ompass, 'g'ear
                    case "science":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        effects.Add(p.Name + ":" + p.Value);
                        break;

                    // Case - Raw Resources cost via Eastern trade : value
                    case "rcostEast":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        effects.Add(p.Name + ":" + p.Value);
                        break;

                    // Case - Raw Resources cost via Western Trade : value
                    case "rcostWest":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        effects.Add(p.Name + ":" + p.Value);
                        break;

                    // Case - Manufactured Resources cost via East Trade : value
                    case "mcostEast":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        effects.Add(p.Name + ":" + p.Value);
                        break;

                    // Case - Manufactured Resources cost via West Trade : value
                    case "mcostWest":
                        Console.WriteLine(p.Name + ":" + p.Value);
                        effects.Add(p.Name + ":" + p.Value);
                        break;

                    default:
                        Console.WriteLine("Error: No such effect " + p.Name);
                        break;
                }
            }

            foreach (JProperty p in _chains.Properties())
            {
                chains.Add((string)p.Value);
            }

            foreach (JProperty p in _previous.Properties())
            {
                previous.Add(string)p.Value);
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
