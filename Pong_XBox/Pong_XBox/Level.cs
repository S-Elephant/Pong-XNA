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
        private List<Sprite> m_Sprites = new List<Sprite>();
        public List<Sprite> Sprites
        {
            get { return m_Sprites; }
            set { m_Sprites = value; }
        }

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
        #endregion

        public Level(int playerCnt)
        {
            // Add players
            Players.Add(new Player(PlayerIndex.One));
            Players.Add(new Player(PlayerIndex.Two));
            if (playerCnt == 1)
                Players[1].Ishuman = false;
            Players.ForEach(p => Sprites.Add(p.Paddle));
        }

        public void Goal()
        {
            Engine.Audio.PlayFX("cheer");
            Ball = new Ball();
            Sprites.Add(Ball);
        }

        public void Update(GameTime gameTime)
        {
            GamePadState gpState = GamePad.GetState(PlayerIndex.One);
            if (GameState == eGameState.Playing)
            {
                Sprites.ForEach(s => s.Update(gameTime));
                Sprites.RemoveAll(s => s.IsDisposed);
                Players.ForEach(p => p.Update(gameTime));
            }
            else if (GameState == eGameState.PreStart)
            {
                if(gpState.IsButtonDown(Buttons.A))
                {
                    Ball = new Ball();
                    Sprites.Add(Ball);
                    GameState = eGameState.Playing;
                }
            }

            if (gpState.IsButtonDown(Buttons.B))
                Engine.Instance.Game.Exit();
        }

        public void Draw()
        {
            Sprites.ForEach(s => s.Draw(Engine.Instance.SpriteBatch));
            Players.ForEach(p => p.Draw(Engine.Instance.SpriteBatch));

            if (GameState == eGameState.PreStart)
                Engine.Instance.SpriteBatch.DrawString(PausedFont, "Press SpaceBar to start", Common.CenterString(PausedFont, "Press SpaceBar to start", Engine.Instance.Width, Engine.Instance.Height), Color.Yellow);
        }
    }
}
