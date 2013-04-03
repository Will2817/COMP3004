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
    public class TextVisual : Visual
    {
        protected SpriteFont font;
        protected string fontName;
        protected string text;

        protected float opacity = 1;
        protected Color stringColor = Color.White;

        protected int topmargin = 2;
        protected int leftmargin = 2;

        public TextVisual(Vector2 _pos, string _text, string _fontName, Color? _stringColor=null, int _leftmargin = 2, int _topmargin = 2)
            : base(_pos)
        {
            stringColor = ((_stringColor.HasValue) ? _stringColor.Value : Color.White) * opacity;
            fontName = _fontName;
            text = _text;
            leftmargin = _leftmargin;
            topmargin = _topmargin;
        }

        public override void LoadContent()
        {
            if (fontName != null) font = Game1.fonts[fontName];
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            if (font != null) spriteBatch.DrawString(font, text, new Vector2(position.X + leftmargin, position.Y + topmargin), stringColor);
        }

        public Visual setText(string _s)
        {
            text = _s;
            return this;
        }

        public string getText()
        {
            return text;
        }

        public void setLeftMargin(int i)
        {
            leftmargin = i;
        }

        public void setTopMargin(int i)
        {
            topmargin = i;
        }
    }
}
