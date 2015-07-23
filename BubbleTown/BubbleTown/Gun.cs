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
    class Gun : GameObject
    {
        public Gun() { }

        public Gun(Texture2D texture, Rectangle rectangle, int height, int width)
            : base(texture, rectangle, height, width)
        {
            Texture = texture;
            Rectangle = rectangle;
            Height = height;
            Width = width;
        }

        public void Create()
        {
            Texture = TextureLoad.Gun;
            Height = 111;
            Width = 110;
            Rectangle = new Rectangle((int)Game1.ScreenSize.X / 2 - Width / 2, (int)Game1.ScreenSize.Y - Height, Width, Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
