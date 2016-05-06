using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TomJerryChasing
{
    public class Menu
    {
        #region Field
        private MenuButton _quitButton;
        private MenuButton _startButton;
        #endregion

        public Menu(ContentManager contentManager, int windowWidth, int windowHeight)
        {
            var centerx = windowWidth / 2;
            var topCentery = windowHeight / 4;

            var buttonCenter = new Vector2(centerx, topCentery);
            _startButton = new MenuButton(contentManager.Load<Texture2D>("playbutton"), buttonCenter, GameState.GameContent);
// ReSharper disable PossibleLossOfFraction
            buttonCenter.Y += windowWidth / 5;
// ReSharper restore PossibleLossOfFraction
            _quitButton = new MenuButton(contentManager.Load<Texture2D>("quitbutton"), buttonCenter, GameState.Quit);

        }
      
        public void Update(MouseState mouseState, SoundBank soundBank)
        {
            _startButton.Update(mouseState, soundBank);
            _quitButton.Update(mouseState,soundBank);          
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _startButton.Draw(spriteBatch);
            _quitButton.Draw(spriteBatch);
          
        }
    }
}
