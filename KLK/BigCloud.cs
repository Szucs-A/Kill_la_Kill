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
    public class BigCloud
    {
        public static Texture2D bigCloud;
        public Vector2 pos;
        public float scale;
        public Boolean removeFlag;
        public float speed;

        public static void LoadContent(ContentManager Content)
        {
            bigCloud = Content.Load<Texture2D>("Images//CloudFront");
        }

        public BigCloud()
        {
            pos = new Vector2(-509, -100 + (Game1.r.Next(0, 50)));
            scale = (float)Game1.r.NextDouble() + 0.2f;
            speed = ((float)Game1.r.NextDouble()  + 1f)* 1.50f;
            removeFlag = false;
        }

        public void Update()
        {
            pos.X += speed;
            if (pos.X > 810)
                removeFlag = true;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(bigCloud, pos, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 1f); 
        }
    }
}
