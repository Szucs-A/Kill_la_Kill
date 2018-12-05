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
    public class cButton
    {
        public Screens DestinationScreen;
        public String Text;

        public Boolean hover;

        public Rectangle boundingBox;

        public int yDif;
        public int xDif;

        public Texture2D te;

        public int listpos;

        public cButton(Screens ds, String str, int yDiff, int xDiff, int x, int y, int width, int height, Texture2D t, int lp)
        {
            DestinationScreen = ds;
            Text = str;
            if(x == -1)
                boundingBox = new Rectangle(-1000, -1000, (int)Game1.Main.MeasureString(str).X, (int)Game1.Main.MeasureString(str).Y);
            else
                boundingBox = new Rectangle(x, y, width, height);

            hover = false;
            yDif = yDiff;
            xDif = xDiff;

            te = t;

            listpos = lp;
        }

        public void Update(MouseState ms)
        {
            if (boundingBox.X == -1000)
            {
                boundingBox.X = (int)((400) - Game1.Main.MeasureString(Text).X / 2 + xDif);
                boundingBox.Y = (int)((250) - Game1.Main.MeasureString(Text).Y / 2 + yDif);
            }
        }
    }
}
