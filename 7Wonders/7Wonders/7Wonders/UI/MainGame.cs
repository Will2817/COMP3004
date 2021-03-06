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
        private const int MAXHANDSIZE = 7;
        private int NUMRESOURCES = Enum.GetNames(typeof(Resource)).Length;
        private int CARDWIDTH;
        private int CARDHEIGHT; 
        private int SEC1WIDTH = Game1.WIDTH / 3;
        private int WONDERHEIGHT = (Game1.HEIGHT - 10) / 6;
        private int WONDERWIDTH = Game1.WIDTH / 3 - 10;
        private int SEC1HEIGHT = Game1.HEIGHT * 2 / 3;
        private int DROPDOWNWIDTH = (Game1.WIDTH / 3) - 135;
        private int DROPDOWNHEIGHT = (Game1.HEIGHT / 2 - (Game1.MAXPLAYER + 1) * MARGIN) / Game1.MAXPLAYER;
        private const int LABELHEIGHT = 35;
        private int LABELWIDTH;
        private int LABELLENGTH;
        private int RESOURCELENGTH;

        private int SCOREWIDTH = (int)(Game1.WIDTH / 2 * 0.62f / 7);

        private TradeInterface trade;
        private Button quit;

        protected Dictionary<int, Dictionary<string, Visual>> seatVisuals;
        protected Dictionary<string, Visual> baseVisuals;
        protected SortedDictionary<string, Visual> hand;
        protected Dictionary<string, Visual> lastPlayed;
        protected Button close;

        protected bool showhand = false;
        protected int seatViewed = 0;
        protected Player player;
        protected Player east;
        protected Player west;
        protected Visual wonder;
        protected Button leftButton;
        protected bool showTrade = false;
        protected bool showScore = false;
        protected bool showLastTurn = false;
        protected bool init = false;

        protected MouseState mousestate;

        public MainGame(Game1 theGame)
            : base("background", 1.0f)
        {
            LABELWIDTH = SEC1WIDTH / 2 - MARGIN;
            CARDWIDTH = (Game1.WIDTH - 8 * (MARGIN * 2) - 30) / MAXHANDSIZE;
            CARDHEIGHT = (int) (CARDWIDTH * 1.612);
            LABELLENGTH = (SEC1WIDTH - MARGIN) / 5;
            RESOURCELENGTH = (SEC1WIDTH - 3 * MARGIN) * 4 / 5;
            player = null;
            wonder = null;
            lastPlayed = new Dictionary<string, Visual>();
            hand = new SortedDictionary<string, Visual>();
            leftButton = new Button(new Vector2(Game1.WIDTH - 27, 200 + CARDHEIGHT / 2 - 7), 15, 15, "", "Font1", "left");
            leftButton.z = 1;
            hand.Add("paperleft", new Visual(new Vector2(Game1.WIDTH - 27, 190), 30, CARDHEIGHT + 30, "paperleft"));
            hand.Add("leftButton", leftButton.setBorder(false));

            close = new Button(new Vector2(Game1.WIDTH - Game1.WIDTH / 9, Game1.HEIGHT * 5/ 8), 75, 40, "Close", "Font1");

            lastPlayed.Add("bg", new Visual(new Vector2(Game1.WIDTH / 3, Game1.HEIGHT / 6), Game1.WIDTH * 2 / 3, Game1.HEIGHT * 2 / 3, "bg"));
            lastPlayed.Add("close", close);

            seatVisuals = new Dictionary<int, Dictionary<string, Visual>>();
            baseVisuals = new Dictionary<String, Visual>();
            baseVisuals.Add("Label1", new Visual(new Vector2(MARGIN, MARGIN), LABELWIDTH, LABELHEIGHT, "Players", "Font1", null, Color.Gray, "grayback"));
            baseVisuals.Add("Label2", new Visual(new Vector2(MARGIN + LABELWIDTH, MARGIN), LABELWIDTH, LABELHEIGHT, "icons"));
            baseVisuals.Add("Divider1", new Visual(new Vector2(SEC1WIDTH - 1, 0), DIVIDERWIDTH, Game1.HEIGHT, "line", Color.Silver));
            baseVisuals.Add("Divider2", new Visual(new Vector2(0, SEC1HEIGHT - 1), Game1.WIDTH, DIVIDERWIDTH, "line", Color.Silver));
            baseVisuals.Add("Age", new Visual(new Vector2(Game1.WIDTH - MARGIN - 75, MARGIN), 75, 75, "age1"));
            baseVisuals.Add("discard", new Visual(new Vector2(Game1.WIDTH - MARGIN - 60, MARGIN * 3 + 120), 60, 60, "0", "Font1", null, Color.White, "deck"));
            baseVisuals["discard"].setLeftMargin(15);
            baseVisuals["discard"].setTopMargin(9);

        }

        private void Initialize()
        {
            lock (this)
            {
                if (init) return;
                trade = new TradeInterface();
                player = Game1.client.getSelf();
                int westSeat = (player.getSeat() - 1 < 0) ? Game1.client.getState().getPlayers().Count - 1 : player.getSeat() - 1;
                int eastSeat = (player.getSeat() + 1 > Game1.client.getState().getPlayers().Count - 1) ? 0 : player.getSeat() + 1;
                east = Game1.client.getState().getPlayers().Values.ElementAt(eastSeat);
                west = Game1.client.getState().getPlayers().Values.ElementAt(westSeat);
                foreach (Player p in Game1.client.getState().getPlayers().Values)
                {
                    Visual conflict = new Visual(new Vector2(Game1.WIDTH - MARGIN - 60, MARGIN * 2 + 75), 60, 60, "0", "Font1", null, Color.White, "conflict");
                    conflict.setLeftMargin(19);
                    conflict.setTopMargin(20);
                    conflict.LoadContent();//BAD HACKS
                    Visual stages = new Visual(new Vector2(Game1.WIDTH - 100, Game1.HEIGHT - 150), 100, 100, "stage13").setVisible(false);
                    Game1.wonders[p.getBoard().getName()].setPosition(new Vector2(5 + SEC1WIDTH, 5 + SEC1HEIGHT)).setWidth(WONDERWIDTH * 2 + 10).setHeight(WONDERHEIGHT * 2).setTexture(p.getBoard().getImageName());
                    seatVisuals.Add(p.getSeat(), new Dictionary<string, Visual>() { { "wonder", Game1.wonders[p.getBoard().getName()] }, { "conflict", conflict }, { "stages", stages } });
                    baseVisuals.Add("player" + p.getSeat(), new Visual(new Vector2(MARGIN, MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), DROPDOWNWIDTH, LABELHEIGHT, (p.getSeat() + 1) + "|" + p.getName(), "Font1", Color.White, (p.getSeat() == player.getSeat()) ? Color.Orange : Color.Gray, "grayback"));
                    baseVisuals.Add("status" + p.getSeat(), new Visual(new Vector2(MARGIN + LABELWIDTH, MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), LABELWIDTH, LABELHEIGHT, "blank"));
                    baseVisuals.Add("gear" + p.getSeat(), new Visual(new Vector2(MARGIN + LABELWIDTH + (LABELWIDTH * 1 / 24), MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), p.getScoreNum(Score.GEAR).ToString(), "Font1"));
                    baseVisuals.Add("tablet" + p.getSeat(), new Visual(new Vector2(MARGIN + LABELWIDTH + (LABELWIDTH * 5 / 24), MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), p.getScoreNum(Score.TABLET).ToString(), "Font1"));
                    baseVisuals.Add("compas" + p.getSeat(), new Visual(new Vector2(MARGIN + LABELWIDTH + (LABELWIDTH * 9 / 24), MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), p.getScoreNum(Score.COMPASS).ToString(), "Font1"));
                    baseVisuals.Add("victory" + p.getSeat(), new Visual(new Vector2(MARGIN + LABELWIDTH + (LABELWIDTH * 13 / 24), MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), p.getScoreNum(Score.VICTORY_BLUE).ToString(), "Font1"));
                    baseVisuals.Add("army" + p.getSeat(), new Visual(new Vector2(MARGIN + LABELWIDTH + (LABELWIDTH * 19 / 24), MARGIN * 2 + (MARGIN + LABELHEIGHT) * (p.getSeat() + 1)), p.getScoreNum(Score.ARMY).ToString(), "Font1"));
                }
                baseVisuals.Add("resources", new Visual(new Vector2(LABELLENGTH + MARGIN * 2, SEC1HEIGHT + MARGIN * 2), RESOURCELENGTH, LABELHEIGHT, "resourceBar"));
                baseVisuals.Add("west", new Visual(new Vector2(MARGIN, SEC1HEIGHT + LABELHEIGHT + MARGIN * 3), LABELLENGTH, LABELHEIGHT, (westSeat + 1) + "|" + "West", "Font1", null, null, "grayback"));
                baseVisuals.Add("east", new Visual(new Vector2(MARGIN, SEC1HEIGHT + LABELHEIGHT * 2 + MARGIN * 4), LABELLENGTH, LABELHEIGHT, (eastSeat + 1) + "|" + "East", "Font1", null, null, "grayback"));
                baseVisuals.Add("self", new Visual(new Vector2(MARGIN, Game1.HEIGHT - (MARGIN + LABELHEIGHT)), LABELLENGTH, LABELHEIGHT, "Self", "Font1", null, null, "grayback"));
                baseVisuals.Add("westresources", new Visual(new Vector2(LABELLENGTH + MARGIN * 2, SEC1HEIGHT + LABELHEIGHT + MARGIN * 3), RESOURCELENGTH, LABELHEIGHT, "emptyResourceBar"));
                baseVisuals.Add("eastresources", new Visual(new Vector2(LABELLENGTH + MARGIN * 2, SEC1HEIGHT + LABELHEIGHT * 2 + MARGIN * 4), RESOURCELENGTH, LABELHEIGHT, "emptyResourceBar"));
                baseVisuals.Add("selfresources", new Visual(new Vector2(LABELLENGTH + MARGIN * 2, Game1.HEIGHT - (MARGIN + LABELHEIGHT)), RESOURCELENGTH, LABELHEIGHT, "emptyResourceBar"));

                int RESOURCEHEIGHT = SEC1HEIGHT + LABELHEIGHT + MARGIN * 3;

                for (int i = 0; i < NUMRESOURCES; i++)
                {
                    baseVisuals.Add("west" + i, new Visual(new Vector2(LABELLENGTH + MARGIN * 2 + RESOURCELENGTH * i / 8 + 5, RESOURCEHEIGHT), west.getResourceNum((Resource)i) + "", "Font1"));

                    baseVisuals.Add("east" + i, new Visual(new Vector2(LABELLENGTH + MARGIN * 2 + RESOURCELENGTH * i / 8 + 5, RESOURCEHEIGHT + LABELHEIGHT + MARGIN), east.getResourceNum((Resource)i) + "", "Font1"));

                    baseVisuals.Add("self" + i, new Visual(new Vector2(LABELLENGTH + MARGIN * 2 + RESOURCELENGTH * i / 8 + 5, Game1.HEIGHT - (MARGIN + LABELHEIGHT)), player.getResourceNum((Resource)i) + "", "Font1"));

                    if ((Resource)i == Resource.COIN)
                    {
                        baseVisuals["east" + i].setLeftMargin(-2);
                        baseVisuals["west" + i].setLeftMargin(-2);
                        baseVisuals["self" + i].setLeftMargin(-2);
                    }
                }
                hand.Add("papermiddle", new Visual(new Vector2(MARGIN + 30 + (CARDWIDTH / 2 + MARGIN) * (7 - player.getHand().Count) + 1, 190), Game1.WIDTH - (MARGIN + 30 + (CARDWIDTH / 2 + MARGIN) * (7 - player.getHand().Count)), CARDHEIGHT + 25, "papermiddle"));

                //updatePlayed();
                updateHands();
                updateScroll();
                //updatePlayers();

                baseVisuals.Add("Scorehead", new Visual(new Vector2(Game1.WIDTH / 4, Game1.HEIGHT / 4), Game1.WIDTH / 2, 50, "scorehead").setVisible(false));
                int n = 1;
                foreach (Player p in Game1.client.getState().getPlayers().Values)
                {
                    baseVisuals.Add("name" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH / 4, Game1.HEIGHT / 4 + 40 * n), (int)(Game1.WIDTH / 2 * 0.296f), 40, p.getName(), "Font1", null, null, "line").setVisible(false));
                    baseVisuals.Add("conflict" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH / 4 + (int)(Game1.WIDTH / 2 * 0.296f), Game1.HEIGHT / 4 + 40 * n), SCOREWIDTH, 40, p.getScoreNum(Score.CONFLICT).ToString(), "Font1", null, null, "line").setVisible(false));
                    baseVisuals.Add("coin" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH / 4 + (int)(Game1.WIDTH / 2 * 0.296f) + SCOREWIDTH, Game1.HEIGHT / 4 + 40 * n), SCOREWIDTH, 40, p.getScoreNum(Score.COIN).ToString(), "Font1", null, null, "line").setVisible(false));
                    baseVisuals.Add("stages" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH / 4 + (int)(Game1.WIDTH / 2 * 0.296f) + SCOREWIDTH * 2, Game1.HEIGHT / 4 + 40 * n), SCOREWIDTH, 40, p.getScoreNum(Score.STAGES).ToString(), "Font1", null, null, "line").setVisible(false));
                    baseVisuals.Add("blue" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH / 4 + (int)(Game1.WIDTH / 2 * 0.296f) + SCOREWIDTH * 3, Game1.HEIGHT / 4 + 40 * n), SCOREWIDTH, 40, p.getScoreNum(Score.VICTORY_BLUE).ToString(), "Font1", null, null, "line").setVisible(false));
                    baseVisuals.Add("commerce" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH / 4 + (int)(Game1.WIDTH / 2 * 0.296f) + SCOREWIDTH * 4, Game1.HEIGHT / 4 + 40 * n), SCOREWIDTH, 40, p.getScoreNum(Score.COMMERCE).ToString(), "Font1", null, null, "line").setVisible(false));
                    baseVisuals.Add("guild" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH / 4 + (int)(Game1.WIDTH / 2 * 0.296f) + SCOREWIDTH * 5, Game1.HEIGHT / 4 + 40 * n), SCOREWIDTH, 40, p.getScoreNum(Score.GUILD).ToString(), "Font1", null, null, "line").setVisible(false));
                    baseVisuals.Add("science" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH / 4 + (int)(Game1.WIDTH / 2 * 0.296f) + SCOREWIDTH * 6, Game1.HEIGHT / 4 + 40 * n), SCOREWIDTH, 40, p.getScoreNum(Score.SCIENCE).ToString(), "Font1", null, null, "line").setVisible(false));
                    baseVisuals.Add("sum" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH / 4 + (int)(Game1.WIDTH / 2 * 0.296f) + SCOREWIDTH * 7, Game1.HEIGHT / 4 + 40 * n), SCOREWIDTH, 40, p.getScoreNum(Score.VICTORY).ToString(), "Font1", null, null, "line").setVisible(false));
                    lastPlayed.Add("player" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH * ((n - 1) % 4 + 2) / 6 + MARGIN, Game1.HEIGHT * ((int)(n / 5) * 2 + 1) / 6 + MARGIN), Game1.WIDTH / 6 - MARGIN * 5, LABELHEIGHT, (p.getSeat() +1) + "|" + p.getName(), "Font1", null, null, "grayback"));
                    lastPlayed.Add("action" + p.getSeat(), new Visual(new Vector2(Game1.WIDTH * ((n - 1) % 4 + 2) / 6 + MARGIN, Game1.HEIGHT * ((int)(n / 5) * 2 + 1) / 6 + LABELHEIGHT + MARGIN * 2), Game1.WIDTH / 6 - MARGIN * 6, Game1.HEIGHT / 3 - MARGIN * 3 - LABELHEIGHT, "coin"));
                    n++; 
                }
                quit = new Button(new Vector2(Game1.WIDTH / 2 - 70, Game1.HEIGHT / 4 + 40 * n + MARGIN), 140, 50, "Quit", "Font1");
                baseVisuals.Add("quit", quit.setVisible(false));


                seatViewed = player.getSeat();
                activeVisuals = seatVisuals[seatViewed];
                LoadContent();
                init = true;
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();
            foreach (Visual v in baseVisuals.Values)
            {
                v.LoadContent();
            }
            foreach (Visual v in hand.Values)
            {
                v.LoadContent();
            }
            if (trade != null) trade.LoadContent();
            foreach (Visual v in lastPlayed.Values)
            {
                v.LoadContent();
            }
        }

        public override void Update(GameTime gameTime, MouseState mouseState)
        {
            if (showLastTurn){
                close.Update(gameTime, mouseState);
                if (close.isClicked())
                {
                    close.reset();
                    showLastTurn = false;
                }
                return;
            }
            if (showTrade)
            {
                trade.Update(gameTime, mouseState);
                Dictionary<string, string> message;
                if ((message = trade.isFinished()) != null)
                {
                    trade.reset();
                    showTrade = false;
                    if (message["completeTrade"] == "true")
                    {
                        leftButton.setEnabled(false);
                        showhand = false;
                        updateScroll();
                    }
                }

                return;
            }
            base.Update(gameTime, mouseState);
            mousestate = mouseState;
            foreach (Visual v in baseVisuals.Values)
            {
                v.Update(gameTime, mouseState);
            }

            lock (hand)
            {
                foreach (Visual v in hand.Values)
                {
                    v.Update(gameTime, mouseState);
                }


                for (int j = 0; j < MAXHANDSIZE; j++)
                {
                    if (hand.ContainsKey("hand" + j))
                    {
                        if (hand["hand" + j].isClicked())
                        {
                            hand["hand" + j].reset();

                            trade.showTrade(hand["hand" + j].getTexture(), j);
                            showTrade = true;
                        }
                    }
                }
            }

            int storeSeat = seatViewed;

            switch (Game1.recordedPresses)
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
            Game1.recordedPresses = "";

            if (storeSeat != seatViewed)
            {
                foreach (Player p in Game1.client.getState().getPlayers().Values)
                    baseVisuals["player" + p.getSeat()].setColor((p.getReady()) ? Color.Green : Color.Gray);
                baseVisuals["player" + seatViewed].setColor(Color.Orange);
            }

            if (leftButton.isClicked())
            {
                leftButton.reset();
                showhand = !showhand;
                updateScroll();
            }

            if (quit.isClicked())
            {
                finished = true;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (!showhand && !showLastTurn && !showScore)
            {
                foreach (KeyValuePair<string, Visual> kp in activeVisuals)
                {
                    if (kp.Key != "wonder" && kp.Key != "stages")
                        if (kp.Value.isMouseOver(mousestate)) kp.Value.Draw(gameTime, spriteBatch);
                }
            }

            foreach (Visual v in baseVisuals.Values)
            {
                v.Draw(gameTime, spriteBatch);
            }

            lock (hand)
            {
                foreach (Visual v in hand.Values.OrderBy(item => item.z))
                {
                    v.Draw(gameTime, spriteBatch);
                }
            }

            if (showTrade)
            {
                trade.Draw(gameTime, spriteBatch);
            }

            if (showLastTurn)
            {
                foreach (Visual v in lastPlayed.Values)
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

        public override void receiveMessage(Dictionary<string, string> message)
        {
            Initialize();
        }

        private void updateHands()
        {
            lock (hand)
            {
                for (int j = 0; j < MAXHANDSIZE; j++)
                {
                    if (hand.ContainsKey("hand" + j)) hand.Remove("hand" + j);
                    if (hand.ContainsKey("glow" + j)) hand.Remove("glow" + j);
                }


                int k = 0;
                foreach (string c in player.getHand())
                {
                    Game1.cards[c].setPosition(new Vector2(MARGIN + 40 + (CARDWIDTH + MARGIN * 2) * k + (CARDWIDTH / 2 + MARGIN) * (7 - player.getHand().Count), 205)).setWidth(CARDWIDTH).setHeight(CARDHEIGHT);
                    Game1.cards[c].z = 2;
                    hand.Add("hand" + k, Game1.cards[c]);
                    Visual v = new Visual(Game1.cards[c]).setRelativePosition(new Vector2(-1, -1)).setRelativeHeight(2).setRelativeWidth(2);
                    v.z = 3;
                    if (Game1.client.constructCost(c) == 0) v.setTexture("greenglow");
                    else if (Game1.client.constructCost(c) > 0) v.setTexture("goldglow");
                    else v.setTexture("redglow");
                    hand.Add("glow" + k, v);
                    k++;
                }
                hand["papermiddle"].setPosition(new Vector2(MARGIN + 30 + (CARDWIDTH / 2 + MARGIN) * (7 - player.getHand().Count) + 1, 190)).setWidth(Game1.WIDTH - (MARGIN + 30 + (CARDWIDTH / 2 + MARGIN) * (7 - player.getHand().Count) + 1));
            }
        }

        private void updateScroll()
        {
            if (showhand)
            {
                hand["papermiddle"].setVisible(true);
                leftButton.setTexture("right");
                leftButton.setPosition(new Vector2(MARGIN + (CARDWIDTH / 2 + MARGIN) * (7 - player.getHand().Count) + 5, 200 + CARDHEIGHT / 2 - 7));
                hand["paperleft"].setPosition(new Vector2(MARGIN + (CARDWIDTH / 2 + MARGIN) * (7 - player.getHand().Count), 190));
            }
            else
            {
                hand["papermiddle"].setVisible(false);
                leftButton.setTexture("left");
                leftButton.setPosition(new Vector2(Game1.WIDTH - 27, 200 + CARDHEIGHT / 2 - 7));
                hand["paperleft"].setPosition(new Vector2(Game1.WIDTH - 30, 190));
            }

            for (int j = 0; j < MAXHANDSIZE; j++)
            {
                if (hand.ContainsKey("hand" + j)) hand["hand" + j].setVisible(showhand);
                if (hand.ContainsKey("glow" + j)) hand["glow" + j].setVisible(showhand);
            }
        }

        private void updatePlayers(GameState gameState)
        {
            foreach (Player p in gameState.getPlayers().Values)
                baseVisuals["player" + p.getSeat()].setColor((p.getReady()) ? Color.Green : Color.Gray);
            baseVisuals["player" + seatViewed].setColor(Color.Orange);
        }

        public void updatePlayed(GameState gameState)
        {
            foreach (Player p in gameState.getPlayers().Values)
            {
                int played1 = 0;
                int played2 = 0;
                int played3 = 0;
                int played4 = 0;

                foreach (string cardID in p.getPlayed())
                {

                    Card c = CardLibrary.getCard(cardID);
                    Game1.cards[c.getImageId()].setWidth(CARDWIDTH).setHeight(CARDHEIGHT);
                    if (c.colour == CardColour.BROWN || c.colour == CardColour.GRAY)
                    {
                        if (played1 < 5)
                        {
                            Game1.cards[c.getImageId()].setPosition(new Vector2(SEC1WIDTH + MARGIN * 2, MARGIN + (MARGIN + ((int)(CARDHEIGHT * 0.25))) * played1));
                            played1++;
                        }
                        else if (played2 < 5)
                        {
                            Game1.cards[c.getImageId()].setPosition(new Vector2(SEC1WIDTH + MARGIN * 3 + CARDWIDTH, MARGIN + (MARGIN + ((int)(CARDHEIGHT * 0.25))) * played2));
                            played2++;
                        }
                        else if (played3 < 5)
                        {
                            Game1.cards[c.getImageId()].setPosition(new Vector2(SEC1WIDTH + MARGIN * 4 + CARDWIDTH * 2, MARGIN + (MARGIN + ((int)(CARDHEIGHT * 0.25))) * played3));
                            played3++;
                        }
                        else
                        {
                            Game1.cards[c.getImageId()].setPosition(new Vector2(SEC1WIDTH + MARGIN * 5 + CARDWIDTH * 3, MARGIN + (MARGIN + ((int)(CARDHEIGHT * 0.25))) * played4));
                            played4++;
                        }
                    }
                    else
                    {
                        if (played4 < 5)
                        {
                            Game1.cards[c.getImageId()].setPosition(new Vector2(SEC1WIDTH + MARGIN * 5 + CARDWIDTH * 3, MARGIN + (MARGIN + ((int)(CARDHEIGHT * 0.25))) * played4));
                            played4++;
                        }
                        else if (played3 < 5)
                        {
                            Game1.cards[c.getImageId()].setPosition(new Vector2(SEC1WIDTH + MARGIN * 4 + CARDWIDTH * 2, MARGIN + (MARGIN + ((int)(CARDHEIGHT * 0.25))) * played3));
                            played3++;
                        }
                        else if (played2 < 5)
                        {
                            Game1.cards[c.getImageId()].setPosition(new Vector2(SEC1WIDTH + MARGIN * 3 + CARDWIDTH, MARGIN + (MARGIN + ((int)(CARDHEIGHT * 0.25))) * played2));
                            played2++;
                        }
                        else
                        {
                            Game1.cards[c.getImageId()].setPosition(new Vector2(SEC1WIDTH + MARGIN * 2 + CARDWIDTH, MARGIN + (MARGIN + ((int)(CARDHEIGHT * 0.25))) * played1));
                            played1++;
                        }
                    }
                    if (!seatVisuals[p.getSeat()].ContainsKey(cardID))
                    {
                        seatVisuals[p.getSeat()].Add(c.getImageId(), Game1.cards[c.getImageId()].setVisible(true));
                    }
                }
            }
        }

        public void updateResources(GameState gameState)
        {
            for (int i = 0; i < NUMRESOURCES; i++)
            {
                baseVisuals["east" + i].setString(east.getResourceNum((Resource) i) + "");

                baseVisuals["west" + i].setString(west.getResourceNum((Resource)i) + "");

                baseVisuals["self" + i].setString(player.getResourceNum((Resource)i) + "");
            }

            foreach (Player p in gameState.getPlayers().Values)
            {
                baseVisuals["gear" + p.getSeat()].setString(p.getScoreNum(Score.GEAR).ToString());
                baseVisuals["tablet" + p.getSeat()].setString(p.getScoreNum(Score.TABLET).ToString());
                baseVisuals["compas" + p.getSeat()].setString(p.getScoreNum(Score.COMPASS).ToString());
                baseVisuals["victory" + p.getSeat()].setString(p.getScoreNum(Score.VICTORY_BLUE).ToString());
                baseVisuals["army" + p.getSeat()].setString(p.getScoreNum(Score.ARMY).ToString());
                seatVisuals[p.getSeat()]["conflict"].setString(p.getScoreNum(Score.CONFLICT).ToString());
                if (p.getBoard().getStagesBuilt() > 0) seatVisuals[p.getSeat()]["stages"].setTexture("stage" + p.getBoard().getStagesBuilt() + p.getBoard().getSide().getStageNum()).setVisible(true);
            }

            baseVisuals["Age"].setTexture("age" + gameState.getAge());
            baseVisuals["discard"].setString(gameState.getDiscards().Count+"");//get actual number of discards

        }

        public void updateLast(GameState gameState)
        {
            int n= 1;
            foreach (Player p in gameState.getPlayers().Values)
            {
                lastPlayed["action" + p.getSeat()].setPosition(new Vector2(Game1.WIDTH * ((n - 1) % 4 + 2) / 6 + MARGIN, Game1.HEIGHT * ((int)(n / 5) * 2 + 1) / 6 + LABELHEIGHT + MARGIN * 2)).setWidth(Game1.WIDTH / 6 - MARGIN * 6).setHeight(Game1.HEIGHT / 3 - MARGIN * 3 - LABELHEIGHT);
                if (p.getLastActions()[0] == ActionType.BUILD_CARD)
                    lastPlayed["action" + p.getSeat()].setTexture(p.getLastCardsPlayed()[0]);
                else if (p.getLastActions()[0] == ActionType.BUILD_WONDER)
                    lastPlayed["action" + p.getSeat()].setTexture("stage" + p.getBoard().getStagesBuilt() + p.getBoard().getSide().getStageNum());
                else
                {
                    lastPlayed["action" + p.getSeat()].setPosition(new Vector2(Game1.WIDTH * ((n - 1) % 4 + 2) / 6 + MARGIN + (Game1.WIDTH / 6 - MARGIN * 6) / 3, Game1.HEIGHT * ((int)(n / 5) * 2 + 1) / 6 + LABELHEIGHT + MARGIN * 2 + (Game1.HEIGHT / 3 - MARGIN * 3 - LABELHEIGHT)/3)).setWidth(Game1.WIDTH / 12 - MARGIN * 2).setHeight(Game1.HEIGHT / 9 - MARGIN - LABELHEIGHT / 3);
                    lastPlayed["action" + p.getSeat()].setTexture("coin");
                }
                n++;
            }
        }

        //observer code
        public override void stateUpdate(GameState gameState, UpdateType code)
        {
            Console.WriteLine("UPDATING STATE");
            if (!init)
            {
                Initialize();
            }
            if (code==UpdateType.HAND_UPDATE)
            {
                leftButton.setEnabled(true);
                showhand = false;
                updateHands();
                updateScroll();
                if (gameState.getTurn() != 1 || gameState.getAge() != 1)
                {
                    updateLast(gameState);
                    showLastTurn = true;
                }
            }
            if (code == UpdateType.PLAYER_UPDATE)
            {
                updatePlayed(gameState);
                updateResources(gameState);
                updatePlayers(gameState);
            }

            if (code == UpdateType.END_UPDATE)
            {
                hand.Clear(); //<-- bit of a hack
                showScore = true;
                baseVisuals["Scorehead"].setVisible(true);
                foreach (Player p in gameState.getPlayers().Values)
                {
                    baseVisuals["name" + p.getSeat()].setVisible(true);
                    baseVisuals["conflict" + p.getSeat()].setString(p.getScoreNum(Score.CONFLICT).ToString()).setVisible(true);
                    baseVisuals["coin" + p.getSeat()].setString(p.getScoreNum(Score.COIN).ToString()).setVisible(true);
                    baseVisuals["stages" + p.getSeat()].setString(p.getScoreNum(Score.STAGES).ToString()).setVisible(true);
                    baseVisuals["blue" + p.getSeat()].setString(p.getScoreNum(Score.VICTORY_BLUE).ToString()).setVisible(true);
                    baseVisuals["commerce" + p.getSeat()].setString(p.getScoreNum(Score.COMMERCE).ToString()).setVisible(true);
                    baseVisuals["guild" + p.getSeat()].setString(p.getScoreNum(Score.GUILD).ToString()).setVisible(true);
                    baseVisuals["science" + p.getSeat()].setString(p.getScoreNum(Score.SCIENCE).ToString()).setVisible(true);
                    baseVisuals["sum" + p.getSeat()].setString(p.getScoreNum(Score.VICTORY).ToString()).setVisible(true);
                }
                quit.setVisible(true);
            }
        }
    }
}
