using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    public class Player
    {
        protected string nameID;
        protected Wonder board;
        protected string ip; // needed?

        protected int victory;
        protected int army;
        protected int coin;
        protected int defeat;

        // Resources
        // Clay, Ore, Stone, Wood, Glass, Loom, Papyrus
        int [] resources = new int [7];

        // Science
        // Tablet, Compass, Gear
        int[] sciences = new int[3];

        public Player(string _name, Wonder _board)
        {
            nameID = _name;
            board = _board;
            ip = null;
            victory = 0;
            army = 0;
            coin = 0; // Starts with 3?
            defeat = 0;

            for (int i = 0; i < resources.Length; i++)
                resources[i] = 0;

            for (int i = 0; i < sciences.Length; i++)
                sciences[i] = 0;

        }

        public string getName()
        {
            return null;//nothing, because screw people who want their name
        }

        // Accessors
        public string getIP()       { return ip; }
        public int getVictory()     { return victory; }
        public int getCoin()        { return coin; }
        public int getDefeat()      { return defeat; }
        public int[] getResources() { return resources; }
        public int[] getSciences()  { return sciences; }

        // Mutators
        public void setNameID(string n)
        {
            nameID = n;
        }

        public void setIP(string _ip)
        {
            ip = _ip;
        }

        public void setVictory(int v)
        {
            if (v >= 0)
                victory = v;
            else
                victory = 0;
        }

        public void addVictory(int v)
        {
            victory += v;
            
        }

        public void setCoin(int c)
        {
            if (c >= 0)
                coin = c;
            else
                coin = 0;
        }

        public void setDefeat(int d)
        {
            if (d >= 0)
                defeat = d;
            else
                defeat = 0;
        }

        public void setResources(int [] r)
        {
            if (r != null)
            {
                for (int i = 0; i < resources.Length; i++)
                    resources[i] = r[i];
            }
        }

        public void setSciences(int [] s)
        {
            if (s != null)
            {
                for (int i = 0; i < sciences.Length; i++)
                    sciences[i] = s[i];
            }
        }


    }
}
