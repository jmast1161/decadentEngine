using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecadentEngine
{
    public class RunGraphic: IGraphic
    {
        private Texture2D texture;
        private int rows;
        private int columns;
        private int currentFrame;

        public RunGraphic(Texture2D runTexture, int rows, int columns)
        {
            texture = runTexture;
            this.rows = rows;
            this.columns = columns;
            currentFrame = 0;
        }

        public void InitGraphic()
        {
            currentFrame = 0;
        }

        public void Update()
        {
            ++currentFrame;
            if (currentFrame == rows * columns)
            {
                currentFrame = 0;
            }
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
