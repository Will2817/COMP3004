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
        protected float opacity;
        protected Color color;
        protected string text;
        protected string sFont;
        protected SpriteFont font;

        public Visual(Visual v)
        {
            position = v.position;
            width = v.width;
            height = v.height;
            opacity = v.opacity;
            color = v.color;
            text = v.text;
            sFont = v.sFont;
            font = v.font;
        }

        public Visual(Game1 theGame, Vector2 _pos, int _w, int _h, string _text)
        {
            game = theGame;
            position = _pos;
            width = _w;
            height = _h;
            texture = _text;
            opacity = 1;
            color = Color.White*opacity;
        }

        public Visual(Game1 theGame, Vector2 _pos, int _w, int _h, string _text, Color _color)
        {
            game = theGame;
            position = _pos;
            width = _w;
            height = _h;
            texture = _text;
            opacity = 1;
            color = _color*opacity;
        }

        public Visual(Game1 theGame, Vector2 _pos, string _text, string _sFont, Color _color)
        {
            game = theGame;
            position = _pos;
            opacity = 1;
            color = _color * opacity;
            sFont = _sFont;
            text = _text;
        }

        public Visual setPosition(Vector2 _vec)
        {
            position = _vec;
            return this;
        }

        public Visual setWidth(int _w)
        {
            width = _w;
            return this;
        }

        public Visual setHeight(int _h)
        {
            height = _h;
            return this;
        }
        public virtual void LoadContent()
        {
            if (text != null)
                font = Game1.fonts[sFont];
            if (texture != null)
                image = Game1.textures[texture];
        }

        public virtual void Update(GameTime gameTime, MouseState mState)
        {
        }

        public virtual void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            if (text != null)
            {
                spriteBatch.DrawString(font, text, new Vector2(position.X + 2, position.Y + 2), color);
            }
            if (image != null)
            {
                spriteBatch.Draw(image, new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), Color.Black*opacity);
                spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, width, height), new Rectangle(0, 0, image.Width, image.Height), color);
            }
        }

        public Visual setImage(string texture)
        {
            image = Game1.textures[texture];
            return this;
        }

        public void setColor(Color _color)
        {
            color = _color*opacity;
        }

        public void setOpacity(float _opacity)
        {
            color *= 1/opacity;
            opacity = (_opacity > 1) ? 1 : Math.Max(0, _opacity);
            color *=  opacity;
        }
    }
}
