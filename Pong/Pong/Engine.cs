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
    public class Engine : IEngine
    {
        public static Engine Instance;


        public IActiveState ActiveState { get; set; }
        public Game Game { get; set; }
        public XactMgr Audio = new XactMgr("Pong");
        public Keyboard1 KB = new Keyboard1();
        public  GraphicsDeviceManager Graphics;
        public  int Width { get { return Graphics.PreferredBackBufferWidth; } }
        public  int Height { get { return Graphics.PreferredBackBufferHeight; } }

        public Rectangle ScreenArea { get { return new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight); } }

        GraphicsDeviceManager IEngine.Graphics { get; set; }

        public Rectangle SafeArea
        {
            get { throw new NotImplementedException(); }
        }

        public SpriteBatch SpriteBatch { get; set; }


    }
}
