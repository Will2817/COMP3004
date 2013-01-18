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

namespace Lotus
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int NUMBER_OF_AXIS = 3;  //X Y Z

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static SpriteFont dataFont; //font used to display data in the window
        public static bool displayInstrumentation = true; //display extra, debug,  info on game window when true
        public static bool performBackfaceCulling = true; //toggle back face culling on and off
        bool spaceKeyEnabled = true;  //used to latch space bar to implement a toggle
        bool BKeyEnabled = true;  //used to latch B character to implement a toggle

        Camera camera;
        Vector3 cameraInitialPosition;

        VertexPositionColor[] axisLines; //vertices of lines

        BasicEffect effect;

        Matrix world;
        Matrix axisWorld;
        Matrix worldTranslationMatrix;
        Matrix worldScaleMatrix;

        CubeModel board;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;

        }

        private void handleKeyboardEvents()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();



            //space bar is used to toggle instrumentation display

            if (spaceKeyEnabled)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    displayInstrumentation = !displayInstrumentation;
                    spaceKeyEnabled = false;
                }
            }
            if (keyboardState.IsKeyUp(Keys.Space)) spaceKeyEnabled = true;

            if (BKeyEnabled)
            {
                if (keyboardState.IsKeyDown(Keys.C))
                {
                    performBackfaceCulling = !performBackfaceCulling;
                    BKeyEnabled = false;
                }
            }
            if (keyboardState.IsKeyUp(Keys.B)) BKeyEnabled = true;


        } //=====end keyboard handling===============

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 700;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "COMP3004 Lotus";

            cameraInitialPosition = new Vector3(1.75f, 0.9f, 1.75f);
            camera = new Camera(this,  //game
                                cameraInitialPosition,  //camera location
                                new Vector3(0, 0, 0),    //target camera is looking at -origin (0,0,0)
                                Vector3.Up  //orientation (0,1,0)
                                );


            Components.Add(camera);

            world = Matrix.Identity;
            axisWorld = Matrix.Identity;

            worldTranslationMatrix = Matrix.Identity;
            worldScaleMatrix = Matrix.Identity;

            board = new CubeModel(this, 2, 0.1f, 2);
            board.translate(Matrix.CreateTranslation(new Vector3(0,-0.05f,0)));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            dataFont = Content.Load<SpriteFont>(@"Fonts/DataFont");

            string[] boardTextures = {
                "Images/wood_grain",  //frontFace
                "Images/wood_grain",  //backFace
                "Images/lotus_board",  //topFace
                "Images/wood_grain",  //bottomFace
                "Images/wood_grain",  //rightFace
                "Images/wood_grain"   //leftFace
            };

            board.LoadContent(boardTextures);

            //Create axis lines for debugging
            axisLines = new VertexPositionColor[(NUMBER_OF_AXIS) * 2];
            //X axis
            axisLines[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Red);
            axisLines[1] = new VertexPositionColor(new Vector3(2, 0, 0), Color.Red);
            //Y axis
            axisLines[2] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Blue);
            axisLines[3] = new VertexPositionColor(new Vector3(0, 2, 0), Color.Blue);
            //Z axis
            axisLines[4] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Yellow);
            axisLines[5] = new VertexPositionColor(new Vector3(0, 0, 2), Color.Yellow);

            // Create new basic effect and properites
            effect = new BasicEffect(GraphicsDevice);
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
        //                      =======
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Handle user interaction with keyboard to trasnslate the object
            handleKeyboardEvents(); //check for exit or debug display etc.


            //translate based on arrow keys of keyboard
            KeyboardState keyboardState = Keyboard.GetState();

            //transform the object center (origin) to where it would be under world transformation
            Vector3 centroidLocation = Vector3.Transform(new Vector3(0, 0, 0), world);
            //find the distance to the camera
            float distanceToCamera = Vector3.Distance(centroidLocation, camera.cameraPosition);
            //create a scale matrix based on the distance to the camera
            worldScaleMatrix = Matrix.CreateScale(3.0f / distanceToCamera);


            board.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        //                      =====
        {
            GraphicsDevice.Clear(Color.Black);


            //set camera info and object info
            effect.World = world;  //position in world to draw object
            //effect.World = worldScaleMatrix * world;  //scale and position in world to draw object
            effect.View = camera.view; //use camera view frustrum
            effect.Projection = camera.projection; //use camera projection
            effect.VertexColorEnabled = true;

            //Uncomment the following code to disable back face culling
            //This can be useful for debugging
            //NOTE this is being done for each draw because using the
            //sprite batch below will reset the CullMode otherwise
            //this could be set once in the LoadContent() method

            RasterizerState rs = new RasterizerState();
            if (!performBackfaceCulling)
            {
                rs.CullMode = CullMode.None;
                GraphicsDevice.RasterizerState = rs;
            }
            else
            {
                rs.CullMode = CullMode.CullCounterClockwiseFace;
                GraphicsDevice.RasterizerState = rs;
            }


            effect.VertexColorEnabled = false;  //because we are about to render textures not color
            effect.TextureEnabled = true;  //because we are about to render textures not colors

            board.Draw(effect);

            if (displayInstrumentation)
            {
                effect.World = axisWorld; //position axis at origin
                // Start using the BasicEffect to draw User primitives
                effect.VertexColorEnabled = true;  //because we are about to render colors not textures
                effect.TextureEnabled = false;  //because we are about to render colors not textures

                effect.CurrentTechnique.Passes[0].Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, axisLines, 0, NUMBER_OF_AXIS);

                //Draw a line to where the origin would be transformed to under the world matrix
                Vector3 originTransformed = Vector3.Transform(new Vector3(0, 0, 0), world);
                VertexPositionColor[] originToTarget = new VertexPositionColor[2];
                //target location
                originToTarget[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Black);
                originToTarget[1] = new VertexPositionColor(originTransformed, Color.Black);

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, originToTarget, 0, 1);

                //Use sprite drawing to draw useful debug text on the game window using screen co-ordinates
                //Note that spriteBatch.Begin() will reset things like the CullMode

                spriteBatch.Begin();

                //draw data output to screen
                //draw mouse location

                //figure out how far the centroid of the object is to the camera position
                Vector3 centroidLocation = Vector3.Transform(new Vector3(0, 0, 0), world);
                spriteBatch.DrawString(dataFont, "Dist to Cam:" + Vector3.Distance(centroidLocation, camera.cameraPosition), new Vector2(Window.ClientBounds.Width - 260, 10), Color.Black);

                //spriteBatch.DrawString(dataFont, "Mouse X=" + Mouse.GetState().X + " Y=" + Mouse.GetState().Y, new Vector2(Window.ClientBounds.Width - 260, 10), Color.Black);

                spriteBatch.End();

            }



            base.Draw(gameTime);
        }
    }
}
