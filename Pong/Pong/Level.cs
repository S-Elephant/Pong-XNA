using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace Pong
{
    public class Level : IActiveState
    {
        #region Members

        public static Rectangle BallArea { get { return new Rectangle(66, BorderHeight - BorderOffsetY - (BorderExtraIncrease/2), Engine.Instance.Width - 132, Engine.Instance.Height - (2 * BorderHeight) + (BorderOffsetY * 2) + BorderExtraIncrease); } }

        private List<Player> m_Players = new List<Player>();
        public List<Player> Players
        {
            get { return m_Players; }
            set { m_Players = value; }
        }

        public enum eGameState { PreStart, Playing }
        private eGameState m_GameState = eGameState.PreStart;
        private eGameState GameState
        {
            get { return m_GameState; }
            set { m_GameState = value; }
        }

        private SpriteFont m_PausedFont = Common.str2Font("Paused");
        private SpriteFont PausedFont
        {
            get { return m_PausedFont; }
            set { m_PausedFont = value; }
        }

        private Ball m_Ball = null;
        public Ball Ball
        {
            get { return m_Ball; }
            set { m_Ball = value; }
        }

        public static Level Instance;
        static readonly Texture2D BG = Common.str2Tex("BG/stars01");
        static readonly List<Texture2D> Borders = new List<Texture2D>()
        {
            Common.str2Tex("beam01"),
            Common.str2Tex("beam02")
        };
        private Texture2D ActiveBorder;
        const int BorderOffsetY = 25;
        const int BorderHeight = 81;
        const int BorderExtraIncrease = 32;
        #endregion

        public Level(int playerCnt, eAIMode aiMode)
        {
            // Add players
            Players.Add(new Player(PlayerIndex.One));
            Players.Add(new Player(PlayerIndex.Two));
            if (playerCnt == 1)
            {
                Players[1].Ishuman = false;
                Players[1].AIMode = aiMode;
            }
            ResetBall();

            // Border
            ActiveBorder = Borders[Maths.RandomNr(0, Borders.Count - 1)];
        }

        public void ResetBall()
        {
            Engine.Instance.Audio.PlaySound("cheer");
            Ball = new Ball();
        }

        public void Update(GameTime gameTime)
        {
            if (GameState == eGameState.Playing)
            {
                Players.ForEach(p => p.Update(gameTime)); // update paddles before the ball
                Ball.Update(gameTime);
            }
            else if (GameState == eGameState.PreStart)
            {
                if (Engine.Instance.KB.KeyIsReleased(Keys.Space))
                {
                    Ball = new Ball();
                    GameState = eGameState.Playing;
                }
            }

            if (Engine.Instance.KB.KeyIsReleased(Keys.Escape))
                Engine.Instance.Game.Exit();
        }

        public void Draw()
        {
            Engine.Instance.SpriteBatch.Draw(BG, Vector2.Zero, Color.White);
            //Engine.Instance.SpriteBatch.Draw(Common.White1px50Trans, BallArea, Color.White);
            Engine.Instance.SpriteBatch.Draw(ActiveBorder, new Rectangle(0, -BorderOffsetY, Engine.Instance.Width, BorderHeight), Color.White);
            Engine.Instance.SpriteBatch.Draw(ActiveBorder, new Rectangle(0, BorderOffsetY + Engine.Instance.Height - BorderHeight, Engine.Instance.Width, BorderHeight), Color.White);
            Ball.Draw();
            Players.ForEach(p => p.Draw());

            if (GameState == eGameState.PreStart)
                Engine.Instance.SpriteBatch.DrawString(PausedFont, "Press SpaceBar to start", Common.CenterString(PausedFont, "Press SpaceBar to start", Engine.Instance.Width, Engine.Instance.Height), Color.Yellow);
        }
    }
}
