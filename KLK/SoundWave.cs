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
    public class SoundWave
    {
        public static Texture2D a;
        public static Texture2D b;
        public static Texture2D c;
        public static Texture2D d;
        public static Texture2D e;
        public static Texture2D f;
        public static SoundEffect sound;

        public Texture2D t;
        public int ElapsedTime;

        public static void LoadContent(ContentManager Content)
        {
            a = Content.Load<Texture2D>("Images//image1");
            b = Content.Load<Texture2D>("Images//image2");
            c = Content.Load<Texture2D>("Images//image3");
            d = Content.Load<Texture2D>("Images//image4");
            e = Content.Load<Texture2D>("Images//image5");
            f = Content.Load<Texture2D>("Images//image6");
            sound = Content.Load<SoundEffect>("Sounds//explosion");
        }

        public SoundWave()
        {
            t = a;
            if(!Game1.mute)
            sound.Play();
        }

        public void Update(GameTime gt)
        {
            ElapsedTime += gt.ElapsedGameTime.Milliseconds;

            if (ElapsedTime > 30)
                t = b;

            if (ElapsedTime > 60)
                t = c;

            if (ElapsedTime > 90)
                t = d;

            if (ElapsedTime > 120)
                t = e;

            if (ElapsedTime > 150)
                t = f;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(t, new Vector2(), Color.White * .4f);
        }
    }
}
