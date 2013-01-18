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
    class CubeModel : PrimitiveModel
    {
        Texture2D frontFace;
        Texture2D backFace;
        Texture2D topFace;
        Texture2D bottomFace;
        Texture2D rightFace;
        Texture2D leftFace;

        float width;
        float height;
        float depth;

        VertexPositionTexture[] vertexTextures; //vertices of triangles with texture mapping

        public CubeModel(Game1 theGame, float w, float h, float d)
            : base(theGame)
        {
            initializeCube();
            dimensionScaleMatrix = Matrix.CreateScale(w / width, h / height, d / depth);
        }

        public CubeModel(Game1 theGame)
            : base(theGame)
        {
            initializeCube();
        }

        public Vector3 getForwardDirectionVector()
        {
            Matrix directionMatrix = orientationMatrix;
            return Vector3.Transform(Vector3.UnitX, directionMatrix);
        }

        private void initializeCube()
        {
            //These are the vertices of a die
            //each of the 6 faces is represented by a quad of two triangles
            //stored in a triangle list format

            //nominal 2x2x2 cube
            dimensionScaleMatrix = Matrix.Identity;
            orientationMatrix = Matrix.Identity;
            orientTranslationMatrix = Matrix.Identity;

            width = 2.0f;
            height = 2.0f;
            depth = 2.0f;

            vertexTextures = new VertexPositionTexture[6 * 2 * 3];

            //front face 6
            vertexTextures[0] = new VertexPositionTexture(new Vector3(-1, 1, 1), new Vector2(0, 0));
            vertexTextures[1] = new VertexPositionTexture(new Vector3(1, 1, 1), new Vector2(1, 0));
            vertexTextures[2] = new VertexPositionTexture(new Vector3(-1, -1, 1), new Vector2(0, 1));
            vertexTextures[3] = new VertexPositionTexture(new Vector3(1, 1, 1), new Vector2(1, 0));
            vertexTextures[4] = new VertexPositionTexture(new Vector3(1, -1, 1), new Vector2(1, 1));
            vertexTextures[5] = new VertexPositionTexture(new Vector3(-1, -1, 1), new Vector2(0, 1));

            //back face 1
            vertexTextures[6] = new VertexPositionTexture(new Vector3(-1, 1, -1), new Vector2(0, 0));
            vertexTextures[7] = new VertexPositionTexture(new Vector3(-1, -1, -1), new Vector2(0, 1));
            vertexTextures[8] = new VertexPositionTexture(new Vector3(1, 1, -1), new Vector2(1, 0));
            vertexTextures[9] = new VertexPositionTexture(new Vector3(1, 1, -1), new Vector2(1, 0));
            vertexTextures[10] = new VertexPositionTexture(new Vector3(-1, -1, -1), new Vector2(0, 1));
            vertexTextures[11] = new VertexPositionTexture(new Vector3(1, -1, -1), new Vector2(1, 1));


            //top face 4
            vertexTextures[12] = new VertexPositionTexture(new Vector3(-1, 1, 1), new Vector2(0, 1)); //0
            vertexTextures[13] = new VertexPositionTexture(new Vector3(-1, 1, -1), new Vector2(0, 0)); //4
            vertexTextures[14] = new VertexPositionTexture(new Vector3(1, 1, -1), new Vector2(1, 0)); //5
            vertexTextures[15] = new VertexPositionTexture(new Vector3(-1, 1, 1), new Vector2(0, 1)); //0
            vertexTextures[16] = new VertexPositionTexture(new Vector3(1, 1, -1), new Vector2(1, 0)); //5
            vertexTextures[17] = new VertexPositionTexture(new Vector3(1, 1, 1), new Vector2(1, 1));  //1

            //bottom face 3
            vertexTextures[18] = new VertexPositionTexture(new Vector3(-1, -1, 1), new Vector2(0, 1)); //3
            vertexTextures[19] = new VertexPositionTexture(new Vector3(1, -1, -1), new Vector2(1, 0)); //6
            vertexTextures[20] = new VertexPositionTexture(new Vector3(-1, -1, -1), new Vector2(0, 0)); //7
            vertexTextures[21] = new VertexPositionTexture(new Vector3(-1, -1, 1), new Vector2(0, 1)); //3
            vertexTextures[22] = new VertexPositionTexture(new Vector3(1, -1, 1), new Vector2(1, 1)); //2
            vertexTextures[23] = new VertexPositionTexture(new Vector3(1, -1, -1), new Vector2(1, 0));  //6

            //right face 5
            vertexTextures[24] = new VertexPositionTexture(new Vector3(1, 1, 1), new Vector2(0, 0)); //1
            vertexTextures[25] = new VertexPositionTexture(new Vector3(1, 1, -1), new Vector2(1, 0)); //5
            vertexTextures[26] = new VertexPositionTexture(new Vector3(1, -1, 1), new Vector2(0, 1)); //2
            vertexTextures[27] = new VertexPositionTexture(new Vector3(1, -1, 1), new Vector2(0, 1)); //2
            vertexTextures[28] = new VertexPositionTexture(new Vector3(1, 1, -1), new Vector2(1, 0)); //5
            vertexTextures[29] = new VertexPositionTexture(new Vector3(1, -1, -1), new Vector2(1, 1));  //6

            //left face 2
            vertexTextures[30] = new VertexPositionTexture(new Vector3(-1, -1, -1), new Vector2(0, 1)); //7
            vertexTextures[31] = new VertexPositionTexture(new Vector3(-1, 1, -1), new Vector2(0, 0)); //4
            vertexTextures[32] = new VertexPositionTexture(new Vector3(-1, 1, 1), new Vector2(1, 0)); //0
            vertexTextures[33] = new VertexPositionTexture(new Vector3(-1, -1, -1), new Vector2(0, 1)); //7
            vertexTextures[34] = new VertexPositionTexture(new Vector3(-1, 1, 1), new Vector2(1, 0)); //0
            vertexTextures[35] = new VertexPositionTexture(new Vector3(-1, -1, 1), new Vector2(1, 1));  //3

        }


        public override void LoadContent(string[] textures)
        {
            frontFace = game.Content.Load<Texture2D>(@textures[0]);
            backFace = game.Content.Load<Texture2D>(@textures[1]);
            topFace = game.Content.Load<Texture2D>(@textures[2]);
            bottomFace = game.Content.Load<Texture2D>(@textures[3]);
            rightFace = game.Content.Load<Texture2D>(@textures[4]);
            leftFace = game.Content.Load<Texture2D>(@textures[5]);


        }

        public override void Update(GameTime gameTime)
        {

            world = dimensionScaleMatrix;
            world *= orientationMatrix;
            world *= orientTranslationMatrix;

            world *= worldTranslationMatrix;
            base.Update(gameTime);
        }

        public override void Draw(BasicEffect effect)
        {

            effect.World = world; //set position to draw model

            effect.VertexColorEnabled = false;  //because we are about to render textures not color
            effect.TextureEnabled = true;  //because we are about to render textures not colors


            effect.Texture = frontFace;
            effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertexTextures, 0, 2);

            effect.Texture = backFace;
            effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertexTextures, 6, 2);

            effect.Texture = topFace;
            effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertexTextures, 12, 2);

            effect.Texture = bottomFace;
            effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertexTextures, 18, 2);

            effect.Texture = rightFace;
            effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertexTextures, 24, 2);

            effect.Texture = leftFace;
            effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertexTextures, 30, 2);
        }
    }
}

