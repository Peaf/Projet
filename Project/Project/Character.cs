using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project
{
    public class Character
    {
        public Texture2D persoTexture;
        public Vector2 persoPosition;
        Rectangle Rectsprite;
        public static Rectangle persoRectangle;
        int screenWidth = 1366, screenHeight = 768;

        int vitesse = 4;
        public int mapnumber = 5, health, ligne = 1, colonne = 1, mana, healthMax, manaMax, Experience, Strenght, Intelligence, Degat, Armor, Lvl;
        public string Direction;
        int timer = 0;
        public bool fight = false, lvlup;
        public Map map, map4, map5;

        public Character(Texture2D newTexture, Vector2 newPosition, Rectangle newRectangle, Rectangle newsprite, int newHealth, int newMana, int newExperience, int newStrenght, int newIntelligence, int newDegat, int newArmor)
        {
            persoTexture = newTexture;
            persoPosition = newPosition;
            persoRectangle = newRectangle;
            Rectsprite = newsprite;
            health = newHealth;
            mana = newMana;
            manaMax = newMana;
            healthMax = newHealth;
            Experience = newExperience;
            Strenght = newStrenght;
            Intelligence = newIntelligence;
            Degat = newDegat;
            Armor = newArmor;
            Lvl = 1;

        }

        public void Update(GameTime gametime)
        {
            KeyboardState KState = Keyboard.GetState();
            lvlup = (Experience  == 100);

            if (lvlup)
            {
                Experience = 0;
                healthMax += 500;
                manaMax += 100;
                health = healthMax;
                mana = manaMax;
                Strenght += 20;
                Intelligence += 50;
                Lvl += 1;
            }
            if (!fight)
            {
                if (KState.IsKeyDown(Keys.Q))
                {
                    //map.SourceRectangle = new Rectangle(map.SourceRectangle.Value.X - gameTime.ElapsedGameTime.Milliseconds / 5, map.SourceRectangle.Value.Y, 1300, 1000);

                    Direction = "left";
                    timer++;

                    if (timer == 15)
                    {
                        timer = 0;
                        if (colonne == 6)
                        {
                            colonne = 3;
                        }
                        else
                        {
                            colonne++;
                        }
                    }

                    if (persoPosition.X <= 0)
                    {
                        map = map4;
                        persoPosition.X = (screenWidth - persoTexture.Width / 6);
                    }
                    else
                    {
                        persoPosition += (new Vector2((-vitesse), 0));
                    }
                }
                else if (KState.IsKeyDown(Keys.Z))
                {
                    //map.SourceRectangle = new Rectangle(map.SourceRectangle.Value.X, map.SourceRectangle.Value.Y - gameTime.ElapsedGameTime.Milliseconds /5, 1300, 1000);

                    Direction = "up";
                    timer++;

                    if (timer == 15)
                    {
                        timer = 0;
                        if (colonne == 6)
                        {
                            colonne = 2;
                        }
                        else
                        {
                            colonne++;
                        }
                    }
                    if (persoPosition.Y <= 0)
                    {
                        mapnumber += 3;
                        persoPosition.Y = screenHeight - persoTexture.Height / 4;
                    }
                    else
                    {
                        persoPosition += new Vector2(0, -vitesse);
                    }
                }
                else if (KState.IsKeyDown(Keys.S))
                {
                    // map.SourceRectangle = new Rectangle(map.SourceRectangle.Value.X, map.SourceRectangle.Value.Y + gameTime.ElapsedGameTime.Milliseconds / 5, 1300, 1000);

                    Direction = "down";
                    timer++;


                    if (timer == 15)
                    {
                        timer = 0;
                        if (colonne == 6)
                        {
                            colonne = 2;
                        }
                        else
                        {
                            colonne++;
                        }
                    }
                    if (persoPosition.Y >= (screenHeight - persoTexture.Height / 4))
                    {
                        mapnumber -= 3;
                        persoPosition.Y = 0;
                    }
                    else
                    {
                        persoPosition += new Vector2(0, vitesse);
                    }

                }
                else if (KState.IsKeyDown(Keys.D))
                {
                    Direction = "right";
                    timer++;


                    if (timer == 15)
                    {
                        timer = 0;
                        if (colonne == 6)
                        {
                            colonne = 3;
                        }
                        else
                        {
                            colonne++;
                        }
                    }
                    if (persoPosition.X >= (screenWidth - persoTexture.Width / 6))
                    {
                        mapnumber += 1;
                        persoPosition.X = 0;
                    }
                    else
                    {
                        persoPosition += new Vector2(vitesse, 0);
                    }

                }
            }
            switch (Direction)
            {
                case "up":

                    ligne = 3;
                    Rectsprite = new Rectangle((colonne - 1) * 32, (ligne - 1) * 63, 30, 63);
                    break;

                case "down":

                    ligne = 1;
                    Rectsprite = new Rectangle((colonne - 1) * 32, (ligne - 1) * 63, 30, 62);
                    break;

                case "left":

                    ligne = 2;
                    Rectsprite = new Rectangle((colonne - 1) * 32, (ligne - 1) * 63, 30, 63);
                    break;
                case "right":

                    ligne = 4;
                    Rectsprite = new Rectangle((colonne - 1) * 32, (ligne - 1) * 63, 30, 63);
                    break;
            }


            persoRectangle = new Rectangle((int)persoPosition.X, (int)persoPosition.Y + persoTexture.Height / 8, persoTexture.Width / 12, persoTexture.Height / 8);
        }

        public void Draw(SpriteBatch spritBatch)
        {
            spritBatch.Draw(persoTexture, persoPosition, Rectsprite, Color.White);
        }

        public void Collision(Rectangle newRectangle)
        {
            if (persoRectangle.TouchTopOf(newRectangle))
            {
                persoPosition.Y = newRectangle.Y - persoRectangle.Height - persoTexture.Height / 8;
            }

            if (persoRectangle.TouchLeftOf(newRectangle))
            {
                persoPosition.X = newRectangle.X - persoRectangle.Width - 4;
            }
            if (persoRectangle.TouchRightOf(newRectangle))
            {
                persoPosition.X = newRectangle.X + newRectangle.Width + 4;
            }
            if (persoRectangle.TouchBottomOf(newRectangle))
                persoPosition.Y = newRectangle.Y + newRectangle.Height + 5 - persoTexture.Height / 8;

        }
    }
}
