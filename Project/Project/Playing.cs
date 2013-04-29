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
using System.IO;

namespace Project
{
    static class Playing
    {
        static Texture2D maison, speechBoxTexture, bookTexture, inventaireTexture, potionTexture, swordTexture,armorTexture;
        static int j = 0;
        static string line;
        static int[,] tab_map8 = new int[26, 44];
        static int[,] tab_map5 = new int[26, 44];
        static Map map = new Map();
        static Map map5 = new Map(), map8 = new Map();
        static StreamReader streamMap8 = new StreamReader("map8.txt");
        static StreamReader streamMap5 = new StreamReader("map5.txt");
        static bool Isfighting = false, inventaire = false, talking = false, lvlUp = false;
        static int turn = -1, timerInventaire = 0,lvlBefore =1;
        static Song song3;
        static Rectangle speechBoxRectangle, bookRectangle, inventaireRectangle;
        static string attackChoisi = "";
        static KeyboardState presentKey, pastKey;

        public static void LoadContent(ContentManager Content, int screenWidth, int screenHeight)
        {

            Tiles.Content = Content;

            Game1.btnNext = new cButton(Content.Load<Texture2D>("Button/Next"), 75, 44);
            Game1.btnNext.setPosition(new Vector2(1200, 650));

            Game1.btnEndFight = new cButton(Content.Load<Texture2D>("Button/EndFight"), 75, 44);
            Game1.btnEndFight.setPosition(new Vector2(1200, 710));

            song3 = Content.Load<Song>("Song/SongFight");
            maison = Content.Load<Texture2D>("Tile/Maison");
            //inventaire
            inventaireTexture = Content.Load<Texture2D>("Menu/inventaire");
            inventaireRectangle = new Rectangle(0, 0, inventaireTexture.Width, inventaireTexture.Height);

            //speech 
            speechBoxTexture = Content.Load<Texture2D>("SpeechBox");
            speechBoxRectangle = new Rectangle(0, 650, speechBoxTexture.Width, speechBoxTexture.Height);

            //Book
            bookTexture = Content.Load<Texture2D>("Book");
            bookRectangle = new Rectangle(200, 330, bookTexture.Width, bookTexture.Height);

            //Potion
            potionTexture = Content.Load<Texture2D>("Potion");
            
            //Sword
            swordTexture = Content.Load<Texture2D>("Sword");
           
            //Armor
            armorTexture = Content.Load<Texture2D>("Armor");

            
           
            //Moteur à particules
            Game1.snow = new ParticleGenerator(Content.Load<Texture2D>("snow"), screenWidth, 50); // verifier le 2 nd arg

            while ((line = streamMap8.ReadLine()) != null)
            {
                char[] splitchar = { ',' };
                line = line.TrimEnd(splitchar); // enleve tout les caracteres "splichar" de la fin
                string[] tiles = line.Split(splitchar);

                for (int i = 0; i < tab_map8.GetUpperBound(1); i++) //Upperbound donne le nbres d'elts d'un tab suivant cette dimension 1 par exemple represente lenbre de colonne par ligne
                {

                    tab_map8[j, i] = int.Parse(tiles[i]);
                }
                j++;
            }

            map8.Generate(tab_map8, 32);
            streamMap8.Close();

            j = 0;

            while ((line = streamMap5.ReadLine()) != null)
            {
                char[] splitchar = { ',' };
                line = line.TrimEnd(splitchar); // enleve tout les caracteres "splichar" de la fin
                string[] tiles = line.Split(splitchar);

                for (int i = 0; i < tab_map5.GetUpperBound(1); i++) //Upperbound donne le nbres d'elts d'un tab suivant cette dimension 1 par exemple represente lenbre de colonne par ligne
                {

                    tab_map5[j, i] = int.Parse(tiles[i]);
                }
                j++;
            }

            map5.Generate(tab_map5, 32);
            streamMap5.Close();
            map = map5;


        }

        public static Game1.GameState Update(GameTime gameTime, int screenWidth, int screenHeight, GraphicsDeviceManager graphics)
        {
            //Item
            Item book = new Item("QuestItem", "Book", "", 0, 1);

            Isfighting = false;
            MouseState mouse = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
            Game1.GameState CurrentGameState = Game1.GameState.Playing;
            if (map == map5)
            {
                //Pnj
                if (!Game1.healer.Collision(Game1.player.persoPosition))
                {
                    Game1.healer.Update(gameTime, 0, "map5");
                }

                if (Game1.healer.Collision(Game1.player.persoPosition))
                {
                    Game1.healer.Update(gameTime, 1, "map5");
                }
                //Enemy
                if (Game1.enemy1.health > 0)
                    Game1.enemy1.Update(gameTime, Game1.player.persoPosition);

                if (Game1.enemy2.health > 0)
                    Game1.enemy2.Update(gameTime, Game1.player.persoPosition);
                if (Game1.enemy1.Collision(Game1.player.persoPosition))
                {
                    Game1.previousPosX = Game1.player.persoPosition.X;
                    Game1.previousPosY = Game1.player.persoPosition.Y;
                    turn = -1;
                    Game1.btnEndFight.isClicked = false;
                    Isfighting = true;
                    MediaPlayer.Play(song3);
                    Game1.enemy = Game1.enemy1;
                    attackChoisi = "";
                }
                else if (Game1.enemy2.Collision(Game1.player.persoPosition))
                {
                    Game1.previousPosX = Game1.player.persoPosition.X;
                    Game1.previousPosY = Game1.player.persoPosition.Y;
                    turn = -1;
                    Isfighting = true;
                    MediaPlayer.Play(song3);
                    Game1.btnEndFight.isClicked = false;
                    Game1.enemy = Game1.enemy2;
                    attackChoisi = "";
                }
                foreach (CollisionTiles tile in map5.CollisionTiles)
                {
                    if (tile.num >= 7)
                    {
                        Game1.player.Collision(tile.Rectangle);
                        /*Game1.enemy2.Collision(tile.Rectangle);
                        Game1.enemy1.Collision(tile.Rectangle);*/
                    }
                }

            }

            if (map == map8)
            {
                if (Game1.bookState == 0)
                {
                    Game1.pnj1.Update(gameTime, 0, "map8");
                }
                //Moteur à particules
                Game1.snow.update(gameTime, graphics.GraphicsDevice);
                foreach (CollisionTiles tile in map8.CollisionTiles)
                {
                    if (tile.num >= 7)
                        Game1.player.Collision(tile.Rectangle);
                }
                if (Character.persoRectangle.Intersects(bookRectangle))
                {
                    Game1.bookState = 1;
                    Game1.invent.addItem(book);
                }
            }
            if (Game1.player.persoPosition.Y <= 0)
            {
                map = map8;
                Game1.player.persoPosition.Y = (screenHeight - Game1.player.persoTexture.Height / 4);
            }
            else if (Game1.player.persoPosition.Y >= screenHeight - Game1.player.persoTexture.Height / 4)
            {
                map = map5;
                Game1.player.persoPosition.Y = Game1.player.persoTexture.Height / 4 - 40;
            }
            if (Isfighting)
            {
                CurrentGameState = Game1.GameState.Fight;
            }

            if (!inventaire)
            {
                timerInventaire++;

                //Perso        
                Game1.player.Update(gameTime);

                //PNJ
                if (Game1.pnj1.Collision(Game1.player.persoPosition))
                {
                    talking = true;
                }
                if (talking)
                {
                    Game1.btnNext.Update(mouse, gameTime);

                    if (Game1.bookState == 1)
                    {
                        Game1.pnj1.Update(gameTime, 1, "map8");
                    }
                    if (Game1.btnNext.isClicked)
                    {
                        Game1.bookState = 2;
                        Game1.btnNext.Update(mouse, gameTime);
                    }
                    if (Game1.bookState == 2)
                    {
                        Game1.invent.removeItem(book);
                        Game1.pnj1.Update(gameTime, 2, "map8");
                    }

                    if (!Game1.pnj1.Collision(Game1.player.persoPosition))
                        talking = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    CurrentGameState = Game1.GameState.Pause;
                }

                if (timerInventaire > 15)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.I))
                    {
                        inventaire = true;
                        timerInventaire = 0;
                    }
                }
            }
            else if (inventaire)
            {
                timerInventaire++;
                if (timerInventaire > 15)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.I))
                    {
                        inventaire = false;
                        timerInventaire = 0;
                    }
                }
                foreach (Item item in Game1.invent.tablObjects)
                {
                    if (mouseRectangle.Intersects(new Rectangle((item.place % 8) * 20 + 30, 40 + 30 * (item.place / 8), bookTexture.Width, bookTexture.Height)) && (mouse.LeftButton == ButtonState.Pressed) && Game1.pastMouse.LeftButton == ButtonState.Released)
                    {
                        Game1.invent.useItem(item);
                        
                    }
                }
            }
            Game1.pastMouse = mouse;

            return (CurrentGameState);
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            //priteBatch.Draw(Content.Load<Texture2D>("Map/map" + Convert.ToSingle(player.mapnumber)), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            map.Draw(spriteBatch);
           if (map == map5)
            {
                Game1.healer.Draw(spriteBatch, 0, "map5");
                if (lvlUp)
                { 
                    spriteBatch.Draw(speechBoxTexture, speechBoxRectangle, Color.White);
                    spriteBatch.DrawString(Game1.spriteFont, "You level up !!!", new Vector2(10, 675), Color.Black);
                    Game1.spriteBatch.DrawString(Game1.spriteFont, "Level : " + Game1.player.Lvl, new Vector2(10, 695), Color.Black);
                    Game1.spriteBatch.DrawString(Game1.spriteFont, "Health : " + Game1.player.health + "/" + Game1.player.healthMax, new Vector2(10, 720), Color.Black);
                    Game1.spriteBatch.DrawString(Game1.spriteFont, "Experience " + Game1.player.Experience + "/" + (Game1.player.Lvl * 100) + "                                                                                                                                                  Press Enter to continue", new Vector2(10, 745), Color.Black);  
                    if (presentKey.IsKeyDown(Keys.Enter) && pastKey.IsKeyUp(Keys.Enter))
                    { 
                        lvlUp = false;
                        lvlBefore = Game1.player.Lvl;
                    }
                    pastKey = presentKey;
                }
               if (Keyboard.GetState().IsKeyDown(Keys.H))
                {
                    Game1.spriteBatch.DrawString(Game1.spriteFont, "Level : " + Game1.player.Lvl, new Vector2(300, 200), Color.Black);
                    Game1.spriteBatch.DrawString(Game1.spriteFont, "Health : " + Game1.player.health, new Vector2(300, 250), Color.Red);
                    Game1.spriteBatch.DrawString(Game1.spriteFont, "Experience " + Game1.player.Experience + "/" + (Game1.player.Lvl * 100), new Vector2(300, 300), Color.Blue);  
                }
                spriteBatch.Draw(maison, new Rectangle(32 * 11, 0, 96, 128), Color.White);
                if (Game1.enemy1.health > 0)
                    Game1.enemy1.Draw(spriteBatch);
                if (Game1.enemy2.health > 0)
                    Game1.enemy2.Draw(spriteBatch);
                
            }

            //GraphicsDevice.Clear(Color.CornflowerBlue);
            Game1.player.Draw(spriteBatch);

            if (map == map8)
            {
                Game1.snow.Draw(spriteBatch);
                if (Game1.bookState != 2)
                {
                    Game1.pnj1.Draw(spriteBatch, 0,"map8");
                }
                else
                {
                    Game1.pnj1.Draw(spriteBatch, 2, "map8");
                }
                if (talking && Game1.bookState == 0)
                {
                    spriteBatch.Draw(speechBoxTexture, speechBoxRectangle, Color.White);
                    spriteBatch.DrawString(Game1.spriteFont, "Arha: I'm Arha can you help me? I'm freezing and I've lost my spell book can you find it for me? I can't leave this place without it", new Vector2(10, 675), Color.Blue);
                }
                if (Game1.bookState == 0)
                {

                    spriteBatch.Draw(bookTexture, bookRectangle, Color.White);
                }

                if (talking && Game1.bookState == 1)
                {
                    spriteBatch.Draw(speechBoxTexture, speechBoxRectangle, Color.White);
                    Game1.btnNext.Draw(spriteBatch);
                    spriteBatch.DrawString(Game1.spriteFont, "Arha: Thanks. Can you do something else for me? I've seen some ennemies in the south can you kill them I want to go home?", new Vector2(10, 675), Color.Blue);
                }
                if (talking && Game1.bookState == 2)
                {
                    spriteBatch.Draw(speechBoxTexture, speechBoxRectangle, Color.White);
                    spriteBatch.DrawString(Game1.spriteFont, "Arha: Can you kill these ennemies for me? I hope they are not too strong for you", new Vector2(10, 675), Color.Blue);
                }
            }

            if (inventaire)
            {
                spriteBatch.Draw(inventaireTexture, inventaireRectangle, Color.White);
                /*if (Game1.bookState == 1)
                {
                    spriteBatch.Draw(bookTexture, bookRectangle2, Color.White);
                }*/


                foreach (Item item in Game1.invent.tablObjects)
                {
                    switch (item.name)
                    {
                        case "healthPotion":
                            spriteBatch.Draw(potionTexture, new Rectangle((item.place% 8) * 20 + 30, 40 + 30 * (item.place/8), bookTexture.Width, bookTexture.Height), Color.White);
                            break;
                        case "Sword":
                            spriteBatch.Draw(swordTexture, new Rectangle((item.place % 8) * 20 + 30, 40 + 30 * (item.place/8), bookTexture.Width, bookTexture.Height), Color.White);
                            break;
                        case "Armor":
                            spriteBatch.Draw(armorTexture, new Rectangle((item.place % 8) * 20 + 30, 40 + 30 * (item.place / 8), bookTexture.Width, bookTexture.Height), Color.White);
                            break;
                        case "Book":
                            spriteBatch.Draw(bookTexture, new Rectangle((item.place % 8) * 20 + 30, 40 + 30 * (item.place / 8), bookTexture.Width, bookTexture.Height), Color.White);
                            break;

                    }
                }
               // if(Inventaire.

            }

        }

    }
}
