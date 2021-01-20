using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;
using XNALib.ParticleEngine2D;

namespace Pong
{
    public class Ball : IXNADispose
    {
        float Velocity = Maths.RandomNr(5,7);

        private Vector2 m_MoveDir;
        public Vector2 MoveDir
        {
            get { return m_MoveDir; }
            private set { m_MoveDir = value; }
        }       
        
        private bool m_IsDisposed = false;
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            set { m_IsDisposed = value; }
        }

        private Player m_LastTouchedPlayer = null;
        private Player LastTouchedPlayer
        {
            get { return m_LastTouchedPlayer; }
            set { m_LastTouchedPlayer = value; }
        }

        public Ball()
        {
            InitBall();
        }

        Circle CollisionCircle;
        SimpleTimer BallTOTimer = new SimpleTimer(5000);

        public const int SIZE = 48;

        SimpleASprite Animation;

        public Vector2 Location
        {
            get { return CollisionCircle.TopLeft; }
            set
            {
                CollisionCircle.Center = value + halfSizeV2;
                Animation.Location = value;
                PE.EmitterLocation = value +halfSizeV2;
            }
        }

            readonly Vector2 halfSizeV2 = new Vector2(SIZE / 2, SIZE / 2);
        
        readonly static List<Texture2D> ParticleTextures = new List<Texture2D>()
        {
            Common.str2Tex("Particles/figure1")
            //Common.str2Tex("Particles/circle"),Common.str2Tex("Particles/figure1"),Common.str2Tex("Particles/figure1"),Common.str2Tex("Particles/figure1"),Common.str2Tex("Particles/star")
        };
        readonly static Color[] ParticleColors = new Color[] { Color.Blue, new Color(128,128,128,128), Color.Red, Color.Yellow };

        ParticleEngine PE;

        private void InitBall()
        {
            // Randomize initial direction
            switch (Maths.RandomNr(1, 4))
            {
                case 1: MoveDir = new Vector2(1, 1); break;
                case 2: MoveDir = new Vector2(-1, 1); break;
                case 3: MoveDir = new Vector2(1, -1); break;
                case 4: MoveDir = new Vector2(-1, -1); break;
            }

            // Set start location
            Vector2 topLeftLocation = new Vector2(Engine.Instance.Width / 2 - SIZE / 2, Engine.Instance.Height / 2 - SIZE / 2);
            Animation = new SimpleASprite(topLeftLocation, "Ball/ball01", SIZE, SIZE, 1, 1, int.MaxValue);

            // Create collision circle
            CollisionCircle = new Circle(topLeftLocation - halfSizeV2, SIZE / 2);

            PE = new ParticleEngine(ParticleTextures, CollisionCircle.Center, 4, false) { ParticleTTLMin = 10, ParticleTTLMax = 20 };
            ChangeTexture();

            Location = topLeftLocation;
        }

        void ChangeTexture()
        {
            Animation.Texture = Common.str2Tex("Ball/ball0" + Maths.RandomNr(1, 4).ToString());
            PE.ParticleDrawColor = ParticleColors[Maths.RandomNr(0, ParticleColors.Length-1)];
        }

        private void HandleCollision()
        {
            // Level boundaries
            if (Location.Y < Level.BallArea.Top || Location.Y + SIZE > Level.BallArea.Bottom)
            {
                ChangeTexture();
                MoveDir = new Vector2(MoveDir.X, -MoveDir.Y);
                Engine.Instance.Audio.PlaySound("blip02");
            }

            // Paddles
            const float CornerDistance = 2f;
            foreach (Player player in Level.Instance.Players)
            {
                if (CollisionCircle.Collide(player.Paddle.CollisionRect))
                {
                    Vector2 depth = Collision.GetIntersectionDepth(CollisionCircle.AABB, player.Paddle.CollisionRect);

                    ChangeTexture();

                    // Reset timeout
                    BallTOTimer.Reset();

                    // Adjust MoveDir X
                    MoveDir = new Vector2(-MoveDir.X, MoveDir.Y);
                    /*
                    // Adjust movedir Y
                    if (CollisionCircle.Center.Y > player.Paddle.Animation.FrameColRect.CenterVector().Y)
                    { // Ball is below the paddles center

                        if ((player.Paddle.Animation.FrameColRect.Bottom - CollisionCircle.Top <= CornerDistance))
                            MoveDir = new Vector2(MoveDir.X, -MoveDir.Y);
                    }
                    else
                    { // Ball is above the paddles center
                        if ((CollisionCircle.Bottom - player.Paddle.Animation.FrameColRect.Top <= CornerDistance))
                            MoveDir = new Vector2(MoveDir.X, -MoveDir.Y);
                    }
                    */
                    // Audio
                    Engine.Instance.Audio.PlaySound("blip01");
                    
                    // Set last touched player
                    LastTouchedPlayer = player;

                    // Correction
                    depth = depth.Abs();
                    if (depth.X < depth.Y)
                    {
                        if (player.PlayerIdx == PlayerIndex.One)
                            Location = new Vector2(Location.X + depth.X, Location.Y);
                        else
                            Location = new Vector2(Location.X - depth.X, Location.Y);
                    }
                    else
                    {
                        if (Animation.CenterLocation.Y < player.Paddle.CollisionRect.CenterVector().Y)
                        {
                            Location = new Vector2(Location.X, Location.Y - depth.Y); // Ball is above paddle
                            if (MoveDir.Y > 0)
                                MoveDir = new Vector2(MoveDir.X, -MoveDir.Y);
                        }
                        else
                        {
                            Location = new Vector2(Location.X, Location.Y + depth.Y); // ball is below paddle
                            if (MoveDir.Y < 0)
                                MoveDir = new Vector2(MoveDir.X, -MoveDir.Y);
                        }
                    }
                    Move();
                    break;
                }
            }

            // Scored?
            bool scored = false;
            if (Location.X < 0 - SIZE)
            {
                scored = true;
                Level.Instance.Players[0].Score--;
            }
            else if (Location.X > Engine.Instance.Width)
            {
                scored = true;
                Level.Instance.Players[1].Score--;
            }
            if (scored)
            {
                IsDisposed = true;
                Level.Instance.ResetBall();
                if (LastTouchedPlayer != null)
                    LastTouchedPlayer.Score++;
            }
        }     

        public void Move()
        {
            Location += MoveDir * Velocity;
        }

        public void Update(GameTime gameTime)
        {
            Move();

            HandleCollision();

            // Timeout
            BallTOTimer.Update(gameTime);
            if (BallTOTimer.IsDone)
            {
                IsDisposed = true;
                Level.Instance.ResetBall();
            }

            Animation.Update(gameTime);
            PE.Update();
        }
       
        public void Draw()
        {
            PE.Draw(Engine.Instance.SpriteBatch);
            Animation.Draw(Engine.Instance.SpriteBatch);
            //CollisionCircle.DrawPrimitive(Engine.Instance.SpriteBatch, Color.Red);
        }
    }
}
