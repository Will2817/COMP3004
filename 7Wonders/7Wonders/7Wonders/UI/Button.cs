// Libraries
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
    public class Button : TextPlusVisual
    {
        protected static int highlight = 50;

        // Button Constructor
        public Button(Vector2 _pos, int _w, int _h, string _t, string _sfont, string texture = null, bool _border = true)
            : base(_pos, _w, _h, _t, _sfont, (texture != null) ? texture : "button")
        {
            textureColor = new Color(255 - highlight, 255 - highlight, 255 - highlight) * opacity;
            stringColor = Color.Black * opacity;
            border = _border;
            topmargin = 7;
            leftmargin = 10;
        }

        // Draw function
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            Color actual = textureColor;
            if (pressed)
            {
                actual = new Color(Math.Max(textureColor.B + highlight, 0), Math.Max(textureColor.G + highlight, 0), Math.Max(textureColor.R + highlight, 0));
            }
            if (border) spriteBatch.Draw(Game1.textures["line"], new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), Color.Black*opacity);
            if (texture != null) spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), actual);
            base.DrawOnlyText(gameTime,spriteBatch);
            
        }

        public void setClicked(bool _bool)
        {
            clicked= _bool;
        }

        public Button setBorder(bool _border)
        {
            base.setBorder(_border);
            return this;
        }

        public Button setEnabled(bool _enable)
        {
            base.setEnabled(_enable);
            return this;
        }

        public Button setColor(Color _color)
        {
            textureColor = _color * opacity;
            return this;
        }
    }
}

