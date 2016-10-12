using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecadentEngine
{
    public interface IGraphic
    {
        Texture2D GetTexture();
        Rectangle GetRectangle();
    }
}
