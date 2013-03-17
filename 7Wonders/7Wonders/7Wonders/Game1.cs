using System;
using System.IO;
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
using Newtonsoft.Json.Linq;
using System.Threading;

namespace _7Wonders
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Dictionary<String, Texture2D> textures;
        public static Dictionary<String, SpriteFont> fonts;
        public static Dictionary<String, Interface> interfaces;
        public static Dictionary<String, Visual> wonders;
        public static Dictionary<String, Visual> cards;

        public static int MAXPLAYER = 7;

        public static string recordedPresses = "";
        static KeyboardState prevState = Keyboard.GetState();

        public static int WIDTH = 800;
        public static int HEIGHT = 600;
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        int cycle = 0;
        Boolean leftkeylock = false;
        Boolean rightkeylock = false;
        Interface activeInterface;
        JObject wondersJson = JObject.Parse(File.ReadAllText("Content/Json/wonderlist.json"));
        JObject cardsJson = JObject.Parse(File.ReadAllText("Content/Json/cards.json"));
        public static Client.Client client;
        public static Server.Server host;

        

        public Game1()
        {
            CardLibrary.init();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            textures = new Dictionary<String, Texture2D>();
            fonts = new Dictionary<String, SpriteFont>();
            client = new Client.Client();

            // Adding Wonders Visual
            wonders = new Dictionary<String, Visual>();
            foreach (JObject j in (JArray)wondersJson["wonders"])
            {
                wonders.Add((string)j["name"], new Visual(new Vector2(0, 0), 0, 0, (string)j["a"]["image"]));
            }

            cards = new Dictionary<string, Visual>();
            foreach (JObject j in (JArray)cardsJson["cards"])
            {
                cards.Add((string)j["name"] + "_" + j["age"] + "_" + j["players"], new Visual(new Vector2(0, 0), 0, 0, (string)j["name"] + "_" + j["age"] + "_" + j["players"]));
            }

            interfaces = new Dictionary<String, Interface>();
            interfaces.Add("mainmenu", new MainMenu());
            interfaces.Add("lobby", new Lobby());
            interfaces.Add("hostlobby", new HostLobby());
            interfaces.Add("maingame", new MainGame(this));
            activeInterface = interfaces["mainmenu"];

            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            device = graphics.GraphicsDevice;
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "7 Wonders";
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            textures.Add("title", Content.Load<Texture2D>("Images/title"));
            textures.Add("line", Content.Load<Texture2D>("Images/line"));
            textures.Add("drop", Content.Load<Texture2D>("Images/down"));
            textures.Add("button", Content.Load<Texture2D>("Images/button"));
            textures.Add("left", Content.Load<Texture2D>("Images/left"));
            textures.Add("right", Content.Load<Texture2D>("Images/right"));
            textures.Add("icons", Content.Load<Texture2D>("Images/icons"));
            textures.Add("blank", Content.Load<Texture2D>("Images/blank"));
            textures.Add("background", Content.Load<Texture2D>("Images/background"));
            textures.Add("grayback", Content.Load<Texture2D>("Images/grayback"));
            textures.Add("age1", Content.Load<Texture2D>("Images/age1"));
            textures.Add("age2", Content.Load<Texture2D>("Images/age2"));
            textures.Add("age3", Content.Load<Texture2D>("Images/age3"));
            textures.Add("resourceBar", Content.Load<Texture2D>("Images/resourceBar"));
            textures.Add("emptyResourceBar", Content.Load<Texture2D>("Images/emptyResourceBar"));
            textures.Add("paperleft", Content.Load<Texture2D>("Images/paper_left"));
            textures.Add("papermiddle", Content.Load<Texture2D>("Images/paper_middle"));
            textures.Add("bg", Content.Load<Texture2D>("Images/bg"));
            textures.Add("discard", Content.Load<Texture2D>("Images/deck"));
            textures.Add("greenglow", Content.Load<Texture2D>("Images/greenglow"));
            textures.Add("goldglow", Content.Load<Texture2D>("Images/goldglow"));
            textures.Add("redglow", Content.Load<Texture2D>("Images/redglow"));
            textures.Add("reast", Content.Load<Texture2D>("Images/r_east"));
            textures.Add("rwest", Content.Load<Texture2D>("Images/r_west"));
            textures.Add("rboth", Content.Load<Texture2D>("Images/r_both"));
            textures.Add("mboth", Content.Load<Texture2D>("Images/m_both"));
            textures.Add("border", Content.Load<Texture2D>("Images/border"));
            foreach (string s in Enum.GetNames(typeof (Resource)))
            {
                if (s!="COIN") textures.Add(s, Content.Load<Texture2D>("Images/Resources/"+s));
            }
            textures.Add("CLAYORE", Content.Load<Texture2D>("Images/Resources/CLAYORE"));
            textures.Add("STONECLAY", Content.Load<Texture2D>("Images/Resources/STONECLAY"));
            textures.Add("STONEORE", Content.Load<Texture2D>("Images/Resources/STONEORE"));
            textures.Add("STONEWOOD", Content.Load<Texture2D>("Images/Resources/STONEWOOD"));
            textures.Add("WOODCLAY", Content.Load<Texture2D>("Images/Resources/WOODCLAY"));
            textures.Add("WOODORE", Content.Load<Texture2D>("Images/Resources/WOODORE"));
            textures.Add("WOODSTONEORECLAY", Content.Load<Texture2D>("Images/Resources/WOODSTONEORECLAY"));
            textures.Add("GLASSLOOMPAPYRUS", Content.Load<Texture2D>("Images/Resources/GLASSLOOMPAPYRUS"));

            textures.Add("plus", Content.Load<Texture2D>("Images/plus"));
            textures.Add("minus", Content.Load<Texture2D>("Images/minus"));
            textures.Add("conflict", Content.Load<Texture2D>("Images/fight1"));
            textures.Add("deck", Content.Load<Texture2D>("Images/deck"));

            foreach (JObject j in (JArray)wondersJson["wonders"])
            {
                textures.Add((string)j["a"]["image"], Content.Load<Texture2D>("Images/Wonders/" + j["a"]["image"]));
                textures.Add((string)j["b"]["image"], Content.Load<Texture2D>("Images/Wonders/" + j["b"]["image"]));
            }

            foreach (JObject j in (JArray)cardsJson["cards"])
            {
                textures.Add((string)j["name"]+"_"+j["age"]+"_"+j["players"], Content.Load<Texture2D>("Images/Cards" + j["image"])); 
            }

            fonts.Add("Font1", Content.Load<SpriteFont>("Fonts/Font1"));

            foreach (Interface i in interfaces.Values)
            {
                i.LoadContent();
            }

            foreach (Visual v in cards.Values)
            {
                v.LoadContent();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();
            if ((!leftkeylock) && (keyboardState.IsKeyDown(Keys.Left)))
            {
                cycle--;
                leftkeylock = true;
            }
            if ((!rightkeylock) && (keyboardState.IsKeyDown(Keys.Right)))
            {
                cycle++;
                rightkeylock = true;
            }

            if ((leftkeylock) && (keyboardState.IsKeyUp(Keys.Left)))
                leftkeylock = false;
            if ((rightkeylock) && (keyboardState.IsKeyUp(Keys.Right)))
                rightkeylock = false;

            activeInterface.Update(gameTime, Mouse.GetState());

            Dictionary<string, string> message;

            if (!client.isConnected() && (activeInterface != interfaces["mainmenu"]))
            {
                activeInterface.reset();
                activeInterface = interfaces["mainmenu"];
                message = new Dictionary<string, string>();
                message.Add("connection", "Lost Connection");
                activeInterface.receiveMessage(message);
                client = new Client.Client();
            }

            else if ((message = activeInterface.isFinished()) != null)
            {
                if (message["nextInterface"] == "hostlobby")
                {
                    Console.WriteLine("StartHost");
                    if (host != null) host.shutdown();
                    host = new Server.Server();
                    Console.WriteLine("StartClient");
                    client.joinHost(true);
                }
                if ((message["nextInterface"] == "mainmenu"))
                {
                    if (host != null) host.shutdown();
                    client.disconnect();
                    client = new Client.Client();
                }
                if ((message["nextInterface"] == "lobby"))
                {
                    Console.WriteLine("Join StartClient");
                    client.joinHost(false);
                    if (!client.isConnected())
                    {
                        message["nextInterface"] = "mainmenu";
                        message["connection"] = "Failed to Connect";
                    }
                }
                if ((message["nextInterface"] == "maingame"))
                {
                    while (!client.getState().isGameInProgress())
                    { }
                    foreach (Player p in client.getState().getPlayers().Values)
                    {
                        Console.WriteLine("HERE ARE the BOARDS: " + p.getBoard().getName());
                    }
                }
                activeInterface.reset();
                activeInterface = interfaces[message["nextInterface"]];
                activeInterface.receiveMessage(message);
            }

            ProcessKeyboard();
            prevState = Keyboard.GetState();

            base.Update(gameTime);
        }
        private void ProcessKeyboard()
        {
            KeyboardState currState = Keyboard.GetState();
            //if (keybState.IsKeyDown(Keys.Space)){
            //    if (displayInstrumentation)
            //        displayInstrumentation=false;
            //    else
            //        displayInstrumentation=true;
            //}
            if (currState.IsKeyDown(Keys.LeftShift) || currState.IsKeyDown(Keys.RightShift))
            {
                if (prevState.IsKeyUp(Keys.A) && currState.IsKeyDown(Keys.A)) recordedPresses += "A";
                if (prevState.IsKeyUp(Keys.B) && currState.IsKeyDown(Keys.B)) recordedPresses += "B";
                if (prevState.IsKeyUp(Keys.C) && currState.IsKeyDown(Keys.C)) recordedPresses += "C";
                if (prevState.IsKeyUp(Keys.D) && currState.IsKeyDown(Keys.D)) recordedPresses += "D";
                if (prevState.IsKeyUp(Keys.E) && currState.IsKeyDown(Keys.E)) recordedPresses += "E";
                if (prevState.IsKeyUp(Keys.F) && currState.IsKeyDown(Keys.F)) recordedPresses += "F";
                if (prevState.IsKeyUp(Keys.G) && currState.IsKeyDown(Keys.G)) recordedPresses += "G";
                if (prevState.IsKeyUp(Keys.H) && currState.IsKeyDown(Keys.H)) recordedPresses += "H";
                if (prevState.IsKeyUp(Keys.I) && currState.IsKeyDown(Keys.I)) recordedPresses += "I";
                if (prevState.IsKeyUp(Keys.J) && currState.IsKeyDown(Keys.J)) recordedPresses += "J";
                if (prevState.IsKeyUp(Keys.K) && currState.IsKeyDown(Keys.K)) recordedPresses += "K";
                if (prevState.IsKeyUp(Keys.L) && currState.IsKeyDown(Keys.L)) recordedPresses += "L";
                if (prevState.IsKeyUp(Keys.M) && currState.IsKeyDown(Keys.M)) recordedPresses += "M";
                if (prevState.IsKeyUp(Keys.N) && currState.IsKeyDown(Keys.N)) recordedPresses += "N";
                if (prevState.IsKeyUp(Keys.O) && currState.IsKeyDown(Keys.O)) recordedPresses += "O";
                if (prevState.IsKeyUp(Keys.P) && currState.IsKeyDown(Keys.P)) recordedPresses += "P";
                if (prevState.IsKeyUp(Keys.Q) && currState.IsKeyDown(Keys.Q)) recordedPresses += "Q";
                if (prevState.IsKeyUp(Keys.R) && currState.IsKeyDown(Keys.R)) recordedPresses += "R";
                if (prevState.IsKeyUp(Keys.S) && currState.IsKeyDown(Keys.S)) recordedPresses += "S";
                if (prevState.IsKeyUp(Keys.T) && currState.IsKeyDown(Keys.T)) recordedPresses += "T";
                if (prevState.IsKeyUp(Keys.U) && currState.IsKeyDown(Keys.U)) recordedPresses += "U";
                if (prevState.IsKeyUp(Keys.V) && currState.IsKeyDown(Keys.V)) recordedPresses += "V";
                if (prevState.IsKeyUp(Keys.W) && currState.IsKeyDown(Keys.W)) recordedPresses += "W";
                if (prevState.IsKeyUp(Keys.X) && currState.IsKeyDown(Keys.X)) recordedPresses += "X";
                if (prevState.IsKeyUp(Keys.Y) && currState.IsKeyDown(Keys.Y)) recordedPresses += "Y";
                if (prevState.IsKeyUp(Keys.Z) && currState.IsKeyDown(Keys.Z)) recordedPresses += "Z";
                if (prevState.IsKeyUp(Keys.OemMinus) && currState.IsKeyDown(Keys.OemMinus)) recordedPresses += "_";
            }
            else
            {
                if (prevState.IsKeyUp(Keys.A) && currState.IsKeyDown(Keys.A)) recordedPresses += "a";
                if (prevState.IsKeyUp(Keys.B) && currState.IsKeyDown(Keys.B)) recordedPresses += "b";
                if (prevState.IsKeyUp(Keys.C) && currState.IsKeyDown(Keys.C)) recordedPresses += "c";
                if (prevState.IsKeyUp(Keys.D) && currState.IsKeyDown(Keys.D)) recordedPresses += "d";
                if (prevState.IsKeyUp(Keys.E) && currState.IsKeyDown(Keys.E)) recordedPresses += "e";
                if (prevState.IsKeyUp(Keys.F) && currState.IsKeyDown(Keys.F)) recordedPresses += "f";
                if (prevState.IsKeyUp(Keys.G) && currState.IsKeyDown(Keys.G)) recordedPresses += "g";
                if (prevState.IsKeyUp(Keys.H) && currState.IsKeyDown(Keys.H)) recordedPresses += "h";
                if (prevState.IsKeyUp(Keys.I) && currState.IsKeyDown(Keys.I)) recordedPresses += "i";
                if (prevState.IsKeyUp(Keys.J) && currState.IsKeyDown(Keys.J)) recordedPresses += "j";
                if (prevState.IsKeyUp(Keys.K) && currState.IsKeyDown(Keys.K)) recordedPresses += "k";
                if (prevState.IsKeyUp(Keys.L) && currState.IsKeyDown(Keys.L)) recordedPresses += "l";
                if (prevState.IsKeyUp(Keys.M) && currState.IsKeyDown(Keys.M)) recordedPresses += "m";
                if (prevState.IsKeyUp(Keys.N) && currState.IsKeyDown(Keys.N)) recordedPresses += "n";
                if (prevState.IsKeyUp(Keys.O) && currState.IsKeyDown(Keys.O)) recordedPresses += "o";
                if (prevState.IsKeyUp(Keys.P) && currState.IsKeyDown(Keys.P)) recordedPresses += "p";
                if (prevState.IsKeyUp(Keys.Q) && currState.IsKeyDown(Keys.Q)) recordedPresses += "q";
                if (prevState.IsKeyUp(Keys.R) && currState.IsKeyDown(Keys.R)) recordedPresses += "r";
                if (prevState.IsKeyUp(Keys.S) && currState.IsKeyDown(Keys.S)) recordedPresses += "s";
                if (prevState.IsKeyUp(Keys.T) && currState.IsKeyDown(Keys.T)) recordedPresses += "t";
                if (prevState.IsKeyUp(Keys.U) && currState.IsKeyDown(Keys.U)) recordedPresses += "u";
                if (prevState.IsKeyUp(Keys.V) && currState.IsKeyDown(Keys.V)) recordedPresses += "v";
                if (prevState.IsKeyUp(Keys.W) && currState.IsKeyDown(Keys.W)) recordedPresses += "w";
                if (prevState.IsKeyUp(Keys.X) && currState.IsKeyDown(Keys.X)) recordedPresses += "x";
                if (prevState.IsKeyUp(Keys.Y) && currState.IsKeyDown(Keys.Y)) recordedPresses += "y";
                if (prevState.IsKeyUp(Keys.Z) && currState.IsKeyDown(Keys.Z)) recordedPresses += "z";
                if (prevState.IsKeyUp(Keys.OemMinus) && currState.IsKeyDown(Keys.OemMinus)) recordedPresses += "-";
            }
            if (prevState.IsKeyUp(Keys.Space) && currState.IsKeyDown(Keys.Space)) recordedPresses += " ";
            if (prevState.IsKeyUp(Keys.OemPeriod) && currState.IsKeyDown(Keys.OemPeriod)) recordedPresses += ".";
            if (prevState.IsKeyUp(Keys.Back) && currState.IsKeyDown(Keys.Back)) recordedPresses += "back";
            if (prevState.IsKeyUp(Keys.D0) && currState.IsKeyDown(Keys.D0)) recordedPresses += "0";
            if (prevState.IsKeyUp(Keys.D1) && currState.IsKeyDown(Keys.D1)) recordedPresses += "1";
            if (prevState.IsKeyUp(Keys.D2) && currState.IsKeyDown(Keys.D2)) recordedPresses += "2";
            if (prevState.IsKeyUp(Keys.D3) && currState.IsKeyDown(Keys.D3)) recordedPresses += "3";
            if (prevState.IsKeyUp(Keys.D4) && currState.IsKeyDown(Keys.D4)) recordedPresses += "4";
            if (prevState.IsKeyUp(Keys.D5) && currState.IsKeyDown(Keys.D5)) recordedPresses += "5";
            if (prevState.IsKeyUp(Keys.D6) && currState.IsKeyDown(Keys.D6)) recordedPresses += "6";
            if (prevState.IsKeyUp(Keys.D7) && currState.IsKeyDown(Keys.D7)) recordedPresses += "7";
            if (prevState.IsKeyUp(Keys.D8) && currState.IsKeyDown(Keys.D8)) recordedPresses += "8";
            if (prevState.IsKeyUp(Keys.D9) && currState.IsKeyDown(Keys.D9)) recordedPresses += "9";
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);
            spriteBatch.Begin();

            //wonders.Values.ElementAt(cycle).Draw(gameTime, spriteBatch);
            // TODO: Add your drawing code here
            activeInterface.Draw(gameTime, spriteBatch);
           // spriteBatch.Draw(textures[image], new Rectangle(0, 0, 100, 40), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void OnExiting(Object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            if (host != null) host.shutdown();
            else client.disconnect();

            Exit();
            // Stop the threads
        }
    }
}
