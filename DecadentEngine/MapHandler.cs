using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace DecadentEngine
{
    public class MapHandler : IGameObject
    {
        private const int SCROLL_AMOUNT = 20;
        private Texture2D tileset;
        private TmxMap map;
        private int tileWidth;
        private int tileHeight;
        private int tilesetTilesWide;
        private int tilesetTilesHigh;
        private Vector2 location;

        public TmxMap Map
        {
            get
            {
                return map;
            }
        }

        public List<Rectangle> CollisionTiles
        {
            get; private set;
        }

        public MapHandler(string mapPath, Texture2D tiles)
        {
            location = new Vector2(0, 0);
            map = new TmxMap(mapPath);
            CollisionTiles = new List<Rectangle>();
            tileset = tiles;
            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;

            tilesetTilesWide = tileset.Width / tileWidth;
            tilesetTilesHigh = tileset.Height / tileHeight;

            for (var i = 0; i < map.Layers[0].Tiles.Count; ++i)
            {
                int gid = map.Layers[0].Tiles[i].Gid;
                if (gid == 5) 
                {
                    int tileFrame = gid - 1;
                    int column = tileFrame % tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                    float x = (i % map.Width) * map.TileWidth;
                    float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;

                    Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
                    tilesetRec.X = (int)x;
                    tilesetRec.Y = (int)y;
                    CollisionTiles.Add(tilesetRec);
                }
            }
        }


        public void Update(AnimatedSprite sprite)
        {
            if (sprite.IsSpriteMoving())
            {
                if (sprite.GetRectangle().X == AnimatedSprite.MAX_DISPLACEMENT)
                {
                    location.X -= SCROLL_AMOUNT;
                }
                else if (sprite.GetRectangle().X == AnimatedSprite.MIN_DISPLACEMENT)
                {
                    location.X += SCROLL_AMOUNT;
                }
            }

            //if (sprite.IsSpriteOnMap())
            //{
            //    if (sprite.GetRectangle().Y > 300)
            //    {
            //        location.Y += SCROLL_AMOUNT;
            //    }
            //    else
            //    {
            //        location.Y -= SCROLL_AMOUNT;
            //    }
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < map.Layers[0].Tiles.Count; i++)
            {
                int gid = map.Layers[0].Tiles[i].Gid;

                // Empty tile, do nothing
                if (gid != 0)
                {
                    int tileFrame = gid - 1;
                    int column = tileFrame % tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                    float x = (i % map.Width) * map.TileWidth + location.X;
                    float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight + location.Y;

                    Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                    spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White);
                }
            }
        }
    }
}
