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
    public class Ground
    {
        //The texture
        public static Texture2D groundTexture;

        //pos of it for shaking
        public Vector2 pos;

        //THIS IS the vector of shaking
        public static Vector2 shaking;

        //this is the is it shaking?
        public static Boolean shake;

        //This helps create the noise
        public int num;

        //loading content
        public static void LoadContent(ContentManager Content)
        {
            groundTexture = Content.Load<Texture2D>("Images//Ground");
        }

        public Ground()
        {
            pos = new Vector2(0, 0);

            shake = false;

            //set to 0 because it adds first
            num = 0;
        }

        public void Update()
        {
            //If the screen is supposed to shake 
            if (shake)
            {
                //add to num
                num++;
                
                //using num make the noise to the vector2
                shaking.X = Noise.Generate(num, 1) * 2;
                shaking.Y = Noise.Generate(1, num) * 2;

                //if its used more than 3 times = exit
                if (num >= 4)
                {
                    shake = false;
                    num = 1;
                }
                
            }
            else
            {
                //reset shaking because it is used every frame for the other objects
                shaking = new Vector2(0, 0);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            //drawing
            sb.Draw(groundTexture, pos + shaking, Color.White);
        }
    }
}
