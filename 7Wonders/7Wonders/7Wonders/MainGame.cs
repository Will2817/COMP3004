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
    class MainGame : Interface
    {
        private const int MARGIN = 5;
        private const int CHECKBOXDIM = 15;
        private const int DIVIDERWIDTH = 2;
        private int SEC1WIDTH = Game1.WIDTH / 3;
        private int WONDERHEIGHT = (Game1.HEIGHT - 10) / 6;
        private int WONDERWIDTH = Game1.WIDTH / 3 - 10;
        private int SEC1HEIGHT = Game1.HEIGHT * 2 / 3;
        private int DROPDOWNWIDTH = (Game1.WIDTH / 3) - 100;
        private int DROPDOWNHEIGHT = (Game1.HEIGHT * 2 / 3 - (Game1.MAXPLAYER + 1) * MARGIN) / Game1.MAXPLAYER;

        protected Dictionary<String, Player> players;
        protected Player player;



        protected static Dictionary<String, Visual> visuals1;

        public MainGame(Game1 theGame)
            : base(theGame, "title", 0.4f)
        {
            players = new Dictionary<string, Player>();
            visuals1 = new Dictionary<String, Visual>();
 
            visuals1.Add("Divider1", new Visual(game, new Vector2(SEC1WIDTH - 1, 0), DIVIDERWIDTH, Game1.HEIGHT, "line", Color.Silver));
            visuals1.Add("Divider2", new Visual(game, new Vector2(0, SEC1HEIGHT - 1), Game1.WIDTH, DIVIDERWIDTH, "line", Color.Silver));

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
           
        }

        public override void Update(GameTime gameTime, MouseState mouseState)
        {
            base.Update(gameTime, mouseState);
            
        }

        public override Dictionary<string, string> isFinished()
        {
            if (finished)
            {
                return MainMenu.createMessage();
            }

            return null;
        }

        public static Dictionary<string, string> createMessage()
        {
            return new Dictionary<string, string>()
                {
                    {"nextInterface", "maingame"}
                };
        }
    }
}
