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
    public class Instance : IEngine
    {
        public IActiveState ActiveState { get; set; }
        public int Width { get { return 1280; } }
        public int Height { get { return 720; } }
        public GraphicsDeviceManager Graphics { get; set; }
        public Game Game { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
    }

    public static class Engine
    {
        public static Instance Instance = new Instance();
        public static Audio Audio = new Audio();
        public static Level Level;
    }
}
