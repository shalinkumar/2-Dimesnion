using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TomJerryChasing
{
    public class Explosion
    {
        #region fields

        private  bool active = true;
        private static Texture2D explosionTexture2D;
        private static Rectangle _sourceRectangle;
        private static Rectangle _drawRectangle;
        private static int _frameWidth;
        private static int _frameHeight;
        private static int _currentFrame;
        private const int Frametime = 10;
        private const int FramesPerRow = 3;
        private const int NumRows = 3;
        private const int NumFrames = 9;
        private static int _elapsedFrametime;
        #endregion

        #region Constructor

        public Explosion()
        {

        }

        public Explosion(Texture2D spriteTexture2D, int x, int y)
        {
            explosionTexture2D = spriteTexture2D;
            Initialize();
            Play(x, y);
        }

        #endregion

        #region private methods

        private static void Initialize()
        {
            _frameWidth = explosionTexture2D.Width / FramesPerRow;
            _frameHeight = explosionTexture2D.Height / NumRows;

            _drawRectangle = new Rectangle(0, 0, _frameWidth, _frameHeight);
            _sourceRectangle = new Rectangle(0, 0, _frameWidth, _frameHeight);
        }

        private static void Play(int x, int y)
        {
            _currentFrame = 0;
            //_elapsedFrametime = 0;
            _drawRectangle.X = x-_frameWidth/2;
            _drawRectangle.Y = y - _frameHeight/2;
            SourceRectangleLocation(_currentFrame);
        }

        private static void SourceRectangleLocation(int framenumber)
        {
            _sourceRectangle.X = (framenumber%FramesPerRow)*_frameWidth;
            _sourceRectangle.Y = (framenumber/FramesPerRow)*_frameHeight;
        }

        #endregion

        #region properties

        public bool Active
        {
            get { return active; }           
        }

        #endregion

        #region Public Methods

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(explosionTexture2D, _drawRectangle, _sourceRectangle, Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (active)
            {
                _elapsedFrametime = gameTime.ElapsedGameTime.Milliseconds;
                if (_elapsedFrametime > Frametime)
                {
                    _elapsedFrametime = 0;
                    if (_currentFrame < NumFrames - 1)
                    {
                        _currentFrame++;
                        SourceRectangleLocation(_currentFrame);
                    }
                    else
                    {
                        active = false;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}
