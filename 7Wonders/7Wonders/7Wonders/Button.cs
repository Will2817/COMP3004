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
        public Button(Game1 theGame, Vector2 _pos,int _w, int _h, string _t, string _sfont, string texture)
            : base(theGame, _pos, _w, _h, (texture != null) ? texture : "line")
        {
            sFont = _sfont;
            tcolor = Color.Gray;
            scolor = Color.DarkGray;
            text = _t;
        }

        public void setText(String t)
        {
            text = t;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            Color actual = tcolor;
            if (pressed)
            {
                actual = new Color(Math.Max(tcolor.B - 50, 0), Math.Max(tcolor.G - 50, 0), Math.Max(tcolor.R - 50, 0), opacity);
            }
            spriteBatch.Draw(Game1.textures["line"], new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), Color.Black*opacity);
            if (texture != "line") spriteBatch.Draw(Game1.textures["line"], new Rectangle((int)position.X, (int)position.Y, width, height), Color.White*opacity);
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, width, height), actual);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 2, position.Y + 2), new Color(255, 255, 255, opacity));
        }
    }
}

