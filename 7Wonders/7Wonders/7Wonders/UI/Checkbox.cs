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
    class Checkbox : TextureVisual
    {
        protected bool selected = false;
        protected bool changed = false;

        // Checkbox constructor - Virtual
        public Checkbox(Vector2 _pos, int _w, int _h)
            : base(_pos, _w, _h, "line")
        {
            textureColor = new Color(255, 255, 255, opacity);
        }

        public Checkbox(Vector2 _pos)
            : base(_pos, 15, 15, "line")
        {
            textureColor = new Color(255, 255, 255, opacity);
        }

        public override void Update(GameTime gameTime, MouseState mState)
        {
            if (!visible) return;
            base.Update(gameTime, mState);
            if (isClicked())
            {
                changed = true;
                selected = !selected;
                swapColours();
                reset();
            }
        }

        public bool hasChanged() { return changed; }
        public bool isSelected() { changed = false; return selected; }
        public Checkbox setSelected(bool _selected) { selected = _selected; swapColours(); return this; }

        private void swapColours()
        {
            if (selected) textureColor = new Color(Color.Green.R, Color.Green.G, Color.Green.B, opacity);
            else textureColor = new Color(255, 255, 255, opacity);
        }
    }
}
