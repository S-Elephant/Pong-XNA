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
    public class Sprite
    {
        private Texture2D m_Texture;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        private bool m_IsDisposed = false;
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            set { m_IsDisposed = value; }
        }

        private Rectangle m_DrawRect;
        public Rectangle DrawRect
        {
            get { return m_DrawRect; }
            set { m_DrawRect = value; }
        }

        private Color m_DrawColor = Color.White;
        public Color DrawColor
        {
            get { return m_DrawColor; }
            set { m_DrawColor = value; }
        }

        private Vector2 m_Location;
        public Vector2 Location
        {
            get { return m_Location; }
            set { m_Location = value;
               // if (Texture != null)
                    DrawRect = new Rectangle((int)value.X, (int)value.Y, Texture.Width, Texture.Height);
            }
        }

        public Sprite(Vector2 location, Texture2D texture)
        {
            Texture = texture; // Always set texture first becaues the location has a setter that uses the texture
            Location = location;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DrawRect, DrawColor);
        }
    }
}
