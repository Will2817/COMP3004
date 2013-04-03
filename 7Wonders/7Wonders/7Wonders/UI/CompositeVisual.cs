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
    class CompositeVisual: Visual
    {
        protected List<Visual> visuals;

        public CompositeVisual(Vector2 _pos)
            :base(_pos)
        {
            visuals = new List<Visual>();
        }

        public void Add(Visual v)
        {
            visuals.Add(v);
        }

        public override void LoadContent()
        {
            foreach (Visual v in visuals) v.LoadContent();
        }

        public override void Update(GameTime gameTime, MouseState mState)
        {
            foreach (Visual v in visuals) v.Update(gameTime, mState);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Visual v in visuals) v.Draw(gameTime, spriteBatch);
        }

        public override Visual setEnabled(bool _enable)
        {
            foreach (Visual v in visuals) v.setEnabled(_enable);
            return this;
        }

        public override Visual setVisible(bool _visible)
        {
            foreach (Visual v in visuals) v.setVisible(_visible);
            return this;
        }
    }
}  
