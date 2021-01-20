using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;
using XNALib.Menu;

namespace Pong
{
    public class GameOptions : BaseMenuBarred
    {
        const string Title = "Set options";
        readonly SpriteFont Font = Common.str2Font("MenuTitle");

#if WINDOWS
        public GameOptions() :
            base(Engine.Instance, null, Engine.Instance.KB)
#elif XBOX
        public GameOptions(int playerCnt) :
            base(Engine.Instance, null)
#endif
        {
            ChoiceDrawColor = Color.White;
            AddChoice("play", "Play");
            AddChoice("aiMode", "AI Mode: ", eAIMode.FollowBall, eAIMode.Center);
            AddChoice("playerCnt", "Player count: ", "1", "2");
            AddChoice("back", "Back");
            OnEnterChoice += new OnEnterChoiceEventHandler(MainMenu_OnEnterChoice);
        }

        void MainMenu_OnEnterChoice(ChoiceBarred choice)
        {
            if (choice.Name == "back")
                Engine.Instance.ActiveState = new MainMenu();
            else if (choice.Name == "play")
            {
                Level.Instance = new Level(int.Parse(ChoiceValues["playerCnt"]), (eAIMode)Enum.Parse(typeof(eAIMode), ChoiceValues["aiMode"], true));
                Engine.Instance.ActiveState = Level.Instance;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
            Engine.Instance.SpriteBatch.DrawString(Font, Title, new Vector2(Engine.Instance.Width / 2 - Font.MeasureString(Title).X / 2, 40), Color.White);
        }
    }
}