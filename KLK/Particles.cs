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
    public class Particle
    {
        //position
        Vector2 position;

        //momentum of where it is going
        Vector2 momentum;

        //the height of the image
        int height;

        //the width of the image
        int width;

        //the color of the image
        Color color;

        //remove it or not?
        public Boolean removeFlag;
        
        //ygravity
        float grav;

        //xgravity
        float sidegrav;

        //stopping
        int ground;

        //disappear variable
        Boolean disappear;

        public Particle(int w, int h, Color c, Vector2 pos, Vector2 momentumGiv, float gravity, float sidegravity, int g, Boolean d, Boolean randomground)
        {
            height = h;
            width = w;
            color = c;

            if (color.B == 0)
            {
                color.B = 1;
            }
            if (color.R == 0)
            {
                color.R = 1;
            }
            if (color.G == 0)
            {
                color.G = 1;
            }
            position = pos;
            momentum = momentumGiv;
            removeFlag = false;
            grav = gravity;
            sidegrav = sidegravity;

            if (!randomground)
                ground = g;
            else
                ground = g + Game1.r.Next(-4, 5);

            disappear = d;
        }

        //updating
        public void Update()
        {
            //adds the momentum
            position += momentum;

            //adds the ygravity
            momentum.Y += grav;

            //adds the xgravity
            momentum.X += sidegrav;

            if (ground != -1 && position.Y > ground)
            {
                position.Y = ground;
                momentum.X = 0;
            }

            if (disappear)
            {
                //subtracts the colors
                color.R -= 1;
                color.G -= 1;
                color.B -= 1;

                //sets the colors to black so it doesn't cycle back to 255
                if (color.R < 2)
                {
                    color.R = 1;
                }
                if (color.B < 2)
                {
                    color.B = 1;
                }
                if (color.G < 2)
                {
                    color.G = 1;
                }

                //if they are all below this start subtracting the opacity
                if (color.R < 200 && color.G < 200 && color.B < 200)
                {
                    color.A -= 1;
                }

                //remove if invisible
                if (color.A < 1)
                {
                    removeFlag = true;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            //draw
            Game1.DrawParticle(sb, color, position, width, height);
        }
    }
}
