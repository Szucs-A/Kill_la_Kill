using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml.Serialization;
using System.IO;

namespace KLK
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Random r = new Random();

        public static KeyboardState kb = new KeyboardState();
        public static KeyboardState oldkb = new KeyboardState();
        public static MouseState ms = new MouseState();

        public static GamePadState gp = new GamePadState();
        public static GamePadState oldgp = new GamePadState();

        public static Texture2D t;

        public static SpriteFont Main;

        public static GameList gameList;

        public static int highScore;

        public static Boolean mute;

        public static Boolean save = false;

        public static Boolean playelevator = false;

        public static int CoutnerforSongs = 0;

        public static Song elevator;

        public static SoundEffect Movement;

        public static Boolean PlayingLoop = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 500;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            // TODO: Add your initialization logic here

            mute = false;

            gameList = new GameList();
            gameList.Initialize();
        }

        public static bool displaywarn = false;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.Load();

            // TODO: use this.Content to load your game content here
            Playing.LoadContent(Content);
            Beginning.LoadContent(Content);
            MainMenu.LoadContent(Content);
            DodgeText.LoadContent(Content);

            Main = Content.Load<SpriteFont>("Font//MainFont");

            elevator = Content.Load<Song>("Sounds//Elevator");

            Movement = Content.Load<SoundEffect>("Sounds//Click");

            t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData<Color>(
                new Color[] { Color.White });// fill the texture with white
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public static bool checkedForGamepad = false;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            ms = Mouse.GetState();
            kb = Keyboard.GetState();
            gp = GamePad.GetState(PlayerIndex.One);

            if (!checkedForGamepad)
            {
                checkedForGamepad = true;

                if (gp.IsConnected)
                {
                    var capabilities = GamePad.GetCapabilities(PlayerIndex.One);

                    if (capabilities != null)
                    {
                        bool hasX = capabilities.HasXButton;
                        bool hasY = capabilities.HasYButton;
                        bool hasA = capabilities.HasAButton;
                        bool hasB = capabilities.HasBButton;
                        bool rightB = capabilities.HasRightShoulderButton;
                        bool leftJ = capabilities.HasLeftYThumbStick;
                        bool hasBack = capabilities.HasBackButton;

                        if (!(hasX && hasY && hasA && hasB && rightB && leftJ && hasBack))
                        {
                            displaywarn = true;
                        }
                    }
                }
            }
            
            if (playelevator)
                CoutnerforSongs += gameTime.ElapsedGameTime.Milliseconds;

            if (CoutnerforSongs > 51000)
            {
                if (!Game1.mute)
                    MediaPlayer.Play(elevator);
                CoutnerforSongs = 0;
                playelevator = false;
            }

            gameList.Update(gameTime);

            if (save)
            {
                Save(highScore);
                Load();
                save = false;
            }

            if (!(gameList.CurrentScreen is Playing) && PlayingLoop)
                PlayingLoop = false;

            if ((Game1.gp.Buttons.X == ButtonState.Pressed && gameList.CurrentScreen is MainMenu && oldgp.Buttons.X == ButtonState.Released) || 
                (Keyboard.GetState().IsKeyDown(Keys.P) && gameList.CurrentScreen is MainMenu && oldkb.IsKeyUp(Keys.P)))
                if (mute == false)
                {
                    mute = true;
                    MediaPlayer.Stop();
                }
                else
                {
                    mute = false;
                    MediaPlayer.Play(MainMenu.mainmenutheme);
                }

            if (gameList.CurrentScreen is Exit)
                Quit();

            oldkb = kb;
            oldgp = gp;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public void Quit()
        {
            this.Exit();
        }
        
        public void Save(int h)
        {
            SaveLoad.Save(h);
        }

        public void Load()
        {
            SaveLoad.Load();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            gameList.Draw(spriteBatch);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public static void DrawParticle(SpriteBatch sb, Color color, Vector2 pos, int width, int length)
        {
            sb.Draw(t,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)pos.X,
                    (int)pos.Y,
                    length, //sb will strech the texture to fill this rectangle
                    width), //width of line, change this to make thicker line
                null,
                color, //colour of line
                0,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
    }
}
