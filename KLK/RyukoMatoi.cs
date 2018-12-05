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
    public class RyukoMatoi
    {
        //AFK STATE
        public static Texture2D RyukoAFK;
        public static Texture2D RyukoDefendUp;
        public static Texture2D RyukoDefendDown;
        public static Texture2D RyukoDeadUp;
        public static Texture2D RyukoDeadDown;

        //This is the number font (its a animation)
        public static Texture2D numbers;

        //The status users
        public const int BLOCKUP = 1;
        public const int BLOCKDOWN = 2;
        public const int AFK = 3;

        //The amount of numbers because it isn't an actual font
        Animation num1 = new Animation();
        Animation num2 = new Animation();
        Animation num3 = new Animation();
        Animation num4 = new Animation();
        Animation num5 = new Animation();
        Animation num6 = new Animation();
        Animation num7 = new Animation();
        Animation num8 = new Animation();
        Animation num9 = new Animation();
        Animation num10 = new Animation();

        //the list that contains the nums
        List<Animation> SList = new List<Animation>();

        //Scoring time
        public float elapsedTime;

        //Status
        public int Status;

        //The current texture
        public Animation texture;

        //SCORE hey
        public int score;

        //hp
        public Boolean dead;

        //BloodLaunchers
        public ParticleLauncher bloodUp1, Bloodup2, Bloodup3;
        public ParticleLauncher bloodDown1, bloodDown2, bloodDown3;
        public ParticleLauncher dropDown1, dropdown2, dropup1, dropup2, dropup3;
        public List<DodgeText> dList = new List<DodgeText>();

        public Boolean showContinue = false;

        public Boolean play;
        public static SoundEffect blockup;
        public static SoundEffect blockd;

        public SoundWave sw;

        //Loading content
        public static void LoadContent(ContentManager Content)
        {
            RyukoAFK = Content.Load<Texture2D>("Images//Ryuko_AFK");
            RyukoDefendDown = Content.Load<Texture2D>("Images//Ryuko_LowBlock");
            RyukoDefendUp = Content.Load<Texture2D>("Images//Ryuko_UpBlock");
            numbers = Content.Load<Texture2D>("Images//Numbers");
            RyukoDeadDown = Content.Load<Texture2D>("Images//KneelDeath");
            RyukoDeadUp = Content.Load<Texture2D>("Images//StandingDeath");

            blockd = Content.Load<SoundEffect>("Sounds//BLockDown");
            blockup = Content.Load<SoundEffect>("Sounds//BlockUp");
        }

        public RyukoMatoi()
        {
            Status = AFK;
            texture = new Animation();
            bloodUp1 = new ParticleLauncher(3456310, 125, Color.Red, new Vector2(470, 390), new Vector2(3, 3), -0.001f, 0.1f, -6, true, 490, false, 1, true);
            Bloodup2 = new ParticleLauncher(13453450, 595, Color.Red, new Vector2(470, 350), new Vector2(3, 3), 0f, 0.1f, -8, true, 490, false, 0, true);
            Bloodup3 = new ParticleLauncher(13453450, 125, Color.Red, new Vector2(470, 330), new Vector2(3, 3), +0.001f, 0.1f, -10, true, 490, false, -1, true);

            dropDown1 = new ParticleLauncher(1500, 3, Color.Red, new Vector2(427, 390), new Vector2(3, 3), 0, 0.1f, 0, false, 490, false, 0, true);
            dropdown2 = new ParticleLauncher(1800, 3, Color.Red, new Vector2(430, 395), new Vector2(3, 3), 0, 0.1f, 0, false, 490, false, 0, true);

            dropup1 = new ParticleLauncher(1500, 3, Color.Red, new Vector2(468, 388), new Vector2(3, 3), 0, 0.1f, 0, false, 490, false, 0, true);
            dropup2 = new ParticleLauncher(1800, 3, Color.Red, new Vector2(479, 408), new Vector2(3, 3), 0, 0.1f, 0, false, 490, false, 0, true);
            dropup3 = new ParticleLauncher(2100, 3, Color.Red, new Vector2(484, 426), new Vector2(3, 3), 0, 0.1f, 0, false, 490, false, 0, true);

            //468 397
            bloodDown1 = new ParticleLauncher(13453450, 555, Color.Red, new Vector2(420, 390), new Vector2(2, 2), -0.2f, 0.10f, 8, true, 490, false, -1, true);
            bloodDown2 = new ParticleLauncher(12357258, 325, Color.Red, new Vector2(420, 350), new Vector2(2, 2), -0.02f, 0.1f, 8, true, 490, false, -4, true);

            //sets to afk
            texture.Initialize(RyukoAFK, new Vector2(370, 290), 200, 200, 5, 200, Color.White, 1f, true);
            SList.Add(num1);
            SList.Add(num2);
            SList.Add(num3);
            SList.Add(num4);
            SList.Add(num5);
            SList.Add(num6);
            SList.Add(num7);
            SList.Add(num8);
            SList.Add(num9);
            SList.Add(num10);
        }

        public Boolean dodged = false;
        public void Check()
        {

            //Checks if kiryuin is at the end of her attack instead of the beginning
            if (Game1.gameList.CurrentScreen.kiryuin != null && Game1.gameList.CurrentScreen.kiryuin.texture.currentFrame == 1 && Game1.gameList.CurrentScreen.kiryuin.Status == KiryuinSatsuki.UPWARDSWING && Status == BLOCKUP && !dodged)
            {
                if (elapsedTime < 150)
                    dodged = true;
                //if you were blocking for more than 3 seconds you get no points
                if (elapsedTime < 3000 && !dodged)
                {
                    //linear relation
                    score += (int)((-3f * elapsedTime) + 10000);
                }

                if (!dodged)
                {
                    //ground shakes if you blocked it no matter what
                    Ground.shake = true;

                    Game1.gameList.CurrentScreen.peopleON = true;

                    if (sw == null)
                        sw = new SoundWave();

                    if (play && !Game1.mute)
                        blockd.Play();

                    play = false;
                }
            }
            else if (Game1.gameList.CurrentScreen.kiryuin != null && Game1.gameList.CurrentScreen.kiryuin.texture.currentFrame == 1 && Game1.gameList.CurrentScreen.kiryuin.Status == KiryuinSatsuki.DOWNWARDSWING && Status == BLOCKDOWN && !dodged)
            {
                if (elapsedTime < 150)
                    dodged = true;
                if (elapsedTime < 3000 && !dodged)
                {
                    score += (int)((-3f * elapsedTime) + 10000);
                }
                if (!dodged)
                {

                    Ground.shake = true;

                    Game1.gameList.CurrentScreen.peopleON = true;

                    if (sw == null)
                        sw = new SoundWave();

                    if (play && !Game1.mute)
                        blockup.Play();

                    play = false;
                }
                
            }

            if (dodged)
            {
                score += 400000;
                dList.Add(new DodgeText());
            }
        }

        public void Update(GameTime gt)
        {
            bloodUp1.Update(gt);
            Bloodup2.Update(gt);
            Bloodup3.Update(gt);
            bloodDown1.Update(gt);
            bloodDown2.Update(gt);

            dropDown1.Update(gt);
            dropdown2.Update(gt);
            dropup1.Update(gt);
            dropup2.Update(gt);
            dropup3.Update(gt);

            for (int i = 0; i < dList.Count; i++)
            {
                if (dList[i].removeFlag)
                {
                    dList.Remove(dList[i]);
                }
            }

            foreach (DodgeText d in dList)
            {
                d.Update();
            }

            //Controlling the animations
            if ((Game1.kb.IsKeyDown(Keys.Up) && Game1.kb.IsKeyUp(Keys.Down)) && !dead)
            {
                //if this is the first frame you pressed up then it sets everything
                if (Status != BLOCKUP)
                {
                    elapsedTime = 0;

                    texture.Initialize(RyukoDefendUp, new Vector2(370, 290) + Ground.shaking, 200, 200, 4, 30, Color.White, 1f, false);

                    Status = BLOCKUP;
                }
                //starts adding time
                elapsedTime += gt.ElapsedGameTime.Milliseconds;

                //checks the game when the animation is finished for blocking up
                if (!texture.Active && !dodged)
                {
                    Check();
                }
            }
            else if ((Game1.kb.IsKeyDown(Keys.Down) && Game1.kb.IsKeyUp(Keys.Up)) && !dead)
            {
                if (Status != BLOCKDOWN)
                {
                    elapsedTime = 0;

                    texture.Initialize(RyukoDefendDown, new Vector2(370, 290) + Ground.shaking, 200, 200, 4, 30, Color.White, 1f, false);

                    Status = BLOCKDOWN;
                }
                elapsedTime += gt.ElapsedGameTime.Milliseconds;

                if (!texture.Active && !dodged)
                {
                    Check();
                }
            }
                //IF none of those are true then it sets this once.
            else if(Status != AFK && !dead)
            {
                elapsedTime = 0;

                Status = AFK;

                texture.Initialize(RyukoAFK, new Vector2(370, 290) + Ground.shaking, 200, 200, 5, 200, Color.White, 1f, true);
            }

            //Updates all the numbers
            foreach (Animation an in SList)
            {
                an.Update(gt);
            }

            //updates the animation
            texture.Update(gt);

            if (sw != null)
                sw.Update(gt);

            if (sw != null && sw.ElapsedTime > 180)
                sw = null;


            if (Keyboard.GetState().IsKeyDown(Keys.Space) && showContinue && Game1.oldkb.IsKeyUp(Keys.Space))
            {
                showContinue = false;
                if(!Game1.mute)
                MediaPlayer.Play(Playing.fightTheme);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            //if the ground is shaking
            if (Ground.shake)
            {
                texture.destinationRect.X += (int)Ground.shaking.X;
                texture.destinationRect.Y += (int)Ground.shaking.Y;
            }
            else
            {
                //resets the textures
                texture.destinationRect.X = 370;
                texture.destinationRect.Y = 290;
            }

            if (showContinue)
                sb.Draw(KiryuinSatsuki.continuetxt, new Vector2(70, 476), Color.White);
            
            //draws the texture
            texture.Draw(sb);

            //Draws the numbers for how big the number is
                for (int i = 0; i < score.ToString().Length; i++)
                {
                    //Intializes the animation texture
                    SList[i].Initialize(numbers, new Vector2(50, 50), 50, 50, 1, 50, Color.White, 1, false);

                    //Selects the position
                    SList[i].Position = new Vector2(1 * i * 20, 1) + Ground.shaking;

                    //Gets the frame
                    SList[i].currentFrame = Int32.Parse(score.ToString().Substring(i, 1));

                    //Draws
                    SList[i].Draw(sb);
                }
            
            bloodUp1.Draw(sb);
            Bloodup2.Draw(sb);
            Bloodup3.Draw(sb);
            bloodDown1.Draw(sb);
            bloodDown2.Draw(sb);

            dropDown1.Draw(sb);
            dropdown2.Draw(sb);
            dropup1.Draw(sb);
            dropup2.Draw(sb);
            dropup3.Draw(sb);


            foreach (DodgeText d in dList)
            {
                d.Draw(sb);
            }
            if (sw != null)
                sw.Draw(sb);
        }
    }
}
