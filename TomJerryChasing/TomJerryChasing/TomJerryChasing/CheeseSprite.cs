using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TomJerryChasing
{
    public class CheeseSprite : Sprite
    {
        private Random Rand = new Random();
        private readonly float _angularVelocity;
        public readonly int TextureIndex;
        public Color Tint;
        private Vector2 _velocity;
        public float Rotation;
        public readonly float Scale;
        private Point _frameSize;

        public CheeseSprite()
            : base()
        {

        }

        public override Vector2 Direction
        {
            get { return Speed; }
        }
        public virtual Vector2 CheeseDirection
        {
            set { Position = value; }
            get { return Speed; }
        }

        public CheeseSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffSet, currentFrame, sheetSize, speed, collisionCueName, scoreValue)
        {
            Position = position;
            _velocity = new Vector2((float)Rand.NextDouble() * 2.0f - 1.0f, (float)Rand.NextDouble() * 2.0f - 1.0f);
            int Speed = (Rand.Next(4) + 1);
            double Angle = 2 * Math.PI * Rand.NextDouble();

            double xDirection = (float)Math.Cos(Angle);
            double yDirection = -1 * (float)Math.Sin(Angle);

            _velocity.X = (float)(Speed * xDirection);
            _velocity.Y = (float)(Speed * yDirection);
            this._frameSize = frameSize;
        }




        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {           
            Position += _velocity;
            if (Position.X < 0)
            {
                _velocity.X = -_velocity.X;
                Position.X = 0f;
            }
            else if (Position.X > clientBounds.Width)
            {
                _velocity.X = -_velocity.X;
                Position.X = clientBounds.Width;
            }

            if (Position.Y < 0)
            {
                _velocity.Y = -_velocity.Y;
                Position.Y = 0f;
            }
            else if (Position.Y > clientBounds.Height)
            {
                _velocity.Y = -_velocity.Y;
                Position.Y = clientBounds.Height;
            }
            Rotation += _angularVelocity;
            base.Update(gameTime, clientBounds);
        }
       
    }
}
