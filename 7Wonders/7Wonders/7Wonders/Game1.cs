using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _7Wonders
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Dictionary<String, Texture2D> textures;
        public static Dictionary<String, SpriteFont> fonts;

        public static int WIDTH = 800;
        public static int HEIGHT = 600;
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        Dictionary<String, Wonder> wonders;
        int cycle = 0;
        Boolean leftkeylock = false;
        Boolean rightkeylock = false;

        Interface activeInterface;

        MainMenu mainMenu;
        Lobby lobby;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            wonders = new Dictionary<String, Wonder>();
            textures = new Dictionary<String, Texture2D>();
            fonts = new Dictionary<String, SpriteFont>();
            mainMenu = new MainMenu(this);
            lobby = new Lobby(this);
            activeInterface = mainMenu;
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
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Window.Title = "7 Wonders";
            wonders.Add("Wonder1a", new Wonder(this, Vector2.Zero, WIDTH, HEIGHT, "wonder1a"));
            wonders.Add("Wonder2a", new Wonder(this, Vector2.Zero, WIDTH, HEIGHT, "wonder2a"));
            wonders.Add("Wonder3a", new Wonder(this, Vector2.Zero, WIDTH, HEIGHT, "wonder3a"));
            wonders.Add("Wonder4a", new Wonder(this, Vector2.Zero, WIDTH, HEIGHT, "wonder4a"));
            wonders.Add("Wonder5a", new Wonder(this, Vector2.Zero, WIDTH, HEIGHT, "wonder5a"));
            wonders.Add("Wonder6a", new Wonder(this, Vector2.Zero, WIDTH, HEIGHT, "wonder6a"));
            wonders.Add("Wonder7a", new Wonder(this, Vector2.Zero, WIDTH, HEIGHT, "wonder7a"));
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
            textures.Add("wonder1a", Content.Load<Texture2D>("Images/wonder1a"));
            textures.Add("wonder2a", Content.Load<Texture2D>("Images/wonder2a"));
            textures.Add("wonder3a", Content.Load<Texture2D>("Images/wonder3a"));
            textures.Add("wonder4a", Content.Load<Texture2D>("Images/wonder4a"));
            textures.Add("wonder5a", Content.Load<Texture2D>("Images/wonder5a"));
            textures.Add("wonder6a", Content.Load<Texture2D>("Images/wonder6a"));
            textures.Add("wonder7a", Content.Load<Texture2D>("Images/wonder7a"));

            fonts.Add("Font1", Content.Load<SpriteFont>("Fonts/Font1"));

            foreach (Wonder w in wonders.Values)
            {
                w.LoadContent();
            }
            mainMenu.LoadContent();
            lobby.LoadContent();
            // TODO: use this.Content to load your game content here
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
            foreach (Wonder w in wonders.Values)
            {
                w.Update(gameTime, Mouse.GetState());
            }
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

            if (cycle<0){
                cycle = wonders.Count-1;
            }
            if (cycle>wonders.Count-1){
                cycle = 0;
            }

            activeInterface.Update(gameTime, Mouse.GetState());

            Dictionary<string, string> message;

            if ((message = activeInterface.isFinished()) != null)
            {
                switch (message["nextInterface"])
                {
                    case ("lobby") :
                        activeInterface = lobby;
                        lobby.recieveMessage(message);
                        break;
                }
            }

            base.Update(gameTime);
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
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
