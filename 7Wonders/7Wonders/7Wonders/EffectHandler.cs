using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{

    public class EffectHandler
    {
        // Add a certain number of x Resource r to Player p
        public static void AddResource(Player p, Resource r, int x)
        {
            int resourceNum = p.getResourceNum(r) + x;
            p.setResourceNum(r, resourceNum);
        }

        // Resource choice
        // temporarily sets their resource
        public statiseChoice(Player p, Resource r)
        {

        }
    }
}
