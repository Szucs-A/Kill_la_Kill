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
    public class FlyingPeople
    {
        public static Texture2D tex;
        Vector2 pos;
        Vector2 speed;
        float scale;
        float rotate;

        public Boolean removeflag;

        public static void LoadContent(ContentManager Content)
        {
            tex = Content.Load<Texture2D>("Images//flying_person");
        }

        public FlyingPeople()
        {
            pos = new Vector2(Game1.r.Next(100, 700), 500);
            scale = 1;
            rotate = MathHelper.ToRadians(Game1.r.Next(0, 360));
            speed = new Vector2(0, -11);
        }

        public void Update()
        {
            pos += speed;

            scale -= 0.01f;
            rotate += MathHelper.ToRadians(3);

            if (pos.Y < 0)
                removeflag = true;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, null, Color.White, rotate, new Vector2(), scale, SpriteEffects.None, 1f);
        }
    }
}
