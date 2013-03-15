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

        private Button buildCard;
        private Button buildStage;
        private Button sellCard;
        private Button close;
        private Button back;
        private Button build;

        private Visual card;
        private Dictionary<string, Visual> visuals1;
        private Dictionary<string, Visual> trade;
        private int cardSpot = 0;
        private bool disableBuild = false;
        private bool buildingCard = true;

        public TradeInterface(Game1 theGame)
            : base(theGame, "bg", new Vector2(Game1.WIDTH/6, Game1.HEIGHT/6), Game1.WIDTH * 2/3, Game1.HEIGHT * 2/3)
        {
            CARDHEIGHT = height - MARGIN * 2;
            CARDWIDTH = (int) (CARDHEIGHT / CARDRATIO) - MARGIN * 2;

            visuals1 = new Dictionary<string, Visual>();
            trade = new Dictionary<string, Visual>();

            card = new Visual(theGame, new Vector2(pos.X + MARGIN, pos.Y + MARGIN), CARDWIDTH, CARDHEIGHT, null);
            buildCard = new Button(theGame, new Vector2(pos.X + width - 150, pos.Y + MARGIN + height * 0 / 4), 100, 50, "Card", "Font1");
            buildStage = new Button(theGame, new Vector2(pos.X + width - 150, pos.Y + MARGIN * 2 + height * 1 / 4), 100, 50, "Stage", "Font1");
            sellCard = new Button(theGame, new Vector2(pos.X + width - 150, pos.Y + MARGIN * 3 + height * 2 / 4), 100, 50, "Sell", "Font1");
            close = new Button(theGame, new Vector2(pos.X + width - 150, pos.Y + MARGIN * 4 + height* 3/4), 100, 50, "Close", "Font1");

            back = new Button(theGame, new Vector2(pos.X + width/5, pos.Y + height * 4 / 5), 100, 40, "Back", "Font1");
            build = new Button(theGame, new Vector2(pos.X + width * 3/5, pos.Y + height * 4 / 5), 100, 40, "Build", "Font1");

            visuals1.Add("buildcard", buildCard);
            visuals1.Add("buildstage", buildStage);
            visuals1.Add("sellCard", sellCard);
            visuals1.Add("card", card);
            visuals1.Add("close", close);

            trade.Add("discount", new Visual(theGame, new Vector2(pos.X + MARGIN * 2, pos.Y + MARGIN), "Requirements:", "Font1", Color.Black));
            trade.Add("cost", new Visual(theGame, new Vector2(pos.X + MARGIN, pos.Y + MARGIN), width/3 - MARGIN * 2, height / 5 - MARGIN * 2, "border").setBorder(false));
            trade.Add("back", back);
            trade.Add("build", build);

            trade.Add("rwest", new Visual(theGame, new Vector2(pos.X + width * 1 / 3, pos.Y + MARGIN), 110, 45, "rwest"));
            trade.Add("reast", new Visual(theGame, new Vector2(pos.X + width * 1 / 3, pos.Y + MARGIN), 110, 45, "reast"));
            trade.Add("rboth", new Visual(theGame, new Vector2(pos.X + width * 1 / 3, pos.Y + MARGIN), 110, 45, "rboth"));
            trade.Add("mboth", new Visual(theGame, new Vector2(pos.X + width * 2 / 3, pos.Y + MARGIN), 110, 45, "mboth"));

            activeVisuals = visuals1;
            hideTrade();
        }

        public void showTrade(string image, int _cardSpot)
        {
            cardSpot = _cardSpot;
            card.setTexture(image);
            foreach (Visual v in activeVisuals.Values)
                v.setVisible(true);
            if (Game1.client.constructCost(Game1.client.getSelf().getHand()[cardSpot].getImageId()) < 0) disableBuild = true;
        }

        public void hideTrade()
        {
            foreach (Visual v in activeVisuals.Values)
                v.setVisible(false);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            foreach (Visual v in visuals1.Values)
                v.LoadContent();
            foreach (Visual v in trade.Values)
                v.LoadContent();
        }

        public override void Update(GameTime gameTime, MouseState mouseState)
        {
            base.Update(gameTime, mouseState);

            if (buildCard.isClicked())
            {
                buildCard.reset();
                if (!disableBuild)
                {
                    //HACKS
                    //int cost = Game1.client.constructCost(Game1.client.getSelf().getHand()[cardSpot].getImageId());
                    //Game1.client.getSelf().addPlayed(Game1.client.getSelf().getHand()[cardSpot]);
                    //Game1.client.getSelf().getHand().RemoveAt(cardSpot);
                    //Game1.client.getSelf().addResource(Resource.COIN, -cost);
                    //
                    buildCard.reset();
                    buildingCard = true;
                    buildTrade();
                    activeVisuals = trade;
                }
            }

            if (buildStage.isClicked())
            {
                //HACKS
                Game1.client.getSelf().getHand().RemoveAt(cardSpot);
                //
                buildStage.reset();
                buildingCard = false;
                finished = true;
            }

            if (sellCard.isClicked())
            {
                //HACKS
                Game1.client.getSelf().getHand().RemoveAt(cardSpot);
                Game1.client.getSelf().addResource(Resource.COIN, 3);
                //
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
                activeVisuals = visuals1;
            }

            if (build.isClicked())
            {
                build.reset();
                //Check if cost is met
                //activeVisuals = visuals1;
                //finished = true;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
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
        }
    }
}
