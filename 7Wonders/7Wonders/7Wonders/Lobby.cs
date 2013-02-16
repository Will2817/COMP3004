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
    class Lobby : Interface
    {
        protected bool random = false;
        protected bool onlyA = false;
        protected static Dictionary<String, Visual> visuals1;

        public Lobby(Game1 theGame)
            : base(theGame, "title", 0.4f)
        {
            visuals1 = new Dictionary<String, Visual>();
            int wide = Game1.WIDTH/3;
            int half = (Game1.WIDTH-wide)/2;
            int height = (Game1.HEIGHT-10) / 6;
            visuals1.Add("Divider1", new Visual(game, new Vector2(wide-5, 0), 2, Game1.HEIGHT, "line", Color.Silver));
            visuals1.Add("Divider2", new Visual(game, new Vector2(0, height*4), wide-6, 2, "line", Color.Silver));
            visuals1.Add("wonder1", new Visual(game, new Vector2(wide, 5), half, height, "wonder1a", Color.White));
            visuals1.Add("wonder2", new Visual(game, new Vector2(wide, 5+height), half, height, "wonder2a", Color.White));
            visuals1.Add("wonder3", new Visual(game, new Vector2(wide, 5+height*2), half, height, "wonder3a", Color.White));
            visuals1.Add("wonder4", new Visual(game, new Vector2(wide, 5+height*3), half, height, "wonder4a", Color.White));
            visuals1.Add("wonder5", new Visual(game, new Vector2(wide + half + 1, 5), half, height, "wonder5a", Color.White));
            visuals1.Add("wonder6", new Visual(game, new Vector2(wide + half + 1, 5 + height), half, height, "wonder6a", Color.White));
            visuals1.Add("wonder7", new Visual(game, new Vector2(wide + half + 1, 5 + height * 2), half, height, "wonder7a", Color.White));
            visuals1.Add("selected", new Visual(game, new Vector2(wide, 5+height*4), half * 2, height*2, "wonder1a", Color.White));
            activeVisuals = visuals1;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            foreach (Visual v in visuals1.Values)
            {
                v.LoadContent();
            }
        }

        public void recieveMessage(Dictionary<string, string> message)
        {
            random = Boolean.Parse(message["random"]);
            onlyA = Boolean.Parse(message["onlyA"]);
        }
    }
}
