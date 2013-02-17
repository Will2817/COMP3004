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
        protected bool random = false;
        protected bool onlyA = false;
        public static Dictionary<String, Visual> visuals1;
        protected Button sideButton;
        protected DropDown dropDown1, dropDown2, dropDown3, dropDown4, dropDown5, dropDown6, dropDown7;
        protected bool viewSideB = false;
        protected bool existsADrop = false;
        protected string dropped = "";
        protected List<string> playerTypes = new List<string>() { "Open", "AIType1", "AIType2", "AIType3" };

        public Lobby(Game1 theGame)
            : base(theGame, "title", 0.4f)
        {
            int wide = Game1.WIDTH / 3;
            int half = (Game1.WIDTH - wide) / 2;
            int height = (Game1.HEIGHT - 10) / 6;
            int height2 = (Game1.HEIGHT - 49) * 2 / 21;
            visuals1 = new Dictionary<String, Visual>();
            sideButton = new Button(game, new Vector2(Game1.WIDTH - 140, Game1.HEIGHT - 140), 140, 40, "Toggle Side", "Font1", null);
            dropDown1 = new DropDown(game, new Vector2(5, 5), wide - 35, height2, new List<string>() { "Open", "AIEasy", "AIMedium", "AIHard" });
            dropDown2 = new DropDown(game, new Vector2(5, 8 + height2 * 1), wide - 35, height2, playerTypes);
            dropDown3 = new DropDown(game, new Vector2(5, 11 + height2 * 2), wide - 35, height2, playerTypes);
            dropDown4 = new DropDown(game, new Vector2(5, 14 + height2 * 3), wide - 35, height2, playerTypes);
            dropDown5 = new DropDown(game, new Vector2(5, 17 + height2 * 4), wide - 35, height2, playerTypes);
            dropDown6 = new DropDown(game, new Vector2(5, 20 + height2 * 5), wide - 35, height2, playerTypes);
            dropDown7 = new DropDown(game, new Vector2(5, 23 + height2 * 6), wide - 35, height2, playerTypes);

            visuals1.Add("Divider1", new Visual(game, new Vector2(wide-5, 0), 2, Game1.HEIGHT, "line", Color.Silver));
            visuals1.Add("Divider2", new Visual(game, new Vector2(0, height*4), wide-6, 2, "line", Color.Silver));
            visuals1.Add("Drop1", dropDown7);
            visuals1.Add("Drop2", dropDown6);
            visuals1.Add("Drop3", dropDown5);
            visuals1.Add("Drop4", dropDown4);
            visuals1.Add("Drop5", dropDown3);
            visuals1.Add("Drop6", dropDown2);
            visuals1.Add("Drop7", dropDown1);
            visuals1.Add("wonder0", new Visual(game, new Vector2(wide, 5), half, height, "wonder0a", Color.White));
            visuals1.Add("wonder1", new Visual(game, new Vector2(wide, 5+height), half, height, "wonder1a", Color.White));
            visuals1.Add("wonder2", new Visual(game, new Vector2(wide, 5+height*2), half, height, "wonder2a", Color.White));
            visuals1.Add("wonder3", new Visual(game, new Vector2(wide, 5+height*3), half, height, "wonder3a", Color.White));
            visuals1.Add("wonder4", new Visual(game, new Vector2(wide + half + 1, 5), half, height, "wonder4a", Color.White));
            visuals1.Add("wonder5", new Visual(game, new Vector2(wide + half + 1, 5 + height), half, height, "wonder5a", Color.White));
            visuals1.Add("wonder6", new Visual(game, new Vector2(wide + half + 1, 5 + height * 2), half, height, "wonder6a", Color.White));
            visuals1.Add("selected", new Visual(game, new Vector2(wide, 5+height*4), half * 2, height*2, "wonder0a", Color.White));
            visuals1.Add("toggleButton", sideButton);
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


        public void receiveMessage(Dictionary<string, string> message)
        {
            random = Boolean.Parse(message["random"]);
            onlyA = Boolean.Parse(message["onlyA"]);
            if (onlyA) sideButton.setVisible(false);
        }

        public override void Update(GameTime gameTime, MouseState mouseState)
        {
            base.Update(gameTime, mouseState);
            if (sideButton.isClicked())
            {
                viewSideB = !viewSideB;
                for (int i = 0; i < 7; i++)
                {
                    string wonder = "wonder" + i;
                    if (viewSideB)
                        visuals1[wonder].setImage(wonder + "b");
                    else
                        visuals1[wonder].setImage(wonder + "a");
                }
                string image = visuals1["selected"].getImage();
                if (viewSideB)
                    visuals1["selected"].setImage(image.Substring(0, image.Length - 1) + "b");
                else
                    visuals1["selected"].setImage(image.Substring(0, image.Length - 1) + "a");

                sideButton.reset();               
            }
            for (int i=0; i<7; i++)
            {
                string wonder = "wonder" + i;
                if (visuals1[wonder].isClicked()){
                    if (viewSideB)
                        visuals1["selected"].setImage(wonder + "b");
                    else
                        visuals1["selected"].setImage(wonder + "a");                        
                }
            }
            for (int i = 1; i < 8; i++)
            {
                DropDown dd = (DropDown) activeVisuals["Drop" + i];
                if (dd.RequestDrop())
                {
                    if ("Drop" + i == dropped)
                    {
                        dd.drop();
                    }
                    if (!existsADrop)
                    {
                        dd.drop();
                        dropped = "Drop" + i;
                        existsADrop = true;
                    }
                    dd.resetRequest();
                }
                if ("Drop" + i == dropped)
                {
                    if (!dd.getDown())
                    {
                        dropped = "";
                        existsADrop = false;
                    }
                }
            }
        }

        public override Dictionary<string, string> isFinished()
        {
            if (finished)
            {
                return new Dictionary<string, string>()
                {
                    {"nextInterface", "lobby"},
                };
            }

            return null;
        }
    }
}