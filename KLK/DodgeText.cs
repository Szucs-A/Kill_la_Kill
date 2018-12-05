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
    public class DodgeText
    {
        public static Texture2D tex;

        Vector2 pos;
        float op;
        public Boolean removeFlag = false;

        public DodgeText()
        {
            pos = new Vector2(400 - tex.Width / 2, 290);
            op = 1;
        }

        public static void LoadContent(ContentManager Content)
        {
            tex = Content.Load<Texture2D>("Images//Dodge");
        }

        public void Update()
        {
            pos.Y -= 3;

            if (pos.Y < 263)
                pos.Y += 2;

            if (pos.Y <= 240)
            {
                pos.Y = 240;
                op -= 0.01f;
            }

            if (op == 0)
                removeFlag = true;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color.White * op);
        }
    }
}
