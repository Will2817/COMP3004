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
    class DropDown : CompositeVisual
    {
        protected Dictionary<string,Button> buttons;
        protected TextureVisual selected;
        protected TextVisual selectText;
        protected Button dropButton;
        protected bool isDown = false;
        protected bool dropRequest = false;

        public DropDown(Vector2 _pos, int _w, int _h, List<string> _options, bool _enabled = true)
            : base(_pos)
        {
            enabled = _enabled;
            buttons = new Dictionary<string, Button>();
            selected = new TextureVisual(_pos, _w, _h, "grayback", Color.Gray);
            selectText = new TextVisual(_pos, _options[0], "Font1", Color.White, 10, 7);

            visuals.Add(selected);
            visuals.Add(selectText);

            int count=1;
            foreach (string s in _options)
            {
                Button b;
                buttons.Add(s, (b = new Button(_pos + new Vector2(0, count * _h), _w + 21, _h, s, "Font1", "grayback", true, Color.White, Color.Orange)));
                buttons[s].setVisible(false);
                visuals.Add(b);
                count++;
            }

            dropButton = new Button(_pos + new Vector2(_w + 1, 0), 20, _h, "", "Font1", "drop");
            visuals.Add(dropButton);
        }

        public override void Update(GameTime gameTime, MouseState mState)
        {
            if (!visible||!enabled) return;
            base.Update(gameTime, mState);
            
            foreach (Button b in buttons.Values)
            {
                if (b.isClicked())
                {
                    selectText.setText(b.getText());
                    drop();
                    b.reset();
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;
            selected.Draw(gameTime, spriteBatch);
            selectText.Draw(gameTime, spriteBatch);
            if (!enabled) return;
            dropButton.Draw(gameTime, spriteBatch);            
            foreach (Visual v in buttons.Values)
            {
                v.Draw(gameTime, spriteBatch);
            }
        }

        public string getSelected()
        {
            return selectText.getText();
        }

        public DropDown setSelected(string _text)
        {
            selectText.setText(_text);
            return this;
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
            foreach (Visual v in buttons.Values)
            {
                if (!isDown) v.setVisible(true);
                else v.setVisible(false);
            }
            isDown = !isDown;
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
