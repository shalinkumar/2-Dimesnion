using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TomJerryChasing
{
    public sealed class JerrySpriteOne : Sprite
    {
        public static int RemoveLength;
        public float Scale = 0.9f;
        private float originalScale = 0.9f;

        public JerrySpriteOne()
        {
        }

        public JerrySpriteOne(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet,
            Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue, float scale)
            : base(
                textureImage, position, frameSize, collisionOffSet, currentFrame, sheetSize, speed, collisionCueName,
                scoreValue)
        {
            Scale = scale;
        }

        public override Vector2 Direction
        {
            get { return Speed; }
        }

        public Vector2 JerryDirection
        {
            set { Position = value; }
        }

        public Vector2 JerryDirectionone
        {
            set { Position = value; }
        }

        public void ResetScale()
        {
            Scale = originalScale;
        }

        public void ModifySpeed(float modifier)
        {
            Scale *= modifier;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Position += Direction;
            base.Update(gameTime, clientBounds);
        }
    }
}