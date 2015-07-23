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
    public class Bullet : Smile
    {
        Random rand = new Random();

        public bool GonnaBePalm { set; get; }
        public bool IsPalm { set; get; }

        public bool GonnaBeEye { set; get; }
        public bool IsEye { set; get; }

        public Vector2 velocity;

        public float tangVelocity = 12f;

        public Bullet()
        {
            IsPalm = false;
            GonnaBePalm = false;
            IsEye = false;
            GonnaBeEye = false;
        }

        public Bullet(Texture2D smileTexture, Rectangle smileRectangle, int height, int width, Vector2 position, int color)
            : base(smileTexture, smileRectangle, height, width, position, color)
        {
            IsPalm = false;
            GonnaBePalm = false;
            IsEye = false;
            GonnaBeEye = false;
        }

        public void Create()
        {
            Height = Game1.sizeOfSmile;
            Width = Game1.sizeOfSmile;
            Position = new Vector2((int)Game1.ScreenSize.X / 2 - Height / 2, (int)Game1.ScreenSize.Y - Height - 15);
            int maxAmountOfBullet = 6;
            if (Game1.maxAmountOfSmile < 6) maxAmountOfBullet = Game1.maxAmountOfSmile;
            int colorOfBullet;
            bool colorIsExist = false;
            do
            {
                colorOfBullet = rand.Next(1, maxAmountOfBullet);
                if (Game1.smile.allSmiles.Count() == 0) break;
                for (int i = 0; i < Game1.smile.allSmiles.Count(); i++)
                    if (Game1.smile.allSmiles[i].ColorType == colorOfBullet)
                        colorIsExist = true;
            }
            while(!colorIsExist);
           
            Texture2D currTexture = TextureLoad.RedSmile;
            bool isBulletExist = false;
            TextureLoad.SetColor(colorOfBullet, ref currTexture, ref isBulletExist);
            ColorType = colorOfBullet;
            Texture = currTexture;
        }

        public void Create(Texture2D texture, int color)
        {
            Height = Game1.sizeOfSmile;
            Width = Game1.sizeOfSmile;
            Position = new Vector2((int)Game1.ScreenSize.X / 2 - Height / 2, (int)Game1.ScreenSize.Y - Height - 15); 
            ColorType = color;
            Texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
