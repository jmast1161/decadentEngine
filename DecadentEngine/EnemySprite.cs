using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DecadentEngine
{
    public class EnemySprite : IGameObject, ISimpleSprite
    {
        public const int MAX_DISPLACEMENT = 1000;
        public const int MIN_DISPLACEMENT = 100;
        private const int DISPLACEMENT = 50;
        private Vector2 location;
        public RunGraphic RunGraphic { get; private set; }
        private SpriteEffects spriteEffects;
        private bool right = true;
        private int update = 0;
        
        public EnemySprite(RunGraphic enemyGraphic, Vector2 startLocation)
        {
            RunGraphic = enemyGraphic;
            spriteEffects = SpriteEffects.FlipHorizontally;
            location = startLocation;
        }

        public Rectangle GetRectangle()
        {
            var sourceRectangle = RunGraphic.GetRectangle();
            return new Rectangle((int)location.X, (int)location.Y, sourceRectangle.Width, sourceRectangle.Height);
        }
        
        public void Update(int gameTime, Dictionary<Rectangle, Common.CollisionSide> collTiles)
        {
            bool colliding = false;
            bool leftColl = false;
            bool rightColl = false;
            foreach (var collTile in collTiles)
            {
                switch (collTile.Value)
                {
                    case Common.CollisionSide.Top:
                        break;
                    case Common.CollisionSide.Bottom:
                        colliding = true;
                        break;
                    case Common.CollisionSide.Left:
                        leftColl = true;
                        break;
                    case Common.CollisionSide.Right:
                        rightColl = true;
                        break;
                }
            }

            if (!colliding)
            {
                location.Y = PhysicsEngine.ApplyGravity(gameTime, location.Y);
            }

            if (update == 0)
            {
                if (right && location.X < MAX_DISPLACEMENT && !rightColl)
                {
                    location.X += DISPLACEMENT;
                }
                else if (!right && location.X > MIN_DISPLACEMENT && !leftColl)
                {
                    location.X -= DISPLACEMENT;
                }
                else
                {
                    right = !right;
                    if (right)
                    {
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        spriteEffects = SpriteEffects.None;
                    }
                }

                RunGraphic.Update();
                ++update;
            }
            else if (update == 4)
            {
                update = 0;
            }
            else
            {
                ++update;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var texture = RunGraphic.GetTexture();
            Rectangle sourceRectangle = RunGraphic.GetRectangle();
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, sourceRectangle.Width, sourceRectangle.Height);            
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(0, 0), spriteEffects, 0.0f);            
        }
    }
}
