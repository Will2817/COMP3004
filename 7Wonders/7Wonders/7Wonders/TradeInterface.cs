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
        private Visual card;
        private Dictionary<string, Visual> visuals1;
        private int cardSpot = 0;
        private bool disableBuild = false;

        public TradeInterface(Game1 theGame)
            : base(theGame, "bg", new Vector2(Game1.WIDTH/4, Game1.HEIGHT/4), Game1.WIDTH/2, Game1.HEIGHT/2)
        {
            CARDHEIGHT = Game1.HEIGHT/2 - MARGIN * 2;
            CARDWIDTH = (int) (CARDHEIGHT / CARDRATIO) - MARGIN * 2;

            visuals1 = new Dictionary<string, Visual>();

            card = new Visual(theGame, new Vector2(pos.X + MARGIN, pos.Y + MARGIN), CARDWIDTH, CARDHEIGHT, null);
            buildCard = new Button(theGame, new Vector2(pos.X + width - 150, pos.Y + MARGIN + height * 0 / 4), 100, 50, "Card", "Font1");
            buildStage = new Button(theGame, new Vector2(pos.X + width - 150, pos.Y + MARGIN * 2 + height * 1 / 4), 100, 50, "Stage", "Font1");
            sellCard = new Button(theGame, new Vector2(pos.X + width - 150, pos.Y + MARGIN * 3 + height * 2 / 4), 100, 50, "Sell", "Font1");
            close = new Button(theGame, new Vector2(pos.X + width - 150, pos.Y + MARGIN * 4 + height* 3/4), 100, 50, "Close", "Font1");

            visuals1.Add("buildcard", buildCard);
            visuals1.Add("buildstage", buildStage);
            visuals1.Add("sellCard", sellCard);
            visuals1.Add("card", card);
            visuals1.Add("close", close);
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
                    int cost = Game1.client.constructCost(Game1.client.getSelf().getHand()[cardSpot].getImageId());
                    Game1.client.getSelf().addPlayed(Game1.client.getSelf().getHand()[cardSpot]);
                    Game1.client.getSelf().getHand().RemoveAt(cardSpot);
                    Game1.client.getSelf().addResource(Resource.COIN, -cost);
                    //
                    buildCard.reset();
                    finished = true;
                }
            }

            if (buildStage.isClicked())
            {
                //HACKS
                Game1.client.getSelf().getHand().RemoveAt(cardSpot);
                //
                buildStage.reset();
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
    }
}
