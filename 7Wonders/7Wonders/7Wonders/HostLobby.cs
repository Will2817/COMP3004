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
    class HostLobby : Lobby
    {

        protected Button startButton;
        protected DropDown dropped = null;
        protected bool existsADrop = false;
        
        public HostLobby(Game1 theGame)
            : base(theGame)
        {
            startButton = new Button(game, new Vector2(90, Game1.HEIGHT - 100), 75, 40, "Start", "Font1");

            foreach (DropDown dd in dropDowns)
            {
                if (dd != dropDowns.First()) dd.setEnabled(true);
            }

            visuals1.Add("startButton", startButton);
        }

        public override void Update(GameTime gameTime, MouseState mouseState)
        {
            base.Update(gameTime, mouseState);
            
            //will need send a message of some sort
  /*          if (backButton.isClicked())
            {
                finished = true;
                backButton.reset();
            }
   */

            for (int i=dropDowns.Count; i >0; i--)
            {

                DropDown dd = (DropDown) dropDowns[i-1];
                if (dd.RequestDrop())
                {
                    if (dd == dropped)
                    {
                        dd.drop();
                    }
                    if (!existsADrop)
                    {
                        dd.drop();
                        dropped = dd;
                        existsADrop = true;
                    }
                    dd.resetRequest();
                }
                if (dd == dropped)
                {
                    if (!dd.getDown())
                    {
                        if (dd.getSelected() != "Open") Game1.host.addAIPlayer(dd.getSelected());
                        dropped = null;
                        existsADrop = false;
                    }
                }
                if (startButton.isClicked())
                {
                    startButton.reset();
                    if (Game1.client.getState().getPlayers().Count > 2)
                    {
                        backToMenu = false;
                        finished = true;
                        //Start game <-- will need to deal with picking wonders
                    }
                    else { }//error message
                }
                
            }
        }

        public override void updateHelper(int i)
        {
            dropDowns[i].setEnabled(true);
            if (dropped != null) dropped.drop();
            dropped = null;
            existsADrop = false;
            base.updateHelper(i);
        }

        public override void receiveMessage(Dictionary<string, string> message)
        {
            Game1.host.setOptions(Boolean.Parse(message["onlyA"]), Boolean.Parse(message["random"]));
        }

        public static Dictionary<string, string> createMessage(bool random, bool onlyA)
        {
            return new Dictionary<string, string>()
                {
                    {"nextInterface", "hostlobby"},
                    {"role" , "host"},
                    {"random", random.ToString()},
                    {"onlyA", onlyA.ToString()}
                };
        }
    }
}