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
    public class TextureVisual : Visual
    {
        protected Texture2D texture;
        protected string textureName;

        protected int width;
        protected int height;

        protected float opacity = 1;
        protected Color textureColor = Color.Gray;

        protected bool pressed = false;
        protected bool clicked = false;
        protected bool border = true;
        public int z { get; set; }

        public TextureVisual(Vector2 _pos, int _w, int _h, string _textureName, Color? _textureColor)
            : base(_pos)
        {
            width = _w;
            height = _h;
            textureName = _textureName;
            textureColor = ((_textureColor.HasValue) ? _textureColor.Value : textureColor) * opacity;
        }

        public override void LoadContent()
        {
            if (textureName != null) texture = Game1.textures[textureName];
        }

        public override void Update(GameTime gameTime, MouseState mState)
        {
            if (!visible || !enabled) return;

            clicked = false;
            if (mState.LeftButton == ButtonState.Pressed)
            {
                if (isMouseOver(mState)) pressed = true;
            }
            else
            {
                if (pressed)
                {
                    if (isMouseOver(mState)) clicked = true;
                    pressed = false;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            if (border) spriteBatch.Draw(texture, new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), Color.Black * opacity);
            if (texture != null) spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), null, textureColor * opacity);
        }

        public bool isPressed()
        {
            return pressed;
        }

        public bool isMouseOver(MouseState mState)
        {
            if ((mState.X > position.X) && (mState.X < position.X + width) &&
                (mState.Y > position.Y) && (mState.Y < position.Y + height))
                return true;
            return false;
        }

        public Visual setWidth(int _w)
        {
            width = _w;
            return this;
        }

        public Visual setHeight(int _h)
        {
            height = _h;
            return this;
        }

        public Visual setTexture(string _texture)
        {
            textureName = _texture;
            texture = Game1.textures[textureName];
            return this;
        }

        public Color getColor()
        {
            return textureColor;
        }

        public void setColor(Color _color)
        {
            textureColor = _color * opacity;
        }

        public void reset()
        {
            clicked = pressed = false;
        }

        public bool isClicked() { return clicked; }

        public Visual setBorder(bool _border)
        {
            border = _border;
            return this;
        }

        public Visual setRelativeWidth(int _w)
        {
            width += _w;
            return this;
        }

        public Visual setRelativeHeight(int _h)
        {
            height += _h;
            return this;
        }
    }
}
