using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TomJerryChasing
{
    public class MenuButton
    {
        #region Fields

        readonly Texture2D _texture2DButton;
        private const int ImagesPerRow = 2;
        int _buttonWidth;

        Rectangle _drawRectangle;
        Rectangle _sourceRectangle;

        readonly GameState _clickState;
        bool _clickStarted;
        bool _buttonReleased = true;
        #endregion

        #region Constructor

        public MenuButton()
        {

        }

        public MenuButton(Texture2D sprite, Vector2 center, GameState clickState)
        {
            _texture2DButton = sprite;
            _clickState = clickState;
            Initialize(center);
        }
        #endregion

        public void Update(MouseState mouseState, SoundBank soundBank)
        {
            if (_drawRectangle.Contains(mouseState.X, mouseState.Y))
            {
                _sourceRectangle.X = _buttonWidth;
                if (mouseState.LeftButton == ButtonState.Pressed && _buttonReleased)
                {

                    _clickStarted = true;
                    _buttonReleased = false;
                    soundBank.PlayCue("buttonClick");

                }
                else if (mouseState.LeftButton == ButtonState.Released)
                {
                    _buttonReleased = true;
                    if (_clickStarted)
                    {
                        Game1.ChangeState(_clickState); _clickStarted = false;
                    }
                }
            }
            else
            {
                _sourceRectangle.X = 0;
                _clickStarted = false;
                _buttonReleased = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture2DButton, _drawRectangle, _sourceRectangle, Color.White);
        }

        public void Initialize(Vector2 centerVector2)
        {

            _buttonWidth = _texture2DButton.Width / ImagesPerRow;
// ReSharper disable PossibleLossOfFraction
            _drawRectangle = new Rectangle(x: (int)(centerVector2.X - _buttonWidth / 2), y: (int)(centerVector2.Y - _texture2DButton.Height / 2), width: _buttonWidth, height: _texture2DButton.Height);
// ReSharper restore PossibleLossOfFraction
            _sourceRectangle=new Rectangle(0,0,_buttonWidth,_texture2DButton.Height);
        }
    }
}
