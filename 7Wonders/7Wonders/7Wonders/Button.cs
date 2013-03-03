﻿// Libraries
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace _7Wonders
{
    // Button class - extends Visual
    public class Button : Visual
    {
        protected static int highlight = 50;

        // Button Constructor
        public Button(Game1 theGame, Vector2 _pos,int _w, int _h, string _t, string _sfont, string texture=null)
            : base(theGame, _pos, _w, _h, (texture != null) ? texture: "button")
        {
            fontName = _sfont;
            textureColor = new Color(255 - highlight, 255 - highlight, 255 - highlight) * opacity;
            stringColor = Color.Black * opacity;
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
                actual = new Color(Math.Max(textureColor.B + highlight, 0), Math.Max(textureColor.G + highlight, 0), Math.Max(textureColor.R + highlight, 0));
            }
            spriteBatch.Draw(Game1.textures["line"], new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), Color.Black*opacity);
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), actual);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 11, position.Y + 7), stringColor);
        }
    }
}

