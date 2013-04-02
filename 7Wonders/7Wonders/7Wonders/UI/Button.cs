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
    class Button : CompositeVisual
    {
        private const int HIGHTLIGHT = 50;
        private TextVisual texv;
        private TextureVisual ttv;


        // Button Constructor
        public Button(Vector2 _pos, int _w, int _h, string _text, string _sfont, string texture = null, bool _border = true, Color? _stringColor = null, Color? _textureColor = null)
            : base(_pos)
        {
            stringColor = ((_stringColor.HasValue) ? _stringColor.Value : Color.Black) * opacity;
            textureColor = ((_textureColor.HasValue) ? _textureColor.Value : new Color(255 - HIGHTLIGHT, 255 - HIGHTLIGHT, 255 - HIGHTLIGHT)) * opacity;    

            texv = new TextVisual(_pos, _text, _sfont,  stringColor, 10, 7);
            ttv = new TextureVisual(_pos, _w, _h, (texture != null) ? texture : "button", textureColor);
            ttv.setBorder(_border);
            visuals.Add(ttv);
            visuals.Add(texv);            
        }

        // Text Mutator
        public void setText(String t)
        {
            texv.setText(t);
        }

        public string getText()
        {
            return texv.getText();
        }

        public bool isClicked(){
            return ttv.isClicked();
        }

        public void reset()
        {
            ttv.reset();
        }

        // Draw function
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            Color store = ttv.getColor();
            if (ttv.isPressed())
            {
                ttv.setColor(new Color(Math.Max(store.B + HIGHTLIGHT, 0), Math.Max(store.G + HIGHTLIGHT, 0), Math.Max(store.R + HIGHTLIGHT, 0)));
            }
            base.Draw(gameTime, spriteBatch);
            ttv.setColor(store);
        }

        public override Visual setEnabled(bool _enable)
        {
            enabled = _enable;
            return base.setEnabled(_enable);
        }

        public override Visual setVisible(bool _visible)
        {
            visible = _visible;
            return base.setVisible(_visible);
        }
    }
}

