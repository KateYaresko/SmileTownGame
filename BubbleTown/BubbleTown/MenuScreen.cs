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
    static class MenuScreen
    {
        private static bool gameStarted = false;

        private static Vector2 MainMenuPosition = new Vector2(445, 50);
        private static Color MainMenuColor = Color.DarkRed;
        private static float alpha = 1f;
        private static float angle = 0f;
        private static string MainMenuString = "MAIN MENU";

        private static Rectangle NewGameLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3, (int)Game1.ScreenSize.X - 20, 30);
        private static Vector2 NewGamePosition = new Vector2(200, Game1.ScreenSize.Y / 3);
        private static Color NewGameColor = Color.DarkGray;
        private static string NewGameString = "NEW GAME";

        private static Rectangle ResumeLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3 + 40 * 2, (int)Game1.ScreenSize.X - 20, 30);
        private static Vector2 ResumePosition = new Vector2(200, Game1.ScreenSize.Y / 3 + 40 * 2);
        private static Color ResumeGameColor = Color.DarkGray;
        private static string ResumeGameString = "CONTINUE";

        private static Rectangle QuitLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3 + 40 * 4, (int)Game1.ScreenSize.X - 20, 30);
        private static Vector2 QuitPosition = new Vector2(200, Game1.ScreenSize.Y / 3 + 40 * 4);
        private static Color QuitColor = Color.DarkGray;
        private static string QuitGameString = "QUIT";

        private static MouseState mouseStateCurrent;
        private static MouseState mouseStatePrevious;

        public static void Update(GameTime gameTime)
        {
            mouseStatePrevious = mouseStateCurrent;
            mouseStateCurrent = Mouse.GetState();
            if (NewGameLineRect.Contains(new Point((int)mouseStateCurrent.X-20, (int)mouseStateCurrent.Y-20)))
            {
                NewGameColor = Color.White;
                ResumeGameColor = Color.DarkGray;
                QuitColor = Color.DarkGray;
                if (mouseStatePrevious.LeftButton == ButtonState.Pressed && mouseStateCurrent.LeftButton == ButtonState.Released)
                {
                    NewGameColor = Color.Red;
 //                   NewGame(Level.EASY_LEVEL);
                    NewGame();
                }
            }
            else if (ResumeLineRect.Contains(new Point((int)mouseStateCurrent.X-20, (int)mouseStateCurrent.Y-20)) && gameStarted == true)
            {
                NewGameColor = Color.DarkGray;
                ResumeGameColor = Color.White;
                QuitColor = Color.DarkGray;
                if (mouseStatePrevious.LeftButton == ButtonState.Pressed && mouseStateCurrent.LeftButton == ButtonState.Released)
                {
                    ResumeGameColor = Color.Red;
                    ResumeGame();
                }
            }
            else if (QuitLineRect.Contains(new Point((int)mouseStateCurrent.X-20, (int)mouseStateCurrent.Y-20)))
            {
                NewGameColor = Color.DarkGray;
                ResumeGameColor = Color.DarkGray;
                QuitColor = Color.White;
                if (mouseStatePrevious.LeftButton == ButtonState.Pressed && mouseStateCurrent.LeftButton == ButtonState.Released)
                {
                    QuitColor = Color.Red;
                    QuitGame();
                }
            }
            else
            {
                NewGameColor = Color.DarkGray;
                ResumeGameColor = Color.DarkGray;
                QuitColor = Color.DarkGray;
            }
        }

        public static void NewGame()
        {
            Game1.newGame();
            gameStarted = true;
            Game1.gameState = GameState.GAME_SCREEN;
        }

        public static void GameOver()
        {
            gameStarted = false;
        }

        public static void ResumeGame()
        {
            Game1.gameState = GameState.GAME_SCREEN;
        }

        public static void QuitGame()
        {
            Game1.Instance.Exit();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureLoad.Menu, new Rectangle(0, 0, 1360, 760), Color.White);
            spriteBatch.DrawString(TextureLoad.GameOverFont, MainMenuString, MainMenuPosition, MainMenuColor);
            spriteBatch.DrawString(TextureLoad.MenuFont, NewGameString, NewGamePosition, NewGameColor);
            spriteBatch.DrawString(TextureLoad.MenuFont, ResumeGameString, ResumePosition, ResumeGameColor);
            spriteBatch.DrawString(TextureLoad.MenuFont, QuitGameString, QuitPosition, QuitColor);
        }
    }
}
