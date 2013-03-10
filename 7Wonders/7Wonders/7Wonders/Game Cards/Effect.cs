using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Effect()
        {

        }
    }
}
