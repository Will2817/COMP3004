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
    class DropDown : Visual
    {
        protected Dictionary<string,Visual> strings;
        protected Visual selected;
        protected Button dropButton;
        protected bool isDown = false;
        protected bool dropRequest = false;

        public DropDown(Game1 theGame, Vector2 _pos, int _w, int _h, List<string> _options)
            :base(theGame, _pos, _w, _h, "line", Color.White)
        {
            strings = new Dictionary<string, Visual>();
            selected = new Visual(game, position, width, height, _options[0], "Font1", Color.White, Color.SlateGray);
            int count=1;
            foreach (string s in _options)
            {
                strings.Add(s, (new Visual(game, position+new Vector2(0, count*height), width+21, height, s,"Font1", Color.White)).setVisible(false));
                count++;
            }

            dropButton = new Button(game, position+new Vector2(width+1, 0),20, height, "", "Font1", "drop");
        }

        public override void LoadContent()
        {
            foreach (Visual v in strings.Values)
            {
                v.LoadContent();
            }
            dropButton.LoadContent();
            selected.LoadContent();
        }

        public override void Update(GameTime gameTime, MouseState mState)
        {
            if (!visible||!enabled) return;
            selected.Update(gameTime, mState);
            dropButton.Update(gameTime, mState);
            
            foreach (Visual v in strings.Values)
            {
                v.Update(gameTime, mState);
            }
            
            foreach (Visual v in strings.Values)
            {
                if (v.isClicked())
                {
                    selected.setString(v.getString());
                    drop();
                    v.reset();
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            selected.Draw(gameTime, spriteBatch);
            if (!enabled) return;
            dropButton.Draw(gameTime, spriteBatch);            
            foreach (Visual v in strings.Values)
            {
                v.Draw(gameTime, spriteBatch);
            }
        }

        public string getSelected()
        {
            return text;
        }

        public DropDown resetRequest()
        {
            dropButton.reset();
            return this;
        }

        public bool RequestDrop()
        {
            return dropButton.isClicked();
        }

        public bool getDown()
        {
            return isDown;
        }

        public void drop()
        {
            foreach (Visual v in strings.Values)
            {
                if (!isDown) v.setVisible(true);
                else v.setVisible(false);
            }
            isDown = !isDown;
        }
    }
}
