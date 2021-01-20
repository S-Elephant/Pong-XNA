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
    public class MainMenuold : IActiveState
    {
        #region Members
        private Texture2D m_Texture;
        private Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }
        private SpriteFont m_TitleFont = Common.str2Font("Title");
        private SpriteFont TitleFont
        {
            get { return m_TitleFont; }
            set { m_TitleFont = value; }
        }
        #endregion

        public MainMenuold()
        {
            //Texture = Common.str2Tex("dodgeSplash");
            Engine.Instance.Audio.PlayMusic("mainMenu");
        }

        public void Update(GameTime gameTime)
        {
            if (Engine.Instance.KB.KeyIsReleased(Keys.D1))
            {
                //Level.Instance = new Level(1);
                Engine.Instance.ActiveState = Level.Instance;
                Engine.Instance.Audio.StopAllMusic();
            }
            else if (Engine.Instance.KB.KeyIsReleased(Keys.D2))
            {
              //  Level.Instance = new Level(2);
                Engine.Instance.ActiveState = Level.Instance;
                Engine.Instance.Audio.StopAllMusic();
            }
            else if (Engine.Instance.KB.KeyIsReleased(Keys.C))
            {
                Engine.Instance.ActiveState = new Credits();
            }
            else if (Engine.Instance.KB.KeyIsReleased(Keys.Escape))
                Engine.Instance.Game.Exit();
        }

        public void Draw()
        {
            Engine.Instance.SpriteBatch.DrawString(TitleFont, "How many players will be playing? [1-2]", Common.CenterStringX(TitleFont, "How many players will be playing? [1-2]", Engine.Instance.Width, 25), Color.Yellow);
            Engine.Instance.SpriteBatch.DrawString(TitleFont, "Press [C] for credits", Common.CenterStringX(TitleFont, "Press [C] for credits", Engine.Instance.Width, Engine.Instance.Height - 50), Color.Yellow);
        }
    }
}
