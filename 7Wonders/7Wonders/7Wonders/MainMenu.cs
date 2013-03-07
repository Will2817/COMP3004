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
        protected static Dictionary<String, Visual> connectDia;

        protected static Button HGame;
        protected static Button JGame;
        protected static Button CRoom;
        protected static Button backButton;
        protected static Button OK;
        
        protected static Checkbox randomBox;
        protected static Checkbox onlyABox;

        protected bool host = false;

        public MainMenu(Game1 theGame)
            :base (theGame, "title")
        {
            visuals1 = new Dictionary<String, Visual>();
            visuals2 = new Dictionary<String, Visual>();
            visuals3 = new Dictionary<String, Visual>();
            connectDia = new Dictionary<String, Visual>();

            HGame = new Button(game, new Vector2(150, 450), 200, 50, "Host Game", "Font1");
            JGame = new Button(game, new Vector2(450, 450), 200, 50, "Join Game", "Font1");
            CRoom = new Button(game, new Vector2(260, 420), 145, 50, "Create Room", "Font1");
            backButton = new Button(game, new Vector2(410, 420), 120, 50, "Back", "Font1");

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

            connectDia.Add("connectBox", new Visual(game, new Vector2(250, 200), 300, 125, "line", Color.Silver));
            connectDia.Add("message", new Visual(game, new Vector2(280, 215), "", "Font1", Color.White));
            OK = new Button(game, new Vector2(280, 250), 120, 50, "Okay", "Font1");
            connectDia.Add("ok", OK);

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
            foreach (Visual v in connectDia.Values)
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
                host = true;
            }
            if (JGame.isClicked())
            {
                JGame.reset();
                // activeVisuals = visuals3;
                finished = true;
                host = false;
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

            if (OK.isClicked())
            {
                OK.reset();
                foreach (string key in connectDia.Keys.Reverse())
                {
                    if (activeVisuals.ContainsKey(key))
                        activeVisuals.Remove(key);
                }
                HGame.setEnabled(true);
                JGame.setEnabled(true);
                CRoom.setEnabled(true);
                backButton.setEnabled(true);
            }
        }

        public override Dictionary<string, string> isFinished()
        {
            if (finished)
            {
                if (host)
                    return HostLobby.createMessage(randomBox.isSelected(), onlyABox.isSelected());
                else return Lobby.createMessage();
            }

            return null;
        }

        public static Dictionary<string, string> createMessage()
        {
            return new Dictionary<string, string>()
                {
                    {"nextInterface", "mainmenu"},
                };
        }

        public override void receiveMessage(Dictionary<string, string> message)
        {
            if (message.ContainsKey("connection"))
            {
                connectDia["message"].setString(message["connection"]);
                foreach (string s in connectDia.Keys)
                {
                    activeVisuals.Add(s, connectDia[s]);
                }
                HGame.setEnabled(false);
                JGame.setEnabled(false);
                CRoom.setEnabled(false);
                backButton.setEnabled(false);
            }
        }
    }
}
