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
    public class Player
    {
        #region Members
        private int m_Score = 0;
        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }
        private bool m_Ishuman = true;
        public bool Ishuman
        {
            get { return m_Ishuman; }
            set { m_Ishuman = value; }
        }

        private Paddle m_Paddle;
        public Paddle Paddle
        {
            get { return m_Paddle; }
            set { m_Paddle = value; }
        }

        private PlayerIndex m_PlayerIdx;
        public PlayerIndex PlayerIdx
        {
            get { return m_PlayerIdx; }
            set { m_PlayerIdx = value; }
        }

        private SpriteFont m_GuiFont = Common.str2Font("Gui");
        private SpriteFont GuiFont
        {
            get { return m_GuiFont; }
            set { m_GuiFont = value; }
        }

        private Dictionary<PlayerIndex, Color> m_PlayerColors = new Dictionary<PlayerIndex, Color>() { { PlayerIndex.One, Color.White }, { PlayerIndex.Two, Color.Green }, { PlayerIndex.Three, Color.Blue }, { PlayerIndex.Four, Color.Purple } };
        private Dictionary<PlayerIndex, Color> PlayerColors
        {
            get { return m_PlayerColors; }
            set { m_PlayerColors = value; }
        }
        #endregion

        public Player(PlayerIndex playerIdx)
        {
            PlayerIdx = playerIdx;
            Paddle = new Paddle(PlayerIdx);
            Paddle.DrawColor = PlayerColors[PlayerIdx];
        }

        public void Update(GameTime gameTime)
        {
            if (Ishuman)
            {
                const int PLAYER_PADDLE_SPEED = 9;

                #region MoveBar
                Vector2 oldBarPos = Paddle.Location;
                Vector2 moveVector = Vector2.Zero;

                if (GPInput.MoveUp(PlayerIdx))
                    moveVector = new Vector2(0, -PLAYER_PADDLE_SPEED);
                if (GPInput.MoveDown(PlayerIdx))
                    moveVector = new Vector2(0, PLAYER_PADDLE_SPEED);

                // Move
                Paddle.Location = Paddle.Location + moveVector;
                // Stay within level boundaries
                Paddle.Location = new Vector2(Paddle.Location.X, MathHelper.Clamp(Paddle.Location.Y, 0, Engine.Instance.Height - Paddle.Texture.Height));
                #endregion
            }
            else // AI
            {
                const int AI_PADDLE_SPEED = 14;
                int BarCenterY = (int)Paddle.Location.Y + Paddle.Texture.Height / 2;
                int adjust = 0;
                adjust = (int)MathHelper.Clamp(Engine.Level.Ball.Location.Y - Engine.Level.Ball.Texture.Height / 2 - BarCenterY, -AI_PADDLE_SPEED, AI_PADDLE_SPEED);
                Paddle.Location = new Vector2(Paddle.Location.X, Paddle.Location.Y + adjust);
                Paddle.Location = new Vector2(Paddle.Location.X, MathHelper.Clamp(Paddle.Location.Y, 0, Engine.Instance.Height - Paddle.Texture.Height));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (PlayerIdx)
            {
                case PlayerIndex.One:
                    spriteBatch.DrawString(GuiFont, string.Format("Score: {0}", Score), new Vector2(10, 10), PlayerColors[PlayerIdx]);
                    break;
                case PlayerIndex.Two:
                    spriteBatch.DrawString(GuiFont, string.Format("Score: {0}", Score), new Vector2(Engine.Instance.Width - 150, 10), PlayerColors[PlayerIdx]);
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}
