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
    class Wonder : Visual
    {
        public Wonder(Game1 theGame, Vector2 _pos, int _w, int _h, string _text)
            :base(theGame, _pos, _w, _h, _text)
        {
        
        }
    }
}
