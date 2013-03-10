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
        private int DROPDOWNWIDTH = (Game1.WIDTH / 3) - 125;
        private int DROPDOWNHEIGHT = (Game1.HEIGHT / 2 - (Game1.MAXPLAYER + 1) * MARGIN) / Game1.MAXPLAYER;
        private const int LABELHEIGHT = 35;
        private const int LABELWIDTH = 100;

        protected Dictionary<int, Dictionary<string, Visual>> seatVisuals;
        protected Dictionary<string, Visual> baseVisuals;
        protected int seatViewed = 0;
        protected Player player;
        protected Visual wonder;

        public MainGame(Game1 theGame)
            : base(theGame, "title", 0.4f)
        {
            player = null;
            wonder = null;
            seatVisuals = new Dictionary<int, Dictionary<string, Visual>>();
            baseVisuals = new Dictionary<String, Visual>();
            baseVisuals.Add("Label1", new Visual(game, new Vector2(MARGIN, MARGIN), DROPDOWNWIDTH, LABELHEIGHT, "Players", "Font1"));
            baseVisuals.Add("Label2", new Visual(game, new Vector2(MARGIN * 2 + DROPDOWNWIDTH, MARGIN), LABELWIDTH, LABELHEIGHT, "Score", "Font1"));
            baseVisuals.Add("Divider1", new Visual(game, new Vector2(SEC1WIDTH - 1, 0), DIVIDERWIDTH, Game1.HEIGHT, "line", Color.Silver));
            baseVisuals.Add("Divider2", new Visual(game, new Vector2(0, SEC1HEIGHT - 1), Game1.WIDTH, DIVIDERWIDTH, "line", Color.Silver));
        }

        private void Initialize()
        {
            player = Game1.client.getSelf();
            foreach (Player p in Game1.client.getState().getPlayers().Values)
            {
                Game1.wonders[p.getBoard().getName()].setPosition(new Vector2(5 + SEC1WIDTH, 5 + SEC1HEIGHT)).setWidth(WONDERWIDTH * 2 + 10).setHeight(WONDERHEIGHT * 2).setTexture(p.getBoard().getImageName());
                seatVisuals.Add(p.getSeat(), new Dictionary<string, Visual>(){{p.getBoard().getImageName(), Game1.wonders[p.getBoard().getName()]}});
                baseVisuals.Add("player" + p.getSeat(), new Visual(game, new Vector2(MARGIN, MARGIN * 2 + LABELHEIGHT + (MARGIN + DROPDOWNHEIGHT) * p.getSeat()), DROPDOWNWIDTH, DROPDOWNHEIGHT, 
                                                                p.getName(), "Font1", Color.White, (p.getSeat() == player.getSeat())? Color.Orange : Color.Gray));
            }
            seatViewed = player.getSeat();
            activeVisuals = seatVisuals[seatViewed];
            LoadContent();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            foreach (Visual v in baseVisuals.Values)
            {
                v.LoadContent();
            }
        }

        public override void receiveMessage(Dictionary<string, string> message)
        {
            Initialize();
        }

        public override void Update(GameTime gameTime, MouseState mouseState)
        {
            base.Update(gameTime, mouseState);
            foreach (Visual v in baseVisuals.Values)
            {
                v.Update(gameTime, mouseState);
            }
            if (Game1.client.isUpdateAvailable()) networkUpdates();

            int storeSeat = seatViewed;

            switch (game.recordedPresses)
            {
                case "1":
                    seatViewed = 0;
                    activeVisuals = seatVisuals[0];
                    break;
                case "2":
                    seatViewed = 1;
                    activeVisuals = seatVisuals[1];
                    break;
                case "3":
                    seatViewed = 2;
                    activeVisuals = seatVisuals[2];
                    break;
                case "4":
                    if (seatVisuals.Count > 3)
                    {
                        seatViewed = 3;
                        activeVisuals = seatVisuals[3];
                    }
                    break;
                case "5":
                    if (seatVisuals.Count > 4)
                    {
                        seatViewed = 4;
                        activeVisuals = seatVisuals[4];
                    }
                    break;
                case "6":
                    if (seatVisuals.Count > 5)
                    {
                        seatViewed = 5;
                        activeVisuals = seatVisuals[5];
                    }
                    break;
                case "7":
                    if (seatVisuals.Count > 6)
                    {
                        seatViewed = 6;
                        activeVisuals = seatVisuals[6];
                    }
                    break;
                default:
                    break;
            }
            game.recordedPresses = "";

            if (storeSeat != seatViewed)
            {
                baseVisuals["player" + storeSeat].setColor(Color.Gray);
                baseVisuals["player" + seatViewed].setColor(Color.Orange);
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            foreach (Visual v in baseVisuals.Values)
            {
                v.Draw(gameTime, spriteBatch);
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

        public static Dictionary<string, string> createMessage()
        {
            return new Dictionary<string, string>()
                {
                    {"nextInterface", "maingame"}
                };
        }

        private void networkUpdates()
        {

        }
    }
}
