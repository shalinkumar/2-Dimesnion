#region File Description

#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion


namespace TomJerryChasing
{
    public class Dog
    {
        private readonly int _windowWidth;
        private readonly int _windowHeight;
        private readonly int _x;
        private readonly int _y;

        enum DogAiState
        {
            Chasing, Caught,
            Wander
        }
        int _Health = 100;
        int _healthIncrement;

        #region Constants


        const int Damage = 25;
        //how fast the dog can move
        const float MaxDogSpeed = 6.5f;

        //how fast the dog can move..
        const float DogTurnSpeed = 0.20f;

        //this value controls the distance at  which the dog will start to chase Tom
        const float DogChaseDistance = 250f;

        //dogcaught distance controls the distance at which the dog will stop  because he has caught Tom
        const float DogCaughtDistance = 70.0f;

        //this content is used to avoid hysterisis,this is commmon in all AI programming
        const float DogHysterisis = 15.0f;
        #endregion

        #region Fields






        Texture2D _dogTexture;
        Vector2 _dogTextureCenter;
        Vector2 _dogPosition;
        DogAiState _dogState = DogAiState.Wander;
        float _dogOrientation;
        Vector2 _dogwanderDirection;


        readonly Random _random = new Random();

        #endregion

        #region Constructor
        public Dog()
        {

        }

        public Dog(ContentManager contentManager, string spriteName, int windowWidth, int windowHeight, int x, int y)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            _x = x;
            _y = y;

            DogLoadContent(contentManager, spriteName);

        }

        #endregion

        #region Initilization


        /// <summary>
        /// Once the GraphicsDevice is setup,
        /// we'll use the viewport to initialize some values. call these in main game1 initialize
        /// </summary>
        public void DogInitialize(Viewport vp)
        {
            // ReSharper disable PossibleLossOfFraction
            _dogPosition = new Vector2(x: vp.Width / 4, y: -1 * vp.Height / 2);
            // ReSharper restore PossibleLossOfFraction

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch == null) throw new ArgumentNullException("spriteBatch");
            spriteBatch.Draw(_dogTexture, _dogPosition, null, Color.White, _dogOrientation, _dogTextureCenter, 1.0f, SpriteEffects.None, 0.0f);
        }

        #endregion

        #region public methods

        public bool UpdateDog(Viewport vp, Vector2 tomPositions, SoundBank _soundBank)
        {
            bool active = true;

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
            //Vector2 tompost=objToms.TomPosition(tomPosition);
            var distanceFromCat = Vector2.Distance(_dogPosition, tomPositions);
            if (!(distanceFromCat > dogChaseThreshold))
            {
                _dogState = distanceFromCat > dogCaughtThreshold ? DogAiState.Chasing : DogAiState.Caught;
            }
            else
            {
                _dogState = DogAiState.Wander;
            }

            var currentDogSpeed = 0.0f;
            switch (_dogState)
            {
                case DogAiState.Chasing:
                    _dogOrientation = TurnToFace(_dogPosition, tomPositions, _dogOrientation, DogTurnSpeed);
                    currentDogSpeed = MaxDogSpeed;
                    break;
                case DogAiState.Wander:
                    Wander(_dogPosition, ref _dogwanderDirection, ref _dogOrientation, DogTurnSpeed, vp);
                    currentDogSpeed = .25f * MaxDogSpeed;
                    break;
                //default:
                case  DogAiState.Caught:

                    //var objToms=new Tom(Damage);
                    //objToms.Takedamage();

                    _Health -= Damage;
                    if (_Health == 75)
                    {                       
                        //currentDogSpeed = 0.0f;
                        //_Health = 0;
                        //var objToms = new Tom();
                        //objToms.Takedamage(false);
                        //active = false;
                    }else if (_Health == 0)
                    {
                        active = false;
                    }
                    break;
            }
            var heading = new Vector2((float)Math.Cos(_dogOrientation), (float)Math.Sin(_dogOrientation));
            _dogPosition += heading * currentDogSpeed;

            // Once we've finished that, we'll use the ClampToViewport helper function
            // to clamp everyone's position so that they stay on the screen.
            _dogPosition = ClampToviewport(_dogPosition, vp);
            return active;
        }


        //public Viewport Resolution(Viewport Vp)
        //{
        //    return Vp;
        //}
        #endregion

        #region private methods
        private void DogLoadContent(ContentManager contentManager, string spritename)
        {
            _dogTexture = contentManager.Load<Texture2D>(spritename);

            //drawRectangle.Width = dogTexture.Width; ;
            //drawRectangle.Height = dogTexture.Height;


            // ReSharper disable PossibleLossOfFraction
            _dogTextureCenter = new Vector2(x: _dogTexture.Width / 2, y: _dogTexture.Height / 2);
            // ReSharper restore PossibleLossOfFraction
        }


        /// <summary>
        /// This function takes a Vector2 as input, and returns that vector "clamped"
        /// to the current graphics viewport. We use this function to make sure that 
        /// no one can go off of the screen.
        /// </summary>
        /// <param name="vector">an input vector</param>
        /// <param name="vp"></param>
        /// <returns>the input vector, clamped between the minimum and maximum of the
        /// viewport.</returns>
        private Vector2 ClampToviewport(Vector2 vector, Viewport vp)
        {
            vector.X = MathHelper.Clamp(vector.X, vp.X, vp.X + vp.Width);
            vector.Y = MathHelper.Clamp(vector.Y, vp.Y, vp.Y + vp.Height);
            return vector;
        }
        private void Wander(Vector2 position, ref Vector2 wanderDirection,
         ref float orientation, float turnSpeed, Viewport vp)
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
                MathHelper.Lerp(-.25f, .25f, (float)_random.NextDouble());
            wanderDirection.Y +=
                MathHelper.Lerp(-.25f, .25f, (float)_random.NextDouble());

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
            screenCenter.X = vp.Width / 2;
            // ReSharper restore PossibleLossOfFraction
            // ReSharper disable PossibleLossOfFraction
            screenCenter.Y = vp.Height / 2;
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
            var distanceFromScreenCenter = Vector2.Distance(screenCenter, position);
            var maxDistanceFromScreenCenter =
                Math.Min(screenCenter.Y, screenCenter.X);

            var normalizedDistance =
                distanceFromScreenCenter / maxDistanceFromScreenCenter;

            var turnToCenterSpeed = .3f * normalizedDistance * normalizedDistance *
                turnSpeed;

            // once we've calculated how much we want to turn towards the center, we can
            // use the TurnToFace function to actually do the work.
            orientation = TurnToFace(position, screenCenter, orientation,
                turnToCenterSpeed);
        }


        private static float TurnToFace(Vector2 position, Vector2 faceThis, float currentAngle, float turnSpeed)
        {
            var x = faceThis.X - position.X;
            var y = faceThis.Y - position.Y;

            var desiredAngle = (float)Math.Atan2(y, x);

            var difference = WrapAngle(desiredAngle - currentAngle);

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
        #endregion

    }
}
