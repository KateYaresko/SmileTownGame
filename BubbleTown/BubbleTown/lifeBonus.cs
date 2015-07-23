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
    public class LifeBonus : Bonus
    {
        Random rand = new Random();

        private int lifes;
        public int Lifes { get { return lifes; } }
        public bool IsStatic { set; get; }

        public List<LifeBonus> allLifeBonuses;

        public LifeBonus()
        {
            IsStatic = false;
            allLifeBonuses = new List<LifeBonus>();
            lifes = 3;
        }

        public LifeBonus(Texture2D texture, Rectangle rectangle, int height, int width, Vector2 position)
            : base(texture, rectangle, height, width, position)
        {
            IsStatic = false;
            allLifeBonuses = new List<LifeBonus>();
            lifes = 3;
        }

        public bool Create(int i, int j)
        {
            int color = rand.Next(0, 10);
            
            LifeBonus life = new LifeBonus(TextureLoad.Heart, new Rectangle(i * Size, j * Size, Size, Size), Size, Size, new Vector2(i * Size, j * Size));
            if (color == (int)bonusType.LIFE && (j == 0 || !isAlone(life)))
            {
                allLifeBonuses.Add(life);
                return true;
            }
            return false;
        }

        public void Create(Vector2 position)
        {
            new LifeBonus(TextureLoad.Heart, new Rectangle((int)position.X, (int)position.Y, Size, Size), Size, Size, position);
            IsStatic = true;
        }

        public static bool isAlone(LifeBonus life)
        {
            for (int i = 0; i < Game1.smile.allSmiles.Count(); i++)
            {
                if (isCollision(life.Position, Game1.smile.allSmiles[i].Position))
                    return false;
            }
            for (int i = 0; i < Game1.lifeBonus.allLifeBonuses.Count(); i++)
            {
                if ((life != Game1.lifeBonus.allLifeBonuses[i]) && isCollision(life.Position, Game1.lifeBonus.allLifeBonuses[i].Position))
                    return false;
            }
            for (int i = 0; i < Game1.choiceBonus.allChoiceBonuses.Count(); i++)
            {
                if (isCollision(life.Position, Game1.choiceBonus.allChoiceBonuses[i].Position))
                    return false;
            }
            for (int i = 0; i < Game1.sightBonus.allSightBonuses.Count(); i++)
            {
                if (isCollision(life.Position, Game1.sightBonus.allSightBonuses[i].Position))
                    return false;
            }
            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
