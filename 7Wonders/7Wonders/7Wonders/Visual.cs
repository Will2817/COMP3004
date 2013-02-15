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
    class Visual
    {
        protected Game1 game;
        protected string texture;
        protected Texture2D image;

        protected Vector2 position;
        protected int imageWidth;
        protected int imageHeight;

        public Visual(Game1 theGame, Vector2 _pos, int _w, int _h, string _text)
        {
            game = theGame;
            position = _pos;
            imageWidth = _w;
            imageHeight = _h;
            texture = _text;
        }

        public void setPosition(Vector2 _vec)
        {
            position = _vec;
        }

        public void setWidth(int _w)
        {
            imageWidth = _w;
        }

        public void setHeight(int _h)
        {
            imageHeight = _h;
        }
        public virtual void LoadContent()
        {
            image = game.Content.Load<Texture2D>(texture);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, imageWidth, imageHeight), new Rectangle(0, 0, image.Width, image.Height), Color.White);
        }
    }
}
