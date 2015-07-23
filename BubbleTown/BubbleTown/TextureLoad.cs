using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BubbleTown
{
    static class TextureLoad
    {
        public static Texture2D RedSmile { get; private set; }
        public static Texture2D BlueSmile { get; private set; }
        public static Texture2D GreenSmile { get; private set; }
        public static Texture2D YellowSmile { get; private set; }
        public static Texture2D PinkSmile { get; private set; }
        public static Texture2D BlackSmile { get; private set; }
        public static Texture2D Background { get; private set; }
        public static Texture2D Gun { get; private set; }
        public static Texture2D Lazer { get; private set; }
        public static Texture2D Paper { get; private set; }
        public static Texture2D Eye { get; private set; }
        public static Texture2D Palm { get; private set; }
        public static Texture2D Heart { get; private set; }
        public static Texture2D Sight { get; private set; }
        public static Texture2D Line { get; private set; }
        public static SpriteFont MenuFont { get; private set; }
        public static SpriteFont GameOverFont { get; private set; }
        public static SpriteFont LifesFont { get; private set; }
        public static Texture2D Menu { get; private set; }
        public static Texture2D GameOver { get; private set; }
        public static Texture2D LevelUp { get; private set; }
        public static Texture2D GameIsWon { get; private set; }

        public static void Load(ContentManager Content)
        {
            RedSmile = Content.Load<Texture2D>("red68");
            BlueSmile = Content.Load<Texture2D>("blue68");
            GreenSmile = Content.Load<Texture2D>("green68");
            YellowSmile = Content.Load<Texture2D>("yellow68");
            PinkSmile = Content.Load<Texture2D>("pink68");
            BlackSmile = Content.Load<Texture2D>("black68");
            Background = Content.Load<Texture2D>("sea");
            Gun = Content.Load<Texture2D>("redBall110");
            Lazer = Content.Load<Texture2D>("lazer130");
            Paper = Content.Load<Texture2D>("paper200");
            Eye = Content.Load<Texture2D>("eye1_68");
            Palm = Content.Load<Texture2D>("palm68");
            Heart = Content.Load<Texture2D>("bonusHeart");
            Sight = Content.Load<Texture2D>("bonusColor");
            Line = Content.Load<Texture2D>("line");
            MenuFont = Content.Load<SpriteFont>("MenuFont");
            GameOverFont = Content.Load<SpriteFont>("GameOverFont");
            LifesFont = Content.Load<SpriteFont>("SpriteFont");
            Menu = Content.Load<Texture2D>("menu1360X760");
            GameOver = Content.Load<Texture2D>("gameOver");
            LevelUp = Content.Load<Texture2D>("levelWon");
            GameIsWon = Content.Load<Texture2D>("gameWon");
        }

        public static void SetColor(int color, ref Texture2D currentTexture, ref bool currentState)
        {
            switch (color)
            {
                case (int)smileColor.RED:
                    {
                        currentTexture = TextureLoad.RedSmile;
                        currentState = true;
                        break;
                    }

                case (int)smileColor.GREEN:
                    {
                        currentTexture = TextureLoad.GreenSmile;
                        currentState = true;
                        break;
                    }

                case (int)smileColor.BLUE:
                    {
                        currentTexture = TextureLoad.BlueSmile;
                        currentState = true;
                        break;
                    }

                case (int)smileColor.YELLOW:
                    {
                        currentTexture = TextureLoad.YellowSmile;
                        currentState = true;
                        break;
                    }

                case (int)smileColor.PINK:
                    {
                        currentTexture = TextureLoad.PinkSmile;
                        currentState = true;
                        break;
                    }

                case (int)smileColor.BLACK:
                    {
                        currentTexture = TextureLoad.BlackSmile;
                        currentState = true;
                        break;
                    }

                case (int)bonusType.LIFE:
                    {
                        currentTexture = TextureLoad.Heart;
                        currentState = true;
                        break;
                    }

                case (int)bonusType.CHOICE:
                    {
                        currentTexture = TextureLoad.Palm;
                        currentState = true;
                        break;
                    }

                case (int)bonusType.EYE:
                    {
                        currentTexture = TextureLoad.Eye;
                        currentState = true;
                        break;
                    }

                case (int)bonusType.SIGHT:
                    {
                        currentTexture = TextureLoad.Sight;
                        currentState = true;
                        break;
                    }
            }
        }
    }
}
