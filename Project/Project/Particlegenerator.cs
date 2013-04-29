using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project
{
    public class ParticleGenerator
    {
        Texture2D texture;

        float spawnWidth;
        float density;

        List<SnowDrops> snowdrops = new List<SnowDrops>();

        float timer;

        Random rand1, rand2;

        public ParticleGenerator(Texture2D newTexture, float newSpawnWidth, float newDensity)
        {
            texture = newTexture;
            spawnWidth = newSpawnWidth;
            density = newDensity;
            rand1 = new Random();
            rand2 = new Random();
        }

        public void Generate()
        {
            double succ1 = rand1.Next();

            snowdrops.Add(new SnowDrops(texture, new Vector2(-50 + (float)rand1.NextDouble() * spawnWidth, 0), new Vector2(1, rand2.Next(3, 6))));
        }

        public void update(GameTime gameTime, GraphicsDevice graphics)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer > 0)
            {
                timer -= 1f / density;
                Generate();
            }

            for (int i = 0; i < snowdrops.Count; i++)
            {
                snowdrops[i].Update();

                if (snowdrops[i].Position.Y > graphics.Viewport.Height)
                {
                    snowdrops.RemoveAt(i);

                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (SnowDrops snowdrop in snowdrops)
                snowdrop.Draw(spriteBatch);
        }
    }
}
