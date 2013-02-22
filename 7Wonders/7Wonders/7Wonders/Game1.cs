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
        public static Dictionary<String, Interface> interfaces;

        public static int WIDTH = 800;
        public static int HEIGHT = 600;
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        int cycle = 0;
        Boolean leftkeylock = false;
        Boolean rightkeylock = false;

        Interface activeInterface;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            textures = new Dictionary<String, Texture2D>();
            fonts = new Dictionary<String, SpriteFont>();
            interfaces = new Dictionary<String, Interface>();

            interfaces.Add("main", new MainMenu(this));
            interfaces.Add("lobby", new Lobby(this));
            activeInterface = interfaces["main"];
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

            for (int i = 0; i < 14; i++)
            {
                if (i<7)
                    textures.Add("wonder"+i+"a", Content.Load<Texture2D>("Images/Wonders/board"+i));
                else
                    textures.Add("wonder"+(i-7)+"b", Content.Load<Texture2D>("Images/Wonders/board"+i));
            }

            fonts.Add("Font1", Content.Load<SpriteFont>("Fonts/Font1"));

            foreach (Interface i in interfaces.Values)
            {
                i.LoadContent();
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

            if ((message = activeInterface.isFinished()) != null)
            {
                activeInterface.reset();
                activeInterface = interfaces[message["nextInterface"]];
                activeInterface.receiveMessage(message);
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
