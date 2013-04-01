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
    public class Interface : Observer
        {
        protected Dictionary<String, Visual> activeVisuals;
        protected string backgroundText;
        protected Texture2D background;
        protected bool finished = false;
        protected string nextInterface = ""; 
        protected float dim;
        protected Vector2 pos = Vector2.Zero;
        protected int width = Game1.WIDTH;
        protected int height = Game1.HEIGHT;

        public Interface(string _backText, float? _dim = null)
        {
            backgroundText = _backText;
            activeVisuals = new Dictionary<string,Visual>();
            if (_dim.HasValue) dim = _dim.Value;
            else dim = 1;
        }

        public Interface(string _backText, Vector2 _pos, int _width, int _height, float _dim = 1)
        {
            backgroundText = _backText;
            activeVisuals = new Dictionary<string, Visual>();
            pos = _pos;
            width = _width;
            height = _height;
            dim = _dim;
        }

        public virtual void LoadContent()
        {
            if (Game1.textures.ContainsKey(backgroundText))
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
            if (background != null) spriteBatch.Draw(background, new Rectangle((int)pos.X, (int)pos.Y, width, height), Color.White * dim);
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

        public virtual void receiveMessage(Dictionary<string, string> message)
        {
        }

        public virtual void reset()
        {
            finished = false;
        }

        public virtual void stateUpdate(GameState gameState,int code)
        {

        }
    }
}

