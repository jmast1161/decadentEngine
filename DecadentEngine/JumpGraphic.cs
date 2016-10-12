using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DecadentEngine
{
    public class JumpGraphic : IGraphic
    {
        public const double JUMP_VELOCITY = 75;
        public bool jumping = false;
        private Texture2D texture;
        private int currentFrame;
        private int rows;
        private int columns;

        public JumpGraphic(Texture2D jumpTexture, int jumpRows, int jumpColumns)
        {
            texture = jumpTexture;
            rows = jumpRows;
            columns = jumpColumns;
        }

        public int GetTotalJumpFrames()
        {
            return rows * columns;
        }

        public void InitGraphic()
        {
            jumping = false;
            currentFrame = 0;
        }

        public void StartJump()
        {
            jumping = true;
        }

        public void Update()
        {
            ++currentFrame;
        }

        public bool IsJumpStarted()
        {
            return currentFrame == 0;
        }

        public bool IsJumpAtPeak()
        {
            return currentFrame == GetTotalJumpFrames() / 2;
        }

        public bool IsJumpFinished()
        {
            return currentFrame == GetTotalJumpFrames();
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public Rectangle GetRectangle()
        {
            int width = texture.Width / columns;
            int height = texture.Height / rows;
            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;
            return new Rectangle(width * column, height * row, width, height);
        }
    }
}
