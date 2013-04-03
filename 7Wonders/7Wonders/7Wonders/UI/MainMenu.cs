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

        public MainMenu()
            : base("title")
        {
            visuals1 = new Dictionary<String, Visual>();
            visuals2 = new Dictionary<String, Visual>();
            visuals3 = new Dictionary<String, Visual>();
            connectDia = new Dictionary<String, Visual>();

            HGame = new Button(new Vector2(Game1.WIDTH / 2 - 100, Game1.HEIGHT / 2), 200, 50, "Host Game", "Font1");
            JGame = new Button(new Vector2(Game1.WIDTH / 2 - 100, Game1.HEIGHT / 2 + 100), 200, 50, "Join Game", "Font1");
            CRoom = new Button(new Vector2(Game1.WIDTH / 2 - 145, Game1.HEIGHT / 2 + 95), 140, 50, "Create Room", "Font1");
            backButton = new Button(new Vector2(Game1.WIDTH / 2 + 10, Game1.HEIGHT /2 + 95), 120, 50, "Back", "Font1");

            randomBox = new Checkbox(new Vector2(Game1.WIDTH / 2 + 75, Game1.HEIGHT / 2 - 40));
            randomBox.setSelected(true).setEnabled(false);//Hack to prevent selection
            onlyABox = new Checkbox(new Vector2(Game1.WIDTH / 2 + 75, Game1.HEIGHT / 2 + 40));

            visuals1.Add("HGame", HGame);
            visuals1.Add("JGame", JGame);

            visuals2.Add("DBox", new TextureVisual(new Vector2(Game1.WIDTH / 2 - 150, Game1.HEIGHT/2 - 150), 300, 300, "line", Color.Silver));
            visuals2.Add("Box1", new TextureVisual(new Vector2(Game1.WIDTH / 2 - 100, Game1.HEIGHT/2 - 125), 200, 50, "line", Color.SlateGray));
            visuals2.Add("String1", new TextVisual(new Vector2(Game1.WIDTH / 2 - 85, Game1.HEIGHT / 2 - 125), "Room Setup", "Font1", Color.White));
            visuals2.Add("Box2", new TextureVisual(new Vector2(Game1.WIDTH / 2 - 100, Game1.HEIGHT / 2 - 50), 150, 50, "line", Color.SlateGray));
            visuals2.Add("String2", new TextVisual(new Vector2(Game1.WIDTH / 2 - 95, Game1.HEIGHT / 2 - 50), "Assign boards", "Font1", Color.White));
            visuals2.Add("Check1", randomBox);
            visuals2.Add("Box3", new TextureVisual(new Vector2(Game1.WIDTH / 2 - 100, Game1.HEIGHT / 2 + 25), 150, 50, "line", Color.SlateGray));
            visuals2.Add("String3", new TextVisual(new Vector2(Game1.WIDTH / 2 - 95, Game1.HEIGHT / 2 + 25), "Only Side A", "Font1", Color.White));
            visuals2.Add("Check2", onlyABox);
            visuals2.Add("CRoom", CRoom);
            visuals2.Add("backButton", backButton);

            connectDia.Add("connectBox", new TextureVisual(new Vector2(Game1.WIDTH / 2 - 150, Game1.HEIGHT / 2 - 75), 300, 125, "line", Color.Silver));
            connectDia.Add("Box", new TextureVisual(new Vector2(Game1.WIDTH / 2 - 100, Game1.HEIGHT / 2 - 70), 225, 50, "line", Color.SlateGray));
            connectDia.Add("message", new TextVisual(new Vector2(Game1.WIDTH / 2 - 90, Game1.HEIGHT / 2 - 65), "", "Font1", Color.White));
            OK = new Button(new Vector2(Game1.WIDTH / 2 - 60, Game1.HEIGHT / 2 - 10), 120, 50, "Okay", "Font1");
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
