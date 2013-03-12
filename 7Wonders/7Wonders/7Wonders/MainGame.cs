﻿using System;
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
        private int DROPDOWNWIDTH = (Game1.WIDTH / 3) - 135;
        //private int DROPDOWNWIDTH = SEC1WIDTH
        private int DROPDOWNHEIGHT = (Game1.HEIGHT / 2 - (Game1.MAXPLAYER + 1) * MARGIN) / Game1.MAXPLAYER;
        private const int LABELHEIGHT = 40;
        private int LABELWIDTH;

        protected Dictionary<int, Dictionary<string, Visual>> seatVisuals;
        protected Dictionary<string, Visual> baseVisuals;
        protected int seatViewed = 0;
        protected Player player;
        protected Visual wonder;

        public MainGame(Game1 theGame)
            : base(theGame, "background", 1.0f)
        {
            LABELWIDTH = SEC1WIDTH / 2 - MARGIN;
            player = null;
            wonder = null;
            seatVisuals = new Dictionary<int, Dictionary<string, Visual>>();
            baseVisuals = new Dictionary<String, Visual>();
            baseVisuals.Add("Label1", new Visual(game, new Vector2(MARGIN, MARGIN), LABELWIDTH, LABELHEIGHT, "Players", "Font1", null, Color.Gray, "grayback"));
            baseVisuals.Add("Label2", new Visual(game, new Vector2(MARGIN+ LABELWIDTH, MARGIN), LABELWIDTH, LABELHEIGHT, "icons"));
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
                baseVisuals.Add("player" + p.getSeat(), new Visual(game, new Vector2(MARGIN, MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), DROPDOWNWIDTH, LABELHEIGHT, 
                                                                p.getName(), "Font1", Color.White, (p.getSeat() == player.getSeat())? Color.Orange : Color.Gray, "grayback"));
                baseVisuals.Add("status" + p.getSeat(), new Visual(game, new Vector2(MARGIN + LABELWIDTH, MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), LABELWIDTH, LABELHEIGHT, "blank"));
                baseVisuals.Add("gear" + p.getSeat(), new Visual(game, new Vector2(MARGIN + LABELWIDTH + (LABELWIDTH * 1 / 24), MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), p.getScoreNum(Score.GEAR).ToString(), "Font1"));
                baseVisuals.Add("tablet" + p.getSeat(), new Visual(game, new Vector2(MARGIN + LABELWIDTH + (LABELWIDTH * 5 / 24), MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), p.getScoreNum(Score.TABLET).ToString(), "Font1"));
                baseVisuals.Add("compas" + p.getSeat(), new Visual(game, new Vector2(MARGIN + LABELWIDTH + (LABELWIDTH * 9 / 24), MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), p.getScoreNum(Score.COMPASS).ToString(), "Font1"));
                int i;
                baseVisuals.Add("victory" + p.getSeat(), new Visual(game, new Vector2(MARGIN + LABELWIDTH + (LABELWIDTH * 13 / 24), MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), ((i = p.getScoreNum(Score.VICTORY_BLUE)) < 10)?  "0" + i : i.ToString(), "Font1"));
                baseVisuals.Add("army" + p.getSeat(), new Visual(game, new Vector2(MARGIN + LABELWIDTH + (LABELWIDTH * 19 / 24), MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), ((i = p.getScoreNum(Score.ARMY)) < 10) ? "0" + i : i.ToString(), "Font1"));

            }

            int LABELLENGTH = (SEC1WIDTH - 2 * MARGIN) / 5;
            int RESOURCELENGTH = (SEC1WIDTH - 2 * MARGIN) * 4 / 5;
            baseVisuals.Add("resources", new Visual(game, new Vector2(LABELLENGTH + MARGIN, SEC1HEIGHT + MARGIN * 2), RESOURCELENGTH, LABELHEIGHT, "resourceBar"));
            baseVisuals.Add("east", new Visual(game, new Vector2(MARGIN, SEC1HEIGHT + LABELHEIGHT + MARGIN * 3), LABELLENGTH, LABELHEIGHT, "East", "Font1", null, null, "grayback"));
            baseVisuals.Add("west", new Visual(game, new Vector2(MARGIN, SEC1HEIGHT + LABELHEIGHT * 2 + MARGIN * 4), LABELLENGTH, LABELHEIGHT, "West", "Font1", null, null, "grayback"));
            baseVisuals.Add("eastresources", new Visual(game, new Vector2(LABELLENGTH + MARGIN, SEC1HEIGHT + LABELHEIGHT + MARGIN * 3), RESOURCELENGTH, LABELHEIGHT, "emptyResourceBar"));
            baseVisuals.Add("westresources", new Visual(game, new Vector2(LABELLENGTH + MARGIN, SEC1HEIGHT + LABELHEIGHT * 2 + MARGIN * 4), RESOURCELENGTH, LABELHEIGHT, "emptyResourceBar"));

            int westSeat = (player.getSeat() - 1 < 0) ? Game1.client.getState().getPlayers().Count - 1 : player.getSeat() - 1;
            int eastSeat = (player.getSeat() + 1 > Game1.client.getState().getPlayers().Count - 1) ? 0 : player.getSeat() + 1;
            Player east = Game1.client.getState().getPlayers().Values.ElementAt(eastSeat);
            Player west = Game1.client.getState().getPlayers().Values.ElementAt(westSeat);

            int RESOURCEHEIGHT = SEC1HEIGHT + LABELHEIGHT + MARGIN * 3;

            baseVisuals.Add("east1", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 0 / 8 + 5, RESOURCEHEIGHT), east.getResourceNum(Resource.CLAY) + "", "Font1"));
            baseVisuals.Add("east2", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 1 / 8 + 5, RESOURCEHEIGHT), east.getResourceNum(Resource.STONE) + "", "Font1"));
            baseVisuals.Add("east3", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 2 / 8 + 5, RESOURCEHEIGHT), east.getResourceNum(Resource.WOOD) + "", "Font1"));
            baseVisuals.Add("east4", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 3 / 8 + 5, RESOURCEHEIGHT), east.getResourceNum(Resource.ORE) + "", "Font1"));
            baseVisuals.Add("east5", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 4 / 8 + 5, RESOURCEHEIGHT), east.getResourceNum(Resource.GLASS) + "", "Font1"));
            baseVisuals.Add("east6", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 5 / 8 + 5, RESOURCEHEIGHT), east.getResourceNum(Resource.PAPYRUS) + "", "Font1"));
            baseVisuals.Add("east7", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 6 / 8 + 5, RESOURCEHEIGHT), east.getResourceNum(Resource.LOOM) + "", "Font1"));
            baseVisuals.Add("east8", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 7 / 8 + 5, RESOURCEHEIGHT), east.getScoreNum(Score.COIN) + "", "Font1"));

            baseVisuals.Add("west1", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 0 / 8 + 5, RESOURCEHEIGHT + LABELHEIGHT + MARGIN), west.getResourceNum(Resource.CLAY) + "", "Font1"));
            baseVisuals.Add("west2", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 1 / 8 + 5, RESOURCEHEIGHT + LABELHEIGHT + MARGIN), west.getResourceNum(Resource.STONE) + "", "Font1"));
            baseVisuals.Add("west3", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 2 / 8 + 5, RESOURCEHEIGHT + LABELHEIGHT + MARGIN), west.getResourceNum(Resource.WOOD) + "", "Font1"));
            baseVisuals.Add("west4", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 3 / 8 + 5, RESOURCEHEIGHT + LABELHEIGHT + MARGIN), west.getResourceNum(Resource.ORE) + "", "Font1"));
            baseVisuals.Add("west5", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 4 / 8 + 5, RESOURCEHEIGHT + LABELHEIGHT + MARGIN), west.getResourceNum(Resource.GLASS) + "", "Font1"));
            baseVisuals.Add("west6", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 5 / 8 + 5, RESOURCEHEIGHT + LABELHEIGHT + MARGIN), west.getResourceNum(Resource.PAPYRUS) + "", "Font1"));
            baseVisuals.Add("west7", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 6 / 8 + 5, RESOURCEHEIGHT + LABELHEIGHT + MARGIN), west.getResourceNum(Resource.LOOM) + "", "Font1"));
            baseVisuals.Add("west8", new Visual(game, new Vector2(LABELLENGTH + MARGIN + RESOURCELENGTH * 7 / 8 + 5, RESOURCEHEIGHT + LABELHEIGHT + MARGIN), west.getScoreNum(Score.COIN) + "", "Font1"));

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
