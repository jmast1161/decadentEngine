using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecadentEngine
{
    public class ScrollingBackground : IGameObject
    {
        private const int SCROLL_AMOUNT = 20;

        //don't do this
        public Texture2D texture;
        public Rectangle rectangle;

        public ScrollingBackground(Texture2D newTexture, Rectangle newRectangle)
        {
            texture = newTexture;
            rectangle = newRectangle;
        }

        public void Update(AnimatedSprite sprite)
        {
            if (sprite.IsSpriteMoving())
            {
                if (sprite.GetRectangle().X == AnimatedSprite.MAX_DISPLACEMENT)
                {
                    rectangle.X -= SCROLL_AMOUNT;
                }
                else if (sprite.GetRectangle().X == AnimatedSprite.MIN_DISPLACEMENT)
                {
                    rectangle.X += SCROLL_AMOUNT;
                }
            }            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
