using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Game_Cards
{
    public class Cost
    {
        public int? coin    { get; set; }
        public int? o       { get; set; }
        public int? c       { get; set; }
        public int? s       { get; set; }
        public int? w       { get; set; }
        public int? g       { get; set; }
        public int? l       { get; set; }
        public int? p       { get; set; }
    
        public Cost()
        {

        }

        public void PrintCost()
        {
            Console.Write("coin: " + coin + "");
        }

    }



}
