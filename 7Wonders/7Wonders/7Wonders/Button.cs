using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace _7Wonders
{
    public class Button : Visual
    {
        protected string sfont;
        protected bool mouseDown = false;
        protected bool pressed = false;
        protected bool clicked = false;
        protected Color bColor;
        protected Color fColor;

        public Button(Game1 theGame, Vector2 _pos,int _w, int _h, string _t, string _sfont, string texture)
            : base(theGame, _pos, _w, _h, (texture != null) ? texture : "line")
        {
            sfont = _sfont;
            bColor = Color.Gray;
            fColor = Color.DarkGray;
            text = _t;
        }

        public override void LoadContent()
        {
            font = Game1.fonts[sfont];
            image = Game1.textures["line"];
        }

        public void setText(String t)
        {
            text = t;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color actual = bColor;
            if (pressed)
            {
                actual = new Color(Math.Max(bColor.B - 50, 0), Math.Max(bColor.G - 50, 0), Math.Max(bColor.R - 50, 0), opacity);
            }
            spriteBatch.Draw(image, new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), new Color(0, 0, 0, opacity));
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, width, height), actual);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 2, position.Y + 2), new Color(255, 255, 255, opacity));
        }
        public override void Update(GameTime gameTime, MouseState mState)
        {
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

        public void reset()
        {
            clicked = pressed = mouseDown = false;
        }

        public bool isClicked() { return clicked; }

    }
}

