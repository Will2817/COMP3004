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
    // Visual Class
    public class Visual
    {
        // Variables
        protected Vector2 position;

        protected float opacity = 1;

        protected bool visible = true;
        protected bool enabled = true;
        public int z{get; set;}

        public Visual(Vector2 _pos)
        {
            position = _pos;
            z = 0;
        }

        // Constructor with Visual as a parameter
        public Visual(Visual v)
        {
            position = v.position;
            opacity = v.opacity;
        }

        public virtual void LoadContent()
        {
        }

        public virtual void Update(GameTime gameTime, MouseState mState)
        {
            if (!visible||!enabled) return;
        }

        public virtual void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            if (!visible) return;
        }
       
        public virtual Visual setPosition(Vector2 _vec)
        {
            position = _vec;
            return this;
        }
       

        public virtual Visual setVisible(bool _visible) { 
            visible = _visible; 
            return this; 
        }
        
        public virtual Visual setEnabled(bool _enable)
        {
            enabled = _enable;
            return this;
        }

        public virtual Visual setRelativePosition(Vector2 _vec)
        {
            position += _vec;
            return this;
        }

        public Vector2 getPosition()
        {
            return position;
        }
    }
}
