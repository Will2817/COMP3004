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
    public abstract class Interface
        {
        protected Game1 game;
        protected static Dictionary<String, Visual> activeVisuals;
        protected string backgroundText;
        protected Texture2D background;
        protected bool finished =false;

        public Interface(Game1 theGame, string _backText)
        {
            game = theGame;
            backgroundText = _backText;
            activeVisuals = new Dictionary<string,Visual>();
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
            spriteBatch.Draw(background, new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.White);
            foreach (Visual v in activeVisuals.Values)
            {
                v.Draw(gameTime, spriteBatch);
            }
        }

        public virtual Dictionary<string, string> isFinished()
        {
            return new Dictionary<string, string>();
        }
    }
}

