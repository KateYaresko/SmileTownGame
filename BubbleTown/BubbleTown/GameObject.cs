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
    public abstract class GameObject
    {
        public Texture2D Texture { set; get; }
        public Rectangle Rectangle { set; get; }

        public int Height { set; get; }
        public int Width { set; get; }

        public GameObject() { }

        public GameObject(Texture2D texture, Rectangle rectangle, int height, int width)
        {
            Texture = texture;
            Rectangle = rectangle;
            Height = height;
            Width = width;
        }

        virtual public void Update() { }

        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
