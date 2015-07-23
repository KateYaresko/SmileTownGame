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
    public class SightBonus : Bonus
    {
        Random rand = new Random();

        public bool IsMove { set; get; }

        public List<SightBonus> allSightBonuses;

        public Vector2 velocity;
        public float tangVelocity = 50f;

        public SightBonus()
        {
            allSightBonuses = new List<SightBonus>();
            IsMove = false;
        }

        public SightBonus(Texture2D texture, Rectangle rectangle, int height, int width, Vector2 position)
            : base(texture, rectangle, height, width, position)
        {
            allSightBonuses = new List<SightBonus>();
            IsMove = false;
        }

        public void Create(Vector2 position)
        {
            new SightBonus(TextureLoad.Sight, new Rectangle((int)position.X, (int)position.Y, Size, Size), Size, Size, position);
            IsMove = true;
        }

        public bool Create(int i, int j)
        {
            int color = rand.Next(0, 10);

            SightBonus sight = new SightBonus(TextureLoad.Eye, new Rectangle(i * Size, j * Size, Size, Size), Size, Size, new Vector2(i * Size, j * Size));
            if (color == (int)bonusType.EYE && (j == 0 || !isAlone(sight)))
            {
                allSightBonuses.Add(sight);
                return true;
            }
            return false;
        }

        public static bool isAlone(SightBonus sight)
        {
            for (int i = 0; i < Game1.smile.allSmiles.Count(); i++)
            {
                if (isCollision(sight.Position, Game1.smile.allSmiles[i].Position))
                    return false;
            }
            for (int i = 0; i < Game1.sightBonus.allSightBonuses.Count(); i++)
            {
                if ((sight != Game1.sightBonus.allSightBonuses[i]) && isCollision(sight.Position, Game1.sightBonus.allSightBonuses[i].Position))
                    return false;
            }
            for (int i = 0; i < Game1.lifeBonus.allLifeBonuses.Count(); i++)
            {
                if (isCollision(sight.Position, Game1.lifeBonus.allLifeBonuses[i].Position))
                    return false;
            }
            for (int i = 0; i < Game1.choiceBonus.allChoiceBonuses.Count(); i++)
            {
                if (isCollision(sight.Position, Game1.choiceBonus.allChoiceBonuses[i].Position))
                    return false;
            }
            return true;
        }

        public bool isCollision(ref SightBonus bonus, Smile currSmile)
        {
            float deltaX = bonus.Position.X - currSmile.Position.X;
            float deltaY = bonus.Position.Y - currSmile.Position.Y;
            double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (r <= Size)
            {
                double k = Size / r;
                float newX = currSmile.Position.X * (1 - (float)k) + bonus.Position.X * (float)k;
                float newY = currSmile.Position.Y * (1 - (float)k) + bonus.Position.Y * (float)k;
                bonus.Position = new Vector2(newX, newY);
                bonus.Rectangle = new Rectangle((int)newX, (int)newY, bonus.Rectangle.Width, bonus.Rectangle.Height);

                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
