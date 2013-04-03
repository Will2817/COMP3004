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
    public class TextPlusVisual : TextureVisual
    {
        protected SpriteFont font;
        protected string fontName;
        protected string text;

        protected float opacity = 1;
        protected Color stringColor = Color.White;

        protected int topmargin = 2;
        protected int leftmargin = 2;

        public TextPlusVisual(Vector2 _pos, int _w, int _h, string _text, string _fontName, string _textureName, Color? _FontColor = null, Color? _textureColor = null, int _topmargin = 2, int _leftmargin = 2)
            : base(_pos, _w, _h, _textureName, _textureColor)
        {
            stringColor = ((_FontColor.HasValue) ? _FontColor.Value : Color.White) * opacity;
            fontName = _fontName;
            text = _text;
            leftmargin = _leftmargin;
            topmargin = _topmargin;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            if (fontName != null) font = Game1.fonts[fontName];
        }

        public override void Update(GameTime gameTime, MouseState mState)
        {
            base.Update(gameTime, mState);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            base.Draw(gameTime, spriteBatch);
            DrawOnlyText(gameTime, spriteBatch);
        }

        public void DrawOnlyText(GameTime gameTime, SpriteBatch spriteBatch)
        {
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

        public TextPlusVisual setLeftMargin(int i)
        {
            leftmargin = i;
            return this;
        }

        public TextPlusVisual setTopMargin(int i)
        {
            topmargin = i;
            return this;
        }

        public TextPlusVisual setVisible(bool _visible)
        {
            base.setVisible(_visible);
            return this;
        }
    }
}
