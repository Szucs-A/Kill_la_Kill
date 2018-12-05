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
    public class KiryuinSatsuki
    {
        //The beginning animation of satsuki
        public static Texture2D KiryuinAFK;

        //the Swing down animation
        public static Texture2D KiryuinSwingDown;

        public static Texture2D speechbubble;

        public const String speech = "Fear is freedom! Subjugation is liberation! Contradiction is truth! \nThose are the facts of this world! And you will surrender to them, \nyou pig in human clothing!";

        //the swing up animation
        public static Texture2D KiryuinSwingUp;

        //pulling the sword off the ground
        public static Texture2D KiryuinPull;

        //The status ints to tell me what swing they are doing
        public const int UPWARDSWING = 1;
        public const int DOWNWARDSWING = 2;
        public const int AFK = 3;

        public static Texture2D uparrow;
        public static Texture2D downarrow;

        //Her texture
        public Animation texture;

        //Used for speeding up the swing time
        public float elapsedTime;

        //What she is doing
        public int Status;

        //Is the game just starting up?
        public Boolean startUp;

        //Starting swing time
        int SwingTime = 1000;

        public Boolean drawcheer = false;

        public String whereat = "";
        public int whereatstr;
        public int stringTime;

        public static SoundEffect fleshhit; 
        public static SoundEffect whoosh;


        public static Texture2D continuetxt;

        //Loading the content
        public static void LoadContent(ContentManager Content)
        {
            KiryuinAFK = Content.Load<Texture2D>("Images//Satsuki_AFK");
            KiryuinSwingDown = Content.Load<Texture2D>("Images//Satsuki_DownSlash");
            KiryuinSwingUp = Content.Load<Texture2D>("Images//Satsuki_UpSlash");
            KiryuinPull = Content.Load<Texture2D>("Images//Satsuki_PullSword");
            speechbubble = Content.Load<Texture2D>("Images//SpeechBubble");
            continuetxt = Content.Load<Texture2D>("Images//Space to Continue");

            uparrow = Content.Load<Texture2D>("Images//uparrow");
            downarrow = Content.Load<Texture2D>("Images//downarrow");

            fleshhit = Content.Load<SoundEffect>("Sounds//Fleshhit");
            whoosh = Content.Load<SoundEffect>("Sounds//Whoosh");
        }

        Boolean flesh = true;

        Boolean hitLock = false;

        public void Check()
        {
            if (!hitLock)
            {
                if (Game1.gameList.CurrentScreen.ryuko.texture.Active || (Status == UPWARDSWING && Game1.gameList.CurrentScreen.ryuko.Status != RyukoMatoi.BLOCKUP) || (Status == DOWNWARDSWING && Game1.gameList.CurrentScreen.ryuko.Status != RyukoMatoi.BLOCKDOWN))
                {
                    if (Status == DOWNWARDSWING)
                    {
                        Game1.gameList.CurrentScreen.ryuko.dead = true;
                        Game1.gameList.CurrentScreen.ryuko.bloodUp1.shootnow = true;
                        Game1.gameList.CurrentScreen.ryuko.Bloodup2.shootnow = true;
                        Game1.gameList.CurrentScreen.ryuko.Bloodup3.shootnow = true;
                        Game1.gameList.CurrentScreen.ryuko.texture.Initialize(RyukoMatoi.RyukoDeadUp, new Vector2(370, 290), 200, 200, 1, 26529865, Color.White, 1f, false);

                        Game1.gameList.CurrentScreen.ryuko.dropup1.active = true;
                        Game1.gameList.CurrentScreen.ryuko.dropup2.active = true;
                        Game1.gameList.CurrentScreen.ryuko.dropup3.active = true;
                        drawcheer = true;

                        if (flesh && !Game1.mute)
                            fleshhit.Play();

                        flesh = false;
                    }
                    else
                    {
                        Game1.gameList.CurrentScreen.ryuko.dead = true;
                        Game1.gameList.CurrentScreen.ryuko.bloodDown1.shootnow = true;
                        Game1.gameList.CurrentScreen.ryuko.bloodDown2.shootnow = true;
                        Game1.gameList.CurrentScreen.ryuko.texture.Initialize(RyukoMatoi.RyukoDeadDown, new Vector2(370, 290), 200, 200, 1, 26529865, Color.White, 1f, false);
                        Game1.gameList.CurrentScreen.ryuko.dropDown1.active = true;
                        Game1.gameList.CurrentScreen.ryuko.dropdown2.active = true;
                        drawcheer = true;

                        if (flesh && !Game1.mute)
                            fleshhit.Play();
                        flesh
                             = false;
                    }
                }
                else
                {
                    hitLock = true;
                }
            }
        }

        public KiryuinSatsuki()
        {
            //Setting status to the beginning
            Status = AFK;

            startUp = true;
            texture = new Animation();
            texture.Initialize(KiryuinAFK, new Vector2(240, 240), 250, 250, 7, 150, Color.White, 1f, true);
        }

        Color cnew = new Color(255, 255, 255, 0);
        Boolean arrows = true;
        public Boolean startthetext = true;

        public Boolean playwhoosh = true;

        public void Update(GameTime gt)
        {
            if (!Game1.gameList.CurrentScreen.ryuko.dead)
            {
                //is the game just starting?
                if (!startUp)
                {
                    //Adding
                    elapsedTime += gt.ElapsedGameTime.Milliseconds;

                    //is she done her previous attack?
                    if (!texture.Active)
                    {
                        //if so she does another swing
                        switch (Game1.r.Next(1, 3))
                        {
                            case UPWARDSWING:
                                Status = UPWARDSWING;
                                arrows = true;
                                texture.Initialize(KiryuinSwingDown, new Vector2(240, 240), 250, 250, 3, 50, Color.White, 1f, false);
                                Game1.gameList.CurrentScreen.ryuko.play = true;
                                hitLock = false;
                                Game1.gameList.CurrentScreen.ryuko.dodged = false;
                                break;
                            case DOWNWARDSWING:
                                Status = DOWNWARDSWING;
                                arrows = true;
                                texture.Initialize(KiryuinSwingUp, new Vector2(240, 240), 250, 250, 3, 50, Color.White, 1f, false);
                                Game1.gameList.CurrentScreen.ryuko.play = true;
                                hitLock = false;
                                Game1.gameList.CurrentScreen.ryuko.dodged = false;
                                break;
                        }
                    }

                    //if her status isn't afk
                    if (Status != AFK)
                    {
                        //Then set the frame times 
                        if (texture.currentFrame == 0)
                        {
                            texture.frameTime = SwingTime;
                            playwhoosh = true;
                        }
                        else if (texture.currentFrame == 1)
                        {
                            texture.frameTime = 100;
                            arrows = false;

                            if (playwhoosh && !Game1.mute)
                                whoosh.Play();

                            playwhoosh = false;
                            Check();
                        }
                    }
                }
                else
                {
                    if (startthetext)
                    {
                        stringTime += gt.ElapsedGameTime.Milliseconds;

                        //If it is the beginining of the game
                        //IF space is pressed she then pulls the sword out of the ground
                        if (Game1.kb.IsKeyDown(Keys.Space) && Game1.oldkb.IsKeyUp(Keys.Space))
                        {
                            if (whereat.Length == speech.Length)
                            {
                                if(!Game1.mute)
                                Game1.Movement.Play();
                                texture.Initialize(KiryuinPull, new Vector2(240, 240), 250, 250, 5, 75, Color.White, 1f, false);
                                startUp = false;
                                if (!Game1.mute)
                                    if (!Game1.PlayingLoop)
                                    {
                                        MediaPlayer.Play(Playing.fightTheme);
                                        Game1.playelevator = false;
                                    }
                            }
                            else
                            {
                                whereat = speech;
                                if(!Game1.mute)
                                Game1.Movement.Play();
                            }
                        }

                        int i = 20;
                        if (whereat.Length == speech.Length)
                            i = 1000;

                        if (stringTime > i)
                        {
                            if (i == 20)
                            {
                                whereatstr++;
                                stringTime = 0;
                            }
                            else
                            {
                                if (cnew.A != 255)
                                    cnew.A++;
                            }
                        }
                        if (whereatstr > speech.Length)
                            whereatstr = speech.Length;

                        if (whereat.Length != speech.Length)
                            whereat = speech.Substring(0, whereatstr);
                    }
                    else
                    {
                        if (Game1.kb.IsKeyDown(Keys.Space) && Game1.oldkb.IsKeyUp(Keys.Space))
                        {
                                texture.Initialize(KiryuinPull, new Vector2(240, 240), 250, 250, 5, 75, Color.White, 1f, false);
                                startUp = false;
                            if(!Game1.mute)
                                Game1.Movement.Play();
                        }
                    }
                }

                //This speeds up the swing amount of time
                if (elapsedTime >= 1000)
                {
                    SwingTime -= 15;
                    elapsedTime = 0;
                }
            }
            //updates the texture
            texture.Update(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            //Adding ground shaking because we want her to shake
            texture.Position += Ground.shaking;

            //draws
            texture.Draw(sb);

            if (startUp && startthetext)
            {
                sb.Draw(speechbubble, new Vector2(0, 0), Color.White);
                sb.DrawString(Game1.Main, whereat, new Vector2(10, 410), Color.Black);
                sb.Draw(continuetxt, new Vector2(320, 460), cnew);
            }

            if(SwingTime > 905){
                if(Status == UPWARDSWING && arrows)
                    sb.Draw(uparrow, new Vector2(526, 330), Color.White);

                else if(Status == DOWNWARDSWING && arrows)
                    sb.Draw(downarrow, new Vector2(526, 330), Color.White);
            }
        }
    }
}
