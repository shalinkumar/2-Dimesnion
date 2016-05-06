using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TomJerryChasing
{
    public abstract class Sprite
    {
        #region Variables

        private const int DefaultMillisecondsPerFrame = 16;
        private readonly int CollisionOffset;
        private readonly int MilliSecondsPerFrame;
        private readonly Texture2D TextureImage;
        private readonly Vector2 originalSpeed;
        private Point CurrentFrame;
        protected Point FrameSize;
        public Vector2 Position;
        protected float Scale = 1;
        private Point SheetSize;
        protected Vector2 Speed;
        private int TimeSinceLastFrame;
        protected float originalScale = 1;
        //Scoring
        public int ScoreValue { get; protected set; }

        #endregion

        #region Properties

        public abstract Vector2 Direction { get; }
        public string collisionCueName { get; private set; }

        public Rectangle ColliRectangle
        {
            get
            {
                return new Rectangle(
                    (int) Position.X + CollisionOffset,
                    (int) Position.Y + CollisionOffset,
                    FrameSize.X - (CollisionOffset*2), FrameSize.Y - (CollisionOffset*2));
            }
        }

        public void ModifyScale(float modifier)
        {
            Scale *= modifier;
        }

        public void ResetScale()
        {
            Scale = originalScale;
        }

        public void ModifySpeed(float modifier)
        {
            Scale *= modifier;
        }

        public void ResetSpeed()
        {
            Speed = originalSpeed;
        }

        #endregion

        public Sprite()
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet, Point currentFrame,
            Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue)
            : this(
                textureImage, position, frameSize, collisionOffSet, currentFrame, sheetSize, speed,
                DefaultMillisecondsPerFrame, collisionCueName, scoreValue)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet, Point currentFrame,
            Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue, float scale)
            : this(
                textureImage, position, frameSize, collisionOffSet, currentFrame, sheetSize, speed,
                DefaultMillisecondsPerFrame, collisionCueName, scoreValue)
        {
            Scale = scale;
        }

       
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet, Point currentFrame,
            Point sheetSize, Vector2 speed, int milliSecondsPerFrame, string collisionCueName, int scoreValue)
        {
            TextureImage = textureImage;
            Position = position;
            FrameSize = frameSize;
            CollisionOffset = collisionOffSet;
            CurrentFrame = currentFrame;
            SheetSize = sheetSize;
            Speed = speed;
            originalSpeed = speed;
            this.collisionCueName = collisionCueName;
            MilliSecondsPerFrame = milliSecondsPerFrame;
            ScoreValue = scoreValue;
        }

        public Vector2 GetPosition
        {
            get { return Position; }
        }

        public bool IsOutOfBounds(Rectangle clientRectangle)
        {
            if (Position.X < -FrameSize.X ||
                Position.X > clientRectangle.Width ||
                Position.Y < -FrameSize.Y ||
                Position.Y > clientRectangle.Height)
            {
                return true;
            }
            return false;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeSinceLastFrame > MilliSecondsPerFrame)
            {
                TimeSinceLastFrame = 0;
                ++CurrentFrame.X;
                if (CurrentFrame.X >= SheetSize.X)
                {
                    CurrentFrame.X = 0;
                    ++CurrentFrame.Y;
                    if (CurrentFrame.Y >= SheetSize.Y)
                    {
                        CurrentFrame.Y = 0;
                    }
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureImage, Position,
                new Rectangle(CurrentFrame.X*FrameSize.X, CurrentFrame.Y*FrameSize.Y, FrameSize.X, FrameSize.Y),
                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}