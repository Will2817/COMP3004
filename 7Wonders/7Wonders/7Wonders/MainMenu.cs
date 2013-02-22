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
    class MainMenu : Interface
    {
        protected static Dictionary<String, Visual> visuals1;
        protected static Dictionary<String, Visual> visuals2;
        protected static Dictionary<String, Visual> visuals3;

        protected static Button HGame;
        protected static Button JGame;
        protected static Button CRoom;
        protected static Button backButton;
        protected static Textbox ip1;
        protected static Textbox ip2;
        protected static Textbox ip3;
        protected static Textbox ip4;

        protected static Checkbox randomBox;
        protected static Checkbox onlyABox;

        public MainMenu(Game1 theGame)
            :base (theGame, "title")
        {
            visuals1 = new Dictionary<String, Visual>();
            visuals2 = new Dictionary<String, Visual>();
            visuals3 = new Dictionary<String, Visual>();

            HGame = new Button(game, new Vector2(150, 450), 200, 50, "Host Game", "Font1");
            JGame = new Button(game, new Vector2(450, 450), 200, 50, "Join Game", "Font1");
            CRoom = new Button(game, new Vector2(260, 420), 145, 50, "Create Room", "Font1");
            backButton = new Button(game, new Vector2(410, 420), 120, 50, "Back", "Font1");
            ip1 = new Textbox(game, new Vector2(270, 300), 55, 35, "", "Font1", 3);
            ip2 = new Textbox(game, new Vector2(335, 300), 55, 35, "", "Font1", 3);
            ip3 = new Textbox(game, new Vector2(400, 300), 55, 35, "", "Font1", 3);
            ip4 = new Textbox(game, new Vector2(465, 300), 55, 35, "", "Font1", 3);

            randomBox = new Checkbox(game, new Vector2(450, 285));
            onlyABox = new Checkbox(game, new Vector2(450, 355));

            visuals1.Add("HGame", HGame);
            visuals1.Add("JGame", JGame);

            visuals2.Add("DBox", new Visual(game,new Vector2(250,200),300,300, "line", Color.Silver));
            visuals2.Add("Box1", new Visual(game, new Vector2(275, 210), 200, 50, "line", Color.SlateGray));
            visuals2.Add("String1", new Visual(game, new Vector2(280, 215), "Room Setup", "Font1", Color.White));
            visuals2.Add("Box2", new Visual(game, new Vector2(260, 270), 150, 50, "line", Color.SlateGray));
            visuals2.Add("String2", new Visual(game, new Vector2(265, 275), "Assign boards", "Font1", Color.White));
            visuals2.Add("Check1", randomBox);
            visuals2.Add("Box3", new Visual(game, new Vector2(260, 340), 150, 50, "line", Color.SlateGray));
            visuals2.Add("String3", new Visual(game, new Vector2(265, 345), "Only Side A", "Font1", Color.White));
            visuals2.Add("Check2", onlyABox);
            visuals2.Add("CRoom", CRoom);
            visuals2.Add("backButton", backButton);

            visuals3.Add("DBox", new Visual(game, new Vector2(250, 200), 300, 300, "line", Color.Silver));
            visuals3.Add("Box1", new Visual(game, new Vector2(275, 210), 200, 50, "line", Color.SlateGray));
            visuals3.Add("String1", new Visual(game, new Vector2(280, 215), "Enter Ip Address", "Font1", Color.White));
            visuals3.Add("ip1", ip1);
            visuals3.Add("ip2", ip2);
            visuals3.Add("ip3", ip3);
            visuals3.Add("ip4", ip4);
            visuals3.Add("backButton", backButton);

            activeVisuals = visuals1;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            foreach (Visual v in visuals1.Values)
            {
                v.LoadContent();
            }
            foreach (Visual v in visuals2.Values)
            {
                v.LoadContent();
            }
            foreach (Visual v in visuals3.Values)
            {
                v.LoadContent();
            }
        }

        public override void Update(GameTime gameTime, MouseState mouseState)
        {
            base.Update(gameTime, mouseState);

            if (HGame.isClicked())
            {
                HGame.reset();
                activeVisuals = visuals2;
            }
            if (JGame.isClicked())
            {
                JGame.reset();
                activeVisuals = visuals3;
            }
            if (CRoom.isClicked())
            {
                CRoom.reset();
                finished = true;
            }
            if (backButton.isClicked()){
                backButton.reset();
                activeVisuals = visuals1;
            }
        }

        public override Dictionary<string, string> isFinished()
        {
            if (finished)
            {
                Console.WriteLine(randomBox.isSelected() + ", " + onlyABox.isSelected());
                return Lobby.createMessage(randomBox.isSelected(), onlyABox.isSelected());
            }

            return null;
        }

        public static Dictionary<string, string> createMessage()
        {
            return new Dictionary<string, string>()
                {
                    {"nextInterface", "main"},
                };
        }
    }
}
