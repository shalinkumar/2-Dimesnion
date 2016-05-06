using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TomJerryChasing
{
    public class ExplosionSprite : Sprite
    {
        #region fields

        private bool active = true;
        private static Texture2D explosionTexture2D;
        public  Rectangle _sourceRectangle;
        public  Rectangle _drawRectangle;
        private static int _frameWidth;
        private static int _frameHeight;
        private static int _currentFrame;
        private const int Frametime = 10;
        private const int FramesPerRow = 3;
        private const int NumRows = 3;
        private const int NumFrames = 9;
        private static int _elapsedFrametime;
        //playing or not
        bool playing = false;
        #endregion

        public override Vector2 Direction
        {
            get { return Speed; }
        }
         
        public bool Active
        {
            get { return active; }
        }

         public ExplosionSprite()
            : base()
        {

        }

      
         public ExplosionSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet,
              Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue, int x, int y)
             : base(
               textureImage, position, frameSize, collisionOffSet, currentFrame, sheetSize, speed, collisionCueName,
               scoreValue)
         {
            
            explosionTexture2D = textureImage;

            _frameWidth = explosionTexture2D.Width / FramesPerRow;
            _frameHeight = explosionTexture2D.Height / NumRows;

            _drawRectangle = new Rectangle(0, 0, _frameWidth, _frameHeight);
            _sourceRectangle = new Rectangle(0, 0, _frameWidth, _frameHeight);

           
             var drawX = (int) position.X;
             var drawY = (int)position.Y;
             Play(drawX, drawY);

         }

        private void Play(int x, int y)
        {
            //reset the tracking values
            playing = true;
            _currentFrame = 0;
            _elapsedFrametime = 0;
            _drawRectangle.X = x - _frameWidth / 2;
            _drawRectangle.Y = y - _frameHeight / 2;
            SourceRectangleLocation(_currentFrame);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
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
            }          
            base.Update(gameTime, clientBounds);
        }

        private  void SourceRectangleLocation(int framenumber)
        {
            _sourceRectangle.X = (framenumber % FramesPerRow) * _frameWidth;
            _sourceRectangle.Y = (framenumber / FramesPerRow) * _frameHeight;
        }
    }
}
