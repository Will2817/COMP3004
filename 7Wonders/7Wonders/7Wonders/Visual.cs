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
        protected Color tcolor = Color.Gray;
        protected Color scolor = Color.White;
        protected string text;
        protected string sFont;
        protected SpriteFont font;

        protected bool mouseDown = false;
        protected bool pressed = false;
        protected bool clicked = false;
        protected bool visible = true;

        public Visual(Visual v)
        {
            position = v.position;
            width = v.width;
            height = v.height;
            opacity = v.opacity;
            tcolor = v.tcolor;
            scolor = v.scolor;
            text = v.text;
            sFont = v.sFont;
            font = v.font;
        }

        public Visual(Game1 theGame, Vector2 _pos, int _w, int _h, string _text, Color ?_color=null)
        {
            game = theGame;
            position = _pos;
            width = _w;
            height = _h;
            texture = _text;
            opacity = 1;
            tcolor = ((_color.HasValue) ? _color.Value : Color.White)*opacity;
            scolor *= opacity;
        }

        public Visual(Game1 theGame, Vector2 _pos, string _text, string _sFont, Color _color)
        {
            game = theGame;
            position = _pos;
            opacity = 1;
            tcolor *= opacity;
            scolor = _color * opacity;
            sFont = _sFont;
            text = _text;
        }

        public Visual(Game1 theGame, Vector2 _pos, int _w, int _h, string _text, string _sFont, Color _scolor, Color ?_tcolor=null)
        {
            game = theGame;
            position = _pos;
            width = _w;
            height = _h;
            opacity = 1;
            scolor = _scolor * opacity;
            tcolor = ((_tcolor.HasValue) ? _tcolor.Value: tcolor)*opacity;            
            sFont = _sFont;
            text = _text;
            texture = "line";
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
            if (!visible) return;
            clicked = false;
            if (mState.LeftButton == ButtonState.Pressed)
            {
                if (!mouseDown)
                {
                    mouseDown = true;
                    if ((mState.X > position.X) && (mState.X < position.X + width) &&
                        (mState.Y > position.Y) && (mState.Y < position.Y + height))
                    {
                        pressed = true;
                    }
                }
                mouseDown = true;
            }
            else
            {
                if (mouseDown)
                {
                    mouseDown = false;
                    if (pressed)
                    {
                        pressed = false;
                        if ((mState.X > position.X) && (mState.X < position.X + width) &&
                            (mState.Y > position.Y) && (mState.Y < position.Y + height))
                        {
                            clicked = true;
                        }
                    }
                }
            }
        }

        public virtual void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            if (!visible) return;
            if (image != null)
            {
                spriteBatch.Draw(image, new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), Color.Black*opacity);
                spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, width, height), new Rectangle(0, 0, image.Width, image.Height), tcolor);
            }
            if (text != null)
            {
                spriteBatch.DrawString(font, text, new Vector2(position.X + 2, position.Y + 2), scolor);
            }
        }

        public Visual setImage(string _texture)
        {
            texture = _texture;
            image = Game1.textures[texture];
            return this;
        }

        public string getImage()
        {
            return texture;
        }

        public void setColor(Color _color)
        {
            tcolor = _color*opacity;
        }

        public void setOpacity(float _opacity)
        {
            tcolor *= 1 / opacity;
            scolor *= 1 / opacity;
            opacity = (_opacity > 1) ? 1 : Math.Max(0, _opacity);
            tcolor *= opacity;
            scolor *= opacity;
        }

        public void reset()
        {
            clicked = pressed = mouseDown = false;
        }

        public bool isClicked() { return clicked; }

        public Visual setVisible(bool _visible) { 
            visible = _visible; 
            return this; 
        }

        public Visual setString(string _s)
        {
            text = _s;
            return this;
        }
        public string getString()
        {
            return text;
        }
    }
}
