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
        public Button(Game1 theGame, Vector2 _pos,int _w, int _h, string _t, string _sfont, string texture=null)
            : base(theGame, _pos, _w, _h, (texture != null) ? texture: "line")
        {
            fontName = _sfont;
            textureColor = Color.Gray;
            stringColor = Color.DarkGray;
            text = _t;
        }

        public void setText(String t)
        {
            text = t;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            Color actual = textureColor;
            if (pressed)
            {
                actual = new Color(Math.Max(textureColor.B - 50, 0), Math.Max(textureColor.G - 50, 0), Math.Max(textureColor.R - 50, 0));
            }
            spriteBatch.Draw(Game1.textures["line"], new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), Color.Black*opacity);
            //if (textureName != "line") spriteBatch.Draw(Game1.textures["line"], new Rectangle((int)position.X, (int)position.Y, width, height), actual);
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), actual);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 2, position.Y + 2), new Color(255, 255, 255, opacity));
        }
    }
}

