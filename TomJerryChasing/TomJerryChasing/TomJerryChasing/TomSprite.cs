using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TomJerryChasing
{
    public class TomSprite : Sprite
    {
        public enum Side { Left, Right }
        private Side _front;
        private double TOLERANCE = 0;
        public Rectangle _drawRectangle;
        public Rectangle _sourceRectangle;
        public Vector2 width;
        public Texture2D _textureImage;
        //how fast the tom can move
        private const int MaxTomSpeed = 8;
        private Point _frameSize;       
        public TomSprite()
            : base()
        {

        }

        public TomSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet,
             Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue)
            : base(
                textureImage, position, frameSize, collisionOffSet, currentFrame, sheetSize, speed, collisionCueName,
                scoreValue)
        {
            this._frameSize = frameSize;           
        }
      
        //get the collisionrectangle for the tom
        public Rectangle CollisionRectangle
        {
            get { return _drawRectangle; }
        }

        public override Vector2 Direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    inputDirection.X -= 1;
                    X -= MaxTomSpeed;
                    _sourceRectangle.X = 0;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    inputDirection.X += 1;
                    X += MaxTomSpeed;
                    _sourceRectangle.X = _frameSize.X;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    inputDirection.Y -= 1;
                    Y -= MaxTomSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    inputDirection.Y += 1;
                    Y += MaxTomSpeed;
                }
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (Math.Abs(gamePadState.ThumbSticks.Left.X) > TOLERANCE)
                {
                    inputDirection.X += gamePadState.ThumbSticks.Left.X;
                }
                if (Math.Abs(gamePadState.ThumbSticks.Left.Y) > TOLERANCE)
                {
                    inputDirection.Y -= gamePadState.ThumbSticks.Left.Y;
                }
                return inputDirection * Speed;
            }
        }

      

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //move the sprite based on direction
            Position += Direction;
            _ClientBounds.Width = clientBounds.Width;
            _ClientBounds.Height = clientBounds.Height;
           
            //if sprite is off the screen, move it back within the game window
            if (Position.X < 0)
            {
                Position.X = 0;
            }
            if (Position.Y < 0)
            {
                Position.Y = 0;
            }
            if (Position.X > clientBounds.Width - FrameSize.X)
            {
                Position.X = clientBounds.Width - FrameSize.X;
            }
            if (Position.Y > clientBounds.Height - FrameSize.Y)
            {
                Position.Y = clientBounds.Height - FrameSize.Y;
                
            }           
            base.Update(gameTime, clientBounds);
        }


        private Rectangle _ClientBounds;

        private int X
        {
            get { return _drawRectangle.X + _drawRectangle.Width / 2; }
            set
            {
                _drawRectangle.X = value - _drawRectangle.Width / 2;
                if (_drawRectangle.Left < 0)
                {
                    _drawRectangle.X = 0;
                }
                else if (_drawRectangle.Right > _ClientBounds.Width)
                {
                    _drawRectangle.X = _ClientBounds.Width - _drawRectangle.Width;
                }
            }
        }
        private int Y
        {
            get { return _drawRectangle.Y + _drawRectangle.Height / 2; }
            set
            {
                _drawRectangle.Y = value - _drawRectangle.Height / 2;
                if (_drawRectangle.Top < 0)
                {
                    _drawRectangle.Y = 0;
                }
                else if (_drawRectangle.Bottom > _ClientBounds.Height)
                {
                    _drawRectangle.Y = _ClientBounds.Height - _drawRectangle.Height;
                }
            }
        }
    }
}
