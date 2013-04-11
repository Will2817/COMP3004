using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    using Game_Cards;

    /* EffectHandler Class
     * This Class has Global Functions!
     * This class will be used to control the effects 
     * of cards along with the effects of Wonders
     */
    public static class EffectHandler
    {
        // Wonder effects boolean
        static bool[] freeBuild = { true, true, true }; // Once per age a player can build for free


        // Used for converting Resources
        static Dictionary<Effect.TypeType, Resource> resourceType = new Dictionary<Effect.TypeType, Resource>
        {
            {Effect.TypeType.WOOD, Resource.WOOD}, {Effect.TypeType.STONE, Resource.STONE}, {Effect.TypeType.ORE, Resource.ORE},
            {Effect.TypeType.CLAY, Resource.CLAY}, {Effect.TypeType.GLASS, Resource.GLASS}, {Effect.TypeType.PAPYRUS, Resource.PAPYRUS},
            {Effect.TypeType.LOOM, Resource.LOOM} // Coin is left out as there are other cases for it
        };

        static Dictionary<Effect.BasisType, CardColour> cardType = new Dictionary<Effect.BasisType, CardColour>
        {
            {Effect.BasisType.BROWN, CardColour.BROWN}, {Effect.BasisType.GRAY, CardColour.GRAY}, {Effect.BasisType.BLUE, CardColour.BLUE}, 
            {Effect.BasisType.GREEN, CardColour.GREEN}, {Effect.BasisType.YELLOW, CardColour.YELLOW}, {Effect.BasisType.RED, CardColour.RED}, 
            {Effect.BasisType.PURPLE, CardColour.PURPLE}
        };

        static Dictionary<Effect.TypeType, Score> scienceType = new Dictionary<Effect.TypeType, Score> 
        {   { Effect.TypeType.TABLET, Score.TABLET }, { Effect.TypeType.COMPASS, Score.COMPASS }, { Effect.TypeType.GEAR, Score.GEAR } };

        public static void ApplyEndGameEffect(GameState gameState)
        {
            int numPlayers = gameState.getPlayers().Count();

            foreach (KeyValuePair<long, Player> player in gameState.getPlayers())
            {
                Player curr = player.Value;
                int position = curr.getSeat();
                Player east = null;
                Player west = null;
                int schoiceCount = 0;

                // Setting up the east and west neighbors
                foreach (KeyValuePair<long, Player> neighbor in gameState.getPlayers())
                {
                    if ((position + 1) % numPlayers == neighbor.Value.getSeat())
                        east = neighbor.Value;
                    if ((position - 1) == neighbor.Value.getSeat() || (position == 0 && neighbor.Value.getSeat() == numPlayers - 1))
                        west = neighbor.Value;
                }

                // Check Wonder board effects first
                // This way if a player has guildCopy, it will take into account that the scientist guild
                // card is added into the players played hand and counted
                for (int i = 0; i < curr.getBoard().getSide().stagesBuilt; i++)
                {
                    foreach (Effect e in curr.getBoard().getSide().getStageEffects(i))
                    {
                        if (e.type.Equals(Effect.TypeType.SCHOICE))
                            schoiceCount += 1;
                        // Loop through Wonder Effects for the players
                        // GUILD COPY - not finished
                        else if (e.type.Equals(Effect.TypeType.GUILD))
                        {
                            CopyGuild(curr, east, west);
                        }
                    } // Foreach loop through Wondestage effects
                } // For loop through the stages

                // Looping through the player's played cards
                foreach (string cardID in curr.getPlayed())
                {
                    Card c = CardLibrary.getCard(cardID);
                    // Looping through the effects of each Card for End Game purposes
                    foreach (Effect e in c.effects)
                    {
                        // Victory Points
                        if (e.type.Equals(Effect.TypeType.VICTORY))
                        {
                            // FROM: NEIGHBORS
                            // and BASIS: CardColour, Wonderstages, Defeat
                            if (e.from != Effect.FromType.NONE && e.from.Equals(Effect.FromType.NEIGHBOURS))
                            {
                                // Apply victory points awarded for each
                                // Wonderstage neigboring cities own
                                if (e.basis.Equals(Effect.BasisType.WONDER))
                                    AddVictoryAllWonders(curr, east, west);

                                //Victory points given per neighbor's conlfict token
                                else if (e.basis.Equals(Effect.BasisType.DEFEAT))
                                    AddVictoryNeighboursConflict(curr, east, west);

                                 // Victory points awarded per certain structure built by neighbours
                                else
                                    AddVictoryNeighboursColour(curr, east, west, cardType[e.basis], e.amount);
                            }
                            // FROM: PLAYER
                            // BASIS: CardColour, Wonderstages, 
                            else if (e.from != Effect.FromType.NONE && e.from.Equals(Effect.FromType.PLAYER))
                            {
                                if (e.basis.Equals(Effect.BasisType.WONDER))
                                    AddVictoryWonder(curr);
                                else
                                    AddVictoryColour(curr, cardType[e.basis], e.amount);
                            }
                            // FROM: ALL
                            // BASIS: Wonderstages
                            else if (e.from != Effect.FromType.NONE && e.from.Equals(Effect.FromType.ALL))
                                AddVictoryAllWonders(curr, east, west);
                        } // End Victory Points

                        // SCIENCE CHOICE
                        else if (e.type.Equals(Effect.TypeType.SCHOICE))
                            schoiceCount += 1;
                    } // End Effect Loop for Cards                    
                } // End Current Player's Card Loop

                for (int i = 0; i < curr.getBoard().getSide().stagesBuilt; i++)
                {
                    foreach (Effect e in curr.getBoard().getSide().getStageEffects(i))
                    {
                        if (e.type.Equals("victory")) curr.addScore(Score.STAGES, e.amount);
                    }
                } 


                // Max Function, will add onto the max science value
                AddScienceChoice(curr, schoiceCount);
                curr.addScore(Score.VICTORY, CalculateScience(curr.getScoreNum(Score.GEAR),curr.getScoreNum(Score.COMPASS),curr.getScoreNum(Score.TABLET)));
                curr.addScore(Score.SCIENCE, CalculateScience(curr.getScoreNum(Score.GEAR), curr.getScoreNum(Score.COMPASS), curr.getScoreNum(Score.TABLET)));
                curr.addScore(Score.VICTORY, curr.getScoreNum(Score.CONFLICT));
                curr.addScore(Score.VICTORY, (int)curr.getResourceNum(Resource.COIN) / 3);
                curr.addScore(Score.COIN, (int)curr.getResourceNum(Resource.COIN) / 3);
            } // End Player Loop
        }

        // Effects to be applied during the last turn of every age
        // Dependant on Wonders
        public static void ApplyEndAge(GameState gameState)
        {
            foreach (KeyValuePair<long, Player> player in gameState.getPlayers())
            {
                Player curr = player.Value;
                foreach (Effect effect in curr.getBoard().getSide().getStageEffects(gameState.getAge()))
                {
                    if (effect.type.Equals(Effect.TypeType.LASTCARD))
                    {

                    }
                } // End Loop for Wonder Effects
            } // End Player Loop
        }

        // Effects of Cards/Wonders to be applied instantly. Other effects will be ignored
        // and left to another global function to deal with it at the end of the game.
        public static void ApplyEffect(Player p, Player east, Player west, Effect e)
        {
            // RESOURCES
            if (resourceType.ContainsKey(e.type))
                AddResource(p, resourceType[e.type], e.amount);

            // VICTORY POINTS
            else if (e.type.Equals(Effect.TypeType.VICTORY))
            {
                // NO FROM/BASIS - add to score
                if (e.from == Effect.FromType.NONE && e.basis == Effect.BasisType.NONE)
                    AddScore(p, Score.VICTORY, e.amount);

                // Adding to the Score for Blue Structures raised
                else if (e.from == Effect.FromType.NONE && e.basis.Equals(Effect.BasisType.BLUE))
                {
                    AddScore(p, Score.VICTORY_BLUE, e.amount);
                    AddScore(p, Score.VICTORY, e.amount);
                }
            }

            // COINS
            else if (e.type.Equals(Effect.TypeType.COIN))
            {
                // No FROM/BASIS - add coin
                if (e.from == Effect.FromType.NONE && e.basis == Effect.BasisType.NONE)
                    AddResource(p, Resource.COIN, e.amount);

                // FROM: PLAYER
                // BASIS: CardColour, Wonderstages
                else if (e.from.Equals(Effect.FromType.PLAYER))
                {
                    if (e.basis.Equals(Effect.BasisType.WONDER))
                        AddCoinWonder(p, e.amount);
                    else
                        AddCoinColour(p, cardType[e.basis], e.amount);
                }
                // FROM: ALL
                else if (e.from.Equals(Effect.FromType.ALL))
                    AddCoinAllColour(p, east, west, cardType[e.basis], e.amount);
            }

            // RESOURCE CHOICE
            else if (e.type.Equals(Effect.TypeType.RCHOICE))
            {
                List<Resource> choice = new List<Resource>();

                foreach (Effect.TypeType c in e.list)
                    choice.Add(resourceType[c]);

                if ((e.basis != Effect.BasisType.NONE) && (e.basis.Equals(Effect.BasisType.YELLOW) || e.basis.Equals(Effect.BasisType.WONDER)))
                    AddResourceUnPurchaseable(p, choice);
                else
                    AddResourceChoice(p, choice);
            }

            // ARMY
            else if (e.type.Equals(Effect.TypeType.ARMY))
                AddScore(p, Score.ARMY, e.amount);

            // Manufactured Resource Trade
            else if (e.type.Equals(Effect.TypeType.MCOST))
                SetManufactedTrade(p);

            // Raw Resource Trade
            else if (e.type.Equals(Effect.TypeType.RCOSTEAST))
                SetRawTradeEast(p);
            else if (e.type.Equals(Effect.TypeType.RCOSTWEST))
                SetRawTradeWest(p);

            // SCIENCE
            else if (e.type.Equals(Effect.TypeType.COMPASS) || e.type.Equals(Effect.TypeType.TABLET) || e.type.Equals(Effect.TypeType.GEAR))
                AddScore(p, scienceType[e.type], e.amount);


        // ===== WONDER SPECIFIC =====
            // FREE BUILD - not finished *wild card --> can be done at anytime during an age
            else if (e.type.Equals(Effect.TypeType.FREEBUILD))
            {

            }

            // DISCARD - End of turn the stage was built and build a card from discard pile for free
            else if (e.type.Equals(Effect.TypeType.DISCARD))
            {

            }

            // Players should know their neighbours east and west
            e.PrintEffect();
        }

        public static void SellCard(Player p) { AddResource(p, Resource.COIN, 3); }

        // Add a certain number of x Resource r to Player p
        // "w", "o", "l", "p" etc
        private static void AddResource(Player p, Resource r, int x)
        {
            int resourceNum = p.getResourceNum(r) + x;
            p.setResourceNum(r, resourceNum);
        }

        private static void AddResourceChoice(Player p, List<Resource> r) { p.addPublicChoices(r); }
        private static void AddResourceUnPurchaseable(Player p, List<Resource> r) { p.addPrivateChoices(r); }

        // Science Choice - player chooses which science to gain from the card at the end of the game
        // NOTE: Should we have this as a max function? eg. Find max of gear, tablet, compass and just add 1?
        private static void AddScienceChoice(Player p, int x)
        {
            int gear = p.getScoreNum(Score.GEAR);
            int compass = p.getScoreNum(Score.COMPASS);
            int tablet = p.getScoreNum(Score.TABLET);            

            if (x == 1)
            {
                int max1 = CalculateScience((gear + 1), compass, tablet);
                int max2 = CalculateScience(gear, (compass + 1), tablet);
                int max3 = CalculateScience(gear, compass, (tablet + 1));

                int maxScore = Math.Max(Math.Max(max1, max2), max3);
                if (maxScore == max1)
                    p.addScore(Score.GEAR, 1);
                else if (maxScore == max2)
                    p.addScore(Score.COMPASS, 1);
                else if (maxScore == max3)
                    p.addScore(Score.TABLET, 1);
            }
            else if (x == 2)
            {
                int max1 = CalculateScience(gear + 2, compass, tablet);
                int max2 = CalculateScience(gear, compass + 2, tablet);
                int max3 = CalculateScience(gear, compass, tablet + 2);
                int max4 = CalculateScience(gear + 1, compass + 1, tablet);
                int max5 = CalculateScience(gear + 1, compass, tablet + 1);
                int max6 = CalculateScience(gear, compass + 1, tablet + 1);

                int maxScore = Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(max1, max2), max3), max4), max5), max6);

                if (maxScore == max1)                
                    p.addScore(Score.GEAR, 2);
                else if (maxScore == max2)
                    p.addScore(Score.COMPASS, 2);
                else if (maxScore == max3)
                    p.addScore(Score.TABLET, 2);
                else if (maxScore == max4)
                {
                    p.addScore(Score.GEAR, 1);
                    p.addScore(Score.COMPASS, 1);
                }
                else if (maxScore == max5)
                {
                    p.addScore(Score.GEAR, 1);
                    p.addScore(Score.TABLET, 1);
                }
                else if (maxScore == max6)
                {
                    p.addScore(Score.COMPASS, 1);
                    p.addScore(Score.TABLET, 1);
                }
            }
        }

        public static int CalculateScience(int g, int c, int t)
        {
            int gear = g;
            int comp = c;
            int tab = t;
            int total = (gear * gear) + (comp * comp) + (tab * tab);

            while (gear >= 1 && comp >= 1 && tab >= 1)
            {
                total += 7;
                gear -= 1;
                comp -= 1;
                tab -= 1;
            }
            return total;
        }

        // Victory Points, Army, Science or any other generic score
        // This could probably be used to replace alot of generic score functions
        private static void AddScore(Player p, Score s, int points) { p.addScore(s, points); }

        // Coin awarded with no "basis" expect the construction of the structure
        private static void AddCoin(Player p, int coin) { p.addResource(Resource.COIN, coin); }

        // Coin awarded on the number of wonderstages a player has buit
        private static void AddCoinWonder(Player p, int amount)
        {
            int coin = p.getBoard().getSide().stagesBuilt * amount;
            p.addResource(Resource.COIN, coin);
        }

        // Coin awarded with the basis of Card Colour the Player owns
        private static void AddCoinColour(Player p, CardColour c, int amount)
        {
            int coin = p.getCardColourCount(c) * amount;
            p.addResource(Resource.COIN, coin);
        }

        // Coin awarded from the number of specific structure colour each neighbours have constructed
        private static void AddCoinAllColour(Player p, Player east, Player west, CardColour c, int amount)
        {
            int coin = p.getCardColourCount(c) + east.getCardColourCount(c) + west.getCardColourCount(c);
            coin *= amount;
            p.addResource(Resource.COIN, coin);
        }

        // Victory Points awarded from the number of specific structure colour each neighbours constructed
        private static void AddVictoryNeighboursColour(Player p, Player east, Player west, CardColour c, int amount)
        {
            p.addScore(Score.VICTORY, GetVictoryNeighboursColour(east, west, c, amount));
        }

        // Victory Points returned from the number of specific structure colour each neighbours constructed
        private static int GetVictoryNeighboursColour(Player east, Player west, CardColour c, int amount)
        {
            return (east.getCardColourCount(c) + west.getCardColourCount(c)) * amount;
        }

        // Victory Points awarded 
        // Through the number of specific structure colour the player has constructed
        private static void AddVictoryColour(Player p, CardColour c, int amount)
        {
            p.addScore(Score.VICTORY, GetVictoryColour(p, c, amount));
        }

        // Victory Points returned from the number of specific structure colour the player has constructed
        private static int GetVictoryColour(Player p, CardColour c, int amount)
        {
            return p.getCardColourCount(c) * amount;
        }

        // Victory Points awarded from the number of conflict points each neighbour has
        private static void AddVictoryNeighboursConflict(Player p, Player east, Player west)
        {            
            p.addScore(Score.VICTORY, GetVictoryNeighboursConflict(east, west));
        }

        // Victory Points returned for the number of defeat tokens each neighbouring city has
        private static int GetVictoryNeighboursConflict(Player east, Player west)
        {

            return east.getScoreType(Score.DEFEAT) + west.getScoreType(Score.DEFEAT);
        }

        // Victory Points awarded from the number of wonderstages the player has built
        private static void AddVictoryWonder(Player p)
        {
            int points = p.getBoard().getSide().stagesBuilt;
            p.addScore(Score.VICTORY, points);
        }

        // Victory Points awarded from the number of wonderstages built from each neighbour including the player
        private static void AddVictoryAllWonders(Player p, Player east, Player west)
        {
            p.addScore(Score.VICTORY, GetVictoryAllWonders(p, east, west));
        }

        // Victory Points returned from the number of wonderstages built from each neighbour including the player
        private static int GetVictoryAllWonders(Player p, Player east, Player west)
        {
            return p.getBoard().getSide().stagesBuilt + east.getBoard().getSide().stagesBuilt + west.getBoard().getSide().stagesBuilt;
        }

        // Trading Cost for East and West Raw Resources
        private static void SetRawTradeEast(Player p) { p.rcostEast = 1; }
        private static void SetRawTradeWest(Player p) { p.rcostWest = 1; }

        // Trading Cost for East && West of manufactured Resources
        private static void SetManufactedTrade(Player p) { p.mcost = 1; }

        // Babylon B [2nd stage]
        // Handle this at the end of every age to play the last card if
        // Player can pay for it, or discard to earn 3 coins or build the third wonder.
        // EXTRA TURN basically
        private static void LastCard(Player p, Card c)
        {
            // stuff
        }

        // Olympia A
        // the player can, once per Age, build a structure of their choice for free
        private static void FreeBuild(Player p, Card c, int age)
        {
            // stuff
            p.addPlayed(c);
        }

        public static bool CheckFreeBuild(int age)
        {
            // stuff
            return freeBuild[age];
        }

        // Olympia B [3rd stage]
        // The third stage allows the player to "copy" a Guild (purple card) of
        // their choice built by one of their two neighboring cities at the end of the game
        private static void CopyGuild(Player p, Player east, Player west)
        {
            List<string> guilds = new List<string>();
            Dictionary<string, int> guildScore = new Dictionary<string,int>();
            string bestCard = null;
            int bestScore = 0;

            // Guild Cards taken from East Player
            foreach (string c in east.getPlayed())
                if (CardLibrary.getCard(c).colour.Equals(CardColour.PURPLE))
                    guilds.Add(c);

            // Guild Cards taken from West Player
            foreach (string c in west.getPlayed())
                if (CardLibrary.getCard(c).colour.Equals(CardColour.PURPLE))
                    guilds.Add(c);                

            // Looping through the Guild Cards available to calculate the best guild
            foreach (string cardID in guilds)
            {
                guildScore.Add(cardID, 0);
                foreach (Effect e in CardLibrary.getCard(cardID).effects)
                {
                    if (e.type.Equals(Effect.TypeType.VICTORY) && e.from.Equals(Effect.FromType.NEIGHBOURS))
                    {
                        if (cardType.ContainsKey(e.basis))
                            guildScore[cardID] = GetVictoryNeighboursColour(east, west, cardType[e.basis], e.amount);
                        else if (e.basis.Equals(Effect.BasisType.DEFEAT))
                        {
                            //Need to finish the function called here
                            guildScore[cardID] = GetVictoryNeighboursConflict(east, west);
                        }
                    }
                    else if (e.type.Equals(Effect.TypeType.VICTORY) && e.from.Equals(Effect.FromType.PLAYER) && cardType.ContainsKey(e.basis))
                        guildScore[cardID] += GetVictoryColour(p, cardType[e.basis], e.amount); // Added multiple times
                    else if (e.type.Equals(Effect.TypeType.VICTORY) && e.from.Equals(Effect.FromType.ALL) && e.basis.Equals(Effect.BasisType.WONDER))
                        guildScore[cardID] = GetVictoryAllWonders(p, east, west);
                    else if (e.type.Equals(Effect.TypeType.SCHOICE))
                    {
                        //CalculateScience(g c t)
                        int gear = p.getScoreNum(Score.GEAR);
                        int comp = p.getScoreNum(Score.COMPASS);
                        int tab = p.getScoreNum(Score.TABLET);
                        
                        int maxScience = Math.Max(Math.Max(
                            CalculateScience(gear + 1, comp, tab), 
                            CalculateScience(gear, comp + 1, tab)), 
                            CalculateScience(gear, comp, tab + 1));

                        guildScore[cardID] = maxScience;
                    }
                }
            } // End Foreach string cardID in guilds

            // Finding the best guild Card
            foreach (KeyValuePair<string, int> key in guildScore)
            {
                if (key.Value > bestScore)
                {
                    bestScore = key.Value;
                    bestCard = key.Key;
                }
            }

            // Best Guild Card
            p.addPlayed(CardLibrary.getCard(bestCard));
        }

        // Halikarnassos A [2nd/3rd stage]
        // look at one of the discarded cards since the beginning of the 
        // game and build one for free
        public static Card Discard(Player p, List<Card> discardPile)
        {
            // stuff
            return null;
        }


        // Handle seperate instances of Score type
        public static int getPoints(Player p, Score s)
        {

            return 0;
        }
    }
}
