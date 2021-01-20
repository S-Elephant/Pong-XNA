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
    public class PSelect : BaseMenuBarred
    {
        public PSelect(IActiveState parent) :
            base(Engine.Instance, parent)
        {
            AddChoice("1", "1 Player");
            AddChoice("2", "2 Players");
            OnEnterChoice += new OnEnterChoiceEventHandler(PSelect_OnEnterChoice);
        }

        void PSelect_OnEnterChoice(ChoiceBarred choice)
        {
            switch (choice.Name)
            {
                case "1":
                     Engine.Level = new Level(1);
                Engine.Instance.ActiveState = Engine.Level;
                Engine.Audio.StopAllMusic();
                    break;
                case "2":
                     Engine.Level = new Level(2);
                Engine.Instance.ActiveState = Engine.Level;
                Engine.Audio.StopAllMusic();
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }
    }
}
