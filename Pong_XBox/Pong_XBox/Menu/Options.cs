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
    public class Options : BaseMenuBarred
    {
        public Options(IActiveState parent) :
            base(Engine.Instance, parent)
        {
            // Resolution
            List<string> resolutions = new List<string>();
            foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                resolutions.Add(string.Format("{0}x{1}", dm.Width, dm.Height));
            }
            resolutions = resolutions.RemoveDuplicates();
            ChoiceBarred resolutionChoice = AddChoice("resolution", "Resolution");
            resolutionChoice.Values.AddRange(resolutions);

            OnEnterChoice += new OnEnterChoiceEventHandler(Options_OnEnterChoice);
        }

        void Options_OnEnterChoice(ChoiceBarred choice)
        {
            switch (choice.Name)
            {
                case "resolution":
                    string[] newResolution = choice.ActiveValue.Split('x');
                    Resolution.SetResolution(int.Parse(newResolution[0]), int.Parse(newResolution[1]), true);
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }
    }
}
