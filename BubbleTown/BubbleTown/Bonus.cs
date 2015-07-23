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
    public abstract class Bonus : GameObject
    {
        public Vector2 Position { set; get; }

        private int size = 68;
        public int Size { get { return size; } }

        public Bonus() { }

        public Bonus(Texture2D texture, Rectangle rectangle, int height, int width, Vector2 position)
            : base(texture, rectangle, height, width)
        {
            Position = position;
        }

        public static bool isCollision(Vector2 item1, Vector2 item2)
        {
            float deltaX = item1.X - item2.X;
            float deltaY = item1.Y - item2.Y;
            double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (r <= 68 + 1) return true;

            return false;
        }

        abstract public override void Draw(SpriteBatch spriteBatch);
    }
}
