using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Résolution de la fenêtre 800x480

namespace DoodleJump
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;

        Texture2D platformSprite;
        Texture2D persoSprite;
        Texture2D springSprite;

        Vector2 persoPos = new Vector2(400, 360);

        const int persoY = 60;
        const int persoX = 62;
        const int platfY = 15;
        const int platfX = 57;

        Vector2[] platfPos = new Vector2[20];
        Random platfRand = new Random();

        bool jumpTop = false;
        bool jumpBottom = true;

        const float defaultSpeedUp = 7;
        const float defaultSpeedDown = 0.5f;
        const int jumpHeight = 160;

        float speedUp = defaultSpeedUp;
        float speedDown = defaultSpeedDown;
        float speedSide = 4;

        int score = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);

        }

        protected override void Initialize()
        {
            for (int i = 0; i < 20; i++)
            {
                platfPos[i] = new Vector2(0 + (platfX / 2), platfY * i);
            }
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            persoSprite = Content.Load<Texture2D>("lik-right");
            platformSprite = Content.Load<Texture2D>("platform");
            font = Content.Load<SpriteFont>("sample");
        }

        protected override void Update(GameTime gameTime)
        {
                if (persoPos.Y >= 200 && jumpTop == false) //monter du jump
                {
                    persoPos.Y -= speedUp;
                    if (speedUp > 0.2)
                    {
                        speedUp -= 0.1f;
                    }
                }
                else if (persoPos.Y <= 360 && jumpBottom == false) // descente du jump
                {
                    persoPos.Y += speedDown;
                    speedDown += 0.2f;
                }

                if (persoPos.Y <= 200) // switch jump direction to down
                {
                    persoSprite = Content.Load<Texture2D>("lik-right-odskok");
                    jumpTop = true;
                    jumpBottom = false;
                    speedUp = defaultSpeedUp;
                }
                else if (persoPos.Y >= 360) // switch jump direction to up
                {
                    persoSprite = Content.Load<Texture2D>("lik-right");
                    jumpBottom = true;
                    jumpTop = false;
                    speedDown = defaultSpeedDown;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left) == true) // Left movement
                {
                    if (persoPos.X >= 250)
                    {
                        if (jumpBottom == true)
                        {
                            persoSprite = Content.Load<Texture2D>("lik-left");
                        }
                        else
                        {
                            persoSprite = Content.Load<Texture2D>("lik-left-odskok");
                        }
                        persoPos.X -= speedSide;
                    }
                }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true) // Right movement
            {
                if (persoPos.X <= 550)
                {
                    if (jumpBottom == true)
                    {
                        persoSprite = Content.Load<Texture2D>("lik-right");
                    }
                    else
                    {
                        persoSprite = Content.Load<Texture2D>("lik-right-odskok");
                    }
                    persoPos.X += speedSide;
                }
            }

            if (jumpBottom == false)
            {
                bool test = false;
                for (int i = 0; i < 20; i ++)
                {
                    test = false;
                    Vector2 temp = new Vector2(platfRand.Next(250, 550), platfRand.Next(platfY, 480));
                    for (int j = 0; j < 20;j ++)
                    {
                        if (temp.X + platfX < platfPos[j].X && temp.X > platfPos[j].X + platfX && temp.Y + platfY < platfPos[j].Y && temp.Y > platfPos[j].Y + platfY)
                        {
                            test = true;
                        }
                    }
                    if (test == true)
                    {
                        platfPos[i] = temp;
                    }
                    else
                    {
                        
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);
            _spriteBatch.Begin();

            //Lines in the background
            Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            for (int i=20; i<=1000; i = i + 20) { 
                _spriteBatch.Draw(pixel, new Rectangle(0, i, 10000, 2),Color.LightGray);
                _spriteBatch.Draw(pixel, new Rectangle(i, 0, 2, 1000), Color.LightGray);
            }
            //Start platform
            _spriteBatch.Draw(platformSprite, new Vector2(400 - (platfX / 2), 360 + (platfY / 2)), Color.White);
            //Perso
            _spriteBatch.Draw(persoSprite, new Vector2(persoPos.X - (persoX/2), persoPos.Y - persoY), Color.White);
            //Score
            _spriteBatch.DrawString(font, ("Score : " + score.ToString()), new Vector2(20, 7), Color.Black);
            //20 platform
            for (int i = 0; i < 20; i++) 
            {
                _spriteBatch.Draw(platformSprite, new Vector2(platfPos[i].X - (platfX / 2), platfPos[i].Y + (platfY / 2)), Color.White);
            }


            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}