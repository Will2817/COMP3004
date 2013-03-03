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
    class Checkbox : Visual
    {
        protected bool selected = false;

        // Checkbox constructor - Virtual
        public Checkbox(Game1 theGame, Vector2 _pos, int _w, int _h)
            :base(theGame, _pos, _w, _h, "line")
        {
            textureColor = new Color(255, 255, 255, opacity);
        }

        public Checkbox(Game1 theGame, Vector2 _pos)
            : base(theGame, _pos, 15, 15, "line")
        {
            textureColor = new Color(255, 255, 255, opacity);
        }

        public override void Update(GameTime gameTime, MouseState mState)
        {
            if (!visible) return;
            base.Update(gameTime, mState);
            if (isClicked())
            {
                selected = !selected;
                if (selected) textureColor = new Color(Color.Green.R, Color.Green.G, Color.Green.B, opacity);
                else textureColor = new Color(255, 255, 255, opacity);
                reset();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            spriteBatch.Draw(texture, new Rectangle((int)position.X-1, (int)position.Y-1, width+2, height+2), new Color(0,0,0,opacity));
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), textureColor);
        }

        public bool isSelected() { return selected; }
        public Checkbox setSelected(bool _selected) { selected = _selected; return this; }
    }
}
