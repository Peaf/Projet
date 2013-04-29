using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Media;

namespace Project
{
    public class Enemy
    {

        Texture2D enemyTexture;
        public Vector2 enemyPosition;
        Rectangle Rectenemy;
        public Rectangle enemyRectangle;
        int screenWidth = 1366, screenHeight = 768;

        int vitesse = 1;
        public int mapnumber = 5;
        public int ligne = 1;
        public int colonne = 1;
        string Direction;
        int timer = 0, timerEnemy = 0;
        public int health, healthMax;

        public Enemy(Texture2D newTexture, Vector2 newPosition, Rectangle newRectangle, Rectangle newsprite, int newHealth)
        {
            enemyTexture = newTexture;
            enemyPosition = newPosition;
            enemyRectangle = newRectangle;
            Rectenemy = newsprite;
            health = newHealth;
            healthMax = newHealth;
        }


        public void Update(GameTime gametime, Vector2 position)
        {
            KeyboardState KState = Keyboard.GetState();

            if (position.X - 45 - enemyPosition.X >= -150 && position.X - enemyPosition.X - 45 < 0 && position.Y - enemyPosition.Y > -100 && position.Y - enemyPosition.Y < 100)
            {
                Direction = "left";
                timer++;

                if (timer == 15)
                {
                    timer = 0;
                    if (colonne == 3)
                    {
                        colonne = 1;
                    }
                    else
                    {
                        colonne++;
                    }
                }
                else
                {
                    enemyPosition += (new Vector2((-vitesse), 0));
                    enemyRectangle.X -= vitesse;
                }
            }

            else if (position.X - 45 - enemyPosition.X <= 150 && position.X - enemyPosition.X - 45 > 0 && position.Y - enemyPosition.Y > -100 && position.Y - enemyPosition.Y < 100)
            {
                Direction = "right";
                timer++;

                if (timer == 15)
                {
                    timer = 0;
                    if (colonne == 3)
                    {
                        colonne = 1;
                    }
                    else
                    {
                        colonne++;
                    }
                }
                if (enemyPosition.X >= (screenWidth - enemyTexture.Width / 6))
                {
                    mapnumber += 1;
                    enemyPosition.X = 0;
                }
                else
                {
                    enemyPosition += new Vector2(vitesse, 0);
                    enemyRectangle.X += vitesse;
                }

            }

            else if (enemyPosition.Y - position.Y >0 && enemyPosition.Y - position.Y < 200 && ((position.X - enemyPosition.X >= 0 && position.X - enemyPosition.X <= 100) ||( position.X - enemyPosition.X >= -100 && position.X - enemyPosition.X <= 0)))
            {

                Direction = "up";
                timer++;

                if (timer == 15)
                {
                    timer = 0;
                    if (colonne == 3)
                    {
                        colonne = 1;
                    }
                    else
                    {
                        colonne++;
                    }
                }
                else
                {
                    enemyPosition += new Vector2(0, -vitesse);
                    enemyRectangle.Y -= vitesse;
                }

            }
           else if (enemyPosition.Y - position.Y <= 0 && enemyPosition.Y - position.Y >= -200  && ((position.X - enemyPosition.X >= 0 && position.X - enemyPosition.X < 100) ||( position.X - enemyPosition.X >= -100 && position.X - enemyPosition.X <= 0)))
         
            {
                Direction = "down";
                timer++;

                if (timer == 15)
                {
                    timer = 0;
                    if (colonne == 3)
                    {
                        colonne = 1;
                    }
                    else
                    {
                        colonne++;
                    }
                }
                
                else
                {
                    enemyPosition += new Vector2(0, vitesse);
                    enemyRectangle.Y += vitesse;
                }

            }
            else //patrouille
            {
                timerEnemy++;
                if (timerEnemy <= 150)
                {
                    Direction = "left";
                    timer++;

                    if (timer == 15)
                    {
                        timer = 0;
                        if (colonne == 3)
                        {
                            colonne = 1;
                        }
                        else
                        {
                            colonne++;
                        }
                    }

                    
                    else
                    {
                        enemyPosition += (new Vector2((-vitesse), 0));
                        enemyRectangle.X -= vitesse;
                    }
                }
                else if (timerEnemy > 150 && timerEnemy <= 200)
                {
                    Direction = "down";
                    timer++;

                    if (timer == 15)
                    {
                        timer = 0;
                        if (colonne == 3)
                        {
                            colonne = 1;
                        }
                        else
                        {
                            colonne++;
                        }
                    }
                    else
                    {
                        enemyPosition += new Vector2(0, vitesse);
                        enemyRectangle.Y += vitesse;
                    }

                }

                else if (timerEnemy > 200 && timerEnemy <= 350)
                {
                    Direction = "right";
                    timer++;

                    if (timer == 15)
                    {
                        timer = 0;
                        if (colonne == 3)
                        {
                            colonne = 1;
                        }
                        else
                        {
                            colonne++;
                        }
                    }
                    else
                    {
                        enemyPosition += new Vector2(vitesse, 0);
                        enemyRectangle.X += vitesse;
                    }
                }
                else if (timerEnemy > 350 && timerEnemy <= 400)
                {

                    Direction = "up";
                    timer++;

                    if (timer == 15)
                    {
                        timer = 0;
                        if (colonne == 3)
                        {
                            colonne = 1;
                        }
                        else
                        {
                            colonne++;
                        }
                    }
                   
                    else
                    {
                        enemyPosition += new Vector2(0, -vitesse);

                        enemyRectangle.Y -= vitesse;
                    }

                }
                else
                {
                    timerEnemy = 0;
                }

            }
            switch (Direction)
            {
                case "up":

                    ligne = 4;
                    Rectenemy = new Rectangle((colonne - 1) * 111, (ligne - 1) * 62, 111, 62);
                    break;

                case "down":

                    ligne = 1;
                    Rectenemy = new Rectangle((colonne - 1) * 111, (ligne - 1) * 62, 111, 62);
                    break;

                case "left":

                    ligne = 2;
                    Rectenemy = new Rectangle((colonne - 1) * 111, (ligne - 1) * 62, 111, 62);
                    break;

                case "right":

                    ligne = 3;
                    Rectenemy = new Rectangle((colonne - 1) * 111, (ligne - 1) * 62, 111, 62);
                    break;

            }
        }

        public bool Collision(Vector2 position)
        {
            return (enemyRectangle.Intersects(Character.persoRectangle));
        }

        /*public void Collision(Rectangle newRectangle)
        {
            if (enemyRectangle.TouchTopOf(newRectangle))
            {
                enemyPosition.Y = newRectangle.Y - enemyRectangle.Height ;
            }

            if (enemyRectangle.TouchLeftOf(newRectangle))
            {
                enemyPosition.X = newRectangle.X - 4;
            }
            if (enemyRectangle.TouchRightOf(newRectangle))
            {
                enemyPosition.X = newRectangle.X + 11;
            }
            if (enemyRectangle.TouchBottomOf(newRectangle))
                enemyPosition.Y = newRectangle.Y + newRectangle.Height + 5 ;

        }*/

        public void Draw(SpriteBatch spritBatch)
        {
            spritBatch.Draw(enemyTexture, enemyPosition, Rectenemy, Color.White);
        }
    }
}