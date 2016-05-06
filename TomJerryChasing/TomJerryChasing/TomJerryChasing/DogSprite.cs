using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TomJerryChasing
{
    public sealed class DogSprite : Sprite
    {


        public DogSprite()
            : base()
        {

        }

        public DogSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffSet,
            Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, int scoreValue)
            : base(
                textureImage, position, frameSize, collisionOffSet, currentFrame, sheetSize, speed, collisionCueName,
                scoreValue)
        {

        }

        public override Vector2 Direction
        {            
            get { return Speed; }           
        }
        public Vector2 DogDirection
        {
          
            set { Position = value; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Position += Direction;
          
           
            base.Update(gameTime, clientBounds);
        }
    }
}