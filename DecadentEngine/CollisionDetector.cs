using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DecadentEngine
{
    public class CollisionDetector
    {
        public static Dictionary<Rectangle, Common.CollisionSide> SpriteMapCollisionRect(ISimpleSprite sprite, MapHandler map)
        {
            Dictionary<Rectangle, Common.CollisionSide> collisionTiles = new Dictionary<Rectangle, Common.CollisionSide>();
            var spriteRect = sprite.GetRectangle();
            foreach (var collTile in map.CollisionTiles)
            {
                if (collTile.Intersects(spriteRect))
                {
                    Common.CollisionSide side = 0;
                    if (collTile.Y + collTile.Height >= spriteRect.Y + spriteRect.Height)
                    {
                        side = Common.CollisionSide.Bottom;
                    }
                    else
                    {
                        if (collTile.X < spriteRect.X)
                        {
                            side = Common.CollisionSide.Left;
                        }
                        else if (collTile.X > spriteRect.X)
                        {
                            side = Common.CollisionSide.Right;
                        }
                    }

                    collisionTiles.Add(collTile, side);
                }
            }

            return collisionTiles;
        }
    }
}
