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
using System.Windows.Forms;

namespace _7Wonders
{
    class MainMenu
    {
        protected Game1 game;
        protected static Dictionary<String, Visual> visuals1;
        protected static Dictionary<String, Visual> visuals2;
        protected static Dictionary<String, Visual> visuals3;
        protected static Dictionary<String, Visual> activeVisuals;
        protected Texture2D background;
        protected Menu menu;

        protected static Button HGame;
        protected static Button JGame;

        public MainMenu(Game1 theGame)
        {
            game = theGame;
            visuals1 = new Dictionary<String, Visual>();
            visuals2 = new Dictionary<String, Visual>();
            visuals3 = new Dictionary<String, Visual>();

            HGame = new Button(game, new Vector2(150, 450), 200, 50, "Host Game", "Fonts/Font1", null);
            JGame = new Button(game, new Vector2(450, 450), 200, 50, "Join Game", "Fonts/Font1", null);

            visuals1.Add("HGame", HGame);
            visuals1.Add("JGame", JGame);

            visuals2.Add("DBox", new Visual(game,new Vector2(250,200),300,300, "Images/line", Color.Silver));
            visuals2.Add("box1", new Visual(game, new Vector2(275, 210), 200, 50, "Images/line", Color.SlateGray));
            visuals2.Add("box2", new Visual(game, new Vector2(260, 270), 150, 50, "Images/line", Color.SlateGray));
            visuals2.Add("check1", new Checkbox(game, new Vector2(450, 285)));
            visuals2.Add("box3", new Visual(game, new Vector2(260, 340), 150, 50, "Images/line", Color.SlateGray));
            visuals2.Add("check2", new Checkbox(game, new Vector2(450, 355)));
            visuals2.Add("CRoom", new Button(game, new Vector2(260, 420), 170, 50, "Create Room", "Fonts/Font1", null));

            visuals3.Add("HGame", HGame);
            visuals3.Add("check2", new Checkbox(game, new Vector2(375, 350)));

            activeVisuals = visuals1;

            menu = new Menu();
        }

        public virtual void LoadContent()
        {
            foreach (Visual v in visuals1.Values)
            {
                v.LoadContent();
            }
            foreach (Visual v in visuals2.Values)
            {
                v.LoadContent();
            }
            foreach (Visual v in visuals3.Values)
            {
                v.LoadContent();
            }
            background = game.Content.Load<Texture2D>("Images/title");
        }

        public virtual void Update(GameTime gameTime, MouseState mouseState)
        {
            foreach (Visual v in activeVisuals.Values)
            {
                v.Update(gameTime, mouseState);
            }
            if (HGame.isClicked())
            {
                HGame.reset();
                activeVisuals = visuals2;
            }

            if (JGame.isClicked())
            {
                JGame.reset();
                activeVisuals = visuals3;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.White);
            foreach (Visual v in activeVisuals.Values)
            {
                v.Draw(gameTime, spriteBatch);
            }
        }
    }
}
