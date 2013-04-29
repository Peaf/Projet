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

    static class Menu
    {
        static Texture2D ImageMenu;
        static cButton PlayButton, QuitButton, OptionsButton;
        static Song song2;
        //public GameState CurrentGameState;

        public static void LoadContent(ContentManager Content, int screenWidth, int screenHeight)
        {

            ImageMenu = Content.Load<Texture2D>("Menu/MainMenu");

            PlayButton = new cButton(Content.Load<Texture2D>("Button/PlayButton"), 100, 75);
            PlayButton.setPosition(new Vector2(screenWidth / 2 - PlayButton.size.X / 2, screenHeight / 2 - 100));

            OptionsButton = new cButton(Content.Load<Texture2D>("Button/OptionsButton"), 250, 100);
            OptionsButton.setPosition(new Vector2(screenWidth / 2 - OptionsButton.size.X / 2, screenHeight / 2));

            QuitButton = new cButton(Content.Load<Texture2D>("Button/QuitButton"), 100, 75);
            QuitButton.setPosition(new Vector2(screenWidth / 2 - QuitButton.size.X / 2, screenHeight / 2 + 125));

            song2 = Content.Load<Song>("Song/Song2");
        }

        public static Game1.GameState Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            Game1.GameState CurrentGameState = Game1.GameState.MainMenu;
            if (PlayButton.isClicked == true)
            {
                MediaPlayer.Stop();
                CurrentGameState = Game1.GameState.Playing;
                MediaPlayer.Play(song2);
            }

            if (OptionsButton.isClicked == true)
                CurrentGameState = Game1.GameState.Options;


            if (QuitButton.isClicked == true)
                Environment.Exit(0);

            OptionsButton.Update(mouse, gameTime);
            QuitButton.Update(mouse, gameTime);
            PlayButton.Update(mouse, gameTime);
            return (CurrentGameState);
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            spriteBatch.Draw(ImageMenu, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            PlayButton.Draw(spriteBatch);
            OptionsButton.Draw(spriteBatch);
            QuitButton.Draw(spriteBatch);

        }



    }
}
