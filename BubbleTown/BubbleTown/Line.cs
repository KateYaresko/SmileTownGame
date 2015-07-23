using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BubbleTown
{
    public class Line : GameObject
    {
        public Line() { }

        public Line(Texture2D texture, Rectangle rectangle, int height, int width)
            : base(texture, rectangle, height, width)
        {
            Texture = texture;
            Rectangle = rectangle;
            Height = height;
            Width = width;
        }

        public void Create()
        {
            Texture = TextureLoad.Line;
            Height = 8;
            Width = (int)Game1.ScreenSize.X;
            Rectangle = new Rectangle(0, 6 * Game1.sizeOfSmile + 2 * Game1.sizeOfSmile, Width, Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
