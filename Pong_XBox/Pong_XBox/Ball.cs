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
    public class Ball : Sprite
    {
        private const int MAX_VELOCITY = 14; // Set maximum to prevent collision-tunneling
        private Vector2 m_BallVelocity = Vector2.Zero;
        private Vector2 Velocity
        {
            get { return m_BallVelocity; }
            set { m_BallVelocity = new Vector2(MathHelper.Clamp(value.X, -MAX_VELOCITY, MAX_VELOCITY), MathHelper.Clamp(value.Y, -MAX_VELOCITY, MAX_VELOCITY)); }
        }

        private TimeSpan m_LastPaddleCollision = new TimeSpan();
        private TimeSpan LastPaddleCollision1
        {
            get { return m_LastPaddleCollision; }
            set { m_LastPaddleCollision = value; }
        }

        private Player m_LastTouchedPlayer = null;
        private Player LastTouchedPlayer
        {
            get { return m_LastTouchedPlayer; }
            set { m_LastTouchedPlayer = value; }
        }

        private Rectangle m_SourceRect;
        private Rectangle SourceRect
        {
            get { return m_SourceRect; }
            set { m_SourceRect = value; }
        }

        public Ball():
            base(Vector2.Zero,Common.str2Tex("ball"))
        {
            InitBall();
            SourceRect = new Rectangle(0, 0, Common.str2Tex("ball").Width, Common.str2Tex("ball").Height);
        }

        private void InitBall()
        {
            // Randomize initial direction
            const int speed = 5;
            switch (Maths.RandomNr(1, 4))
            {
                case 1: Velocity = new Vector2(speed, speed); break;
                case 2: Velocity = new Vector2(-speed, speed); break;
                case 3: Velocity = new Vector2(speed, -speed); break;
                case 4: Velocity = new Vector2(-speed, -speed); break;
            }

            Location = new Vector2(Engine.Instance.Width / 2 - Texture.Width / 2, Engine.Instance.Height / 2 - Texture.Height / 2);
        }

        private void DoCollision()
        {
            // Level boundaries
            if (Location.Y < 0 || Location.Y + Texture.Height > Engine.Instance.Height)
            {
                Velocity = new Vector2(Velocity.X, -Velocity.Y);
                Engine.Audio.PlayFX("hitBoundary");
            }

            // Player bars
            if (LastPaddleCollision1.TotalMilliseconds >= 1500)
            {
                foreach (Player player in Engine.Level.Players)
                    if (DrawRect.Intersects(player.Paddle.DrawRect))
                    {
                        ChangeVelocity(1);
                        LastPaddleCollision1 = new TimeSpan();
                        Velocity = new Vector2(-Velocity.X, Velocity.Y);
                        Engine.Audio.PlayFX("hitBar");
                        LastTouchedPlayer = player;

                        // Vertical collision ball to paddle handling
                        if (Math.Abs(player.Paddle.DrawRect.Top - DrawRect.Bottom) <= Math.Abs(Velocity.Y) ||
                            Math.Abs(player.Paddle.DrawRect.Bottom - DrawRect.Top) <= Math.Abs(Velocity.Y))
                            Velocity = new Vector2(Velocity.X, -Velocity.Y);
                    }
            }

            // Scored?
            bool scored = false;
            if (Location.X < 0 - Texture.Width)
            {
                scored = true;
                Engine.Level.Players[0].Score--;
            }
            else if (Location.X > Engine.Instance.Width)
            {
                scored = true;
                Engine.Level.Players[1].Score--;
            }
            if (scored)
            {
                IsDisposed = true;
                Engine.Level.Goal();
                if (LastTouchedPlayer != null)
                    LastTouchedPlayer.Score++;
            }
        }

        public void ChangeVelocity(int amount)
        {
            Vector2 newVelocity = Velocity;
            if (Velocity.X < 0)
                newVelocity.X += -amount;
            else
                newVelocity.X += amount;
            if (Velocity.Y < 0)
                newVelocity.Y += -amount;
            else
                newVelocity.Y += amount;
            Velocity = newVelocity;
        }

        public void Move()
        {
            Move(1);
        }

        public void Move(int step)
        {
            for (int i = 1; i <= step; i++)
                Location = Location + new Vector2(Velocity.X, Velocity.Y);
        }

        public override void Update(GameTime gameTime)
        {
            LastPaddleCollision1 += gameTime.ElapsedGameTime;
            Move();

            DoCollision();

            base.Update(gameTime);
        }
       
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
