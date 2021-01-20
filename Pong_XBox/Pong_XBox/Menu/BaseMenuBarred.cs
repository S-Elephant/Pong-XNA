using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    public class BaseMenuBarred : IActiveState
    {
        protected delegate void OnEnterChoiceEventHandler(ChoiceBarred choice);
        protected event OnEnterChoiceEventHandler OnEnterChoice;
        #region Members
        public List<ChoiceBarred> Choices = new List<ChoiceBarred>();

        public ChoiceBarred ActiveChoice {
            get
            {
                if (Choices.Count > 0)
                    return Choices[ChoiceIndex];
                else
                    return null;
            }
        }

        private int m_ChoiceIndex = 0;
        private int ChoiceIndex
        {
            get { return m_ChoiceIndex; }
            set
            {
                if (Choices.Count > 0)
                    m_ChoiceIndex = (int)MathHelper.Clamp(value, 0, Choices.Count - 1);
                else
                    m_ChoiceIndex = -1;
            }
        }

        public IActiveState Parentmenu;

        protected Color ChoiceDrawColor = Color.Goldenrod;

        private Texture2D m_Texture;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        private Rectangle m_BarDrawRect = Rectangle.Empty;
        private Rectangle BarDrawRect
        {
            get {
                if (ActiveChoice == null)
                    return Rectangle.Empty;
                else
                {
                    Vector2 size = ActiveChoice.Font.MeasureString(ActiveChoice.Text);
                    return new Rectangle(ActiveChoice.Location.Xi(), ActiveChoice.Location.Yi(), size.Xi(), size.Yi());
                }
            }
        }

        protected IEngine IEngine;
        public int ChoiceOffsetY = 300;
        #endregion

        public BaseMenuBarred(IEngine engine, IActiveState parent)
        {
            IEngine = engine;
            Parentmenu = parent;
        }

        public ChoiceBarred AddChoice(string name, string text)
        {
            ChoiceBarred newChoice = new ChoiceBarred(new Vector2(0,ChoiceOffsetY), text, name, ChoiceDrawColor);
            newChoice.Location = new Vector2(IEngine.Width / 2 - newChoice.Font.MeasureString(newChoice.Text).X / 2, newChoice.Location.Y);
            Choices.Add(newChoice);
            ChoiceOffsetY = 5 + ChoiceOffsetY + (int)Choices[Choices.Count - 1].Font.MeasureString(Common.MeasureString).Y;
            return newChoice;
        }

        public void AddChoice(string name, string text, params string[] values)
        {
            AddChoice(name,text);
            Choices[Choices.Count - 1].Values.AddRange(values);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (GPInput.AnyPlayerPressedButton(Buttons.LeftThumbstickDown) || GPInput.AnyPlayerPressedButton(Buttons.RightThumbstickDown) || GPInput.AnyPlayerPressedButton(Buttons.DPadDown))
            {
                ChoiceIndex++;
            }
            if (GPInput.AnyPlayerPressedButton(Buttons.LeftThumbstickUp) || GPInput.AnyPlayerPressedButton(Buttons.RightThumbstickUp) || GPInput.AnyPlayerPressedButton(Buttons.DPadUp))
            {
                ChoiceIndex--;
            }
            if (GPInput.AnyPlayerPressedButton(Buttons.LeftThumbstickRight) || GPInput.AnyPlayerPressedButton(Buttons.RightThumbstickRight) || GPInput.AnyPlayerPressedButton(Buttons.DPadRight))
            {
                ActiveChoice.ValueIndex++;
            }
            if (GPInput.AnyPlayerPressedButton(Buttons.LeftThumbstickLeft) || GPInput.AnyPlayerPressedButton(Buttons.RightThumbstickLeft) || GPInput.AnyPlayerPressedButton(Buttons.DPadLeft))
            {
                ActiveChoice.ValueIndex--;
            }
            if (Choices.Count > 0 && GPInput.AnyPlayerPressedButton(Buttons.A))
                OnEnterChoice(ActiveChoice);
            if (GPInput.AnyPlayerPressedButton(Buttons.B))
            {
                if (Parentmenu != null)
                    IEngine.ActiveState = Parentmenu;
            }

            Choices.ForEach(c => c.Update(gameTime));
        }

        public virtual void Draw()
        {
            if (Choices.Count > 0 && BarDrawRect != Rectangle.Empty)
                IEngine.SpriteBatch.Draw(Common.White1px50Trans, BarDrawRect, Color.White);
            Choices.ForEach(c => c.Draw(IEngine.SpriteBatch));
        }
    }
}
