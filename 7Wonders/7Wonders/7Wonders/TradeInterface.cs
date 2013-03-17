using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace _7Wonders
{
    public class TradeInterface : Interface
    {
        private const int MARGIN = 5;
        private const float CARDRATIO = 1.612f;
        private int CARDHEIGHT;
        private int CARDWIDTH;
        private int SECTIONWIDTH;

        private static int RSIZE;

        private Button buildCard;
        private Button buildStage;
        private Button sellCard;
        private Button close;
        private Button back;
        private Button build;

        private Visual card;
        private Dictionary<string, Visual> visuals1;
        private Dictionary<string, Visual> trade;
        private static Dictionary<Resource, int> cost;
        private static Dictionary<Visual, Resource> requirements;
        private static Dictionary<Resource, TradeHelper> westHelpers;
        private static Dictionary<Resource, TradeHelper> eastHelpers;
        private int cardSpot = 0;
        private bool disableBuild = false;
        private bool buildingCard = false;
        private bool needtrade = false;
        private int cardCost = 0;
        Player self;
        Player west;
        Player east;

        static int eastCoin = 0;
        static int westCoin = 0;

        public TradeInterface()
            : base("bg", new Vector2(Game1.WIDTH / 3, Game1.HEIGHT / 6), Game1.WIDTH * 2 / 3, Game1.HEIGHT * 2 / 3)
        {
            CARDHEIGHT = height - MARGIN * 2;
            CARDWIDTH = (int) (CARDHEIGHT / CARDRATIO) - MARGIN * 2;
            SECTIONWIDTH = (width - MARGIN * 4) / 3;
            RSIZE = (int)(height * 0.054f);

            self = Game1.client.getSelf();
            west = Game1.client.westPlayer(self);
            east = Game1.client.eastPlayer(self);

            visuals1 = new Dictionary<string, Visual>();
            trade = new Dictionary<string, Visual>();
            requirements = new Dictionary<Visual, Resource>();

            card = new Visual(new Vector2(pos.X + MARGIN, pos.Y + MARGIN), CARDWIDTH, CARDHEIGHT, null);
            buildCard = new Button(new Vector2(pos.X + width - 150, pos.Y + MARGIN + height * 0 / 4), 100, 50, "Card", "Font1");
            buildStage = new Button(new Vector2(pos.X + width - 150, pos.Y + MARGIN * 2 + height * 1 / 4), 100, 50, "Stage", "Font1");
            sellCard = new Button(new Vector2(pos.X + width - 150, pos.Y + MARGIN * 3 + height * 2 / 4), 100, 50, "Sell", "Font1");
            close = new Button(new Vector2(pos.X + width - 150, pos.Y + MARGIN * 4 + height * 3 / 4), 100, 50, "Close", "Font1");

            back = new Button(new Vector2(pos.X + width / 5, pos.Y + height * 9 / 10), 100, 40, "Back", "Font1");
            build = new Button(new Vector2(pos.X + width * 3 / 5, pos.Y + height * 9 / 10), 100, 40, "Build", "Font1");

            visuals1.Add("buildcard", buildCard);
            visuals1.Add("buildstage", buildStage);
            visuals1.Add("sellCard", sellCard);
            visuals1.Add("card", card);
            visuals1.Add("close", close);

            trade.Add("discount", new Visual(new Vector2(pos.X + MARGIN * 2, pos.Y + MARGIN), "Requirements:", "Font1", Color.Black));
            trade.Add("costborder", new Visual(new Vector2(pos.X + MARGIN - 1, pos.Y + MARGIN - 1), SECTIONWIDTH + 2, (int)(RSIZE * 2.5f) + 2, "border").setBorder(false));
            trade.Add("label1", new Visual(new Vector2(pos.X + MARGIN * 2, pos.Y + (int)(RSIZE * 2.5) + MARGIN * 2), "West", "Font1", Color.Black));
            trade.Add("border1", new Visual(new Vector2(pos.X + MARGIN - 1, pos.Y + (int)(RSIZE * 2.5) + MARGIN * 2 - 1), SECTIONWIDTH + 2, (int)(RSIZE * 1.5f) + 2, "border").setBorder(false));
            trade.Add("label2", new Visual(new Vector2(pos.X + SECTIONWIDTH + MARGIN * 3, pos.Y + (int)(RSIZE * 2.5) + MARGIN * 2), "Self", "Font1", Color.Black));
            trade.Add("border2", new Visual(new Vector2(pos.X + SECTIONWIDTH + MARGIN * 2 - 1, pos.Y + (int)(RSIZE * 2.5) + MARGIN * 2 - 1), SECTIONWIDTH + 2, (int)(RSIZE * 1.5f) + 2, "border").setBorder(false));
            trade.Add("label3", new Visual(new Vector2(pos.X + (SECTIONWIDTH + MARGIN) * 2 + MARGIN * 2, pos.Y + (int)(RSIZE * 2.5) + MARGIN * 2), "East", "Font1", Color.Black));
            trade.Add("border3", new Visual(new Vector2(pos.X + (SECTIONWIDTH + MARGIN) * 2 + MARGIN - 1, pos.Y + (int)(RSIZE * 2.5) + MARGIN * 2 - 1), SECTIONWIDTH + 2, (int)(RSIZE * 1.5f) + 2, "border").setBorder(false));

            trade.Add("border4", new Visual(new Vector2(pos.X + (SECTIONWIDTH + MARGIN) * 0 + MARGIN - 1, pos.Y + RSIZE * 4 + MARGIN * 3 - 1), SECTIONWIDTH + 2, RSIZE * 6 + 2, "border").setBorder(false));
            trade.Add("border5", new Visual(new Vector2(pos.X + (SECTIONWIDTH + MARGIN) * 1 + MARGIN - 1, pos.Y + RSIZE * 4 + MARGIN * 3 - 1), SECTIONWIDTH + 2, RSIZE * 6 + 2, "border").setBorder(false));
            trade.Add("border6", new Visual(new Vector2(pos.X + (SECTIONWIDTH + MARGIN) * 2 + MARGIN - 1, pos.Y + RSIZE * 4 + MARGIN * 3 - 1), SECTIONWIDTH + 2, RSIZE * 6 + 2, "border").setBorder(false));

            westHelpers = new Dictionary<Resource, TradeHelper>();
            eastHelpers = new Dictionary<Resource, TradeHelper>();
            foreach (Resource r in Enum.GetValues(typeof(Resource))){
                if (r != Resource.COIN)
                {
                    westHelpers.Add(r, new TradeHelper(r, 3, new Vector2(pos.X + MARGIN, pos.Y + ((int)r + 4) * RSIZE + MARGIN * 3)));
                    eastHelpers.Add(r, new TradeHelper(r, 3, new Vector2(pos.X + SECTIONWIDTH * 2+ MARGIN * 3, pos.Y + ((int)r + 4) * RSIZE + MARGIN * 3)));
                }

            }

            trade.Add("back", back);
            trade.Add("build", build);

            trade.Add("rwest", new Visual(new Vector2(pos.X + width * 1 / 3 + MARGIN, pos.Y + MARGIN), 110, 45, "rwest"));
            trade.Add("reast", new Visual(new Vector2(pos.X + width * 1 / 3 + MARGIN, pos.Y + MARGIN), 110, 45, "reast"));
            trade.Add("rboth", new Visual(new Vector2(pos.X + width * 1 / 3 + MARGIN, pos.Y + MARGIN), 110, 45, "rboth"));
            trade.Add("mboth", new Visual(new Vector2(pos.X + width * 2 / 3 + MARGIN * 2, pos.Y + MARGIN), 110, 45, "mboth"));

            for (int i = 0; i < 7; i++)
            {
                requirements.Add(new Visual(new Vector2(pos.X + MARGIN * 2 + RSIZE * i, pos.Y + MARGIN + 30), RSIZE, RSIZE, null).setVisible(false), Resource.CLAY);
            }

            activeVisuals = visuals1;
            hideTrade();
        }

        public void showTrade(string image, int _cardSpot)
        {
            cardSpot = _cardSpot;
            card.setTexture(image);
            foreach (Visual v in activeVisuals.Values)
                v.setVisible(true);

            cardCost = Game1.client.constructCost(image);
            if (CardLibrary.getCard(image).cost.ContainsKey(Resource.COIN))
            {
                needtrade = false;
            }

            if (cardCost < 0) disableBuild = true;
            else if (cardCost == 0) needtrade = false;
            else needtrade = true;
            
        }

        public void hideTrade()
        {
            foreach (Visual v in activeVisuals.Values)
                v.setVisible(false);
            if (buildingCard)
            {
                foreach (Visual v in requirements.Keys)
                {
                    v.setVisible(false);
                }
                foreach (TradeHelper t in westHelpers.Values)
                {
                    t.setVisible(false);
                }
                foreach (TradeHelper t in eastHelpers.Values)
                {
                    t.setVisible(false);
                }
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();
            foreach (Visual v in visuals1.Values)
                v.LoadContent();
            foreach (Visual v in trade.Values)
                v.LoadContent();
            foreach (TradeHelper t in westHelpers.Values)
                t.LoadContent();
            foreach (TradeHelper t in eastHelpers.Values)
                t.LoadContent();
        }

        public override void Update(GameTime gameTime, MouseState mouseState)
        {
            base.Update(gameTime, mouseState);

            if (buildCard.isClicked())
            {
                buildCard.reset();
                if (!disableBuild)
                {
                    buildCard.reset();
                    if (needtrade)
                    {
                        buildingCard = true;
                        buildTrade();
                        activeVisuals = trade;
                    }
                    else
                    {
                        Game1.client.playCard(new Dictionary<string, ActionType>() { { card.getTexture(), ActionType.BUILD_CARD } }, 0, 0);
                        finished = true;
                    }
                }
            }

            if (buildStage.isClicked())
            {
                //HACKS
                //Game1.client.getSelf().getHand().RemoveAt(cardSpot);
                //
                buildStage.reset();
                //buildingCard = false;
                //finished = true;
            }

            if (sellCard.isClicked())
            {
                Game1.client.playCard(new Dictionary<string, ActionType>() { { card.getTexture(), ActionType.SELL_CARD } }, 0, 0);
                sellCard.reset();
                finished = true;
            }

            if (close.isClicked())
            {
                close.reset();
                finished = true;
            }

            if (back.isClicked())
            {
                back.reset();
                buildingCard = false;
                activeVisuals = visuals1;
            }

            if (build.isClicked())
            {
                build.reset();
                if (isComplete())
                {
                    Game1.client.playCard(new Dictionary<string, ActionType>() { { card.getTexture(), ActionType.BUILD_CARD } }, westCoin, eastCoin);
                    activeVisuals = visuals1;
                    hideTrade();
                    finished = true;
                }
            }

            if (buildingCard)
            {
                foreach (Visual v in requirements.Keys)
                    v.Update(gameTime, mouseState);
                foreach (TradeHelper t in westHelpers.Values)
                    t.Update(gameTime, mouseState);
                foreach (TradeHelper t in eastHelpers.Values)
                    t.Update(gameTime, mouseState);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (buildingCard)
            {
                foreach (Visual v in requirements.Keys)
                    v.Draw(gameTime, spriteBatch);
                foreach (TradeHelper t in westHelpers.Values)
                    t.Draw(gameTime, spriteBatch);
                foreach (TradeHelper t in eastHelpers.Values)
                    t.Draw(gameTime, spriteBatch);
            }
        }

        private bool isComplete()
        {
            foreach (int i in cost.Values)
            {
                if (i > 0) return false;
            }
            return true;
        }

        public override Dictionary<string, string> isFinished()
        {
            if (finished)
                return new Dictionary<string, string>();
            return null;
        }

        public override void reset()
        {
            base.reset();
            disableBuild = false;
        }

        private void buildTrade()
        {
            foreach (Visual v in requirements.Keys)
            {
                v.setVisible(false);
            }
            foreach (TradeHelper t in westHelpers.Values)
            {
                t.setVisible(false);
                t.reset();
            }
            foreach (TradeHelper t in eastHelpers.Values)
            {
                t.setVisible(false);
                t.reset();
            }
            int i = 0;
            int w = 0;
            int e = 0;
            cost = ConstructionUtils.outsourcedCosts(self, CardLibrary.getCard(card.getTexture()).cost);
            foreach (KeyValuePair<Resource, int> kp in cost)
            {
                for (int j = 0; j < kp.Value; j++)
                {
                    Visual v = requirements.Keys.ElementAt(i).setTexture(kp.Key.ToString()).setVisible(true);
                    requirements[v] = kp.Key;
                    i++;
                }

                if (west.getResourceNum(kp.Key) > 0)
                {
                    westHelpers[kp.Key].setY((int)pos.Y + (w + 4) * RSIZE + MARGIN * 3);
                    westHelpers[kp.Key].setMax(Math.Min(west.getResourceNum(kp.Key), kp.Value));
                    westHelpers[kp.Key].setVisible(true);
                    w++;
                }
                if (east.getResourceNum(kp.Key) > 0)
                {
                    eastHelpers[kp.Key].setY((int)pos.Y + (e + 4) * RSIZE + MARGIN * 3);
                    eastHelpers[kp.Key].setMax(Math.Min(east.getResourceNum(kp.Key), kp.Value));
                    eastHelpers[kp.Key].setVisible(true);
                    e++;
                }
                
            }
        }

        private static void updateCost()
        {
            foreach (Visual v in requirements.Keys)
            {
                v.setVisible(false);
            }
            int i = 0;
            foreach (KeyValuePair<Resource, int> kp in cost)
            {
                for (int j = 0; j < kp.Value; j++)
                {
                    Visual v = requirements.Keys.ElementAt(i).setTexture(kp.Key.ToString()).setVisible(true);
                    requirements[v] = kp.Key;
                    i++;
                }
            }
            
            westCoin= 0;
            foreach (KeyValuePair<Resource, TradeHelper> kp in westHelpers)
            {
                if (kp.Key >= Resource.ORE) westCoin += kp.Value.getValue() * Game1.client.getSelf().mcost;
                else westCoin += kp.Value.getValue() * Game1.client.getSelf().rcostWest;
            }
            eastCoin = 0;
            foreach (KeyValuePair<Resource, TradeHelper> kp in eastHelpers)
            {
                if (kp.Key >= Resource.ORE) eastCoin += kp.Value.getValue() * Game1.client.getSelf().mcost;
                else eastCoin += kp.Value.getValue() * Game1.client.getSelf().rcostEast;
            }
        }

        private class TradeHelper
        {
            private int max;
            private int num = 0;
            private Visual resource;
            private Button plus;
            private Button minus;
            private Visual total;
            private Resource r;

            public TradeHelper(Resource _r, int _max, Vector2 position)
            {
                r = _r;
                max = _max;
                resource = new Visual(position, RSIZE, RSIZE, _r.ToString()).setBorder(false);
                plus = new Button(position + new Vector2(RSIZE + MARGIN, 0), RSIZE, RSIZE, null, null, "plus", false);
                minus = new Button(position + new Vector2((RSIZE + MARGIN) * 2, 0), RSIZE, RSIZE, null, null, "minus", false);
                total = new Visual(position + new Vector2((RSIZE + MARGIN) * 3, 0), RSIZE, RSIZE, num.ToString(), "Font1", null, Color.Gray, "line", 0).setBorder(false);
                setVisible(false);
            }

            public void LoadContent()
            {
                resource.LoadContent();
                plus.LoadContent();
                minus.LoadContent();
                total.LoadContent();
            }

            public void setY(int y)
            {
                resource.setPosition(new Vector2(resource.getPosition().X, y));
                plus.setPosition(new Vector2(plus.getPosition().X, y));
                minus.setPosition(new Vector2(minus.getPosition().X, y));
                total.setPosition(new Vector2(total.getPosition().X, y));
            }

            public void reset()
            {
                num = 0;
            }

            public void setMax(int _max)
            {
                num = 0;
                max = _max;
            }

            public int getValue()
            {
                return int.Parse(total.getString());
            }

            public void Update(GameTime gameTime, MouseState mouseState)
            {
                resource.Update(gameTime, mouseState);
                plus.Update(gameTime, mouseState);
                minus.Update(gameTime, mouseState);
                total.Update(gameTime, mouseState);

                if (plus.isClicked())
                {
                    plus.reset();
                    if ((num < max)&&(cost[r] > 0))
                    {
                        cost[r]--;
                        num++;
                        total.setString(num.ToString());
                        updateCost();
                    }                    
                }

                if (minus.isClicked())
                {
                    minus.reset();
                    if (num > 0)
                    {
                        cost[r]++;
                        num--;
                        total.setString(num.ToString());
                        updateCost();
                    }
                }
            }

            public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
            {
                resource.Draw(gameTime, spriteBatch);
                plus.Draw(gameTime, spriteBatch);
                minus.Draw(gameTime, spriteBatch);
                total.Draw(gameTime, spriteBatch);
            }

            public void setVisible(bool _visible)
            {
                resource.setVisible(_visible);
                plus.setVisible(_visible);
                minus.setVisible(_visible);
                total.setVisible(_visible);
            }

            public List<Visual> getVisuals()
            {
                return new List<Visual>() { resource, plus, minus, total };
            }
        }
    }
}
