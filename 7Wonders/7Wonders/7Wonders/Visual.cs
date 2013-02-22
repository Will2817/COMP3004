﻿using System;
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
    public class Visual
    {
        protected Game1 game;

        protected Texture2D texture;
        protected string textureName;

        protected SpriteFont font;
        protected string fontName;
        protected string text;       

        protected Vector2 position;
        protected int width;
        protected int height;

        protected float opacity = 1;
        protected Color textureColor = Color.Gray;
        protected Color stringColor = Color.White;

        protected bool pressed = false;
        protected bool clicked = false;
        protected bool visible = true;
        protected bool enabled = true;

        public Visual(Visual v)
        {
            game = v.game;
            textureName = v.textureName;
            texture = v.texture;
            fontName = v.fontName;
            font = v.font;
            text = v.text;
            position = v.position;
            width = v.width;
            height = v.height;
            opacity = v.opacity;
            textureColor = v.textureColor;
            stringColor = v.stringColor;
            text = v.text;
        }

        public Visual(Game1 theGame, Vector2 _pos, int _w, int _h, string _textureName, Color ?_color=null)
        {
            game = theGame;
            position = _pos;
            width = _w;
            height = _h;
            textureName = _textureName;
            textureColor = ((_color.HasValue) ? _color.Value : Color.White)*opacity;
            stringColor *= opacity;
        }

        public Visual(Game1 theGame, Vector2 _pos, string _text, string _fontName, Color _color)
        {
            game = theGame;
            position = _pos;
            textureColor *= opacity;
            stringColor = _color * opacity;
            fontName = _fontName;
            text = _text;
        }

        public Visual(Game1 theGame, Vector2 _pos, int _w, int _h, string _text, string _fontName, Color _stringColor, Color ?_textureColor=null)
        {
            game = theGame;
            position = _pos;
            width = _w;
            height = _h;
            stringColor = _stringColor * opacity;
            textureColor = ((_textureColor.HasValue) ? _textureColor.Value : textureColor) * opacity;            
            fontName = _fontName;
            text = _text;
            textureName = "line";
        }

        public virtual void LoadContent()
        {
            if (fontName != null)
                font = Game1.fonts[fontName];
            if (textureName != null)
                texture = Game1.textures[textureName];
        }

        public virtual void Update(GameTime gameTime, MouseState mState)
        {
            if (!visible||!enabled) return;

            clicked = false;
            if (mState.LeftButton == ButtonState.Pressed)
            {
                if (isMouseOver(mState)) pressed = true;
            }
            else
            {
                if (pressed)
                {
                    if(isMouseOver(mState)) clicked = true;
                    pressed = false;
                }
            }
        }

        public virtual void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            if (!visible) return;
            if (texture != null)
            {
                spriteBatch.Draw(texture, new Rectangle((int)position.X - 1, (int)position.Y - 1, width + 2, height + 2), Color.Black * opacity);
                spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), new Rectangle(0, 0, texture.Width, texture.Height), textureColor);
            }
            if (text != null)
            {
                spriteBatch.DrawString(font, text, new Vector2(position.X + 2, position.Y + 2), stringColor);
            }
        }

        private bool isMouseOver(MouseState mState)
        {
            if ((mState.X > position.X) && (mState.X < position.X + width) &&
                (mState.Y > position.Y) && (mState.Y < position.Y + height))
                return true;
            return false;
        }

        public Visual setPosition(Vector2 _vec)
        {
            position = _vec;
            return this;
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

        public string getTexture()
        {
            return textureName;
        }

        public void setColor(Color _color)
        {
            textureColor = _color * opacity;
        }

        public void setOpacity(float _opacity)
        {
            textureColor *= 1 / opacity;
            stringColor *= 1 / opacity;
            opacity = (_opacity > 1) ? 1 : Math.Max(0, _opacity);
            textureColor *= opacity;
            stringColor *= opacity;
        }

        public void reset()
        {
            clicked = pressed = false;
        }

        public bool isClicked() { return clicked; }

        public Visual setVisible(bool _visible) { 
            visible = _visible; 
            return this; 
        }

        public Visual setString(string _s)
        {
            text = _s;
            return this;
        }

        public string getString()
        {
            return text;
        }

        public virtual Visual setEnabled(bool _enable)
        {
            enabled = _enable;
            return this;
        }
    }
}
