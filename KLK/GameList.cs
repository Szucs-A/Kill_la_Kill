using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KLK
{
    public class GameList
    {
        public Screens CurrentScreen;
        public MouseState ms;
        public MouseState oldms;

        public GameList()
        {

        }

        public void Initialize()
        {
            CurrentScreen = new MainMenu();
            CurrentScreen.Initialize();

            ms = new MouseState();
            oldms = new MouseState();
        }

        public void Update(GameTime gt)
        {
            ms = Mouse.GetState();

            CurrentScreen.Update(ms, gt);

            oldms = ms;
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void Draw(SpriteBatch sb)
        {
            CurrentScreen.Draw(sb);
        }
    }
}
