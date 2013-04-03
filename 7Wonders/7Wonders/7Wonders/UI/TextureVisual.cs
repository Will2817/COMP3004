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

        protected Color textureColor = Color.White;

        protected bool pressed = false;
        protected bool clicked = false;
        protected bool border = true;
        public int z { get; set; }

        public TextureVisual(Vector2 _pos, int _w, int _h, string _textureName, Color? _textureColor = null)
            : base(_pos)
        {
            width = _w;
            height = _h;
            textureName = _textureName;
            textureColor = ((_textureColor.HasValue) ? _textureColor.Value : textureColor) * opacity;
        }

        public TextureVisual(TextureVisual tv)
            : base(tv.getPosition())
        {
            width = tv.width;
            height = tv.height;
            textureName = tv.textureName;
            textureColor = tv.getColor();
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
            if (texture != null)
            {
                if (border) spriteBatch.Draw(texture, new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), Color.Black * opacity);
                spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), null, textureColor * opacity);
            }
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

        public TextureVisual setWidth(int _w)
        {
            width = _w;
            return this;
        }

        public TextureVisual setHeight(int _h)
        {
            height = _h;
            return this;
        }

        public TextureVisual setPosition(Vector2 _vec)
        {
            position = _vec;
            return this;
        }


        public string getTexture()
        {
            return textureName;
        }

        public TextureVisual setTexture(string _texture)
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

        public virtual TextureVisual setBorder(bool _border)
        {
            border = _border;
            return this;
        }

        public TextureVisual setRelativeWidth(int _w)
        {
            width += _w;
            return this;
        }

        public TextureVisual setRelativeHeight(int _h)
        {
            height += _h;
            return this;
        }

        public TextureVisual setRelativePosition(Vector2 _vec)
        {
            position += _vec;
            return this;
        }

        public TextureVisual setEnabled(bool _enable)
        {
            base.setEnabled(_enable);
            return this;
        }

        public TextureVisual setVisible(bool _visible)
        {
            base.setVisible(_visible);
            return this;
        }
    }
}
