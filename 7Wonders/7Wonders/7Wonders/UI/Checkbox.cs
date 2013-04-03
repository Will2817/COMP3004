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
    class Checkbox : CompositeVisual
    {
        private TextureVisual tv; 
        private bool selected = false;
        private bool changed = false;

        // Checkbox constructor - Virtual
        public Checkbox(Vector2 _pos, int _w, int _h)
            : base(_pos)
        {
            tv = new TextureVisual(_pos, _w, _h, "line", Color.White);
            visuals.Add(tv);
        }

        public Checkbox(Vector2 _pos)
            : base(_pos)
        {
            tv = new TextureVisual(_pos, 15, 15, "line", Color.White);
            visuals.Add(tv);
        }

        public override void Update(GameTime gameTime, MouseState mState)
        {
            if (!visible) return;
            base.Update(gameTime, mState);
            if (tv.isClicked())
            {
                changed = true;
                selected = !selected;
                swapColours();
                tv.reset();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            base.Draw(gameTime, spriteBatch);
        }

        public bool hasChanged() { return changed; }
        public bool isSelected() { changed = false; return selected; }
        public Checkbox setSelected(bool _selected) { selected = _selected; swapColours(); return this; }

        private void swapColours()
        {
            if (selected) tv.setColor(new Color(Color.Green.R, Color.Green.G, Color.Green.B, opacity));
            else tv.setColor(Color.White);
        }
    }
}
