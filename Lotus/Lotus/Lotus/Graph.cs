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
            Color color = Color.Gray;
            foreach (Node sourceNode in nodes.Values)
            {
                foreach (Node destNode in sourceNode.neighbours.Values)
                {



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
            public float x, z;
            public static float rad = 0.06f;
            public static float y = 0f;
            public Color color = Color.Gray;
            public VertexPositionTexture[] vertexTextures;

            public Node(string name, List<Node> adjacents, float _x = 0, float _z = 0)
            {
                value = name;
                neighbours = new Dictionary<string, Node>();
                vertexTextures = new VertexPositionTexture[2 * 3];

                x = _x;
                z = _z;

                vertexTextures[0] = new VertexPositionTexture(new Vector3(x - rad, y, z + rad), new Vector2(0, 1)); //0 y values need to be fixed to board height
                vertexTextures[1] = new VertexPositionTexture(new Vector3(x - rad, y, z - rad), new Vector2(0, 0)); //4
                vertexTextures[2] = new VertexPositionTexture(new Vector3(x + rad, y, z - rad), new Vector2(1, 0)); //5
                vertexTextures[3] = new VertexPositionTexture(new Vector3(x - rad, y, z + rad), new Vector2(0, 1)); //0
                vertexTextures[4] = new VertexPositionTexture(new Vector3(x + rad, y, z - rad), new Vector2(1, 0)); //5
                vertexTextures[5] = new VertexPositionTexture(new Vector3(x + rad, y, z + rad), new Vector2(1, 1));  //1

                foreach (Node neighbour in adjacents)
                {
                    neighbours[neighbour.value] = neighbour;
                }
            }

            public bool isInRange(float qx, float qz)
            {
                float xDist = qx - x;
                float zDist = qz - z;
                double dist = Math.Sqrt(xDist * xDist + zDist * zDist);
                return dist <= rad;
            }
        }
    }
}