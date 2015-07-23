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
    public enum GameState
    {
        MENU_SCREEN,
        GAME_SCREEN,
        GAME_OVER_SREEN,
        WINNER_SCREEN
    }

    public enum smileColor
    {
        RED    = 1,
        GREEN  = 2,
        BLUE   = 3,
        YELLOW = 4,
        PINK   = 5,
        BLACK  = 6
    }

    public enum Level
    {
        EASY = 1,
        MEDIUM = 2,
        HARD = 3,
        SUPER_HARD = 4,
        IMPOSSIBLE = 5
    }

    public enum bonusType
    {
        LIFE   = 7,
        CHOICE = 8,
        EYE    = 9,
        SIGHT  = 10
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {     
        Random rand = new Random();

        public static int level = (int)Level.EASY;

        public static GameState gameState = GameState.MENU_SCREEN;
        public static Game1 Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Gun gun;
        Lazer lazer;
        Background background;
        Background paper;
        Line line;
        public static Bullet bullet;
        public static Bullet nextBullet;
        public static SightBonus sight;
        public static LifeBonus life;

        const float tangVelocity = 5f;

        bool isShooting = false;
        bool down = true;
        bool rotation = false;
        bool drawSight = false;
        bool isTaken = false;

        public static bool heartIsAvailable = false;
        public static bool sightIsAvailable = false;
        public static bool palmIsAvailable = false;

        int tmpColorOfBullet = 1;

        KeyboardState pastKey;

        public static double maxa = 1360; // для нахождения смайла в колизии с минимальным удалением от пушки
        public static int sizeOfSmile = 68;
        public static int maxNumInRow = 1360 / sizeOfSmile;
        public static int maxNumInCol;
        public static int maxAmountOfSmile;

        public static int amountOfShot = 0;
        public static int maxAmountOfShot = 7;

        public static Smile smile;
        public static LifeBonus lifeBonus;
        public static ChoiceBonus choiceBonus;
        public static SightBonus sightBonus;

        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1360;
            graphics.PreferredBackBufferHeight = 760;
            IsMouseVisible = true;
            this.Window.Title = "Bubble Town Game";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureLoad.Load(Content);

            background = new Background(TextureLoad.Background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);
            paper = new Background(TextureLoad.Paper, new Rectangle(0, background.Height - 200, 251, 200), 251, 200);

            font = TextureLoad.LifesFont;

            gun = new Gun();
            gun.Create();

            lazer = new Lazer();
            lazer.Create();

            line = new Line();
            line.Create();

            life = new LifeBonus(TextureLoad.Heart, new Rectangle(paper.Width / 2, background.Height - 2 * sizeOfSmile - 25, sizeOfSmile, sizeOfSmile), sizeOfSmile, sizeOfSmile, new Vector2(paper.Width / 2, background.Height - 2 * sizeOfSmile - 25));

            newGame();      
        }

        public static void setConstants()
        {
            switch (level)
            {
                case (int)Level.EASY:
                    {
                        maxAmountOfShot = 10;
                        maxNumInCol = 3;
                        maxAmountOfSmile = 4;
                        heartIsAvailable = true;
                        sightIsAvailable = true;
                        palmIsAvailable = true;
                        break;
                    }
                case (int)Level.MEDIUM:
                    {
                        maxAmountOfShot = 9;
                        maxNumInCol = 4;
                        maxAmountOfSmile = 5;
                        heartIsAvailable = true;
                        sightIsAvailable = true;
                        palmIsAvailable = true;
                        break;
                    }
                case (int)Level.HARD:
                    {
                        maxAmountOfShot = 8;
                        maxNumInCol = 5;
                        maxAmountOfSmile = 6;
                        heartIsAvailable = true;
                        sightIsAvailable = true;
                        palmIsAvailable = true;
                        break;
                    }
                case (int)Level.SUPER_HARD:
                    {
                        maxAmountOfShot = 7;
                        maxNumInCol = 6;
                        maxAmountOfSmile = 7;
                        heartIsAvailable = true;
                        sightIsAvailable = true;
                        palmIsAvailable = true;
                        break;
                    }
                case (int)Level.IMPOSSIBLE:
                    {
                        maxAmountOfShot = 20;
                        maxNumInCol = 2;
                        maxAmountOfSmile = 2;
                        heartIsAvailable = true;
                        sightIsAvailable = true;
                        palmIsAvailable = true;
                        break;
                    }
            }
        }

        public static void newGame()
        {
            setConstants();

            amountOfShot = 0;

            smile = new Smile();
            lifeBonus = new LifeBonus();
            choiceBonus = new ChoiceBonus();
            sightBonus = new SightBonus();

            for (int i = 0; i < maxNumInRow; i++)
                for (int j = 0; j < maxNumInCol; j++)
                {
                    if (smile.Create(i, j)) ;
                    else if (heartIsAvailable && lifeBonus.Create(i, j));
                    else if (sightIsAvailable && choiceBonus.Create(i, j));
                    else if (palmIsAvailable && sightBonus.Create(i, j));
                }

            bullet = new Bullet();
            bullet.Create();

            nextBullet = new Bullet();
            nextBullet.Create();
            nextBullet.Position = new Vector2(140, ScreenSize.Y - sizeOfSmile - 20);            
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                gameState = GameState.MENU_SCREEN;

            if (gameState == GameState.MENU_SCREEN)
            {
                MenuScreen.Update(gameTime);
            }
            else if (gameState == GameState.GAME_SCREEN)
            {
                lazer.Rectangle = new Rectangle((int)lazer.Position.X, (int)lazer.Position.Y, lazer.Width, lazer.Height);
                lazer.Original = new Vector2(lazer.Width / 2, lazer.Height);

                bullet.Rectangle = new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, sizeOfSmile, sizeOfSmile);
                nextBullet.Rectangle = new Rectangle((int)nextBullet.Position.X, (int)nextBullet.Position.Y, sizeOfSmile, sizeOfSmile);

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    lazer.Rotation += 0.01f;

                    if (rotation)
                    {
                        //rotation = true; //уже true из условия 

                        createSight();
                        updateSight();

                        bullet.IsEye = true;
                    }

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    lazer.Rotation -= 0.01f;

                    if (rotation)
                    {
                       // rotation = true;  //уже true из условия

                        createSight();
                        updateSight();

                        bullet.IsEye = true;
                    }

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && pastKey.IsKeyUp(Keys.Space) && !isShooting)
                    shooting();

                pastKey = Keyboard.GetState();

                UpdateBullet();

                updateMap();
            }
                
            else if (gameState == GameState.GAME_OVER_SREEN)
            {
                GameOverScreen.Update(gameTime);
            }
                
            else if (gameState == GameState.WINNER_SCREEN)
            {
                WinnersScreen.Update(gameTime);
            }
            
            base.Update(gameTime);

        }

        void updateMap()
        {                 
            if (down && amountOfShot != 0 && (amountOfShot % maxAmountOfShot == 0))
            {
                
                for (int i = 0; i < smile.allSmiles.Count(); i++)
                {
                    smile.allSmiles[i].Rectangle = new Rectangle(smile.allSmiles[i].Rectangle.X, smile.allSmiles[i].Rectangle.Y + sizeOfSmile, smile.allSmiles[i].Rectangle.Width, smile.allSmiles[i].Rectangle.Height);
                    smile.allSmiles[i].Position = new Vector2(smile.allSmiles[i].Position.X, smile.allSmiles[i].Position.Y + sizeOfSmile);
                }
                
                for (int i = 0; i < lifeBonus.allLifeBonuses.Count(); i++)
                {
                     lifeBonus.allLifeBonuses[i].Rectangle = new Rectangle(lifeBonus.allLifeBonuses[i].Rectangle.X, lifeBonus.allLifeBonuses[i].Rectangle.Y + sizeOfSmile, lifeBonus.allLifeBonuses[i].Rectangle.Width, lifeBonus.allLifeBonuses[i].Rectangle.Height);
                     lifeBonus.allLifeBonuses[i].Position = new Vector2(lifeBonus.allLifeBonuses[i].Position.X, lifeBonus.allLifeBonuses[i].Position.Y + sizeOfSmile);
                }

                for (int i = 0; i < choiceBonus.allChoiceBonuses.Count(); i++)
                {
                    choiceBonus.allChoiceBonuses[i].Rectangle = new Rectangle(choiceBonus.allChoiceBonuses[i].Rectangle.X, choiceBonus.allChoiceBonuses[i].Rectangle.Y + sizeOfSmile, choiceBonus.allChoiceBonuses[i].Rectangle.Width, choiceBonus.allChoiceBonuses[i].Rectangle.Height);
                    choiceBonus.allChoiceBonuses[i].Position = new Vector2(choiceBonus.allChoiceBonuses[i].Position.X, choiceBonus.allChoiceBonuses[i].Position.Y + sizeOfSmile);
                }

                for (int i = 0; i < sightBonus.allSightBonuses.Count(); i++)
                {
                    sightBonus.allSightBonuses[i].Rectangle = new Rectangle(sightBonus.allSightBonuses[i].Rectangle.X, sightBonus.allSightBonuses[i].Rectangle.Y + sizeOfSmile, sightBonus.allSightBonuses[i].Rectangle.Width, sightBonus.allSightBonuses[i].Rectangle.Height);
                    sightBonus.allSightBonuses[i].Position = new Vector2(sightBonus.allSightBonuses[i].Position.X, sightBonus.allSightBonuses[i].Position.Y + sizeOfSmile);
                }

                for (int i = 0; i < maxNumInRow; i++)
                {
                    if (smile.Create(i, 0));
                    else if (heartIsAvailable && lifeBonus.Create(i, 0));
                    else if (palmIsAvailable && choiceBonus.Create(i, 0));
                    else if (sightIsAvailable && sightBonus.Create(i, 0));
                }

                if (smile.allSmiles.Count != 0)
                {
                    destroySmiles();
                    MenuScreen.GameOver();
                }
                else gameState = GameState.WINNER_SCREEN;

                if (isGameOver())
                    gameState = GameState.GAME_OVER_SREEN;

                switch (level)
                {
                    case (int)Level.EASY:
                        {
                            maxAmountOfShot = 9;
                            break;
                        }
                    case (int)Level.MEDIUM:
                        {
                            maxAmountOfShot = 8;
                            break;
                        }
                    case (int)Level.HARD:
                        {
                            maxAmountOfShot = 7;
                            break;
                        }
                    case (int)Level.SUPER_HARD:
                        {
                            maxAmountOfShot = 6;
                            break;
                        }
                    case (int)Level.IMPOSSIBLE:
                        {
                            maxAmountOfShot = 5;
                            break;
                        }
                }
                amountOfShot = 0;

                down = false;
            }
        }

        bool isGameOver()
        {
            foreach (Smile currSmile in smile.allSmiles)
            {
                if (isCollision(line, currSmile)) return true;
            }
            return false;
        }

        bool isLevelWon()
        {
            if (smile.allSmiles.Count() == 0) return true;
            return false;
        }

        public void shooting()
        {
            if (bullet.IsEye) bullet.IsEye = false;
            isShooting = true;
            rotation = false;
            drawSight = false;
            bullet.velocity = new Vector2((float)Math.Cos(lazer.Rotation), (float)Math.Sin(lazer.Rotation)) * 5f + bullet.velocity;
            bullet.velocity.X = (float)Math.Cos(lazer.Rotation - 1.57f) * bullet.tangVelocity;
            bullet.velocity.Y = (float)Math.Sin(lazer.Rotation - 1.57f) * bullet.tangVelocity;
        }
        
        public void UpdateBullet()
        {
            for (int i = 0; i < lifeBonus.allLifeBonuses.Count(); i++)
                if (isCollision(lifeBonus.allLifeBonuses[i], bullet))
                {
                    maxAmountOfShot += lifeBonus.allLifeBonuses[i].Lifes;
                    lifeBonus.allLifeBonuses.Remove(lifeBonus.allLifeBonuses[i]);
                    i--;
                }

            for (int i = 0; i < choiceBonus.allChoiceBonuses.Count(); i++)
                if (isCollision(choiceBonus.allChoiceBonuses[i], bullet))
                {
                    bullet.GonnaBePalm = true;

                    choiceBonus.allChoiceBonuses.Remove(choiceBonus.allChoiceBonuses[i]);
                    i--;
                }

            for (int i = 0; i < sightBonus.allSightBonuses.Count(); i++)
                if (isCollision(sightBonus.allSightBonuses[i], bullet))
                {
                    bullet.GonnaBeEye = true;

                    sightBonus.allSightBonuses.Remove(sightBonus.allSightBonuses[i]);
                    i--;
                }

            bool lastSmileSight = false; 
            maxa = 1360; // устанавливаем максим расстояние от пушки до смайла, нужно для установки мишени
            foreach (Smile currSmile in smile.allSmiles)
                {
                    if (bullet.GonnaBeEye && (isCollision(ref bullet, currSmile) || bullet.Rectangle.Y >= background.Height))
                    {
                        createSmile();
                        createSight();
                        updateSight();

                        createRandomBullet();

                        bullet.IsEye = true;
                        bullet.GonnaBeEye = false;

                        tmpColorOfBullet = bullet.ColorType;                 

                        break;
                    }

                    else if (bullet.IsEye && (isCollision(ref sight, currSmile) || sight.Rectangle.Y >= background.Height))
                    {// установка мишени
                        
                        sight.velocity.X = 0;
                        sight.velocity.Y = 0;

                        drawSight = true;

                        lastSmileSight = true;
                        bullet.GonnaBeEye = false;

                        rotation = true;
                        break;
                    }

                    else if (bullet.GonnaBePalm && (isCollision(ref bullet, currSmile) || bullet.Rectangle.Y >= background.Height))
                    {
                        createSmile();                      
                        createBullet(TextureLoad.Palm, (int)bonusType.CHOICE);
                        bullet.IsPalm = true;
                        bullet.GonnaBePalm = false;
                        break;
                    }
                    else if (bullet.IsPalm && (isImposition(bullet, currSmile) || bullet.Rectangle.Y >= background.Height))
                    {
                        bullet.velocity.X = 0;
                        bullet.velocity.Y = 0;

                        int colorOfBullet = currSmile.ColorType;
                        Texture2D currTexture = TextureLoad.RedSmile;

                        for (int i = 0; i < smile.allSmiles.Count; i++)
                            if (currSmile == smile.allSmiles[i])
                            {
                                smile.allSmiles.Remove(smile.allSmiles[i]);
                                break;
                            }

                        bool currState = false;
                        TextureLoad.SetColor(colorOfBullet, ref currTexture, ref currState);
                        createBullet(currTexture, colorOfBullet);

                        currSmile.IsDead = true;
                        bullet.IsPalm = false;
                        bullet.GonnaBePalm = false;
                        isTaken = false;
                        break;
                    }

                    else if (!bullet.IsPalm && isCollision(ref bullet, currSmile) || bullet.Rectangle.Y >= background.Height)
                    {
                        createSmile();
                        createRandomBullet();
                        break;
                    }
                }

            if (lastSmileSight) bullet.IsEye = false;

            if (bullet.IsEye)
            {
                sight.Position += sight.velocity;
                sight.Rectangle = new Rectangle(sight.Rectangle.X + (int)sight.velocity.X, sight.Rectangle.Y + (int)sight.velocity.Y, sight.Width, sight.Height);

                if (sight.Rectangle.Y < background.Height - sight.Height)
                {
                    if (sight.Rectangle.X <= 0)
                    {
                        sight.Rectangle = new Rectangle(0, sight.Rectangle.Y, sight.Width, sight.Height);
                        sight.Position = new Vector2(0, sight.Rectangle.Y);
                        sight.velocity.X *= -1;
                    }

                    if (sight.Rectangle.X >= background.Width - sight.Width)
                    {
                        sight.Rectangle = new Rectangle(background.Width - sight.Width, sight.Rectangle.Y, sight.Width, sight.Height);
                        sight.Position = new Vector2(background.Width - sight.Width, sight.Rectangle.Y);
                        sight.velocity.X *= -1;
                    }

                    else if (sight.Rectangle.Y <= 0)
                    {
                        sight.Rectangle = new Rectangle(sight.Rectangle.X, 0, sight.Width, sight.Height);
                        sight.Position = new Vector2(sight.Rectangle.X, 0);
                        sight.velocity.Y *= -1;
                    }
                }
            }

            else
            {
                bullet.Position += bullet.velocity;
                bullet.Rectangle = new Rectangle(bullet.Rectangle.X + (int)bullet.velocity.X, bullet.Rectangle.Y + (int)bullet.velocity.Y, bullet.Width, bullet.Height);

                if (bullet.Rectangle.Y < background.Height/* - bullet.Height*/)
                {
                    if (bullet.Rectangle.X <= 0)
                    {
                        bullet.Rectangle = new Rectangle(0, bullet.Rectangle.Y, bullet.Width, bullet.Height);
                        bullet.Position = new Vector2(0, bullet.Rectangle.Y);
                        bullet.velocity.X *= -1;
                    }

                    else if (bullet.Rectangle.X >= background.Width - bullet.Width)
                    {
                        bullet.Rectangle = new Rectangle(background.Width - bullet.Width, bullet.Rectangle.Y, bullet.Width, bullet.Height);
                        bullet.Position = new Vector2(background.Width - bullet.Width, bullet.Rectangle.Y);
                        bullet.velocity.X *= -1;
                    }

                    else if (bullet.Rectangle.Y <= 0)
                    {
                        bullet.Rectangle = new Rectangle(bullet.Rectangle.X, 0, bullet.Width, bullet.Height);
                        bullet.Position = new Vector2(bullet.Rectangle.X, 0);
                        bullet.velocity.Y *= -1;
                    }
                }
                else if(bullet.velocity.X != 0 || bullet.velocity.Y != 0)
                {
                    bullet.velocity.X = 0;
                    bullet.velocity.Y = 0;
                    createRandomBullet();
                }
            }

        }

        void createRandomBullet()
        {
            if(smile.allSmiles.Count != 0) destroySmiles();
            else gameState = GameState.WINNER_SCREEN;

            int maxAmountOfBullet = 6;

            int colorOfBullet;
            if (rotation) colorOfBullet = tmpColorOfBullet;
            else
            {
                bool colorIsExist = false;
                do
                {
                    colorOfBullet = rand.Next(1, maxAmountOfBullet);
                    if (smile.allSmiles.Count() == 0) break;
                    for (int i = 0; i < smile.allSmiles.Count(); i++)
                        if (smile.allSmiles[i].ColorType == colorOfBullet)
                            colorIsExist = true;
                }
                while (!colorIsExist);
            }
            
            Texture2D currTexture = TextureLoad.RedSmile;
            bool currState = false;
            TextureLoad.SetColor(colorOfBullet, ref currTexture, ref currState);
            createBullet(currTexture, colorOfBullet);
        }

        void createSight()
        {
            drawSight = false;

            sight = new SightBonus(TextureLoad.Sight, new Rectangle(background.Width / 2 - sizeOfSmile / 2, background.Height - sizeOfSmile - 15, sizeOfSmile, sizeOfSmile), sizeOfSmile, sizeOfSmile, new Vector2(background.Width / 2 - sizeOfSmile / 2, background.Height - sizeOfSmile - 15));
            sight.IsMove = true;
        }

        void updateSight()
        {
            sight.velocity = new Vector2((float)Math.Cos(lazer.Rotation), (float)Math.Sin(lazer.Rotation)) * 5f + sight.velocity;
            sight.velocity.X = (float)Math.Cos(lazer.Rotation - 1.57f) * sight.tangVelocity;
            sight.velocity.Y = (float)Math.Sin(lazer.Rotation - 1.57f) * sight.tangVelocity;
        }

        void createBullet(Texture2D currTexture, int colorOfBullet)
        {
            if (smile.allSmiles.Count != 0) destroySmiles();
            else gameState = GameState.WINNER_SCREEN;

            amountOfShot++;

            if (bullet.GonnaBePalm || isTaken || bullet.GonnaBeEye)
            {
                bullet.Create(currTexture, colorOfBullet);
                isTaken = true;
            }

            else
            {
                bullet.Create(nextBullet.Texture, nextBullet.ColorType);
                nextBullet.Texture = currTexture;
                nextBullet.ColorType = colorOfBullet;
            }

            isShooting = false;

            down = true;
        }

        void createSmile()
        {
            bullet.velocity.X = 0;
            bullet.velocity.Y = 0;
            smile.allSmiles.Add(new Smile(bullet.Texture, bullet.Rectangle, bullet.Height, bullet.Width, bullet.Position, bullet.ColorType));
        }

        bool isAlone(Smile currSmile)
        {
            for (int i = 0; i < smile.allSmiles.Count(); i++)
            {
                if ((currSmile != smile.allSmiles[i]) && isCollision(currSmile, smile.allSmiles[i]))
                    return false;
            }
            return true;
        }

        bool isAlone(Bonus bonus)
        {
            for (int i = 0; i < smile.allSmiles.Count(); i++)
            {
                if (isCollision(bonus, smile.allSmiles[i]))
                    return false;
            }
            return true;
        }

        bool isAlone(Smile currSmile, List<bool> was, ref bool isYZero)
        {
            if (currSmile.Position.Y == 0) isYZero = true;
            if (isYZero) return false;

            for (int i = smile.allSmiles.Count() - 1; i >= 0; i--)
                if (!was[i] && isCollision(currSmile, smile.allSmiles[i]))
                {
                    was[i] = true;
                    if (!isAlone(smile.allSmiles[i], was, ref isYZero)) break;
                }

            if (isYZero) return false;

            return true;
        }

        public void destroySmiles()
        {
            if (isLevelWon())
                gameState = GameState.WINNER_SCREEN;

            bool everyoneIsDead = false;
            int current = smile.allSmiles.Count() - 1;
            smile.allSmiles[current].IsGoingToDie = true;
            int countDead = 0;
           
            while (!everyoneIsDead)
            {
                everyoneIsDead = true;

                for (int i = 0; i < smile.allSmiles.Count(); i++)
                {
                    if (isCollision(smile.allSmiles[i], smile.allSmiles[current]) && smile.allSmiles[i].ColorType == smile.allSmiles[current].ColorType && current != i)
                    {
                        smile.allSmiles[i].IsGoingToDie = true;
                        smile.allSmiles[current].IsDead = true;
                        countDead++;
                    }                    
                }

                for (int i = 0; i < smile.allSmiles.Count(); i++)
                {
                    if (smile.allSmiles[i].IsGoingToDie && !smile.allSmiles[i].IsDead && (i != current))
                    {
                        current = i;
                        everyoneIsDead = false;
                        break;
                    }
                }
                   
            }

            smile.allSmiles[smile.allSmiles.Count() - 1].IsGoingToDie = false;

            if (countDead > 2)
            {
                for (int i = 0; i < smile.allSmiles.Count(); i++)
                    if (smile.allSmiles[i].IsDead) deleteDeadSmiles(ref i);                
            }

            for (int i = 0; i < smile.allSmiles.Count(); i++)
            {
                List<bool> was = new List<bool>(smile.allSmiles.Count());
                for (int j = 0; j < smile.allSmiles.Count(); j++)
                    was.Add(false);
                was[i] = true;
                bool isYZero = false;

                if (isAlone(smile.allSmiles[i], was, ref isYZero))
                    deleteDeadSmiles(ref i);
            }

            for (int i = 0; i < smile.allSmiles.Count(); i++)
            {
                smile.allSmiles[i].IsDead = false;
                smile.allSmiles[i].IsGoingToDie = false;
            }

            for (int i = 0; i < lifeBonus.allLifeBonuses.Count(); i++)
                if (lifeBonus.allLifeBonuses[i].Position.Y != 0 && LifeBonus.isAlone(lifeBonus.allLifeBonuses[i]))
                {
                    lifeBonus.allLifeBonuses.Remove(lifeBonus.allLifeBonuses[i]);
                    i--;
                    maxAmountOfShot += 3;
                }
            
            for (int i = 0; i < choiceBonus.allChoiceBonuses.Count(); i++)
                if (choiceBonus.allChoiceBonuses[i].Position.Y != 0 && ChoiceBonus.isAlone(choiceBonus.allChoiceBonuses[i]))
                {
                    choiceBonus.allChoiceBonuses.Remove(choiceBonus.allChoiceBonuses[i]);
                    i--;
                }

            for (int i = 0; i < sightBonus.allSightBonuses.Count(); i++)
                if (sightBonus.allSightBonuses[i].Position.Y != 0 && SightBonus.isAlone(sightBonus.allSightBonuses[i]))
                {
                    sightBonus.allSightBonuses.Remove(sightBonus.allSightBonuses[i]);
                    i--;
                }
                
        }

        void deleteDeadSmiles(ref int i)
        {
            smile.allSmiles.Remove(smile.allSmiles[i]);
            i--;
        }

        public bool isCollision(Smile smile1, Smile smile2)
        {
            float deltaX = smile1.Position.X - smile2.Position.X;
            float deltaY = smile1.Position.Y - smile2.Position.Y;
            double r = deltaX * deltaX + deltaY * deltaY;
//            double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (r <= sizeOfSmile * sizeOfSmile + 1) return true;
//            if (r <= sizeOfSmile + 1) return true;

            return false;
        }

        public bool isCollision(Line line, Smile currSmile)
        {
            if (currSmile.Rectangle.Y >= line.Rectangle.Y) return true;
            return false;
        }

        public bool isCollision(ref Bullet bullet, Smile currSmile)
        {
            float deltaX = bullet.Position.X - currSmile.Position.X;
            float deltaY = bullet.Position.Y - currSmile.Position.Y;
            double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (r <= sizeOfSmile)
            {             
                double k = sizeOfSmile / r;
                float newX = currSmile.Position.X * (1 - (float)k) + bullet.Position.X * (float)k;
                float newY = currSmile.Position.Y * (1 - (float)k) + bullet.Position.Y * (float)k;
                bullet.Position = new Vector2(newX, newY);
                bullet.Rectangle = new Rectangle((int)newX, (int)newY, bullet.Rectangle.Width, bullet.Rectangle.Height);               
                return true;
            }
            return false;
        }

        public bool isCollision(ref SightBonus bonus, Smile currSmile)
        {
            float deltaX = bonus.Position.X - currSmile.Position.X;
            float deltaY = bonus.Position.Y - currSmile.Position.Y;
            double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (r <= sizeOfSmile)
            {
                double k = sizeOfSmile / r;
                float newX = currSmile.Position.X * (1 - (float)k) + bonus.Position.X * (float)k;
                float newY = currSmile.Position.Y * (1 - (float)k) + bonus.Position.Y * (float)k;
                bonus.Position = new Vector2(newX, newY);
                bonus.Rectangle = new Rectangle((int)newX, (int)newY, bonus.Rectangle.Width, bonus.Rectangle.Height);
                
                return true;
            }

            return false;
        }

        public bool isCollision(Bonus bonus, Smile currSmile)
        {
            int deltaX = (int)Math.Abs(bonus.Position.X - currSmile.Position.X);
            int deltaY = (int)Math.Abs(bonus.Position.Y - currSmile.Position.Y);

            if (Math.Sqrt(deltaX * deltaX + deltaY * deltaY) <= sizeOfSmile) return true;
            return false;
        }

        public bool isImposition(Bullet bullet, Smile currSmile)
        {
            int deltaX = (int)Math.Abs(bullet.Position.X - currSmile.Position.X);
            int deltaY = (int)Math.Abs(bullet.Position.Y - currSmile.Position.Y);

            if (Math.Sqrt(deltaX * deltaX + deltaY * deltaY) <= sizeOfSmile / 2 + sizeOfSmile / 4) return true;
            return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (gameState == GameState.MENU_SCREEN)
            {
                MenuScreen.Draw(spriteBatch);
            }
            else if (gameState == GameState.GAME_SCREEN)
            {
                background.Draw(spriteBatch);

                line.Draw(spriteBatch);

                foreach (Smile currSmile in smile.allSmiles)
                    currSmile.Draw(spriteBatch);

                foreach (LifeBonus currLife in lifeBonus.allLifeBonuses)
                    currLife.Draw(spriteBatch);

                foreach (ChoiceBonus choice in choiceBonus.allChoiceBonuses)
                    choice.Draw(spriteBatch);

                foreach (SightBonus currSight in sightBonus.allSightBonuses)
                    currSight.Draw(spriteBatch);

                lazer.Draw(spriteBatch);
                gun.Draw(spriteBatch);

                if (drawSight) sight.Draw(spriteBatch);

                bullet.Draw(spriteBatch);

                paper.Draw(spriteBatch);

                nextBullet.Draw(spriteBatch);

                life.Draw(spriteBatch);

                int str = maxAmountOfShot - amountOfShot;
                spriteBatch.DrawString(font, str.ToString(), new Vector2(paper.Width / 2 + 20, background.Height - 2 * smile.Size - 25 + 16), Color.Black);

                spriteBatch.DrawString(font, "Next:", new Vector2(60, background.Height - 2 * smile.Size - 25 + smile.Size + 30), Color.Black);
            }

            else if (gameState == GameState.GAME_OVER_SREEN)
            {
                GameOverScreen.Draw(spriteBatch);
            }

            else if (gameState == GameState.WINNER_SCREEN)
            {
                WinnersScreen.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
