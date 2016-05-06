using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TomJerryChasing
{
    public class AutomatedSprite : Sprite
    {
        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffSet, currentFrame, sheetSize, speed, collisionCueName, scoreValue)
        {
        }
        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue, float scale)
            : base(textureImage, position, frameSize, collisionOffSet, currentFrame, sheetSize, speed, collisionCueName, scoreValue, scale)
        {
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet, Point currentFrame, Point sheetSize, Vector2 speed, int milliSecondsPerFrame, string collisionCueName, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffSet, currentFrame, sheetSize, speed, milliSecondsPerFrame, collisionCueName, scoreValue)
        {
        }

        public override Vector2 Direction
        {
            get { return Speed; }
        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Position += Direction;

            base.Update(gameTime, clientBounds);
        }
    }
}
