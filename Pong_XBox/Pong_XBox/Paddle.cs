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
    public class Paddle : Sprite
    {
        private PlayerIndex m_PlayerIdx;
        public PlayerIndex PlayerIdx
        {
            get { return m_PlayerIdx; }
            set { m_PlayerIdx = value; }
        }

        const int EDGE_OFFSET = 50;

        public Paddle(PlayerIndex playerIdx)
            : base(Vector2.Zero, Common.str2Tex("verticalBar"))
        {
            PlayerIdx = playerIdx;

            switch (PlayerIdx)
            {
                case PlayerIndex.One:
                    Location = new Vector2(EDGE_OFFSET, Engine.Instance.Height / 2 - Common.str2Tex("verticalBar").Height / 2);
                    break;
                case PlayerIndex.Two:
                    Location = new Vector2(Engine.Instance.Width - EDGE_OFFSET - Common.str2Tex("verticalBar").Width, Engine.Instance.Height / 2 - Common.str2Tex("verticalBar").Height / 2);
                    break;
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// Call after changing the resolution
        /// </summary>
        public void UpdatePositionFromEdge()
        {
            switch (PlayerIdx)
            {
                case PlayerIndex.One:
                    Location = new Vector2(EDGE_OFFSET, Location.Y);
                    break;
                case PlayerIndex.Two:
                    Location = new Vector2(Engine.Instance.Width - EDGE_OFFSET, Location.Y);
                    break;
                /*
            case PlayerIndex.Three:
                break;
            case PlayerIndex.Four:
                break;
                */
                default:
                    throw new Exception();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
