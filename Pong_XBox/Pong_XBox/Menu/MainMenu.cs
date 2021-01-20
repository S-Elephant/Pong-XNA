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
    public class MainMenu : BaseMenuBarred
    {
        #region Members
        private SpriteFont TitleFont = Common.str2Font("Title");
        #endregion

        public MainMenu():
            base(Engine.Instance,null)
        {
            //Texture = Common.str2Tex("dodgeSplash");
            Engine.Audio.PlayMusic("mainMenuMusic", true, Audio.eMusicOptions.Override);

            AddChoice("start", "Start");
            AddChoice("options", "Options");
            AddChoice("credits", "Credits");
            AddChoice("exit", "Exit");
            OnEnterChoice += new OnEnterChoiceEventHandler(MainMenu_OnEnterChoice);
        }

        void MainMenu_OnEnterChoice(ChoiceBarred choice)
        {
            switch (choice.Name)
            {
                case "start":
                    Engine.Instance.ActiveState = new PSelect(this);
                    break;
                case "options":
                    Engine.Instance.ActiveState = new Options(this);
                    break;
                case "credits":
                    Engine.Instance.ActiveState = new Credits(this);
                    break;
                case "exit":
                    Engine.Instance.Game.Exit();
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();

            //Engine.Instance.SpriteBatch.DrawString(TitleFont, "How many players will be playing? [1-2]", Common.CenterStringX(TitleFont, "How many players will be playing? [1-2]", Engine.Instance.Width, 25), Color.Yellow);
            //Engine.Instance.SpriteBatch.DrawString(TitleFont, "Press [C] for credits", Common.CenterStringX(TitleFont, "Press [C] for credits", Engine.Instance.Width, Engine.Instance.Height - 50), Color.Yellow);
        }
    }
}
