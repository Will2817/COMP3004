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

namespace Lotus
{

    /*This class represents a 3D model made up of basic vertex primitives and triangles
     * It provides a base class for specific vertex-based models to inherit from
     * 
     * It defines a protocol for translating and rotating the model
     * The draw world location and orientation is provided by the getWorld() public method
     */
    class PrimitiveModel
    {
        protected Game1 game;
        protected Matrix world = Matrix.Identity;

        //matrices used to locate model in the world
        protected Matrix worldTranslationMatrix; //location of model in the world
        protected Matrix worldScaleMatrix; //world scaling of model


        //matrices used to orient model relative of other model parts
        //e.g. to position the wing of an airplane relative to the fuselage
        protected Matrix dimensionScaleMatrix;
        protected Matrix orientationMatrix;
        protected Matrix orientTranslationMatrix;


        public PrimitiveModel(Game1 g)
        {
            game = g;

            worldTranslationMatrix = Matrix.Identity;
            worldScaleMatrix = Matrix.Identity;
        }

        public void orientTranslate(float tx, float ty, float tz)
        {
            orientTranslationMatrix *= Matrix.CreateTranslation(new Vector3(tx, ty, tz));
        }

        //return the drawing world orientation for the model
        public virtual Matrix getWorld()
        {
            return world;
        }

        //translate the model from it current location
        public void translate(Matrix translationMatrix)
        {
            worldTranslationMatrix *= translationMatrix;
        }
        //scale
        public void scale(Matrix scaleMatrix)
        {
            worldScaleMatrix *= scaleMatrix;
        }

        public virtual void LoadContent()
        {
        }

        public virtual void LoadContent(string[] textures)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(BasicEffect effect)
        {
        }
    }

}
