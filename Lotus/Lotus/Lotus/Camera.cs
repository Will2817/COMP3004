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
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        //view and projection matrices of the camera
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        //these vectors will be used to recreate the camera view matrix each frame
        public Vector3 cameraPosition { get; protected set; }
        private Vector3 defaultCameraPosition;
        private Vector3 cameraUp;
        private Vector3 cameratarget;

        private float radians = 0.0f;
        private float zoomLevel = 2.0f;
        

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            //create the matrix that describes where the camera is and where
            //it is looking
            //pos = location of the camera
            //target = point where camera is looking
            //up = defines the up orientation of the camera

            //view = Matrix.CreateLookAt(pos, target, up);
            cameratarget = target;
            cameraPosition = pos;
            defaultCameraPosition = pos;
            cameraUp = up;

            CreateLookAt();

            projection = Matrix.CreatePerspectiveFieldOfView(
                 MathHelper.PiOver4,  //field of view
                 (float) Game.Window.ClientBounds.Width / (float) Game.Window.ClientBounds.Height,  //aspect ration
                 1,  //near field distance
                 100  //far field distance
                 );
        }

        private void CreateLookAt()
        {
            //Create a LookAt view matrix using the camera direction
            //This will turn the camera direction and position into a target point
            view = Matrix.CreateLookAt(cameraPosition, cameratarget, cameraUp);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
 
            //Keybaord Handler
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                radians += MathHelper.Pi / 150;
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                radians -= MathHelper.Pi / 150;
            }

            //Prevents radians variable from getting to big
            if (radians > MathHelper.TwoPi)
            {
                radians -= MathHelper.TwoPi;
            }
            if (radians < -MathHelper.TwoPi)
            {
                radians += MathHelper.TwoPi;
            }

            cameraPosition = new Vector3((float)Math.Sin(radians)* defaultCameraPosition.X, defaultCameraPosition.Y, (float)Math.Cos(radians)* defaultCameraPosition.Z);

            CreateLookAt();  //rebuild camera view matrix

            base.Update(gameTime);
        }
    }
}
