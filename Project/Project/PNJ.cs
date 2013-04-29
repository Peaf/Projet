using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Media;
using Microsoft.Xna.Framework.Content;

namespace Project
{
    public class PNJ
    {
        static public Vector2 pnjPosition, origine;
        static public Texture2D pnjTexture;
        static public Rectangle pnjRectangle;
        static int ligne = 0, colonne = 0, timerAnimation = 0;

        public PNJ(Texture2D newTexture, Vector2 newPosition, Rectangle newRectangle, Vector2 newOrigine)
        {
            pnjTexture = newTexture;
            pnjPosition = newPosition;
            pnjRectangle = newRectangle;
        }

        public bool Collision(Vector2 position)
        {
            return (position.X <= pnjPosition.X && position.X >= pnjPosition.X - 40 && position.Y - pnjPosition.Y < 100 && position.Y - pnjPosition.Y > -30);
        }


        public void Update(GameTime gameTime, int state, string map)
        {
            if (map == "map8")
            {
                if (state == 0)
                {
                    ligne = 0;
                    colonne = 0;
                    pnjRectangle = new Rectangle(colonne * 48, ligne * 94, 48, 94);
                }
                if (state == 1)
                {
                    timerAnimation++;
                    ligne = 3;
                    if (timerAnimation == 15)
                    {
                        if (colonne == 0)
                        {
                            colonne = 1;
                        }
                        else
                        {
                            colonne = 0;
                        }
                        timerAnimation = 0;
                        pnjRectangle = new Rectangle(colonne * 48, ligne * 94, 48, 94);
                    }

                }
                if (state == 2)
                {
                    ligne = 2;
                    colonne = 0;
                    pnjRectangle = new Rectangle(colonne * 48, ligne * 94, 48, 94);
                }
            }
            if (map == "map5")
            {
                if (state == 0)
                {
                    colonne = 5;
                    pnjRectangle = new Rectangle(colonne * 85, 0, 85, 80);
                }

                if (state == 1)
                {
                    timerAnimation++;
                    if (timerAnimation == 15)
                    {
                        if (colonne == 5)
                        {
                            colonne = 0;
                        }
                        else
                        {
                            colonne++;
                        }
                        timerAnimation = 0;
                        pnjRectangle = new Rectangle(colonne * 85, 0, 85, 80);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spritBatch, int bookState, string map)
        {
            if (map == "map8")
            {
                spritBatch.Draw(pnjTexture, pnjPosition, pnjRectangle, Color.White);
                if (bookState == 2)
                {
                    spritBatch.Draw(pnjTexture, pnjPosition, pnjRectangle, Color.White, 0f, origine, 1.0f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            if (map == "map5")
            {
                spritBatch.Draw(pnjTexture, pnjPosition, pnjRectangle, Color.White, 0f, origine, 1.0f, SpriteEffects.FlipHorizontally, 0);
            }

        }

    }
}
