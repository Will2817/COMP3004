using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace _7Wonders
{
    public class Textbox : Visual
    {
        protected bool hasFocus = false;
        protected char cursor = '|';
        protected string displayedText = "";
        protected int lengthCap;

        public Textbox(Vector2 _pos, int _w, int _h, string _t, string _sfont, int _cap)
            : base(_pos, _w, _h, "line")
        {
            fontName = _sfont;
            textureColor = Color.White * opacity;
            stringColor = Color.Black * opacity;
            text = "";
            lengthCap = _cap;

        }

        public override void Update(GameTime gameTime, MouseState mState)
        {
            base.Update(gameTime, mState);
            if (isClicked())
            {
                hasFocus = true;
                reset();
            }
            if (clickOutside(mState))
            {
                displayedText = text;
                hasFocus = false;
            }
            if (hasFocus)
            {
                if ((Game1.recordedPresses == "back")&&(text.Length>0))
                    text = text.Substring(0, text.Length - 1);
                else 
                    if (text.Length < lengthCap) text += Game1.recordedPresses;
                Game1.recordedPresses = "";
                displayedText = text;
                if (gameTime.TotalGameTime.Milliseconds < 500)
                {
                    displayedText += cursor;
                }
            }


        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            Color actual = textureColor;
            spriteBatch.Draw(Game1.textures["line"], new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), Color.Black * opacity);
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), actual);
            spriteBatch.DrawString(font, displayedText, new Vector2(position.X + 5, position.Y + 5), stringColor);
        }

        public bool clickOutside(MouseState mState){
            if ((mState.LeftButton == ButtonState.Pressed)&&(!isMouseOver(mState)))
            {
                return true;
            }
            return false;
        }
    }
}

