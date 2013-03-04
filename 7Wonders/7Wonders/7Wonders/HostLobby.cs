﻿using System;
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

        protected List<string> playerTypes = new List<string>() { "Open", "AIType1", "AIType2", "AIType3" };

        protected List<Visual> dropDowns;

        protected Button startButton;
        protected DropDown dropped = null;
        protected bool existsADrop = false;
        
        public HostLobby(Game1 theGame)
            : base(theGame)
        {
            startButton = new Button(game, new Vector2(90, Game1.HEIGHT - 100), 75, 40, "Start", "Font1");

            dropDowns = new List<Visual>();
            dropDowns.Add((new DropDown(game, new Vector2(MARGIN, MARGIN), DROPDOWNWIDTH, DROPDOWNHEIGHT, new List<string>() { "Host Player" })).setEnabled(false));
            
            for (int i = 1; i < NUMPLAYERS; i++)
            {
                dropDowns.Add(new DropDown(game, new Vector2(MARGIN, MARGIN + (MARGIN + DROPDOWNHEIGHT) * i), DROPDOWNWIDTH, DROPDOWNHEIGHT, playerTypes));
            }

            for (int i = dropDowns.Count; i > 0; i--)
            {
                visuals1.Add("drop"+i, dropDowns[i-1]);
            }

            visuals1.Add("startButton", startButton);
        }
        
        public override void receiveMessage(Dictionary<string, string> message)
        {
            random = Boolean.Parse(message["random"]);
            onlyA = Boolean.Parse(message["onlyA"]);
            if (onlyA) sideButton.setVisible(false);
            else sideButton.setVisible(true);
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
                        dropped = null;
                        existsADrop = false;
                    }
                }
            }
        }

        public override Dictionary<string, string> isFinished()
        {
            if (finished)
            {
                return MainMenu.createMessage();
            }

            return null;
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