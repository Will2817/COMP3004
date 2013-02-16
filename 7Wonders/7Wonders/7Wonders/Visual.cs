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
    public class Visual
    {
        protected Game1 game;
        protected string texture;
        protected Texture2D image;

        protected Vector2 position;
        protected int width;
        protected int height;
        protected int opacity;
        protected Color color;

        public Visual(Game1 theGame, Vector2 _pos, int _w, int _h, string _text)
        {
            game = theGame;
            position = _pos;
            width = _w;
            height = _h;
            texture = _text;
            opacity = 255;
            color = new Color(255, 255, 255, opacity);
        }

        public Visual(Game1 theGame, Vector2 _pos, int _w, int _h, string _text, Color _color)
        {
            game = theGame;
            position = _pos;
            width = _w;
            height = _h;
            texture = _text;
            opacity = 255;
            color = new Color(_color.R, _color.G, _color.B, opacity);
        }

        public void setPosition(Vector2 _vec)
        {
            position = _vec;
        }

        public void setWidth(int _w)
        {
            width = _w;
        }

        public void setHeight(int _h)
        {
            height = _h;
        }
        public virtual void LoadContent()
        {
            image = game.Content.Load<Texture2D>(texture);
        }

        public virtual void Update(GameTime gameTime, MouseState mState)
        {
        }

        public virtual void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), new Color(0, 0, 0, opacity));
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, width, height), new Rectangle(0, 0, image.Width, image.Height), new Color(color.R, color.G, color.B,opacity));
        }

        public void setColor(Color _color)
        {
            color = _color;
        }

        public void setOpacity(int _opacity)
        {
            opacity = (_opacity > 255) ? 255 : Math.Max(0, _opacity);
        }
    }
}
