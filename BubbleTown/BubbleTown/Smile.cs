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
    public class Smile : GameObject
    {
        Random rand = new Random();

        public Vector2 Position { set; get; }
        public bool IsGoingToDie { get; set; }
        public bool IsDead { get; set; }
        public int ColorType { get; set; }

        private int size = 68;
        public int Size { get{return size;}}

        public List<Smile> allSmiles;

        public Smile()
        {
            allSmiles = new List<Smile>();
            IsGoingToDie = false;
            IsDead = false;
        }

        public Smile(Texture2D texture, Rectangle rectangle, int height, int width, Vector2 position, int color)
            : base(texture, rectangle, height, width)
        {
            allSmiles = new List<Smile>();
            Position = position;
            ColorType = color;
            IsGoingToDie = false;
            IsDead = false;
        }

        public bool Create(int i, int j)
        {
            int color = rand.Next(1, 9);
            Texture2D currentTexture = TextureLoad.RedSmile;
            bool isExist = false;
            TextureLoad.SetColor(color, ref currentTexture, ref isExist);
            Vector2 position = new Vector2(i * Size, j * Size);

            Smile smile = new Smile(currentTexture, new Rectangle(i * Size, j * Size, Size, Size), Size, Size, position, color);
            if (isExist && color > 0 && color < Game1.maxAmountOfSmile && (j == 0 || !isAlone(smile)))
            {
                allSmiles.Add(smile);
                return true;
            }
            return false;
        }

        private bool isAlone(Smile smile)
        {
            for (int i = 0; i < allSmiles.Count(); i++)
            {
                if ((smile != allSmiles[i]) && isCollision(smile, allSmiles[i]))
                    return false;
            }
            return true;
        }

        public bool isCollision(Smile smile1, Smile smile2)
        {
            float deltaX = smile1.Position.X - smile2.Position.X;
            float deltaY = smile1.Position.Y - smile2.Position.Y;
            double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (r <= Size + 1) return true;

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
