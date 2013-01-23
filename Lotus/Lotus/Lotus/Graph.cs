using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Lotus
{
    public class Graph
    {
        public Dictionary<String, Node> nodes;
        

        public Graph()
        {
            nodes = new Dictionary<string, Node>();
        }

        public void draw(Game1 game, BasicEffect effect)
        {
            effect.VertexColorEnabled = false;  //because we are about to render textures not color
            effect.TextureEnabled = true;  //because we are about to render textures not colors
            VertexPositionTexture[] vertexTextures;
            foreach (Node sourceNode in nodes.Values)
            {
                foreach (Node destNode in sourceNode.neighbours.Values)
                {
                    Matrix world = Matrix.Identity;

                    vertexTextures = new VertexPositionTexture[2 * 3];

                    float width = 0.01f;
                    float length = (destNode.position-sourceNode.position).Length();
                    float angle = (float)Math.Atan2(destNode.position.X - sourceNode.position.X, destNode.position.Z - sourceNode.position.Z);
                    Matrix rotate = Matrix.CreateRotationY(angle-(float)MathHelper.PiOver2);
                    Matrix translate = Matrix.CreateTranslation(sourceNode.position);                                       

                    Vector3 vertex1 = new Vector3(0, 0, width);
                    Vector3 vertex2 = new Vector3(0, 0, -width);
                    Vector3 vertex3 = new Vector3(length, 0, width);
                    Vector3 vertex4 = new Vector3(length, 0, -width);

                    vertexTextures[0] = new VertexPositionTexture(vertex3, new Vector2(0, 1));
                    vertexTextures[1] = new VertexPositionTexture(vertex1, new Vector2(0, 0));
                    vertexTextures[2] = new VertexPositionTexture(vertex2, new Vector2(1, 0));
                    vertexTextures[3] = new VertexPositionTexture(vertex3, new Vector2(0, 1));
                    vertexTextures[4] = new VertexPositionTexture(vertex2, new Vector2(1, 0));
                    vertexTextures[5] = new VertexPositionTexture(vertex4, new Vector2(1, 1));
                    world *= rotate * translate;
                    effect.World = world;
                    effect.Texture = Game1.textures["line"];
                    effect.CurrentTechnique.Passes[0].Apply();
                    game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertexTextures, 0, 2);


                    //effect.Texture = Game1.textures["line"];
                    //effect.CurrentTechnique.Passes[0].Apply();
                    //GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertexTextures, 0, 2);
                   // batch.Draw(Game1.textures["line"], new Rectangle((int)sourceNode.x, (int)sourceNode.z, (int)Math.Sqrt(Math.Pow((sourceNode.x - destNode.x), 2) + Math.Pow((sourceNode.z - destNode.z), 2)), 5),
                   //     null, color, (float)Math.Atan2((destNode.z - sourceNode.z), (destNode.x - sourceNode.x)), new Vector2(0, 0), SpriteEffects.None, 0.0f);
                }
            }
            foreach (Node sourceNode in nodes.Values)
            {
                effect.World = Matrix.Identity;
                effect.Texture = Game1.textures["circle"];
                effect.CurrentTechnique.Passes[0].Apply();
                game.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, sourceNode.vertexTextures, 0, 2);
                //batch.Draw(Game1.textures["circle"], new Rectangle((int)sourceNode.x - Node.rad, (int)sourceNode.z - Node.rad, Node.rad * 2, Node.rad * 2), sourceNode.color);
            }
        }

        public void add(String name, String[] adjacents, float _x, float _z)
        {
            List<string> neighbours = new List<string>();
            foreach (String s in adjacents)
            {
                neighbours.Add(s);
            }

            add(name, neighbours, _x, _z);
        }

        public string findNodeClicked(float _x, float _z)
        {
            foreach (Node node in nodes.Values){
                if (node.isInRange(_x, _z)){
                    return node.value;
                }
            }
            return "None";
        }

        public void add(string name, List<string> adjacents, float _x, float _z)
        {
            List<Node> adj = new List<Node>();

            if (adjacents != null)
            {
                foreach (string candidate in adjacents)
                {
                    if (nodes.ContainsKey(candidate))
                    {
                        adj.Add(nodes[candidate]);
                    }

                }
            }
            nodes.Add(name, new Node(name, adj, _x, _z));
        }

        public class Node
        {
            public Dictionary<string, Node> neighbours;
            public string value;
            public static float rad = 0.06f;
            public static float y = 0f;
            public Color color = Color.Gray;
            public VertexPositionTexture[] vertexTextures;
            public Vector3 position;

            public Node(string name, List<Node> adjacents, float _x = 0, float _z = 0)
            {
                value = name;
                neighbours = new Dictionary<string, Node>();
                vertexTextures = new VertexPositionTexture[2 * 3];

                position = new Vector3(_x, y, _z);

                vertexTextures[0] = new VertexPositionTexture(position + new Vector3(-rad, 0,rad), new Vector2(0, 1)); //0 y values need to be fixed to board height
                vertexTextures[1] = new VertexPositionTexture(position + new Vector3(-rad, 0,-rad), new Vector2(0, 0)); //4
                vertexTextures[2] = new VertexPositionTexture(position + new Vector3(rad, 0,-rad), new Vector2(1, 0)); //5
                vertexTextures[3] = new VertexPositionTexture(position + new Vector3(-rad, 0,rad), new Vector2(0, 1)); //0
                vertexTextures[4] = new VertexPositionTexture(position + new Vector3(rad, 0,-rad), new Vector2(1, 0)); //5
                vertexTextures[5] = new VertexPositionTexture(position + new Vector3(rad, 0,rad), new Vector2(1, 1));  //1

                foreach (Node neighbour in adjacents)
                {
                    neighbours[neighbour.value] = neighbour;
                }
            }

            public bool isInRange(float qx, float qz)
            {
                float xDist = qx - position.X;
                float zDist = qz - position.Z;
                double dist = Math.Sqrt(xDist * xDist + zDist * zDist);
                return dist <= rad;
            }
        }
    }
}