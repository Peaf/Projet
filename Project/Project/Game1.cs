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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static Character player;

        public static PNJ pnj1,healer;
        public static Enemy enemy, enemy1, enemy2;
        Song song1, song2;

        Random rand = new Random();
        Video video;
        VideoPlayer playerVideo;

        //Pause
        public static cButton btnNext, btnEndFight;
        public static ParticleGenerator snow;

        public static MouseState pastMouse;
        KeyboardState presentKey;
        KeyboardState pastKey;

        StreamReader streamMap8 = new StreamReader("map8.txt");
        StreamReader streamMap5 = new StreamReader("map5.txt");
        public static SpriteFont spriteFont;

        //Vector2 textvect = new Vector2();
        bool Isfighting = false, inventaire = false, talking = false, playOnce = true;
        //public static bool paused = false;
        int screenWidth, screenHeight; //taille de l'ecran
        public static float previousPosX, previousPosY;
        public static int bookState = 0;

        int[,] tab_map8 = new int[26, 44];
        int[,] tab_map5 = new int[26, 44];

        public static Inventaire invent = new Inventaire();

        public enum GameState
        {
            Video,
            Title,
            MainMenu,
            Options,
            Playing,
            Fight,
            GameOver,
            Pause
        }
        public GameState CurrentGameState = GameState.Playing;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            player = new Character(Content.Load<Texture2D>("Sprites/Player"), new Vector2(388, 130), new Rectangle(260 - 30, 438, 30, 59), new Rectangle(0, 0, 30, 59), 500, 200, 0, 50, 10, 15, 50);

            /*  this.graphics.IsFullScreen = true;
              this.graphics.ApplyChanges();
           
              this.graphics.ApplyChanges();*/
            screenWidth = 1366;
            screenHeight = 768;
            invent.Initialize();
            invent.addItem(new Item("Potion","healthPotion","health",50 , 1));
            invent.addItem(new Item("Weapon","Sword","dmg", 30, 1));
            invent.addItem(new Item("Armor","Armor","",100,1));
            invent.addItem(new Item("Potion", "healthPotion", "health", 50, 1));
            invent.addItem(new Item("Weapon", "Sword", "dmg", 30, 1));
            //  invent.addItem(new Item("Potion", 1));

            base.Initialize();
        }

        protected override void LoadContent()
        {

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            graphics.ApplyChanges();

            IsMouseVisible = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            song1 = Content.Load<Song>("Song/Song1");
            song2 = Content.Load<Song>("Song/Song2");

            //Video
            video = Content.Load<Video>("Intro");
            playerVideo = new VideoPlayer();

            //Menu
            Menu.LoadContent(Content, screenWidth, screenHeight);

            //Playing
            Playing.LoadContent(Content, screenWidth, screenHeight);

            //Fight
            Fight.LoadContent(Content, spriteBatch, screenWidth, screenHeight);

            //Enemy
            enemy1 = new Enemy(Content.Load<Texture2D>("Sprites/enemy"), new Vector2(800, 600), new Rectangle(815, 600, 50, 62), new Rectangle(0, 0, 30, 59), 200);
            enemy2 = new Enemy(Content.Load<Texture2D>("Sprites/enemy"), new Vector2(350, 400), new Rectangle(365, 400, 50, 62), new Rectangle(0, 0, 30, 59), 200);

            //PNJ
            healer = new PNJ(Content.Load<Texture2D>("Sprites/Healer"), new Vector2(1220, 495), new Rectangle(1220, 495, 658, 696), new Vector2(1220, 495));
            pnj1 = new PNJ(Content.Load<Texture2D>("Sprites/PnjAnimation"), new Vector2(1110, 290), new Rectangle(1110, 290, 276, 378), new Vector2(1110, 290));
             //Pause
            Pause.LoadContent(Content, screenWidth, screenHeight);

            //Options

            Options.LoadContent(Content, screenWidth, screenHeight);

            //TexteHealth
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            presentKey = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            /* if (CurrentGameState == GameState.Playing && !inventaire && !talking)
             {
                 IsMouseVisible = false;
             }
             else
             {
                 IsMouseVisible = true;
             }*/

            switch (CurrentGameState)
            {
                case GameState.Video:

                    if (playOnce)
                    {
                        playOnce = false;
                    }

                    if (presentKey.IsKeyDown(Keys.Enter) && pastKey.IsKeyUp(Keys.Enter))
                    {
                        CurrentGameState = GameState.MainMenu;
                        MediaPlayer.Play(song1);
                    }
                    break;

                case GameState.Title:

                    if (presentKey.IsKeyDown(Keys.Enter))
                    {
                        CurrentGameState = GameState.Video;
                        playerVideo.Play(video);
                    }
                    break;

                case GameState.MainMenu:

                    CurrentGameState = Menu.Update(gameTime);

                    break;

                case GameState.Playing:

                    CurrentGameState = Playing.Update(gameTime, screenWidth, screenHeight, graphics);

                    break;

                case GameState.Pause:

                    CurrentGameState = Pause.Update(gameTime);

                    break;

                case GameState.Options:

                    CurrentGameState = Options.Update(gameTime);

                    break;

                case GameState.Fight:

                    CurrentGameState = Fight.Update(gameTime, screenWidth, screenHeight);

                    break;

                case GameState.GameOver:
                    if (presentKey.IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }

                    break;
            }

            pastKey = presentKey;
            pastMouse = mouse;
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch (CurrentGameState)
            {
                case GameState.Video:
                    Texture2D videoTexture = playerVideo.GetTexture();
                    spriteBatch.Draw(videoTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    break;
                case GameState.Title:
                    {
                        spriteBatch.Draw(Content.Load<Texture2D>("Menu/Title"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    }

                    break;

                case GameState.MainMenu:

                    Menu.Draw(gameTime, spriteBatch, screenWidth, screenHeight);

                    break;

                case GameState.Playing:

                    Playing.Draw(gameTime, spriteBatch, screenWidth, screenHeight);

                    break;

                case GameState.Pause:

                    Playing.Draw(gameTime, spriteBatch, screenWidth, screenHeight);
                    Pause.Draw(gameTime, spriteBatch, screenWidth, screenHeight);

                    break;

                case GameState.Options:

                    Options.Draw(gameTime, spriteBatch, screenWidth, screenHeight);

                    break;

                case GameState.Fight:
                    Fight.Draw(gameTime, spriteBatch, screenWidth, screenHeight);
                    break;

                case GameState.GameOver:

                    spriteBatch.Draw(Content.Load<Texture2D>("Menu/GameOver"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    break;

            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
