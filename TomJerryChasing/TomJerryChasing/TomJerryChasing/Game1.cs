#region File Description

#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace TomJerryChasing
{
    /// <summary>
    ///     This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        // field to keep track of game state      
        public static GameState _state;
        private readonly List<CheeseSprite> _cheeseFlakes = new List<CheeseSprite>();
        private readonly List<Explosion> _explosionsList = new List<Explosion>();
        private readonly Random _rand = new Random();
        public readonly string[] displaystrings = new string[2];
        private readonly GraphicsDeviceManager graphics;

        public int JerryScore;
        public int NumberLives = 3;
        private int RemLength;
        private Vector2 TomPosition;
        public int TomScore;
        private AudioEngine _audioEngine;
        private Texture2D _explosionTexture2D;
        private Menu _menu;
        public int _nextWaveMilliseconds;
        private int _score;
        private string _scoreString;
        private SoundBank _soundBank;
        private Texture2D _texture2DBackGround;
        private TimeSpan _timeElapsed;
        public TimeSpan _timeremaining;
        private WaveBank _wavebank;
        private Rectangle _worldRect;
        private Matrix _worldToScreenMatrix;
        private int ac = 0;
        private bool active = true;
        private Texture2D cheeseTexture;
        private GamePadState gamepadState;
        private int jerryOneRemoveLength;
        private int jerryRemoveLength;
        private string jerryScore = string.Empty;
        private KeyboardState keyboardState;
        private int numberLivesRemaining = 3;
        private int powerUpExpiration;
        private SpriteFont scoreFont;
        private Rectangle screenRectangle;
        private SpriteBatch spriteBatch;
        private SpriteManager spriteManager;
        private SpriteFont titlefFont;
        private TouchCollection touchCollection;

        #region  Constants

        private const string Score_string_prefix = "Tom Score: ";

        private const int Window_Width = 200;
        private const int Window_Height = 200;

        #endregion

        public Game1()
        {
            JerryOneActive = false;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferWidth = width - Window_Width;
            graphics.PreferredBackBufferHeight = height - Window_Height;
            IsMouseVisible = true;
        }


        //Random
        public Random Rnd { get; set; }

        public string Levels { get; set; }

        public bool JerryOneActive { get; set; }

        //Lives
        public int NumberLivesRemaining
        {
            get { return numberLivesRemaining; }
            set
            {
                numberLivesRemaining = value;
                if (numberLivesRemaining == 0)
                {
                    _state = GameState.PostGameThree;
                }
            }
        }

        public void JerryAddScore(int score)
        {
            JerryScore += score;
        }

        public void TomAddScore(int score)
        {
            TomScore += score;
        }

        public static void ChangeState(GameState newState)
        {
            _state = newState;
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            Rnd = new Random();
            base.Initialize();
            SetGameState(GameState.Mainmenu);
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load audio content
            _audioEngine = new AudioEngine(@"Content\GameAudio.xgs");
            _wavebank = new WaveBank(_audioEngine, @"Content\Wave Bank.xwb");
            _soundBank = new SoundBank(_audioEngine, @"Content\Sound Bank.xsb");

            //start playing background music
            _soundBank.PlayCue("backgroundMusic");

            //initialize menu objects
            _menu = new Menu(Content, Window.ClientBounds.Width, Window.ClientBounds.Height);

            // TODO: use this.Content to load your game content here                   
            //Background
            _texture2DBackGround = Content.Load<Texture2D>("background5");
            //Display string
            titlefFont = Content.Load<SpriteFont>("TitleFont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");

            _scoreString = GetScore(_score);
        }

        /// <summary>
        ///     Move from one game state to the next
        /// </summary>
        private void SetGameState(GameState gameState)
        {
            _state = gameState;

            switch (_state)
            {
                case GameState.Mainmenu:
                    _menu.Update(Mouse.GetState(), _soundBank);
                    jerryScore = string.Empty;
                    _score = 0;
                    _timeElapsed = TimeSpan.Zero;
                    _nextWaveMilliseconds = 35;
                    _cheeseFlakes.Clear();
                    break;
                case GameState.Play:

                    break;
                case GameState.PostGameOne:
                    //_timeremaining = TimeSpan.Zero;
                    break;
                case GameState.Quit:
                    break;
            }
        }


        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (_timeremaining.TotalMilliseconds > 0)
            {
                _timeElapsed += gameTime.ElapsedGameTime;
                _timeremaining -= gameTime.ElapsedGameTime;
            }
            // Allows the game to exit
            if (keyboard.IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            //process input based on the game state

            switch (_state)
            {
                case GameState.Mainmenu:
                    _menu.Update(Mouse.GetState(), _soundBank);
                    break;
                case GameState.GameContent:
                    break;
                case GameState.Play:                  
                    break;
                case GameState.PostGameOne:
                    if (keyboard.IsKeyDown(Keys.Space))
                    {
                        SetGameState(GameState.Mainmenu);
                        return;
                    }
                    break;
                case GameState.PostGameTwo:
                    if (keyboard.IsKeyDown(Keys.Space))
                    {
                        SetGameState(GameState.Mainmenu);
                        return;
                    }
                    break;
                case GameState.PostGameThree:
                    if (keyboard.IsKeyDown(Keys.Space))
                    {
                        SetGameState(GameState.Mainmenu);
                        return;
                    }
                    break;
                case GameState.PostGameFour:
                    if (keyboard.IsKeyDown(Keys.Space))
                    {
                        SetGameState(GameState.Mainmenu);
                        return;
                    }
                    break;
                default:
                    Exit();
                    break;
            }

            base.Update(gameTime);
        }

        private string GetScore(int score)
        {
            return Score_string_prefix + score;
        }


        //draw text with 1 pixel shadow
        private void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, int x, int y, Color color)
        {
            spriteBatch.DrawString(font, text, new Vector2(x + 1, y + 1), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(x, y), color);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            switch (_state)
            {
                case GameState.Mainmenu:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();
                    _menu.Draw(spriteBatch);
                    jerryScore = String.Empty;
                    spriteBatch.End();
                    break;
                case GameState.Play:
                    spriteBatch.Begin();
                    Vector2 stringVector2Dimension;
                    String jerry = "";
                    String jerryone = "";
                    // draw the text
                    Vector2 stringDimensions;


                    spriteBatch.Draw(_texture2DBackGround,
                        new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null, Color.White, 0,
                        Vector2.Zero, SpriteEffects.None, 0);

                    // Jerry Score
                    spriteBatch.DrawString(scoreFont, "Jerry Score : " + JerryScore,
                        new Vector2(Window.ClientBounds.Width - (150), 10), Color.DarkRed, 0, Vector2.Zero, 1,
                        SpriteEffects.None, 1);

                    // Tom Score
                    spriteBatch.DrawString(scoreFont, "Tom Score : " + TomScore,
                        new Vector2(Window.ClientBounds.Width - (150), 40), Color.DarkRed, 0, Vector2.Zero, 1,
                        SpriteEffects.None, 1);


                    string timeRemainingString = string.Format("Time:{0:00}:{1:00:0}", _timeremaining.Minutes,
                        _timeremaining.Seconds + _timeremaining.Milliseconds / 1000.0f);
                    Color timeColor = Color.White;
                    if (_timeremaining.TotalMilliseconds < 10000)
                    {
                        timeColor.B = (byte)(255 * _timeremaining.TotalMilliseconds / 10000);
                        timeColor.G = (byte)(255 * _timeremaining.TotalMilliseconds / 10000);
                    }
                    DrawString(spriteBatch, scoreFont, timeRemainingString, 0, 30, timeColor);

                    foreach (Explosion explosion in _explosionsList)
                    {
                        explosion.Draw(spriteBatch);
                    }

                    if (displaystrings[0] != null)
                    {
                        if (displaystrings[0].IndexOf("Level-2", StringComparison.Ordinal) >= -1 ||
                            displaystrings[0].IndexOf("Level-3", StringComparison.Ordinal) >= -1)
                        {
                            displaystrings[1] = "Your Score: " + TomScore;

                            stringDimensions = titlefFont.MeasureString(displaystrings[0]);
                            DrawString(spriteBatch, titlefFont, displaystrings[0],
                                ((Window.ClientBounds.Width / 2) - (int)(stringDimensions.X) / 2),
                                (Window.ClientBounds.Height / 2) + 1 * 30, Color.Aqua);

                            stringDimensions = titlefFont.MeasureString(displaystrings[1]);
                            DrawString(spriteBatch, titlefFont, displaystrings[1],
                                ((Window.ClientBounds.Width / 2) - (int)(stringDimensions.X) / 2),
                                (Window.ClientBounds.Height / 2) + 2 * 30, Color.Aqua);
                        }
                    }

                    spriteBatch.End();
                    break;
                case GameState.GameContent:
                    GraphicsDevice.Clear(Color.Black);
                    //Draw game over text
                    spriteBatch.Begin();
                    string[] instructionGameContent = { "Avoid the dog or die!", "time runs out!", "Tap space to start" };

                    for (int i = 0; i < instructionGameContent.Length; i++)
                    {
                        stringDimensions = titlefFont.MeasureString(instructionGameContent[i]);
                        DrawString(spriteBatch, titlefFont, instructionGameContent[i],
                            ((Window.ClientBounds.Width / 2) - (int)(stringDimensions.X) / 2),
                            (Window.ClientBounds.Height / 2) + i * 30, Color.Aqua);
                    }
                    spriteBatch.End();
                    break;
                case GameState.PostGameOne:


                    GraphicsDevice.Clear(Color.Black);

                    //Draw game over text
                    spriteBatch.Begin();
                    string[] instructionPostGameOne =
                    {
                        "Game Over! The jerry won!", "Your score: " + TomScore + "",
                        "Tap space to restart"
                    };

                    for (int i = 0; i < instructionPostGameOne.Length; i++)
                    {
                        stringDimensions = titlefFont.MeasureString(instructionPostGameOne[i]);
                        DrawString(spriteBatch, titlefFont, instructionPostGameOne[i],
                            ((Window.ClientBounds.Width / 2) - (int)(stringDimensions.X) / 2),
                            (Window.ClientBounds.Height / 2) + i * 30, Color.Aqua);
                    }
                    spriteBatch.End();
                    break;
                case GameState.PostGameTwo:
                    GraphicsDevice.Clear(Color.Black);
                    //Draw game over text
                    spriteBatch.Begin();
                    string[] instructionPostGameTwo =
                    {
                        "You Win!", "Your score: " + TomScore + "",
                        "Tap space to restart"
                    };
                    for (int i = 0; i < instructionPostGameTwo.Length; i++)
                    {
                        stringDimensions = titlefFont.MeasureString(instructionPostGameTwo[i]);
                        DrawString(spriteBatch, titlefFont, instructionPostGameTwo[i],
                            ((Window.ClientBounds.Width / 2) - (int)(stringDimensions.X) / 2),
                            (Window.ClientBounds.Height / 2) + i * 30, Color.Aqua);
                    }
                    spriteBatch.End();
                    break;
                case GameState.PostGameThree:
                    GraphicsDevice.Clear(Color.Black);
                    //Draw game over text
                    spriteBatch.Begin();
                    string[] instructionPostGameThree =
                    {
                        "Game Over! Dog killed you!", "Your score: " + TomScore + "",
                        "Tap space to restart"
                    };
                    for (int i = 0; i < instructionPostGameThree.Length; i++)
                    {
                        stringDimensions = titlefFont.MeasureString(instructionPostGameThree[i]);
                        DrawString(spriteBatch, titlefFont, instructionPostGameThree[i],
                            ((Window.ClientBounds.Width / 2) - (int)(stringDimensions.X) / 2),
                            (Window.ClientBounds.Height / 2) + i * 30, Color.Aqua);
                    }
                    spriteBatch.End();
                    break;
                case GameState.PostGameFour:
                    GraphicsDevice.Clear(Color.Black);
                    //Draw game over text
                    spriteBatch.Begin();
                    string[] instructionPostGameFour =
                    {
                        "Game Over! you won!", "Your score: " + TomScore + "",
                        "Tap space to restart"
                    };
                    for (int i = 0; i < instructionPostGameFour.Length; i++)
                    {
                        stringDimensions = titlefFont.MeasureString(instructionPostGameFour[i]);
                        DrawString(spriteBatch, titlefFont, instructionPostGameFour[i],
                            ((Window.ClientBounds.Width / 2) - (int)(stringDimensions.X) / 2),
                            (Window.ClientBounds.Height / 2) + i * 30, Color.Aqua);
                    }
                    spriteBatch.End();
                    break;
            }


            base.Draw(gameTime);
        }

        public void Wander(Vector2 position, ref Vector2 wanderDirection,
            ref float orientation, float turnSpeed, Rectangle clientBounds)
        {
            // The wander effect is accomplished by having the character aim in a random
            // direction. Every frame, this random direction is slightly modified.
            // Finally, to keep the characters on the center of the screen, we have them
            // turn to face the screen center. The further they are from the screen
            // center, the more they will aim back towards it.

            // the first step of the wander behavior is to use the random number
            // generator to offset the current wanderDirection by some random amount.
            // .25 is a bit of a magic number, but it controls how erratic the wander
            // behavior is. Larger numbers will make the characters "wobble" more,
            // smaller numbers will make them more stable. we want just enough
            // wobbliness to be interesting without looking odd.
            wanderDirection.X +=
                MathHelper.Lerp(-.25f, .25f, (float)_rand.NextDouble());
            wanderDirection.Y +=
                MathHelper.Lerp(-.25f, .25f, (float)_rand.NextDouble());

            // we'll renormalize the wander direction, ...
            if (wanderDirection != Vector2.Zero)
            {
                wanderDirection.Normalize();
            }
            // ... and then turn to face in the wander direction. We don't turn at the
            // maximum turning speed, but at 15% of it. Again, this is a bit of a magic
            // number: it works well for this sample, but feel free to tweak it.
            orientation = TurnToFace(position, position + wanderDirection, orientation,
                .15f * turnSpeed);


            // next, we'll turn the characters back towards the center of the screen, to
            // prevent them from getting stuck on the edges of the screen.
            Vector2 screenCenter = Vector2.Zero;
            // ReSharper disable PossibleLossOfFraction
            screenCenter.X = clientBounds.Width / 2;
            // ReSharper restore PossibleLossOfFraction
            // ReSharper disable PossibleLossOfFraction
            screenCenter.Y = clientBounds.Height / 2;
            // ReSharper restore PossibleLossOfFraction

            // Here we are creating a curve that we can apply to the turnSpeed. This
            // curve will make it so that if we are close to the center of the screen,
            // we won't turn very much. However, the further we are from the screen
            // center, the more we turn. At most, we will turn at 30% of our maximum
            // turn speed. This too is a "magic number" which works well for the sample.
            // Feel free to play around with this one as well: smaller values will make
            // the characters explore further away from the center, but they may get
            // stuck on the walls. Larger numbers will hold the characters to center of
            // the screen. If the number is too large, the characters may end up
            // "orbiting" the center.
            float distanceFromScreenCenter = Vector2.Distance(screenCenter, position);
            float maxDistanceFromScreenCenter =
                Math.Min(screenCenter.Y, screenCenter.X);

            float normalizedDistance =
                distanceFromScreenCenter / maxDistanceFromScreenCenter;

            float turnToCenterSpeed = .3f * normalizedDistance * normalizedDistance *
                                      turnSpeed;

            // once we've calculated how much we want to turn towards the center, we can
            // use the TurnToFace function to actually do the work.
            orientation = TurnToFace(position, screenCenter, orientation,
                turnToCenterSpeed);
        }


        public static float TurnToFace(Vector2 position, Vector2 faceThis, float currentAngle, float turnSpeed)
        {
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;

            var desiredAngle = (float)Math.Atan2(y, x);

            float difference = WrapAngle(desiredAngle - currentAngle);

            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            return WrapAngle(currentAngle + difference);
        }

        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }
    }
}