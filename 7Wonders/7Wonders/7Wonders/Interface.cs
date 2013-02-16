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
    abstract class Interface
        {
        protected Game1 game;
        protected Dictionary<String, Visual> activeVisuals;
        protected string backgroundText;
        protected Texture2D background;
        protected bool finished = false;
        protected float dim;

        public Interface(Game1 theGame, string _backText, float ?_dim=null)
        {
            game = theGame;
            backgroundText = _backText;
            activeVisuals = new Dictionary<string,Visual>();
            if (_dim.HasValue) dim = _dim.Value;
            else dim = 1;
        }

        public virtual void LoadContent()
        {
            background = Game1.textures[backgroundText];
        }

        public virtual void Update(GameTime gameTime, MouseState mouseState)
        {
            foreach (Visual v in activeVisuals.Values)
            {
                v.Update(gameTime, mouseState);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.White*dim);
            foreach (Visual v in activeVisuals.Values)
            {
                v.Draw(gameTime, spriteBatch);
            }
        }

        public virtual Dictionary<string, string> isFinished()
        {
            return null;
        }

        public void setDim(int _dim)
        {
            dim = _dim;
        }
    }
}

