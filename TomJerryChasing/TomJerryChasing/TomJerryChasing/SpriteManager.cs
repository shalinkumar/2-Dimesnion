using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TomJerryChasing
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private int ac = 0;
        //Scoring
        int automatedSpritePointValue = 10;
        int CheeseSpritePointValue = 10;
        int jerrySpritePointValue = 10;

        //add reference microsoft.xna.framework.xact
        //audio components
        AudioEngine _audioEngine;
        WaveBank _wavebank;
        SoundBank _soundBank;

        //Spawning variables
        int enemySpawnMinMilliseconds = 3000;
        int enemySpawnMaxMilliseconds = 4000;
        int cheeseSpawnMinMilliseconds = 2000;
        int cheeseSpawnMaxMilliseconds = 3000;
        int enemyMinSpeed = 2;
        int enemyMaxSpeed = 6;
        int nextSpawnTime = 0;
        int nextCheeseSpawnTime = 0;

        List<AutomatedSprite> livesList = new List<AutomatedSprite>();

        int powerUpExpiration;



        #region Dog
        enum DogAiState
        {
            Chasing, Caught,
            Wander
        }
        DogAiState _dogState = DogAiState.Wander;
        public float _dogOrientation;
        Vector2 _dogwanderDirection;
        private Vector2 DogPosition;
        private Texture2D _dogSprite;
        private Vector2 _dogTextureCenter;
        private DogSprite _dogControlSprite = new DogSprite();
        List<DogSprite> listdDogSprites = new List<DogSprite>();

        #region Constants

        const int Damage = 25;
        //how fast the dog can move
        const float MaxDogSpeed = 8.5f;

        //how fast the dog can move..
        const float DogTurnSpeed = 0.20f;

        //this value controls the distance at  which the dog will start to chase Tom
        const float DogChaseDistance = 350f;

        //dogcaught distance controls the distance at which the dog will stop  because he has caught Tom
        const float DogCaughtDistance = 70.0f;

        //this content is used to avoid hysterisis,this is commmon in all AI programming
        const float DogHysterisis = 15.0f;
        #endregion
        #endregion

        #region  Jerry

        private readonly List<JerrySpriteOne> JerrySpriteListOne = new List<JerrySpriteOne>();
        private readonly List<JerrySpriteTwo> JerrySpriteListTwo = new List<JerrySpriteTwo>();
        private readonly List<JerrySpriteThree> JerrySpriteListThree = new List<JerrySpriteThree>();
        private readonly JerrySpriteOne JsSpriteOne = new JerrySpriteOne();
        private readonly JerrySpriteTwo JsSpriteTwo = new JerrySpriteTwo();
        private readonly JerrySpriteThree jsSpriteThree = new JerrySpriteThree();
        private Vector2 JerryPositionOne;
        private Vector2 JerryPositionTwo;
        private Vector2 JerryPositionThree;

        private float _jerryOrientationOne;
        private float _jerryOrientationTwo;
        public float _jerryOrientationThree;
        private JerryAiState _jerryStateOne = JerryAiState.Wander;
        private JerryAiState _jerryStateTwo = JerryAiState.Wander;
        private JerryAiState _jerryStateThree = JerryAiState.Wander;
        private Vector2 _jerryTextureCenterOne;
        private Vector2 _jerryTextureCenterThree;
        private Vector2 _jerryTextureCenterTwo;
        private Texture2D _jerryTextureOne;
        private Texture2D _jerryTextureThree;
        private Texture2D _jerryTextureTwo;
        private Vector2 _jerryWnaderDirectionOne;
        private Vector2 _jerryWnaderDirectionTwo;
        private Vector2 _jerryWnaderDirectionThree;

        #region  Constants

        //how fast the Jerry can move
        //low speed
        private const float MaxJerrySpeedOne = 8f;
        //High speed
        //const float MaxJerrySpeed = 12.5f;
        //how fast the Jerry can turn
        private const float JerryTurnSpeedOne = 0.50f;

        // JerryEvadeDistance controls the distance at which the Jerry will flee from
        // tom. If the Jerry is further than "JerryEvadeDistance" pixels away, he will
        // consider himself safe.
        private const float JerryEvadeDistanceOne = 350.0f;

        // this constant is similar to TankHysteresis. The value is larger than the
        // dog's hysteresis value because the Jerry is faster than the dog: with a
        // higher velocity, small fluctuations are much more visible.
        private const float JerryHysteresisOne = 60.0f;

        //this value controls the distance at  which the jerry will start to chase cheese
        private const float JerryChaseDistanceOne = 400f;

        //dogcaught distance controls the distance at which the dog will stop  because he has caught Tom
        private const float JerryCaughtDistanceOne = 70.0f;

        //how fast the Jerry can move
        //low speed
        private const float MaxJerrySpeedTwo = 8f;
        //High speed
        //const float MaxJerrySpeed = 12.5f;
        //how fast the Jerry can turn
        private const float JerryTurnSpeedTwo = 0.50f;

        // JerryEvadeDistance controls the distance at which the Jerry will flee from
        // tom. If the Jerry is further than "JerryEvadeDistance" pixels away, he will
        // consider himself safe.
        private const float JerryEvadeDistanceTwo = 350.0f;

        // this constant is similar to TankHysteresis. The value is larger than the
        // dog's hysteresis value because the Jerry is faster than the dog: with a
        // higher velocity, small fluctuations are much more visible.
        private const float JerryHysteresisTwo = 60.0f;

        //this value controls the distance at  which the jerry will start to chase cheese
        private const float JerryChaseDistanceTwo = 400f;

        //dogcaught distance controls the distance at which the dog will stop  because he has caught Tom
        private const float JerryCaughtDistanceTwo = 70.0f;

        //how fast the Jerry can move
        //low speed
        private const float MaxJerrySpeedThree = 8f;
        //High speed
        //const float MaxJerrySpeed = 12.5f;
        //how fast the Jerry can turn
        private const float JerryTurnSpeedThree = 0.50f;

        // JerryEvadeDistance controls the distance at which the Jerry will flee from
        // tom. If the Jerry is further than "JerryEvadeDistance" pixels away, he will
        // consider himself safe.
        private const float JerryEvadeDistanceThree = 350.0f;

        // this constant is similar to TankHysteresis. The value is larger than the
        // dog's hysteresis value because the Jerry is faster than the dog: with a
        // higher velocity, small fluctuations are much more visible.
        private const float JerryHysteresisThree = 60.0f;

        //this value controls the distance at  which the jerry will start to chase cheese
        private const float JerryChaseDistanceThree = 400f;

        //dogcaught distance controls the distance at which the dog will stop  because he has caught Tom
        private const float JerryCaughtDistanceThree = 70.0f;

        #endregion

        private enum JerryAiState
        {
            Wander,
            Evading,
            Caught
        }

        #endregion

        #region Cheese

        private Texture2D cheeseTexture;
        List<CheeseSprite> cheeseList = new List<CheeseSprite>();
        readonly List<CheeseSprite> _removeList = new List<CheeseSprite>();
        private int likelihoodAutomated = 75;
        private CheeseSprite _cheeseSprite = new CheeseSprite();
        #endregion

        #region Tom

        private TomSprite _playerControlledSprite;
        Texture2D _tomSprite;
        #endregion

        #region Explosion
        Texture2D explosionspritestrip;
        List<ExplosionSprite> explosionList = new List<ExplosionSprite>();
        List<ExplosionSprite> _explosionsRemoveList = new List<ExplosionSprite>();
        #endregion

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            ResetSpawnTime();
            ResetCheeseSpawnTime();
            DogPosition = new Vector2(x: Game.Window.ClientBounds.Width / 4, y: -1 * Game.Window.ClientBounds.Height / 2);
            JerryPositionOne = new Vector2((Game.Window.ClientBounds.Width + 150) - Game.Window.ClientBounds.Width, (Game.Window.ClientBounds.Height - 50));
            JerryPositionTwo = new Vector2((Game.Window.ClientBounds.Width + 110) - Game.Window.ClientBounds.Width, (Game.Window.ClientBounds.Height - 48));
            JerryPositionThree = new Vector2((Game.Window.ClientBounds.Width + 50) - Game.Window.ClientBounds.Width, (Game.Window.ClientBounds.Height - 50));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //Tom
            _tomSprite = Game.Content.Load<Texture2D>("Tom");
            _playerControlledSprite = new TomSprite(
             _tomSprite,
             new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2), new Point(_tomSprite.Width / 2, _tomSprite.Height), 10, new Point(0, 0),
             new Point(1, 1), new Vector2(6, 6), "Tom", automatedSpritePointValue);
            _playerControlledSprite._drawRectangle.Width = _tomSprite.Width / 2;
            _playerControlledSprite._drawRectangle.Height = _tomSprite.Height;           
            _playerControlledSprite._sourceRectangle = new Rectangle(0, 2, _tomSprite.Width / 2, _tomSprite.Height);

            //Dog

            _dogSprite = Game.Content.Load<Texture2D>("dogchase");
            _dogTextureCenter = new Vector2(x: _dogSprite.Width / 2, y: _dogSprite.Height / 2);           
            listdDogSprites.Add(new DogSprite(_dogSprite, new Vector2(x: DogPosition.X, y: DogPosition.Y / 2), new Point(200, 110), 10, new Point(0, 0), new Point(1, 1), new Vector2(6, 6), "Dog",
                automatedSpritePointValue));

            //Jerry
            _jerryTextureOne = Game.Content.Load<Texture2D>("jerrychase0");
            _jerryTextureTwo = Game.Content.Load<Texture2D>("jerrychase1");
            _jerryTextureThree = Game.Content.Load<Texture2D>("jerrychase2");
            if (_jerryTextureOne != null)
                _jerryTextureCenterOne = new Vector2(_jerryTextureOne.Width / 2, _jerryTextureOne.Height / 2);

            if (_jerryTextureTwo != null)
                _jerryTextureCenterTwo = new Vector2(_jerryTextureTwo.Width / 2, _jerryTextureTwo.Height / 2);

            if (_jerryTextureThree != null)
                _jerryTextureCenterThree = new Vector2(_jerryTextureThree.Width / 2, _jerryTextureThree.Height / 2);
            for (int i = 0; i < ((Game1)Game).NumberLives; i++)
            {
                int offset = 0;
                if (i == 0)
                {
                   
                    JerrySpriteListOne.Add(new JerrySpriteOne(_jerryTextureOne, new Vector2((Game.Window.ClientBounds.Width + 150) - Game.Window.ClientBounds.Width, (Game.Window.ClientBounds.Height - 50)), new Point(200, 110), 10,
              new Point(0, 0), new Point(1, 1), new Vector2(6, 6), "Jerry",
              jerrySpritePointValue, JsSpriteOne.Scale));

                }
                else if (i == 1)
                {                    
                    JerrySpriteListTwo.Add(new JerrySpriteTwo(_jerryTextureTwo, new Vector2((Game.Window.ClientBounds.Width + 110) - Game.Window.ClientBounds.Width, (Game.Window.ClientBounds.Height - 48)), new Point(200, 110), 10,
               new Point(0, 0), new Point(1, 1), new Vector2(6, 6), "Jerry",
               jerrySpritePointValue, JsSpriteTwo.Scale));
                }
                else if (i == 2)
                {
                    JerrySpriteListThree.Add(new JerrySpriteThree(_jerryTextureThree, new Vector2((Game.Window.ClientBounds.Width + 50) - Game.Window.ClientBounds.Width, (Game.Window.ClientBounds.Height - 50)), new Point(200, 110), 10,
               new Point(0, 0), new Point(1, 1), new Vector2(6, 6), "Jerry",
               jerrySpritePointValue, jsSpriteThree.Scale));
                }
            }
            explosionspritestrip = Game.Content.Load<Texture2D>("explosion");
        }

        private void LoadNumberLives()
        {
            for (int i = 0; i < ((Game1)Game).NumberLivesRemaining; ++i)
            {
                int offset = 10 + i * 40;
                livesList.Add(new AutomatedSprite(
                    Game.Content.Load<Texture2D>("Tom"),
                    new Vector2(offset, 75), new Point(_tomSprite.Width / 2, _tomSprite.Height), 10,
                    new Point(0, 0), new Point(1, 1), Vector2.Zero,
                    null, 0, .5f));
            }
        }

        private void ResetSpawnTime()
        {
            nextSpawnTime = ((Game1)Game).Rnd.Next(enemySpawnMinMilliseconds, enemySpawnMaxMilliseconds);
        }

        private void ResetCheeseSpawnTime()
        {
            nextCheeseSpawnTime = ((Game1)Game).Rnd.Next(cheeseSpawnMinMilliseconds, cheeseSpawnMaxMilliseconds);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            Vector2 CheesePosition = new Vector2();
            KeyboardState keyboard = Keyboard.GetState();
            //Update the lives left sprites
            switch (Game1._state)
            {
                case GameState.GameContent:
                    if (keyboard.IsKeyDown(Keys.Space))
                    {
                        ((Game1)Game).JerryScore = 0;
                        ((Game1)Game).TomScore = 0;
                        ((Game1)Game).Levels = "";
                        ((Game1)Game)._timeremaining = TimeSpan.Zero;
                        ((Game1)Game)._timeremaining = TimeSpan.FromSeconds(30);
                        ((Game1)Game).NumberLivesRemaining = 3;
                        ((Game1)Game).displaystrings[0] = null;
                        DogPosition.X = 0;
                        DogPosition.Y = 0;
                        for (int i = 0; i < JerrySpriteListOne.Count; i++)
                        {
                            JerrySpriteOne js = JerrySpriteListOne[i];
                            JerryPositionOne.X = (Game.Window.ClientBounds.Width + 110) - Game.Window.ClientBounds.Width;
                            JerryPositionOne.Y = (Game.Window.ClientBounds.Height - 48);
                            js.ResetScale();                                              
                        }                                           
                        for (int i = 0; i < JerrySpriteListTwo.Count; i++)
                        {
                            JerrySpriteTwo js = JerrySpriteListTwo[i];
                            JerryPositionTwo.X = (Game.Window.ClientBounds.Width + 110) - Game.Window.ClientBounds.Width;
                            JerryPositionTwo.Y = (Game.Window.ClientBounds.Height - 48);
                            js.Position = JerryPositionTwo;
                            js.ResetScale();
                            _jerryOrientationTwo = 0.0f;      
                        }
                        for (int i = 0; i < JerrySpriteListThree.Count; i++)
                        {
                            JerrySpriteThree js = JerrySpriteListThree[i];
                            JerryPositionThree.X = (Game.Window.ClientBounds.Width + 50) - Game.Window.ClientBounds.Width;
                            JerryPositionThree.Y = (Game.Window.ClientBounds.Height - 50);
                            js.Position = JerryPositionThree;
                            js.ResetScale();
                            _jerryOrientationThree = 0.0f; 
                        }                                                                  
                        _playerControlledSprite._drawRectangle.X = Game.Window.ClientBounds.Width / 2;
                        _playerControlledSprite._drawRectangle.Y = Game.Window.ClientBounds.Height / 2;
                        LoadNumberLives();
                        Game1.ChangeState(GameState.Play);
                        return;
                    }
                    break;

                case GameState.Play:

                    foreach (ExplosionSprite explosion in explosionList)
                    {
                        explosion.Update(gameTime, Game.Window.ClientBounds);
                    }
                    for (int ii = explosionList.Count - 1; ii >= 0; ii--)
                    {
                        if (!explosionList[ii].Active)
                        {
                            explosionList.RemoveAt(ii);
                        }
                    }
                    _playerControlledSprite.Update(gameTime, Game.Window.ClientBounds);

                    foreach (Sprite sprite in livesList)
                        sprite.Update(gameTime, Game.Window.ClientBounds);

                    if (listdDogSprites.Count > 0)
                    {
                        for (int i = 0; i < listdDogSprites.Count; i++)
                        {
                            DogSprite sp = listdDogSprites[i];
                            sp.Update(gameTime, Game.Window.ClientBounds);                          
                            DogPosition = DogTomCollision(sp, explosionList);
                            sp.DogDirection = DogPosition;
                        }
                    }
                    else
                    {
                        //Determine if it's time to spawn
                        nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
                        if (nextSpawnTime < 0)
                        {
                            listdDogSprites.Add(new DogSprite(_dogSprite, new Vector2(x: DogPosition.X, y: DogPosition.Y / 2), new Point(200, 110), 10, new Point(0, 0), new Point(1, 1), new Vector2(6, 6), "Dog",
                   automatedSpritePointValue));
                            // Reset spawn timer
                            ResetSpawnTime();
                        }
                    }

                    nextCheeseSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
                    if (nextCheeseSpawnTime < 0)
                    {
                        int snowflakeCount = 10;
                        for (var i = 0; i < snowflakeCount; i++)
                        {
                            SpawnEnemy();
                            ResetCheeseSpawnTime();
                        }
                    }
                    var CountCheck = 0;
                    for (int i = 0; i < JerrySpriteListOne.Count; i++)
                    {
                        JerrySpriteOne js = JerrySpriteListOne[i];
                        js.Update(gameTime, Game.Window.ClientBounds);
                        JerryPositionOne = JerryUpdate(js);

                        if (_playerControlledSprite.CollisionRectangle.Intersects(js.ColliRectangle))
                        {                            
                            powerUpExpiration = 1000;
                            js.ModifySpeed(1.2f);                          
                        }
                        
                        js.JerryDirection = JerryPositionOne;                       
                        CheckPowerUpExpirationOne(gameTime, js);
                    }

                    for (var i = 0; i < cheeseList.Count; ++i)
                    {
                        CheeseSprite s = cheeseList[i];
                        s.Update(gameTime, Game.Window.ClientBounds);
                        CheesePosition = s.GetPosition;
                        JerryPositionOne = JerryCheeseUpdate(JerryPositionOne, CheesePosition, Game.Window.ClientBounds, cheeseList, _removeList);
                       
                        if (_playerControlledSprite.CollisionRectangle.Intersects(s.ColliRectangle))
                        {
                            ((Game1)Game).TomAddScore(cheeseList[i].ScoreValue);
                            cheeseList.RemoveAt(i);
                        }
                    }

                    if (((Game1)Game)._timeremaining.Seconds.Equals(02))
                    {
                        if (((Game1)Game).TomScore >= ((Game1)Game).JerryScore)
                        {
                            //Level increased
                            ac++;
                            ((Game1)Game)._timeremaining += TimeSpan.FromSeconds(((Game1)Game)._nextWaveMilliseconds);
                            var abc = ac + 1;
                            ((Game1)Game).displaystrings[0] = "Level-" + abc + "";
                            ((Game1)Game).Levels = "Level-" + abc + "";                           
                        }
                        if (((Game1)Game).TomScore < ((Game1)Game).JerryScore)
                        {
                            Game1._state = GameState.PostGameOne;
                            return;
                        }
                    }
                    if (((Game1)Game).Levels == "Level-2")
                    {
                        for (int i = 0; i < JerrySpriteListTwo.Count; i++)
                        {
                            JerrySpriteTwo js = JerrySpriteListTwo[i];
                            js.Update(gameTime, Game.Window.ClientBounds);
                            JerryPositionTwo = JerryUpdate(js);
                            if (_playerControlledSprite.CollisionRectangle.Intersects(js.ColliRectangle))
                            {
                                powerUpExpiration = 1000;
                                js.ModifySpeed(1.2f);
                            }
                            js.Position = JerryPositionTwo;
                            CheckPowerUpExpirationTwo(gameTime, js);
                        }

                        for (int i = 0; i < cheeseList.Count; ++i)
                        {
                            CheeseSprite s = cheeseList[i];
                            s.Update(gameTime, Game.Window.ClientBounds);
                            CheesePosition = s.GetPosition;

                            JerryPositionTwo = JerryCheeseUpdateTwo(JerryPositionTwo, CheesePosition, Game.Window.ClientBounds,
                                cheeseList,
                                _removeList);
                        }
                    }
                    if (((Game1)Game).Levels == "Level-3")
                    {
                        for (int i = 0; i < JerrySpriteListThree.Count; i++)
                        {
                            JerrySpriteThree js = JerrySpriteListThree[i];
                            js.Update(gameTime, Game.Window.ClientBounds);
                            JerryPositionThree = JerryUpdate(js);
                            if (_playerControlledSprite.CollisionRectangle.Intersects(js.ColliRectangle))
                            {
                                powerUpExpiration = 1000;
                                js.ModifySpeed(1.2f);
                            }
                            js.Position = JerryPositionThree;
                            CheckPowerUpExpirationThree(gameTime, js);
                        }

                        for (int i = 0; i < cheeseList.Count; ++i)
                        {
                            CheeseSprite s = cheeseList[i];
                            s.Update(gameTime, Game.Window.ClientBounds);
                            CheesePosition = s.GetPosition;

                            JerryPositionThree = JerryCheeseUpdateThree(JerryPositionThree, CheesePosition, Game.Window.ClientBounds,
                                cheeseList,
                                _removeList);
                        }
                    }
                    if (((Game1) Game).Levels == "Level-4")
                    {
                        Game1.ChangeState(GameState.PostGameFour);
                    }
                    if (((Game1)Game)._timeremaining.Seconds.Equals(34))
                    {
                        ((Game1)Game).displaystrings[0] = null;
                    }                    
                    break;
            }


            base.Update(gameTime);
        }

        private Vector2 DogTomCollision(DogSprite sp, List<ExplosionSprite> explosionList)
        {
            var dogChaseThreshold = DogChaseDistance;
            var dogCaughtThreshold = DogCaughtDistance;

            switch (_dogState)
            {
                case DogAiState.Wander:
                    dogChaseThreshold -= DogHysterisis / 2;
                    break;
                case DogAiState.Chasing:
                    dogChaseThreshold += DogHysterisis / 2;
                    dogCaughtThreshold -= DogHysterisis / 2;
                    break;
                case DogAiState.Caught:
                    dogCaughtThreshold += DogHysterisis / 2;
                    break;
            }
            var distanceFromTom = Vector2.Distance(sp.GetPosition, _playerControlledSprite.GetPosition);
            if (distanceFromTom > dogChaseThreshold)
            {
                _dogState = DogAiState.Wander;              
            }
            else if (distanceFromTom > dogCaughtThreshold)
            {
                _dogState = DogAiState.Chasing;
            }
            else
            {
                _dogState = DogAiState.Caught;
            }           

            var currentDogSpeed = 0.0f;
            switch (_dogState)
            {
                case DogAiState.Chasing:
                    _dogOrientation = Game1.TurnToFace(sp.GetPosition, _playerControlledSprite.GetPosition, _dogOrientation, DogTurnSpeed);
                    currentDogSpeed = MaxDogSpeed;
                    break;
                case DogAiState.Wander:
                    ((Game1)Game).Wander(sp.GetPosition, ref _dogwanderDirection, ref _dogOrientation, DogTurnSpeed, Game.Window.ClientBounds);
                    currentDogSpeed = .25f * MaxDogSpeed;
                    break;
                //default:
                case DogAiState.Caught:
                    

                    for (int i = 0; i < listdDogSprites.Count; i++)
                    {
                        

                        if (_playerControlledSprite.CollisionRectangle.Intersects(listdDogSprites[i].ColliRectangle))
                        {
                            //currentDogSpeed = 0.0f;
                            // NOTE: Changed explosion class to start playing on creation                       
                            explosionList.Add(new ExplosionSprite(explosionspritestrip, new Vector2(x: _playerControlledSprite.Position.X, y: _playerControlledSprite.Position.Y), new Point(0, 0), 10, new Point(0, 0), new Point(3, 3), new Vector2(0, 0), "Dog",
                             automatedSpritePointValue, _playerControlledSprite.ColliRectangle.X, _playerControlledSprite.ColliRectangle.Y));

                            listdDogSprites.RemoveAt(i);

                            powerUpExpiration = 3000;
                            _playerControlledSprite.ModifySpeed(.8f);                           
                            if (livesList.Count > 0)
                            {
                                livesList.RemoveAt(livesList.Count - 1);
                                --((Game1)Game).NumberLivesRemaining;


                            }
                        }
                    }

                    // automatedSpritePointValue, _dogControlSprite.ColliRectangle.Center.X, _dogControlSprite.ColliRectangle.Center.Y));

                    break;
            }
            var heading = new Vector2((float)Math.Cos(_dogOrientation), (float)Math.Sin(_dogOrientation));
            DogPosition += heading * currentDogSpeed;
            return DogPosition;
        }

        private Vector2 JerryUpdate(JerrySpriteOne jsSprite)
        {
            var distanceFromTom = Vector2.Distance(jsSprite.GetPosition, _playerControlledSprite.GetPosition);

            if (distanceFromTom > JerryEvadeDistanceOne + JerryHysteresisOne)
            {
                _jerryStateOne = JerryAiState.Wander;
            }
            else if (distanceFromTom < JerryEvadeDistanceOne - JerryHysteresisOne)
            {
                _jerryStateOne = JerryAiState.Evading;
            }
            float currentJerrySpeed = 0.0f;
            if (_jerryStateOne == JerryAiState.Evading)
            {
                Vector2 seekPosition = 2 * jsSprite.GetPosition - _playerControlledSprite.GetPosition;

                _jerryOrientationOne = Game1.TurnToFace(jsSprite.GetPosition, seekPosition, _jerryOrientationOne,
                    JerryTurnSpeedOne);
                currentJerrySpeed = MaxJerrySpeedOne;
            }
            else if (_jerryStateOne == JerryAiState.Wander)
            {
                ((Game1)Game).Wander(jsSprite.GetPosition, ref _jerryWnaderDirectionOne, ref _jerryOrientationOne,
                    JerryTurnSpeedOne, Game.Window.ClientBounds);
                currentJerrySpeed = .25f * MaxJerrySpeedOne;
            }
            var heading = new Vector2((float)Math.Cos(_jerryOrientationOne), (float)Math.Sin(_jerryOrientationOne));

            return JerryPositionOne += heading * currentJerrySpeed;
        }

        private Vector2 JerryUpdate(JerrySpriteTwo jsSprite)
        {

            float distanceFromTom = Vector2.Distance(jsSprite.GetPosition, _playerControlledSprite.GetPosition);

            if (distanceFromTom > JerryEvadeDistanceTwo + JerryHysteresisTwo)
            {
                _jerryStateTwo = JerryAiState.Wander;
            }
            else if (distanceFromTom < JerryEvadeDistanceTwo - JerryHysteresisTwo)
            {
                _jerryStateTwo = JerryAiState.Evading;
            }
            float currentJerrySpeed = 0.0f;
            if (_jerryStateTwo == JerryAiState.Evading)
            {
                Vector2 seekPosition = 2 * jsSprite.GetPosition - _playerControlledSprite.GetPosition;

                _jerryOrientationTwo = Game1.TurnToFace(jsSprite.GetPosition, seekPosition, _jerryOrientationTwo,
                    JerryTurnSpeedTwo);
                currentJerrySpeed = MaxJerrySpeedTwo;
            }
            else if (_jerryStateTwo == JerryAiState.Wander)
            {
                ((Game1)Game).Wander(jsSprite.GetPosition, ref _jerryWnaderDirectionTwo, ref _jerryOrientationTwo,
                    JerryTurnSpeedTwo, Game.Window.ClientBounds);
                currentJerrySpeed = .25f * MaxJerrySpeedTwo;
            }
            var heading = new Vector2((float)Math.Cos(_jerryOrientationTwo), (float)Math.Sin(_jerryOrientationTwo));

            return JerryPositionTwo += heading * currentJerrySpeed;
        }

        private Vector2 JerryUpdate(JerrySpriteThree jsSprite)
        {

            float distanceFromTom = Vector2.Distance(jsSprite.GetPosition, _playerControlledSprite.GetPosition);

            if (distanceFromTom > JerryEvadeDistanceThree + JerryHysteresisThree)
            {
                _jerryStateThree = JerryAiState.Wander;
            }
            else if (distanceFromTom < JerryEvadeDistanceThree - JerryHysteresisThree)
            {
                _jerryStateThree = JerryAiState.Evading;
            }
            float currentJerrySpeed = 0.0f;
            if (_jerryStateThree == JerryAiState.Evading)
            {
                Vector2 seekPosition = 2 * jsSprite.GetPosition - _playerControlledSprite.GetPosition;

                _jerryOrientationThree = Game1.TurnToFace(jsSprite.GetPosition, seekPosition, _jerryOrientationThree,
                    JerryTurnSpeedThree);
                currentJerrySpeed = MaxJerrySpeedThree;
            }
            else if (_jerryStateThree == JerryAiState.Wander)
            {
                ((Game1)Game).Wander(jsSprite.GetPosition, ref _jerryWnaderDirectionThree, ref _jerryOrientationThree,
                    JerryTurnSpeedThree, Game.Window.ClientBounds);
                currentJerrySpeed = .25f * MaxJerrySpeedThree;
            }
            var heading = new Vector2((float)Math.Cos(_jerryOrientationThree), (float)Math.Sin(_jerryOrientationThree));

            return JerryPositionThree += heading * currentJerrySpeed;
        }

        private Vector2 JerryCheeseUpdate(Vector2 jerryPosition, Vector2 cheesePosition, Rectangle clientBounds,
           List<CheeseSprite> cheeseFlakes, List<CheeseSprite> removeList)
        {
            float jerryChaseThreshold = JerryChaseDistanceOne;
            float jerryCaughtThreshold = JerryCaughtDistanceOne;
            _jerryStateOne = JerryAiState.Wander;
            switch (_jerryStateOne)
            {
                case JerryAiState.Wander:
                    jerryChaseThreshold -= JerryHysteresisOne / 2;
                    break;
                case JerryAiState.Evading:
                    jerryChaseThreshold += JerryHysteresisOne / 2;
                    jerryCaughtThreshold -= JerryHysteresisOne / 2;
                    break;
                case JerryAiState.Caught:
                    jerryCaughtThreshold += JerryHysteresisOne / 2;
                    break;
            }
            float distanceFromCheese = Vector2.Distance(jerryPosition, cheesePosition);
            if (distanceFromCheese > jerryChaseThreshold)
            {
                _jerryStateOne = JerryAiState.Wander;
            }
            else if (distanceFromCheese > jerryCaughtThreshold)
            {
                _jerryStateOne = JerryAiState.Evading;
            }
            else
            {
                _jerryStateOne = JerryAiState.Caught;
            }

            int currentJerrySpeed = 0;

            switch (_jerryStateOne)
            {
                case JerryAiState.Evading:
                    {
                        Vector2 seekPosition = 2 * cheesePosition - jerryPosition;
                        //chasing
                        _jerryOrientationOne = Game1.TurnToFace(jerryPosition, seekPosition, _jerryOrientationOne,
                            JerryTurnSpeedOne);
                        currentJerrySpeed = (int)MaxJerrySpeedOne;
                    }
                    break;
                case JerryAiState.Wander:
                    ((Game1)Game).Wander(jerryPosition, ref _jerryWnaderDirectionOne, ref _jerryOrientationOne,
                        JerryTurnSpeedOne, clientBounds);
                    currentJerrySpeed = (int)(.25f * MaxJerrySpeedOne);
                    break;
                case JerryAiState.Caught:
                    var jerryRect = new Rectangle((int)jerryPosition.X - _jerryTextureOne.Width / 2,
                        (int)jerryPosition.Y - _jerryTextureOne.Height / 2, _jerryTextureOne.Width,
                        _jerryTextureOne.Height);
                    int aa = 0;

                    if (jerryRect.Contains((int)cheesePosition.X, (int)cheesePosition.Y))
                    {
                        aa++;
                        JerrySpriteOne.RemoveLength += aa;
                        removeList.AddRange(
                            cheeseFlakes.Select(
                                cheeseflake =>
                                    new
                                    {
                                        cheeseflake,
                                        jerryRect =
                                            new Rectangle((int)jerryPosition.X - _jerryTextureOne.Width / 2,
                                                (int)jerryPosition.Y - _jerryTextureOne.Height / 2,
                                                _jerryTextureOne.Width,
                                                _jerryTextureOne.Height)
                                    })
                                .Where(
                                    @t =>
                                        @t.jerryRect.Contains((int)@t.cheeseflake.Position.X,
                                            (int)@t.cheeseflake.Position.Y))
                                .Select(@t => @t.cheeseflake));
                        foreach (JerrySpriteOne t in JerrySpriteListOne)
                        {
                            JerrySpriteOne s = t;
                            ((Game1)Game).JerryAddScore(t.ScoreValue);
                        }                       
                    }


                    foreach (CheeseSprite cheese in removeList)
                    {
                        cheeseFlakes.Remove(cheese);
                    }
                    removeList.Clear();
                    break;
            }
            var heading = new Vector2((float)Math.Cos(_jerryOrientationOne), (float)Math.Sin(_jerryOrientationOne));
            return JerryPositionOne += heading * currentJerrySpeed;
        }

        private Vector2 JerryCheeseUpdateTwo(Vector2 jerryPosition, Vector2 cheesePosition, Rectangle clientBounds,
            List<CheeseSprite> cheeseFlakes, List<CheeseSprite> removeList)
        {
            float jerryChaseThreshold = JerryChaseDistanceTwo;
            float jerryCaughtThreshold = JerryCaughtDistanceTwo;
            _jerryStateTwo = JerryAiState.Wander;
            switch (_jerryStateTwo)
            {
                case JerryAiState.Wander:
                    jerryChaseThreshold -= JerryHysteresisTwo / 2;
                    break;
                case JerryAiState.Evading:
                    jerryChaseThreshold += JerryHysteresisTwo / 2;
                    jerryCaughtThreshold -= JerryHysteresisTwo / 2;
                    break;
                case JerryAiState.Caught:
                    jerryCaughtThreshold += JerryHysteresisTwo / 2;
                    break;
            }
            float distanceFromCheese = Vector2.Distance(jerryPosition, cheesePosition);
            if (distanceFromCheese > jerryChaseThreshold)
            {
                _jerryStateTwo = JerryAiState.Wander;
            }
            else if (distanceFromCheese > jerryCaughtThreshold)
            {
                _jerryStateTwo = JerryAiState.Evading;
            }
            else
            {
                _jerryStateTwo = JerryAiState.Caught;
            }

            int currentJerrySpeed = 0;

            switch (_jerryStateTwo)
            {
                case JerryAiState.Evading:
                    {
                        Vector2 seekPosition = 2 * cheesePosition - jerryPosition;
                        //chasing
                        _jerryOrientationTwo = Game1.TurnToFace(jerryPosition, seekPosition, _jerryOrientationTwo,
                            JerryTurnSpeedTwo);
                        currentJerrySpeed = (int)MaxJerrySpeedTwo;
                    }
                    break;
                case JerryAiState.Wander:
                    ((Game1)Game).Wander(jerryPosition, ref _jerryWnaderDirectionTwo, ref _jerryOrientationTwo,
                        JerryTurnSpeedTwo, clientBounds);
                    currentJerrySpeed = (int)(.25f * MaxJerrySpeedTwo);
                    break;
                case JerryAiState.Caught:
                    var jerryRect = new Rectangle((int)jerryPosition.X - _jerryTextureTwo.Width / 2,
                        (int)jerryPosition.Y - _jerryTextureTwo.Height / 2, _jerryTextureTwo.Width,
                        _jerryTextureTwo.Height);
                    int aa = 0;

                    if (jerryRect.Contains((int)cheesePosition.X, (int)cheesePosition.Y))
                    {
                        aa++;
                        JerrySpriteTwo.RemoveLength += aa;
                        removeList.AddRange(
                            cheeseFlakes.Select(
                                cheeseflake =>
                                    new
                                    {
                                        cheeseflake,
                                        jerryRect =
                                            new Rectangle((int)jerryPosition.X - _jerryTextureTwo.Width / 2,
                                                (int)jerryPosition.Y - _jerryTextureTwo.Height / 2,
                                                _jerryTextureTwo.Width,
                                                _jerryTextureTwo.Height)
                                    })
                                .Where(
                                    @t =>
                                        @t.jerryRect.Contains((int)@t.cheeseflake.Position.X,
                                            (int)@t.cheeseflake.Position.Y))
                                .Select(@t => @t.cheeseflake));
                        foreach (JerrySpriteTwo t in JerrySpriteListTwo)
                        {
                            JerrySpriteTwo s = t;
                            ((Game1)Game).JerryAddScore(t.ScoreValue);
                        }                       
                    }


                    foreach (CheeseSprite cheese in removeList)
                    {
                        cheeseFlakes.Remove(cheese);
                    }
                    removeList.Clear();
                    break;
            }
            var heading = new Vector2((float)Math.Cos(_jerryOrientationTwo), (float)Math.Sin(_jerryOrientationTwo));
            return JerryPositionTwo += heading * currentJerrySpeed;
        }

        private Vector2 JerryCheeseUpdateThree(Vector2 jerryPosition, Vector2 cheesePosition, Rectangle clientBounds,
           List<CheeseSprite> cheeseFlakes, List<CheeseSprite> removeList)
        {
            float jerryChaseThreshold = JerryChaseDistanceThree;
            float jerryCaughtThreshold = JerryCaughtDistanceThree;
            _jerryStateThree = JerryAiState.Wander;
            switch (_jerryStateThree)
            {
                case JerryAiState.Wander:
                    jerryChaseThreshold -= JerryHysteresisThree / 2;
                    break;
                case JerryAiState.Evading:
                    jerryChaseThreshold += JerryHysteresisThree / 2;
                    jerryCaughtThreshold -= JerryHysteresisThree / 2;
                    break;
                case JerryAiState.Caught:
                    jerryCaughtThreshold += JerryHysteresisThree / 2;
                    break;
            }
            float distanceFromCheese = Vector2.Distance(jerryPosition, cheesePosition);
            if (distanceFromCheese > jerryChaseThreshold)
            {
                _jerryStateThree = JerryAiState.Wander;
            }
            else if (distanceFromCheese > jerryCaughtThreshold)
            {
                _jerryStateThree = JerryAiState.Evading;
            }
            else
            {
                _jerryStateThree = JerryAiState.Caught;
            }

            int currentJerrySpeed = 0;

            switch (_jerryStateThree)
            {
                case JerryAiState.Evading:
                    {
                        Vector2 seekPosition = 2 * cheesePosition - jerryPosition;
                        //chasing
                        _jerryOrientationThree = Game1.TurnToFace(jerryPosition, seekPosition, _jerryOrientationThree,
                            JerryTurnSpeedThree);
                        currentJerrySpeed = (int)MaxJerrySpeedThree;
                    }
                    break;
                case JerryAiState.Wander:
                    ((Game1)Game).Wander(jerryPosition, ref _jerryWnaderDirectionThree, ref _jerryOrientationThree,
                        JerryTurnSpeedThree, clientBounds);
                    currentJerrySpeed = (int)(.25f * MaxJerrySpeedThree);
                    break;
                case JerryAiState.Caught:
                    var jerryRect = new Rectangle((int)jerryPosition.X - _jerryTextureThree.Width / 2,
                        (int)jerryPosition.Y - _jerryTextureThree.Height / 2, _jerryTextureThree.Width,
                        _jerryTextureThree.Height);
                    int aa = 0;

                    if (jerryRect.Contains((int)cheesePosition.X, (int)cheesePosition.Y))
                    {
                        aa++;
                        JerrySpriteThree.RemoveLength += aa;
                        removeList.AddRange(
                            cheeseFlakes.Select(
                                cheeseflake =>
                                    new
                                    {
                                        cheeseflake,
                                        jerryRect =
                                            new Rectangle((int)jerryPosition.X - _jerryTextureThree.Width / 2,
                                                (int)jerryPosition.Y - _jerryTextureThree.Height / 2,
                                                _jerryTextureThree.Width,
                                                _jerryTextureThree.Height)
                                    })
                                .Where(
                                    @t =>
                                        @t.jerryRect.Contains((int)@t.cheeseflake.Position.X,
                                            (int)@t.cheeseflake.Position.Y))
                                .Select(@t => @t.cheeseflake));
                        foreach (JerrySpriteThree t in JerrySpriteListThree)
                        {
                            JerrySpriteThree s = t;
                            ((Game1)Game).JerryAddScore(t.ScoreValue);
                        }                        
                    }


                    foreach (CheeseSprite cheese in removeList)
                    {
                        cheeseFlakes.Remove(cheese);
                    }
                    removeList.Clear();
                    break;
            }
            var heading = new Vector2((float)Math.Cos(_jerryOrientationThree), (float)Math.Sin(_jerryOrientationThree));
            return JerryPositionThree += heading * currentJerrySpeed;
        }

        private void SpawnEnemy()
        {
            Vector2 speedVector2 = Vector2.Zero;
            Vector2 positionVector2 = Vector2.Zero;
            //Default framesize
            Point frameSize = new Point(75, 75);

            // Randomly choose which side of the screen to place enemy,
            // then randomly create a position along that side of the screen
            // and randomly choose a speed for the enemy
            switch (((Game1)Game).Rnd.Next(4))
            {
                case 0: // LEFT to RIGHT
                    positionVector2 = new Vector2(
                        -frameSize.X, ((Game1)Game).Rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));
                    speedVector2 = new Vector2(((Game1)Game).Rnd.Next(
                        enemyMinSpeed,
                        enemyMaxSpeed), 0);
                    break;
                case 1: // RIGHT to LEFT
                    positionVector2 = new
                        Vector2(
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                        ((Game1)Game).Rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));

                    speedVector2 = new Vector2(-((Game1)Game).Rnd.Next(
                        enemyMinSpeed, enemyMaxSpeed), 0);
                    break;
                case 2: // BOTTOM to TOP
                    positionVector2 = new Vector2(((Game1)Game).Rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X),
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

                    speedVector2 = new Vector2(0,
                        -((Game1)Game).Rnd.Next(enemyMinSpeed,
                        enemyMaxSpeed));
                    break;
                case 3: // TOP to BOTTOM
                    positionVector2 = new Vector2(((Game1)Game).Rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X), -frameSize.Y);

                    speedVector2 = new Vector2(0,
                        ((Game1)Game).Rnd.Next(enemyMinSpeed,
                        enemyMaxSpeed));
                    break;
            }
            int random = ((Game1)Game).Rnd.Next(100);
            if (random < likelihoodAutomated)
            {
                // Create an AutomatedSprite.
                // Get new random number to determine whether to
                // create a three-blade or four-blade sprite.
                if (((Game1)Game).Rnd.Next(2) == 0)
                {
                    // Create a four-blade enemy
                    //SpriteList.Add(new CheeseSprite(cheeseTexture, new Vector2(100, 50), new Point(75, 75), 10, new Point(0, 0), new Point(6, 8), Vector2.Zero, Game.Window.ClientBounds));
                    int snowflakeCount = 3;                   
                    cheeseList.Add(
                        new CheeseSprite(
                            Game.Content.Load<Texture2D>("cheese"),
                            positionVector2, new Point(50, 26), 10, new Point(0, 0),
                        // positionVector2, new Point(45, 35), 10, new Point(0, 0),
                            new Point(1, 1), speedVector2, "Cheese",
                            CheeseSpritePointValue));
                }                
            }
        }

        private void CheckPowerUpExpirationOne(GameTime gameTime, JerrySpriteOne js)
        {
            // Is a power-up active?
            if (powerUpExpiration > 0)
            {
                // Decrement power-up timer
                powerUpExpiration -=
                    gameTime.ElapsedGameTime.Milliseconds;
                if (powerUpExpiration <= 0)
                {
                    // If power-up timer has expired, end all power-ups
                    powerUpExpiration = 0;
                    //   _objJerry.ResetScale();
                    js.ResetScale();
                }
            }
        }

        private void CheckPowerUpExpirationTwo(GameTime gameTime, JerrySpriteTwo js)
        {
            // Is a power-up active?
            if (powerUpExpiration > 0)
            {
                // Decrement power-up timer
                powerUpExpiration -=
                    gameTime.ElapsedGameTime.Milliseconds;
                if (powerUpExpiration <= 0)
                {
                    // If power-up timer has expired, end all power-ups
                    powerUpExpiration = 0;
                    //   _objJerry.ResetScale();
                    js.ResetScale();
                }
            }
        }

        private void CheckPowerUpExpirationThree(GameTime gameTime, JerrySpriteThree js)
        {
            // Is a power-up active?
            if (powerUpExpiration > 0)
            {
                // Decrement power-up timer
                powerUpExpiration -=
                    gameTime.ElapsedGameTime.Milliseconds;
                if (powerUpExpiration <= 0)
                {
                    // If power-up timer has expired, end all power-ups
                    powerUpExpiration = 0;
                    //   _objJerry.ResetScale();
                    js.ResetScale();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);           
            switch (Game1._state)
            {
                case GameState.Play:
                    foreach (ExplosionSprite explosion in explosionList)
                    {                       
                        spriteBatch.Draw(explosionspritestrip, explosion._drawRectangle, explosion._sourceRectangle, Color.White);
                    }
                    spriteBatch.Draw(_tomSprite, _playerControlledSprite._drawRectangle, _playerControlledSprite._sourceRectangle, Color.White);

                    foreach (Sprite sprite in livesList)
                        sprite.Draw(gameTime, spriteBatch);

                    foreach (DogSprite sp in listdDogSprites)
                    {
                        spriteBatch.Draw(_dogSprite, sp.GetPosition, null, Color.White, _dogOrientation, _dogTextureCenter, 1.0f, SpriteEffects.None, 0.0f);
                    }
                    for (int i = 0; i < JerrySpriteListOne.Count; i++)
                    {
                        JerrySpriteOne js = JerrySpriteListOne[i];

                        spriteBatch.Draw(_jerryTextureOne, js.GetPosition, null, Color.White, _jerryOrientationOne,
                            _jerryTextureCenterOne, js.Scale, SpriteEffects.None, 0);
                    }
                    for (int i = 0; i < JerrySpriteListTwo.Count; i++)
                    {
                        JerrySpriteTwo js = JerrySpriteListTwo[i];
                        spriteBatch.Draw(_jerryTextureTwo, js.GetPosition, null, Color.White, _jerryOrientationTwo,
                            _jerryTextureCenterTwo, js.Scale, SpriteEffects.None, 0);
                    }
                    for (int i = 0; i < JerrySpriteListThree.Count; i++)
                    {
                        JerrySpriteThree js = JerrySpriteListThree[i];
                        spriteBatch.Draw(_jerryTextureThree, js.GetPosition, null, Color.White, _jerryOrientationThree,
                            _jerryTextureCenterThree, js.Scale, SpriteEffects.None, 0);
                    }
                   
                    foreach (CheeseSprite cheeseflake in cheeseList)
                    {
                        cheeseflake.Draw(gameTime, spriteBatch);
                    }                   
                    break;
            }          
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
