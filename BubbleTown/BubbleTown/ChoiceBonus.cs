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
    public class ChoiceBonus : Bonus
    {
        Random rand = new Random();

        public List<ChoiceBonus> allChoiceBonuses;

        public ChoiceBonus()
        {
            allChoiceBonuses = new List<ChoiceBonus>();
        }

        public ChoiceBonus(Texture2D texture, Rectangle rectangle, int height, int width, Vector2 position)
            : base(texture, rectangle, height, width, position)
        {
            allChoiceBonuses = new List<ChoiceBonus>();
        }

        public bool Create(int i, int j)
        {
            int color = rand.Next(0, 10);

            ChoiceBonus choice = new ChoiceBonus(TextureLoad.Palm, new Rectangle(i * Size, j * Size, Size, Size), Size, Size, new Vector2(i * Size, j * Size));
            if (color == (int)bonusType.CHOICE && (j == 0 || !isAlone(choice)))
            {
                allChoiceBonuses.Add(choice);
                return true;
            }
            return false;
        }

        public static bool isAlone(ChoiceBonus choice)
        {
            for (int i = 0; i < Game1.smile.allSmiles.Count(); i++)
            {
                if (isCollision(choice.Position, Game1.smile.allSmiles[i].Position))
                    return false;
            }
            for (int i = 0; i < Game1.choiceBonus.allChoiceBonuses.Count(); i++)
            {
                if ((choice != Game1.choiceBonus.allChoiceBonuses[i]) && isCollision(choice.Position, Game1.choiceBonus.allChoiceBonuses[i].Position))
                    return false;
            }
            for (int i = 0; i < Game1.lifeBonus.allLifeBonuses.Count(); i++)
            {
                if (isCollision(choice.Position, Game1.lifeBonus.allLifeBonuses[i].Position))
                    return false;
            }
            for (int i = 0; i < Game1.sightBonus.allSightBonuses.Count(); i++)
            {
                if (isCollision(choice.Position, Game1.sightBonus.allSightBonuses[i].Position))
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
