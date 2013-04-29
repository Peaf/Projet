using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project
{
    static class Fight
    {
        static cButton btnStartFight, btnAttack1, btnSpell, btnObjects, btnEndFight;
        static Texture2D speechBoxTexture, healthBoxTexture, manaTexture, enemyHealthTexture, fightBackTexture, enemyFightTexture, healthTexture, persoFight;
        static Rectangle speechBoxRectangle;
        static int turn = -1, degat, manaPerdu, timerAnimation = 0, colonne = 0, ligne = 0, nbreAnimation = 0;
        static Rectangle healthBoxRectangle, healthRectangle, manaRectangle, enemyHealthRectangle, fightBackRectangle, enemyFightRectangle, persoFightRectangle;
        static MouseState pastMouse;
        static string attackChoisi = "";
        static Random rand = new Random();
        static KeyboardState presentKey, pastKey;
        static Song songGameOver, songVictory, song2;
        static bool Isfighting = false, xp = true;
        static Vector2 persoFightPosition;
        static Vector2 origin;


        public static void LoadContent(ContentManager Content, SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            speechBoxTexture = Content.Load<Texture2D>("SpeechBox");
            speechBoxRectangle = new Rectangle(0, 650, speechBoxTexture.Width, speechBoxTexture.Height);

            fightBackTexture = Content.Load<Texture2D>("Menu/FightBack");
            fightBackRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            enemyFightTexture = Content.Load<Texture2D>("Sprites/enemyFight");
            enemyFightRectangle = new Rectangle(1000, screenHeight / 2 + 120, 103, 60);

            btnStartFight = new cButton(Content.Load<Texture2D>("Button/StartFight"), 150, 70);
            btnStartFight.setPosition(new Vector2(screenWidth / 2 - 100, screenHeight / 2));

            btnAttack1 = new cButton(Content.Load<Texture2D>("Button/AttackButton"), 120, 45);
            btnAttack1.setPosition(new Vector2(50, screenHeight / 2 + 200));

            btnSpell = new cButton(Content.Load<Texture2D>("Button/SpellButton"), 120, 45);
            btnSpell.setPosition(new Vector2(170, screenHeight / 2 + 200));

            btnObjects = new cButton(Content.Load<Texture2D>("Button/ObjectsButton"), 120, 45);
            btnObjects.setPosition(new Vector2(100, screenHeight / 2 + 250));

            btnEndFight = new cButton(Content.Load<Texture2D>("Button/EndFight"), 75, 44);
            btnEndFight.setPosition(new Vector2(1200, 650));

            songVictory = Content.Load<Song>("Song/Victory");
            songGameOver = Content.Load<Song>("Song/songGameOver");
            song2 = Content.Load<Song>("Song/Song2");

            healthTexture = Content.Load<Texture2D>("Barres/Health");
            manaTexture = Content.Load<Texture2D>("Barres/Mana");
            healthBoxTexture = Content.Load<Texture2D>("Barres/HealthBox");
            enemyHealthTexture = Content.Load<Texture2D>("Barres/HealthEnemy");


            //animation 
            persoFight = Content.Load<Texture2D>("AnimationFight");
            persoFightRectangle = new Rectangle(200, screenHeight / 2 + 100, 320, 847);
            persoFightPosition = new Vector2(200, screenHeight / 2 + 300);
            origin = new Vector2(100, (screenHeight / 2 + 100) / 2);


        }

        public static Game1.GameState Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            timerAnimation++;
            ligne = 0;

            if (timerAnimation == 15)
            {
                timerAnimation = 0;
                if (colonne == 3)
                {
                    colonne = 0;
                }
                else
                {
                    colonne++;
                }
            }
            persoFightRectangle = new Rectangle(colonne * 80, ligne * 77, 80, 77);
            presentKey = Keyboard.GetState();
            Game1.GameState CurrentGameState = Game1.GameState.Fight;
            MouseState mouse = Mouse.GetState();
            Game1.player.persoPosition.X = 200;
            Game1.player.persoPosition.Y = screenHeight / 2 + 100;
            Game1.player.fight = true;
            Game1.player.colonne = 2;
            Game1.player.Direction = "right";
            Game1.player.Update(gameTime);
            healthBoxRectangle = new Rectangle(10, 10, healthBoxTexture.Width, healthBoxTexture.Height);
            healthRectangle = new Rectangle(16, 14, (Game1.player.health * 379) / Game1.player.healthMax, 35);
            manaRectangle = new Rectangle(115, 62, (Game1.player.mana * 280) / Game1.player.manaMax, manaTexture.Height);
            enemyHealthRectangle = new Rectangle((1030 - Game1.enemy.health / 2), (screenHeight / 2 - enemyHealthTexture.Height / 2 + 100), Game1.enemy.health, enemyHealthTexture.Height);

            if (turn == -1 && btnStartFight.isClicked)
            {
                turn = 0;
            }
            if (turn % 2 == 0 && Game1.player.health > 0 && Game1.enemy.health > 0)
            {
                if (btnAttack1.isClicked && pastMouse.LeftButton == ButtonState.Released)
                {
                    btnAttack1.isClicked = false;
                    attackChoisi = "Basic attack";
                    btnAttack1.Update(mouse, gameTime);
                    timerAnimation = 0;
                    degat = Game1.player.Degat + rand.Next(0, 30) + Game1.player.Strenght / 2;
                    manaPerdu = 0;
                    nbreAnimation = 0;
                    colonne = 0;
                }

                if (btnSpell.isClicked && pastMouse.LeftButton == ButtonState.Released)
                {
                    btnSpell.isClicked = false;
                    attackChoisi = "Fire Ball";
                    btnSpell.Update(mouse, gameTime);
                    degat = rand.Next(150, 170) + Game1.player.Intelligence + Game1.player.Degat;
                    manaPerdu = 20;
                    nbreAnimation = 0;
                    colonne = 0;

                }
                if (attackChoisi == "Basic attack" && nbreAnimation < 2)
                {
                    ligne = 7;
                    if (timerAnimation % 30 == 0)
                    {
                        if (colonne == 3)
                        {
                            colonne = 0;
                            nbreAnimation++;
                        }
                        else
                        {
                            colonne++;
                            nbreAnimation++;
                        }
                    }
                    persoFightRectangle = new Rectangle(colonne * 80, ligne * 77, 80, 77);
                }
                if (attackChoisi == "Fire Ball" && nbreAnimation < 2)
                {
                    ligne = 8;
                    if (timerAnimation % 30 == 0)
                    {
                        if (colonne == 3)
                        {
                            colonne = 0;
                            nbreAnimation++;
                        }
                        else
                        {
                            colonne++;
                            nbreAnimation++;
                        }
                    }
                    persoFightRectangle = new Rectangle(colonne * 80, ligne * 77, 80, 77);
                }
                if (presentKey.IsKeyDown(Keys.Enter) && pastKey.IsKeyUp(Keys.Enter) && (attackChoisi == "Basic attack" || attackChoisi == "Fire Ball"))
                {
                    if (degat >= Game1.enemy.health)
                    {
                        MediaPlayer.Play(songVictory);
                    }
                    Game1.enemy.health -= degat;
                    Game1.player.mana -= manaPerdu;
                    turn++;
                    attackChoisi = "";
                }
            }

            else if (presentKey.IsKeyDown(Keys.Enter) && pastKey.IsKeyUp(Keys.Enter) && turn % 2 == 1 && Game1.player.health > 0 && Game1.enemy.health > 0)
            {
                Game1.player.health = Game1.player.health + Game1.player.Armor - rand.Next(100, 120);
                turn++;
            }
            if (Game1.player.health <= 0)
            {
                CurrentGameState = Game1.GameState.GameOver;
                MediaPlayer.Play(songGameOver);
            }
            if (Game1.enemy.health <= 0)
            {
                if (xp)
                {
                    Game1.player.Experience += 100;
                    xp = false;
                }

                Game1.enemy.health = 0;
                if (btnEndFight.isClicked)
                {
                    Game1.player.persoPosition.X = Game1.previousPosX;
                    Game1.player.persoPosition.Y = Game1.previousPosY;
                    Game1.player.fight = false;
                    btnStartFight.isClicked = false;
                    CurrentGameState = Game1.GameState.Playing;
                    Isfighting = false;
                    Game1.enemy.enemyPosition.X = -100;
                    Game1.enemy.enemyPosition.Y = -100;
                    Game1.enemy.enemyRectangle = new Rectangle(0, 0, 0, 0);
                    MediaPlayer.Play(song2);
                    xp = true;
                }
            }

            btnStartFight.Update(mouse, gameTime);
            btnAttack1.Update(mouse, gameTime);
            btnObjects.Update(mouse, gameTime);
            btnSpell.Update(mouse, gameTime);
            btnEndFight.Update(mouse, gameTime);
            pastKey = presentKey;
            pastMouse = mouse;
            return (CurrentGameState);
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            spriteBatch.Draw(fightBackTexture, fightBackRectangle, Color.White);
            //Game1.player.Draw(spriteBatch);
            spriteBatch.Draw(enemyFightTexture, enemyFightRectangle, Color.White);
            spriteBatch.Draw(healthBoxTexture, healthBoxRectangle, Color.White);
            spriteBatch.Draw(manaTexture, manaRectangle, Color.White);
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
            spriteBatch.DrawString(Game1.spriteFont, Game1.player.health + "/" + Game1.player.healthMax, new Vector2(healthTexture.Width / 2 - 16, 21), Color.White);
            spriteBatch.DrawString(Game1.spriteFont, Game1.player.mana + "/" + Game1.player.manaMax, new Vector2(manaTexture.Width / 2 + 88, 60), Color.White);
            spriteBatch.Draw(enemyHealthTexture, enemyHealthRectangle, Color.White);
            spriteBatch.DrawString(Game1.spriteFont, Game1.enemy.health + "/" + Game1.enemy.healthMax, new Vector2(1000, screenHeight / 2 + 89), Color.Black);
            spriteBatch.Draw(speechBoxTexture, speechBoxRectangle, Color.White);
            spriteBatch.Draw(persoFight, persoFightPosition, persoFightRectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
            if (turn == -1)
            {
                spriteBatch.DrawString(Game1.spriteFont, "You're attacked !!!", new Vector2(10, 675), Color.Black);
                btnStartFight.Draw(spriteBatch);
            }
            if ((turn % 2 == 1) && Game1.player.health > 0 && Game1.enemy.health > 0)
            {
                spriteBatch.DrawString(Game1.spriteFont, "The ennemy attack you                                                                                                                              (Press ENTER to continue)", new Vector2(10, 675), Color.Black);
            }
            if (turn % 2 == 0 && attackChoisi == "" && Game1.player.health > 0 && Game1.enemy.health > 0)
            {
                spriteBatch.DrawString(Game1.spriteFont, "It's your turn choose your fate", new Vector2(10, 675), Color.Black);
                btnAttack1.Draw(spriteBatch);
                btnSpell.Draw(spriteBatch);
                btnObjects.Draw(spriteBatch);
            }
            if (turn % 2 == 0 && (attackChoisi != ""))
            {
                spriteBatch.DrawString(Game1.spriteFont, "Do you want to use the attack: " + attackChoisi + "?                                                                                                                        (press ENTER to continue)", new Vector2(10, 675), Color.Black);

            }
            if (Game1.player.lvlup)
            {
                btnEndFight.Draw(spriteBatch);
                spriteBatch.DrawString(Game1.spriteFont, "You lvlup !!!                                                                                                 click the ARROW to continue", new Vector2(10, 675), Color.Black);

            }
            else if (Game1.enemy.health <= 0)
            {
                btnEndFight.Draw(spriteBatch);
                spriteBatch.DrawString(Game1.spriteFont, "You win !!!                                                                                                 click the ARROW to continue", new Vector2(10, 675), Color.Black);
            }

        }
    }
}
