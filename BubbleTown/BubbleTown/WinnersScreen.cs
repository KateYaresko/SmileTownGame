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
    static class WinnersScreen
    {
        private static Vector2 FirstCheerPosition = new Vector2(850, 70);
        private static Vector2 SecondCheerPosition = new Vector2(180, 70);

        private static Vector2 YouWonPosition = new Vector2(345, 50);
        private static Color YouWonColor = Color.DarkRed;
        private static float alpha = 1f;
        private static float angle = 0f;
        private static string YouWonString = "Level is passed!";

        private static Rectangle BackToMainMenuLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3, (int)Game1.ScreenSize.X - 20, 30);
        private static Vector2 BackToMainMenuPosition = new Vector2(200, Game1.ScreenSize.Y / 3);
        private static Color BackToMainMenuColor = Color.DarkGray;
        private static string BackToMainMenuString = "MAIN MENU";

        private static Rectangle RestartLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3 + 40 * 2, (int)Game1.ScreenSize.X - 20, 30);
        private static Vector2 RestartPosition = new Vector2(200, Game1.ScreenSize.Y / 3 + 40 * 2);
        private static Color RestartColor = Color.DarkGray;
        private static string RestartString = "RESTART";

        private static Rectangle NextLevelLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3 + 40 * 4, (int)Game1.ScreenSize.X - 20, 30);
        private static Vector2 NextLevelPosition = new Vector2(200, Game1.ScreenSize.Y / 3 + 40 * 4);
        private static Color NextLevelColor = Color.DarkGray;
        private static string NextLevelString = "NEXT LEVEL";

        private static Rectangle QuitLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3 + 40 * 6, (int)Game1.ScreenSize.X - 20, 30);
        private static Vector2 QuitPosition = new Vector2(200, Game1.ScreenSize.Y / 3 + 40 * 6);
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
            mouseStatePrevious = mouseStateCurrent;
            mouseStateCurrent = Mouse.GetState();
            alpha = (float)Math.Abs(Math.Cos(angle));
            angle += 0.05f;
            if (BackToMainMenuLineRect.Contains(new Point((int)mouseStateCurrent.X - 20, (int)mouseStateCurrent.Y - 20)))
            {
                BackToMainMenuColor = Color.White;
                RestartColor = Color.DarkGray;
                QuitColor = Color.DarkGray;
                NextLevelColor = Color.DarkGray;
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
                NextLevelColor = Color.DarkGray;
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
                NextLevelColor = Color.DarkGray;
                if (mouseStatePrevious.LeftButton == ButtonState.Pressed && mouseStateCurrent.LeftButton == ButtonState.Released)
                {
                    MenuScreen.QuitGame();
                }
            }
            else if (NextLevelLineRect.Contains(new Point((int)mouseStateCurrent.X - 20, (int)mouseStateCurrent.Y - 20)) && Game1.level != (int)Level.IMPOSSIBLE)
            {
                BackToMainMenuColor = Color.DarkGray;
                RestartColor = Color.DarkGray;
                QuitColor = Color.DarkGray;
                NextLevelColor = Color.White;

                if (mouseStatePrevious.LeftButton == ButtonState.Pressed && mouseStateCurrent.LeftButton == ButtonState.Released)
                {
                    if (Game1.level == (int)Level.EASY)
                        Game1.level = (int)Level.MEDIUM;

                    else if (Game1.level == (int)Level.MEDIUM)
                        Game1.level = (int)Level.HARD;
                    
                    else if (Game1.level == (int)Level.HARD)
                        Game1.level = (int)Level.SUPER_HARD;
                    
                    else if (Game1.level == (int)Level.SUPER_HARD)
                        Game1.level = (int)Level.IMPOSSIBLE;

                    MenuScreen.NewGame();
                }
            }
            else
            {
                BackToMainMenuColor = Color.DarkGray;
                RestartColor = Color.DarkGray;
                QuitColor = Color.DarkGray;
                NextLevelColor = Color.DarkGray;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (Game1.level == (int)Level.IMPOSSIBLE)
            {
                YouWonString = "Game is won!";
                spriteBatch.Draw(TextureLoad.GameIsWon, new Rectangle(0, 0, 1360, 760), Color.White);
                QuitLineRect = new Rectangle(10, (int)Game1.ScreenSize.Y / 3 + 40 * 4, (int)Game1.ScreenSize.X - 20, 30);
                QuitPosition = new Vector2(200, Game1.ScreenSize.Y / 3 + 40 * 4);
            }
            else
            {
                spriteBatch.Draw(TextureLoad.LevelUp, new Rectangle(0, 0, 1360, 760), Color.White);
                spriteBatch.DrawString(TextureLoad.MenuFont, NextLevelString, NextLevelPosition, NextLevelColor); 
            }
            spriteBatch.DrawString(TextureLoad.GameOverFont, YouWonString, YouWonPosition, YouWonColor);
            spriteBatch.DrawString(TextureLoad.MenuFont, BackToMainMenuString, BackToMainMenuPosition, BackToMainMenuColor);
            spriteBatch.DrawString(TextureLoad.MenuFont, RestartString, RestartPosition, RestartColor);
            spriteBatch.DrawString(TextureLoad.MenuFont, QuitString, QuitPosition, QuitColor);                          
        }
         
    }
}
