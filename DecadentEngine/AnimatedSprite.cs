using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DecadentEngine
{
    public class AnimatedSprite : IGameObject, ISimpleSprite
    {
        public const int MAX_DISPLACEMENT = 1000;
        public const int MIN_DISPLACEMENT = 100;
        private const int DISPLACEMENT = 50;
        private SpriteEffects spriteEffects;
        private bool _direction = true;
        private Vector2 location;
        public bool right = true;
        private float originalY;
        private double spriteVelocity = 0;
        public JumpGraphic JumpGraphic { get; private set; }
        public RunGraphic RunGraphic { get; private set; }        
        private bool directionPressed = false;
        private bool spriteOnMap = false;

        public AnimatedSprite(RunGraphic runGraphic, JumpGraphic jumpGraphic, Vector2 startLocation)
        {
            JumpGraphic = jumpGraphic;
            RunGraphic = runGraphic;
            originalY = startLocation.Y;
            location = startLocation;
        }
        
        public bool IsSpriteMoving()
        {
            return directionPressed;
        }

        public bool IsSpriteOnMap()
        {
            return spriteOnMap;
        }

        public Rectangle GetRectangle()
        {
            Rectangle sourceRectangle;
            if (JumpGraphic.jumping)
            {
                sourceRectangle = JumpGraphic.GetRectangle();
            }
            else
            {
                sourceRectangle = RunGraphic.GetRectangle();
            }
            
            return new Rectangle((int)location.X, (int)location.Y - 60, sourceRectangle.Width, sourceRectangle.Height);
        }

        public void Update(int gameTime, Dictionary<Rectangle, Common.CollisionSide> collTiles)
        {
            bool colliding = false;
            bool sideColl = false;
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
                        sideColl = true;
                        break;
                    case Common.CollisionSide.Right:
                        sideColl = true;
                        break;
                }
            }

            directionPressed = false;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                directionPressed = true;
                FlipSprite(true);
                right = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                directionPressed = true;
                FlipSprite(false);
                right = false;
            }

            if ((Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up)) &&  !JumpGraphic.jumping)
            {
                JumpGraphic.StartJump();
            }

            if (!colliding)
            {
                location.Y = PhysicsEngine.ApplyGravity(gameTime, location.Y);
                spriteOnMap = false;
            }
            else
            {
                spriteOnMap = true;
            }

            if (JumpGraphic.jumping)
            {
                if (JumpGraphic.IsJumpStarted())
                {
                    spriteVelocity += JumpGraphic.JUMP_VELOCITY;
                }

                JumpGraphic.Update();
                
                double updateTime = gameTime;
                double timeScalar = updateTime / 75.0;
                
                var velocity = PhysicsEngine.ACCEL_GRAVITY * timeScalar;
                if (JumpGraphic.IsJumpAtPeak())
                {
                    spriteVelocity = 0;    
                }

                spriteVelocity -= velocity;
                var position = spriteVelocity * timeScalar;
                                
                var newY = location.Y - (float)position;
                location.Y = newY;
                                
                if (JumpGraphic.IsJumpFinished())
                {
                    RunGraphic.InitGraphic();
                    JumpGraphic.InitGraphic();
                    spriteVelocity = 0;
                    location.Y = originalY;
                }

                if (directionPressed)
                {
                    UpdateDirection();
                }
            }
            else if(directionPressed)
            {
                if (!sideColl || !right)
                {
                    RunGraphic.Update();
                    UpdateDirection();
                }
            }            
        }

        public void FlipSprite(bool direction)
        {
            if (direction != _direction)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
                direction = _direction;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var texture = GetTexture();
            Rectangle sourceRectangle;
            if (JumpGraphic.jumping)
            {
                sourceRectangle = JumpGraphic.GetRectangle();
            }
            else
            {
                sourceRectangle = RunGraphic.GetRectangle();
            }

            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, sourceRectangle.Width, sourceRectangle.Height);            
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(0, 0), spriteEffects, 0.0f);
        }
        
        private void UpdateDirection()
        {
            if (right && location.X < MAX_DISPLACEMENT)
            {
                location.X += DISPLACEMENT;
            }
            else if (!right && location.X > MIN_DISPLACEMENT)
            {
                location.X -= DISPLACEMENT;
            }
        }

        private Texture2D GetTexture()
        {
            Texture2D texture = null;
            if (JumpGraphic.jumping)
            {
                texture = JumpGraphic.GetTexture();
            }
            else
            {
                texture = RunGraphic.GetTexture();
            }

            return texture;
        }
    }
}
