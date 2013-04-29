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

namespace Project
{
    class Sprite
    {
        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
        }

        Rectangle? sourceRectangle = null;
        public Rectangle? SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        double index = 0;
        int maxIndex = 0;

        public Sprite(Vector2 position, Rectangle? sourceRectangle) //determine la position du sprite
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
        }

        public void LoadContent(ContentManager content, string assetName, int maxIndex) //Chargement des textures des sprites
        {
            texture = content.Load<Texture2D>(assetName);
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime)
        {
            if (maxIndex != 0)
            {
                index += gameTime.ElapsedGameTime.Milliseconds * 0.001;
            }
            if (index > maxIndex)
            {
                index = 0;
            }
            sourceRectangle = new Rectangle((int)index * sourceRectangle.Value.X, sourceRectangle.Value.Y, sourceRectangle.Value.Width, sourceRectangle.Value.Height);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White);
        }



    }
}

