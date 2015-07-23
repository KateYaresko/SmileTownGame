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
    class Lazer : GameObject
    {
        public Vector2 Original { set; get; }
        public Vector2 Position { set; get; }
        public float Rotation { set; get; }

        public Lazer() { }

        public Lazer(Texture2D texture, Rectangle rectangle, Vector2 original, Vector2 position, int height, int width)
            : base(texture, rectangle,  height, width)
        {
            Original = original;
            Position = position;
        }

        public void Create()
        {
            Texture = TextureLoad.Lazer;
            Height = 130;
            Width = 9;
            Position = new Vector2((int)Game1.ScreenSize.X / 2, (int)Game1.ScreenSize.Y - 111 / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Original, 1f, SpriteEffects.None, 0);
        }
    }
}
