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
    public class Paddle
    {
        private PlayerIndex m_PlayerIdx;
        public PlayerIndex PlayerIdx
        {
            get { return m_PlayerIdx; }
            set { m_PlayerIdx = value; }
        }

        private SimpleASprite Animation;

        public Vector2 Location
        {
            get { return Animation.Location; }
            set { Animation.Location = value; }
        }

        public Rectangle CollisionRect { get { return Misc.RectFromCenter(Animation.FrameColRect.CenterVector(), 16, 64); } }

        public static List<Texture2D> PaddleTextures = new List<Texture2D>()
        {
            Common.str2Tex("Paddle/paddle01_1"),
            Common.str2Tex("Paddle/paddle01_2"),
            Common.str2Tex("Paddle/paddle01_3"),
            Common.str2Tex("Paddle/paddle01_4"),
            Common.str2Tex("Paddle/paddle01_5"),
            Common.str2Tex("Paddle/paddle01_6")
        };

        public Paddle(PlayerIndex playerIdx)            
        {
            PlayerIdx = playerIdx;
            Animation = new SimpleASprite(Common.InvalidVector2, PaddleTextures[Maths.RandomNr(0, PaddleTextures.Count - 1)], PaddleTextures[0].Width, PaddleTextures[0].Height, 1, 1, int.MaxValue);

            switch (PlayerIdx)
            {
                case PlayerIndex.One:
                    Animation.Location = new Vector2(Level.BallArea.Left-CollisionRect.Width, Engine.Instance.Height / 2 - Animation.FrameSize.Y / 2);
                    break;
                case PlayerIndex.Two:
                    Animation.Location = new Vector2(Level.BallArea.Right - PaddleTextures[0].Width, Engine.Instance.Height / 2 - Animation.FrameSize.Y / 2);
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
                    Animation.Location = new Vector2(Level.BallArea.Left-Animation.FrameSize.X, Animation.Location.Y);
                    break;
                case PlayerIndex.Two:
                    Animation.Location = new Vector2(Level.BallArea.Right, Animation.Location.Y);
                    break;
                default:
                    throw new Exception();
            }
        }

        public void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }

        public void Draw()
        {
            Animation.Draw(Engine.Instance.SpriteBatch);
            //Engine.Instance.SpriteBatch.Draw(Common.White1px50Trans, CollisionRect, Color.Purple);
        }
    }
}
