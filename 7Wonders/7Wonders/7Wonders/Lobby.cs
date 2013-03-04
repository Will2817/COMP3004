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

namespace _7Wonders
{
    class Lobby : Interface
    {
        private const int MARGIN = 5;
        private const int CHECKBOXDIM = 15;
        private const int DIVIDERWIDTH = 2;
        private int SEC1WIDTH = Game1.WIDTH / 3;
        private int WONDERHEIGHT = (Game1.HEIGHT - 10) / 6;
        private int WONDERWIDTH = Game1.WIDTH / 3 - 10;
        private int SEC1HEIGHT = Game1.HEIGHT * 2/3;
        private int DROPDOWNWIDTH = (Game1.WIDTH / 3) - 100;
        private int DROPDOWNHEIGHT = (Game1.HEIGHT*2/3 - (Game1.MAXPLAYER + 1) * MARGIN) / Game1.MAXPLAYER;        

        protected List<string> playerTypes = new List<string>() { "Open", "AIType1", "AIType2", "AIType3" };

        protected static Dictionary<String, Visual> visuals1;
        protected List<Visual> dropDowns;
        protected List<Visual> readyCBs;
        protected Dictionary<String, Visual> wonders;

        protected Button sideButton;
        protected Button backButton;
        protected Button startButton;
        protected DropDown dropped = null;

        protected const int NUMPLAYERS = 7;
        protected bool random = false;
        protected bool onlyA = false;
        protected bool viewSideB = false;
        protected bool existsADrop = false;
        
        public Lobby(Game1 theGame)
            : base(theGame, "title", 0.4f)
        {
            sideButton = new Button(game, new Vector2(Game1.WIDTH - 140, Game1.HEIGHT - 140), 140, 40, "Toggle Side", "Font1");
            backButton = new Button(game, new Vector2(10, Game1.HEIGHT - 100), 75, 40, "Back", "Font1");
            startButton = new Button(game, new Vector2(90, Game1.HEIGHT - 100), 75, 40, "Start", "Font1");

            dropDowns = new List<Visual>();
            dropDowns.Add((new DropDown(game, new Vector2(MARGIN, MARGIN), DROPDOWNWIDTH, DROPDOWNHEIGHT, new List<string>() { "Host Player" })).setEnabled(false));
            for (int i = 1; i < NUMPLAYERS; i++)
            {
                dropDowns.Add(new DropDown(game, new Vector2(MARGIN, MARGIN + (MARGIN + DROPDOWNHEIGHT) * i), DROPDOWNWIDTH, DROPDOWNHEIGHT, playerTypes));
            }

            readyCBs = new List<Visual>();
            for (int i = 0; i < NUMPLAYERS; i++)
            {
                readyCBs.Add(new Checkbox(game, new Vector2(50 + DROPDOWNWIDTH, 20 + (MARGIN + DROPDOWNHEIGHT) * i), CHECKBOXDIM, CHECKBOXDIM));
            }

            wonders = new Dictionary<String, Visual>();

            visuals1 = new Dictionary<String, Visual>();
            visuals1.Add("Divider1", new Visual(game, new Vector2(SEC1WIDTH - 1, 0), DIVIDERWIDTH, Game1.HEIGHT, "line", Color.Silver));
            visuals1.Add("Divider2", new Visual(game, new Vector2(0, SEC1HEIGHT - 1), Game1.WIDTH, DIVIDERWIDTH, "line", Color.Silver));

            for (int i = dropDowns.Count; i > 0; i--)
            {
                visuals1.Add("drop"+i, dropDowns[i-1]);
            }
            for (int i = 0; i < readyCBs.Count; i++)
            {
                visuals1.Add("ready" + i, readyCBs[i]);
            }

            int count = 0;
            int count2 = 1;
            //need to work on this so that it adapts better to number of wonders
            foreach (Wonder w in Game1.wonders.Values)
            {
                w.getVisual().setPosition(new Vector2(5 + SEC1WIDTH * count2, 5 + WONDERHEIGHT * count)).setWidth(WONDERWIDTH).setHeight(WONDERHEIGHT);
                visuals1.Add(w.getName(), w.getVisual());
                count++;

                if (count>3)
                {
                    count=0;
                    count2++;
                }
                
            }

            visuals1.Add("selected", new Visual(game, new Vector2(5 + SEC1WIDTH, 5 + SEC1HEIGHT), WONDERWIDTH * 2 + 10, WONDERHEIGHT * 2, Game1.wonders.Values.First().getVisual().getTexture()));
            visuals1.Add("toggleButton", sideButton);
            visuals1.Add("backButton", backButton);
            visuals1.Add("startButton", startButton);
            activeVisuals = visuals1;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            foreach (Visual v in visuals1.Values)
            {
                v.LoadContent();
            }
        }

        public override void receiveMessage(Dictionary<string, string> message)
        {
            random = Boolean.Parse(message["random"]);
            onlyA = Boolean.Parse(message["onlyA"]);
            if (onlyA) sideButton.setVisible(false);
            else sideButton.setVisible(true);
        }

        public override void Update(GameTime gameTime, MouseState mouseState)
        {
            base.Update(gameTime, mouseState);
            if (sideButton.isClicked())
            {
                viewSideB = !viewSideB;
                foreach (Wonder w in Game1.wonders.Values)
                {
                    if (viewSideB)
                        w.setSideB();
                    else
                        w.setSideA();
                }
                string image = visuals1["selected"].getTexture();
                if (viewSideB)
                    visuals1["selected"].setTexture(image.Substring(0, image.Length - 1) + "B");
                else
                    visuals1["selected"].setTexture(image.Substring(0, image.Length - 1) + "A");

                sideButton.reset();               
            }
            if (backButton.isClicked())
            {
                finished = true;
                backButton.reset();
            }

            foreach (Wonder w in Game1.wonders.Values)
            {
                if (visuals1[w.getName()].isClicked())
                {
                    if (viewSideB)
                        visuals1["selected"].setTexture(w.getName() + "_B");
                    else
                        visuals1["selected"].setTexture(w.getName() + "_A");
                }
            }

            for (int i=dropDowns.Count; i >0; i--)
            {
                DropDown dd = (DropDown) dropDowns[i-1];
                if (dd.RequestDrop())
                {
                    if (dd == dropped)
                    {
                        dd.drop();
                    }
                    if (!existsADrop)
                    {
                        dd.drop();
                        dropped = dd;
                        existsADrop = true;
                    }
                    dd.resetRequest();
                }
                if (dd == dropped)
                {
                    if (!dd.getDown())
                    {
                        dropped = null;
                        existsADrop = false;
                    }
                }
            }
        }

        public override Dictionary<string, string> isFinished()
        {
            if (finished)
            {
                return MainMenu.createMessage();
            }

            return null;
        }

        public static Dictionary<string, string> createMessage(string _role, bool random, bool onlyA)
        {
            return new Dictionary<string, string>()
                {
                    {"nextInterface", "lobby"},
                    {"role" , _role},
                    {"random", random.ToString()},
                    {"onlyA", onlyA.ToString()}
                };
        }
    }
}