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
    static class GameOverScreen
    {
        private static Vector2 Size = new Vector2(131, 130);
        private static int NumberOfFrames = 24;
        private static int CurrentFrame = 0;
        private static int TimeInMilliseconds = 0;
        private static Vector2 FirstCrabPosition = new Vector2(825, 235);
        private static Vector2 SecondCrabPosition = new Vector2(345, 235);

        private static Vector2 GameOverPosition = new Vector2(445, 50);
        private static Color GameOverColor = Color.DarkRed;
        private static float alpha = 1f;
        private static float angle = 0f;
        private static string GameOverString = "GAME OVER!";

        private static Rectangle ScoreLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3 + 60 * 2, (int)Game1.ScreenSize.X / 2 - 50, 45);
        private static Vector2 ScorePosition = new Vector2(210, Game1.ScreenSize.Y / 3 + 60 * 2);
        private static Color ScoreColor = Color.DarkGray;

        private static Rectangle HeighScoreRect = new Rectangle((int)Game1.ScreenSize.X / 2 + 40, (int)Game1.ScreenSize.Y / 3 + 60 * 2, (int)Game1.ScreenSize.X / 2 - 50, 45);
        private static Vector2 HighScorePosition = new Vector2(700, Game1.ScreenSize.Y / 3 + 60 * 2);
        private static Color HighScoreColor = Color.DarkGray;

        private static Rectangle BackToMainMenuLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3, (int)Game1.ScreenSize.X - 20, 30);
        private static Vector2 BackToMainMenuPosition = new Vector2(200, Game1.ScreenSize.Y / 3);
        private static Color BackToMainMenuColor = Color.DarkGray;
        private static string BackToMainMenuString = "MAIN MENU";

        private static Rectangle RestartLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3 + 40 * 2, (int)Game1.ScreenSize.X - 20, 30);
        private static Vector2 RestartPosition = new Vector2(200, Game1.ScreenSize.Y / 3 + 40 * 2);
        private static Color RestartColor = Color.DarkGray;
        private static string RestartString = "RESTART";

        private static Rectangle QuitLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3 + 40 * 4, (int)Game1.ScreenSize.X - 20, 30);
        private static Vector2 QuitPosition = new Vector2(200, Game1.ScreenSize.Y / 3 + 40 * 4);
        private static Color QuitColor = Color.DarkGray;
        private static string QuitString = "QUIT";

        private static MouseState mouseStateCurrent;
        private static MouseState mouseStatePrevious;

        public static void GameOver()
        {
            MenuScreen.GameOver();
            Game1.gameState = GameState.GAME_OVER_SREEN;
        }

        public static void Update(GameTime gameTime)
        {
            TimeInMilliseconds++;
            if (NumberOfFrames > 1)
            {
                if (TimeInMilliseconds > 2)
                {
                    CurrentFrame = (CurrentFrame + 1) % NumberOfFrames;
                    TimeInMilliseconds = 0;
                }
            }

            mouseStatePrevious = mouseStateCurrent;
            mouseStateCurrent = Mouse.GetState();

            alpha = (float)Math.Abs(Math.Cos(angle));
            angle += 0.05f;
            if (BackToMainMenuLineRect.Contains(new Point((int)mouseStateCurrent.X - 20, (int)mouseStateCurrent.Y - 20)))
            {
                BackToMainMenuColor = Color.White;
                RestartColor = Color.DarkGray;
                QuitColor = Color.DarkGray;
                if (mouseStatePrevious.LeftButton == ButtonState.Pressed && mouseStateCurrent.LeftButton == ButtonState.Released)
                {
                    Game1.gameState = GameState.MENU_SCREEN;
                }
            }
            else if (RestartLineRect.Contains(new Point((int)mouseStateCurrent.X - 20, (int)mouseStateCurrent.Y - 20)))
            {
                BackToMainMenuColor = Color.DarkGray;
                RestartColor = Color.White;
                QuitColor = Color.DarkGray;
                if (mouseStatePrevious.LeftButton == ButtonState.Pressed && mouseStateCurrent.LeftButton == ButtonState.Released)
                {
                    MenuScreen.NewGame();
                }
            }
            else if (QuitLineRect.Contains(new Point((int)mouseStateCurrent.X - 20, (int)mouseStateCurrent.Y - 20)))
            {
                BackToMainMenuColor = Color.DarkGray;
                RestartColor = Color.DarkGray;
                QuitColor = Color.White;
                if (mouseStatePrevious.LeftButton == ButtonState.Pressed && mouseStateCurrent.LeftButton == ButtonState.Released)
                {
                    MenuScreen.QuitGame();
                }
            }
            else
            {
                BackToMainMenuColor = Color.DarkGray;
                RestartColor = Color.DarkGray;
                QuitColor = Color.DarkGray;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureLoad.GameOver, new Rectangle(0, 0, 1360, 760), Color.White);
            spriteBatch.DrawString(TextureLoad.GameOverFont, GameOverString, GameOverPosition, GameOverColor);
            spriteBatch.DrawString(TextureLoad.MenuFont, RestartString, RestartPosition, RestartColor);
            spriteBatch.DrawString(TextureLoad.MenuFont, QuitString, QuitPosition, QuitColor);
            spriteBatch.DrawString(TextureLoad.MenuFont, BackToMainMenuString, BackToMainMenuPosition, BackToMainMenuColor);
            Rectangle SourceRect = new Rectangle(CurrentFrame * ((int)Size.X), 0, (int)Size.X, (int)Size.Y);
        }
    }
}
