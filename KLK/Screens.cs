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
    public abstract class Screens
    {
        //Satsuki
        public KiryuinSatsuki kiryuin;

        //Ryuko
        public RyukoMatoi ryuko;

        public Boolean peopleON = false;

        public List<cButton> ButtonList = new List<cButton>();

        abstract public void Update(MouseState ms, GameTime gt);

        abstract public void Draw(SpriteBatch sb);

        abstract public void Initialize();
    }

    public class MainMenu : Screens
    {
        public static Texture2D menu;
        public static Texture2D play;
        public static Texture2D exit;
        public static Song mainmenutheme;
        public float op = 0;
        public static Texture2D blackground;
        public static Song eastereggTheme;

        public Screens pressedSpace = null;

        cButton playingButton;
        public static int buttonpos = 0;
        cButton quitButton;

        ParticleLauncher q, w, e, r, t;
        public static Texture2D test;
        float rotate;
        float scale;
        int random;

        public static void LoadContent(ContentManager Content)
        {
            menu = Content.Load<Texture2D>("Images//MainMenu");
            test = Content.Load<Texture2D>("Images/test");
            play = Content.Load<Texture2D>("Images/Play");
            exit = Content.Load<Texture2D>("Images/Exit");
            mainmenutheme = Content.Load<Song>("Sounds//MainMenuTheme");
            blackground = Content.Load<Texture2D>("Images/black");
            eastereggTheme = Content.Load<Song>("Sounds//easteregg1");
        }

        public MainMenu()
        {

        }

        Boolean eastergg = false;
        int timer;

        public override void Initialize()
        {
            playingButton = new cButton(new Beginning(), "Play", 0, 0, 70, 330, 120, 50, play, 0);
            quitButton = new cButton(new Exit(), "Exit", 60, 0, 76, 403, 120, 50, exit, 1);
            ButtonList.Add(playingButton);
            ButtonList.Add(quitButton);
            q = new ParticleLauncher(2000, 3, Color.Red, new Vector2(50, 50), new Vector2(5, 5), 0f, 0, 0, true, -1, true, 0, false);
            w = new ParticleLauncher(1900, 3, Color.Red, new Vector2(600, 70), new Vector2(5, 5), 0f, 0, 0, true, -1, true, 0, false);
            e = new ParticleLauncher(2200, 3, Color.Red, new Vector2(250, 200), new Vector2(3, 3), 0f, 0, 0, true, -1, true, 0, false);
            r = new ParticleLauncher(2600, 3, Color.Red, new Vector2(75, 120), new Vector2(3, 3), 0f, 0, 0, true, -1, true, 0, false);
            t = new ParticleLauncher(3000, 3, Color.Red, new Vector2(400, 150), new Vector2(3, 3), 0f, 0, 0, true, -1, true, 0, false);
            rotate = 0;
            scale = 1;
            random = -1;
            if (!Game1.mute)
            {
                MediaPlayer.Play(mainmenutheme);
                MediaPlayer.IsRepeating = true;
            }
        }

        Boolean adding = true;
        Boolean subtracting = false;
        Boolean playingMlp = false;

        public override void Update(MouseState ms, GameTime gt)
        {
            if(adding)
            rotate += MathHelper.ToRadians(0.5f);

            if(!adding)
                rotate -= MathHelper.ToRadians(0.5f);


            if(MathHelper.ToDegrees(rotate) > 20 || MathHelper.ToDegrees(rotate) < -20)
                if(adding)
                    adding = false;
                else
                    adding  = true;

            if (subtracting)
                scale -= 0.01f;

            if (!subtracting)
                scale += 0.01f;


            if (scale > 2.5 || scale < 1)
                if (subtracting)
                    subtracting = false;
                else
                    subtracting = true;

            if ((Keyboard.GetState().IsKeyDown(Keys.Escape) && Game1.oldkb.IsKeyUp(Keys.Escape)) || 
                (Game1.gp.Buttons.B == ButtonState.Pressed && Game1.oldgp.Buttons.B == ButtonState.Released))
            {
                Game1.gameList.CurrentScreen = new Exit();
                Game1.gameList.CurrentScreen.Initialize();
            }

            q.Update(gt);
            w.Update(gt);
            e.Update(gt);
            r.Update(gt);
            t.Update(gt);

            if((Keyboard.GetState().IsKeyDown(Keys.Up)) || (Game1.gp.ThumbSticks.Left.Y > 0 && Game1.oldgp.ThumbSticks.Left.Y == 0))
            {
                if (buttonpos != 0)
                {
                    buttonpos--;
                }
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.Down)) || (Game1.gp.ThumbSticks.Left.Y < 0 && Game1.oldgp.ThumbSticks.Left.Y == 0))
            {
                if (buttonpos != 1)
                {
                    buttonpos++;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1) && Keyboard.GetState().IsKeyDown(Keys.NumPad2) && Keyboard.GetState().IsKeyDown(Keys.NumPad3) && !playingMlp && !Game1.mute)
            {
                MediaPlayer.Play(eastereggTheme);
                eastergg = true;
                random = 3;
                playingMlp = true;
            }
            if (eastergg)
                timer += gt.ElapsedGameTime.Milliseconds;

            if (timer > 33400)
            {
                if (!Game1.mute)
                {
                    MediaPlayer.Play(mainmenutheme);
                    timer = 0;
                    eastergg = false;
                    playingMlp = false;
                }
            }

            foreach (cButton button in ButtonList)
            {
                button.Update(Mouse.GetState());

                if (buttonpos == button.listpos)
                {
                    button.hover = true;
                }
                else
                    button.hover = false;

                if ((Keyboard.GetState().IsKeyDown(Keys.Space)&& button.hover && pressedSpace == null) || 
                    (Game1.gp.Buttons.A == ButtonState.Pressed && button.hover && pressedSpace == null))
                {
                    pressedSpace = button.DestinationScreen;
                    if(!Game1.mute)
                    Game1.Movement.Play();
                }
            }

            if (pressedSpace != null)
            {
                MediaPlayer.Volume -= 0.01f;
                op += 0.01f;
                if (MediaPlayer.Volume == 0)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Volume = 1;
                    Game1.gameList.CurrentScreen = pressedSpace;
                    Game1.gameList.CurrentScreen.Initialize();
                }
            }

            if((Keyboard.GetState().IsKeyDown(Keys.N) && Game1.oldkb.IsKeyUp(Keys.N)) || (Game1.gp.Buttons.RightShoulder == ButtonState.Pressed && Game1.oldgp.Buttons.RightShoulder == ButtonState.Released)){
                random = Game1.r.Next(0, 19);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(menu, new Vector2(0, 0), Color.White);
            foreach (cButton button in Game1.gameList.CurrentScreen.ButtonList)
            {
                if (button.hover)
                {
                    sb.Draw(button.te, new Vector2(button.boundingBox.X - 3, button.boundingBox.Y - 3), Color.Gray);
                }
                sb.Draw(button.te, new Vector2(button.boundingBox.X, button.boundingBox.Y), Color.White);
            }

            q.Draw(sb);
            w.Draw(sb);
            e.Draw(sb);
            r.Draw(sb);
            t.Draw(sb);

            sb.DrawString(Game1.Main, "Press N(Right Bumper) for a new Hint\nPress P(X) to mute the game", new Vector2(76, 460),Color.White, 0, new Vector2(), 0.7f, SpriteEffects.None, 1);
            if(random == -1)
                sb.DrawString(Game1.Main, "Your HighScore: " + Game1.highScore.ToString(), new Vector2(600, 400), Color.White, rotate, new Vector2(80, 50 / 2), scale, SpriteEffects.None ,1);

            if(random == 1)
            sb.DrawString(Game1.Main, "Gift Wrapped For\nGoodall, Meghan, \nand Erin!", new Vector2(600, 400), Color.White, rotate, new Vector2(80, 82 / 2), scale, SpriteEffects.None ,1);
            if(random == 0)
                sb.DrawString(Game1.Main, "Hint: More points\nfor defending \nin short bursts!", new Vector2(600, 400), Color.White, rotate, new Vector2(80, 82 / 2), scale, SpriteEffects.None, 1);
            if(random == 2)
                sb.DrawString(Game1.Main, "Hint: Don't admire\nKiryuin or Ryuko!", new Vector2(600, 400), Color.White, rotate, new Vector2(80, 82 / 2), scale, SpriteEffects.None, 1);
            if(random == 3)
                sb.DrawString(Game1.Main, "Rainbow Dash\nis best pony lol", new Vector2(600, 400), Color.White, rotate, new Vector2(80, 82 / 2), scale, SpriteEffects.None, 1);
            if(random == 4)
                sb.DrawString(Game1.Main, "Deleting Broswer \nhistory hold on...", new Vector2(600, 400), Color.White, rotate, new Vector2(80, 82 / 2), scale, SpriteEffects.None, 1);
            if(random == 5)
                sb.DrawString(Game1.Main, "Mr. Goodall, Riley \nhas always \nbeen here.", new Vector2(600, 400), Color.White, rotate, new Vector2(80, 82 / 2), scale, SpriteEffects.None, 1);
            if(random == 6)
                sb.DrawString(Game1.Main, "i.imgur.com/70SHHjS.gif", new Vector2(600, 400), Color.White, rotate, new Vector2(120, 15), scale, SpriteEffects.None, 1);
            if(random == 7)
                sb.DrawString(Game1.Main, "Press Space to Select", new Vector2(600, 400), Color.White, rotate, new Vector2(120, 15), scale, SpriteEffects.None, 1);
            if (random == 8)
                sb.DrawString(Game1.Main, "Be sure to hold your block\nfor as long as possible", new Vector2(600, 400), Color.White, rotate, new Vector2(120, 15), scale, SpriteEffects.None, 1);
            if (random == 9)
                sb.DrawString(Game1.Main, "Rookie Mistake", new Vector2(600, 400), Color.White, rotate, new Vector2(60, 15), scale, SpriteEffects.None, 1);
            if (random == 10)
                sb.DrawString(Game1.Main, "Ain't nobody got time\nfor that", new Vector2(600, 400), Color.White, rotate, new Vector2(100, 15), scale, SpriteEffects.None, 1);
            if (random == 11)
                sb.DrawString(Game1.Main, "Press 1,2,3 all together\non the numpad...", new Vector2(600, 400), Color.White, rotate, new Vector2(120, 15), scale, SpriteEffects.None, 1);
            if (random == 12)
                sb.DrawString(Game1.Main, "Want some elevator music?\nWait a bit", new Vector2(600, 400), Color.White, rotate, new Vector2(100, 15), scale, SpriteEffects.None, 1);
            if (random == 13)
                sb.DrawString(Game1.Main, "Performing a dodge\nis a 80ms gap", new Vector2(600, 400), Color.White, rotate, new Vector2(100, 15), scale, SpriteEffects.None, 1);
            if (random == 14)
                sb.DrawString(Game1.Main, "Shut up and dance with me!", new Vector2(600, 400), Color.White, rotate, new Vector2(120, 0), scale, SpriteEffects.None, 1);
            if (random == 15)
                sb.DrawString(Game1.Main, "Yeah, my mama she told me\nDont worry about your style", new Vector2(600, 400), Color.White, rotate, new Vector2(120, 15), scale, SpriteEffects.None, 1);
            if (random == 16)
                sb.DrawString(Game1.Main, "Kappa", new Vector2(600, 400), Color.White, rotate, new Vector2(30, 15), scale, SpriteEffects.None, 1);
            if (random == 17)
                sb.DrawString(Game1.Main, "If that is what death is like,\nI'm never dying - Riley", new Vector2(600, 400), Color.White, rotate, new Vector2(150, 15), scale, SpriteEffects.None, 1);
            if (random == 18)
                sb.DrawString(Game1.Main, "Jontron > AngryVideoGameNerd", new Vector2(600, 400), Color.White, rotate, new Vector2(140, 15), scale, SpriteEffects.None, 1);

            if(Game1.displaywarn)
                sb.DrawString(Game1.Main, "Your controller is not compatable with this game.", new Vector2(450, 477), Color.White, 0, new Vector2(), 0.7f, SpriteEffects.None, 1);

            sb.Draw(blackground, new Vector2(), Color.White * op);
        }
    }

    public class Beginning : Screens
    {
        public static Texture2D heel;
        public static Texture2D ground4foot;
        public static Texture2D namepart1;
        public static Texture2D namepart2;
        public static Texture2D fullname;
        public static Texture2D blur1;
        public static Texture2D blur2;
        public static Texture2D skiptexture;
        public static SoundEffect boom;

        //public static SoundEffect beginning;
        public static Song beginning;


        public float rotation;
        public Vector2 pos;
        public Boolean finished;
        int elapsedTime;
        int opacityofText;

        public ParticleLauncher heellaunch;
        public ParticleLauncher toe;

        public Beginning()
        {

        }

        public static void LoadContent(ContentManager Content)
        {
            heel = Content.Load<Texture2D>("Images//Satsuki_Boot");
            ground4foot = Content.Load<Texture2D>("Images//ground4foot");
            namepart1 = Content.Load<Texture2D>("Images//English_Part1");
            namepart2 = Content.Load<Texture2D>("Images//English_Part2");
            fullname = Content.Load<Texture2D>("Images//English_Name");

            blur1 = Content.Load<Texture2D>("Images//Blur1");
            blur2 = Content.Load<Texture2D>("Images//Blur2");
            skiptexture = Content.Load<Texture2D>("Images//skip");
            beginning = Content.Load<Song>("Sounds//Beginning");
            boom = Content.Load<SoundEffect>("Sounds//Boom");
        }

        Boolean played1 = false;
        Boolean played2 = false;
        Boolean played3 = false;

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(ground4foot, new Vector2(0, 60), Color.White);
            sb.Draw(heel, pos, null, Color.White, rotation, new Vector2(0, heel.Height), 1.0f, SpriteEffects.None, 1f);
            heellaunch.Draw(sb);
            toe.Draw(sb);

            if (elapsedTime > 1500)
            {
                sb.Draw(namepart1, new Vector2(0, 0), Color.White);
                if(!played1 && !Game1.mute)
                boom.Play();
                played1 = true;
            }
            if (elapsedTime > 2500)
            {
                sb.Draw(namepart2, new Vector2(0, 0), Color.White);
                if(!played2 && !Game1.mute)
                boom.Play();
                played2 = true;
            }
            if (elapsedTime > 3500)
            {
                sb.Draw(fullname, new Vector2(0, 0), Color.White);
                if (!played3 && !Game1.mute)
                boom.Play();
                played3 = true;
            }
            if(elapsedTime > 6400)
                sb.Draw(blur1, new Vector2(0, 0), Color.White);
            if (elapsedTime > 6450)
                sb.Draw(blur2, new Vector2(0, 0), Color.White);

            sb.Draw(skiptexture, new Vector2(400 - 105, 500 - 25), new Color(opacityofText, opacityofText, opacityofText));
        }

        public override void Initialize()
        {
            Game1.playelevator = true;
            rotation = MathHelper.ToRadians(330);
            finished = false;
            pos = new Vector2(125, 350);
            heellaunch = new ParticleLauncher(10000, 2, Color.Black, new Vector2(150, 455), new Vector2(15, 15), -0.5f, -0.6f, 0, true, -1, true, 0, false);
            toe = new ParticleLauncher(10000, 2, Color.Black, new Vector2(475, 455), new Vector2(15, 15), -0.2f, -0.01f, -10, true, -1, true, 0, false);
            opacityofText = 0;
        }

        public override void Update(MouseState ms, GameTime gt)
        {
            elapsedTime += gt.ElapsedGameTime.Milliseconds;
            heellaunch.Update(gt);
            toe.Update(gt);

            if ((Game1.kb.IsKeyDown(Keys.Space) && Game1.oldkb.IsKeyUp(Keys.Space)) || (Game1.gp.Buttons.A == ButtonState.Pressed && Game1.oldgp.Buttons.A == ButtonState.Released))
            {
                Game1.gameList.CurrentScreen = new Playing();
                Game1.gameList.CurrentScreen.Initialize();
                if(!Game1.mute)
                Game1.Movement.Play();
            }

            if ((Game1.kb.IsKeyDown(Keys.Escape) || Game1.kb.IsKeyDown(Keys.M)) || (Game1.gp.Buttons.B == ButtonState.Pressed))
            {
                MediaPlayer.Stop();
                Game1.gameList.CurrentScreen = new MainMenu();
                Game1.gameList.CurrentScreen.Initialize();
                if(!Game1.mute)
                Game1.Movement.Play();
            }
            

            if (pos.Y >= 455)
            {
                if (pos.Y == 460)
                {
                    heellaunch.shootnow = true;

                    if(!Game1.mute)
                    MediaPlayer.Play(beginning);
                    
                    toe.shootnow = true;
                }

                if (!finished)
                {
                    rotation += MathHelper.ToRadians(5);
                    pos.Y += 1;
                }

                if (Math.Round(MathHelper.ToDegrees(rotation)) == 360)
                {
                    finished = true;
                }
            }
            else
            {
                pos.Y += 10;
            }

            if(pos.X < 160)
            pos.X += 1;

            if (elapsedTime > 1000)
                opacityofText += 1;

            if (elapsedTime > 6500)
            {
                Game1.gameList.CurrentScreen = new Playing();
                Game1.gameList.CurrentScreen.Initialize();
            }
        }
    }

    public class Playing : Screens
    {

        //The ground
        public Ground ground;

        List<BigCloud> bList = new List<BigCloud>();
        List<SmallCloud> cList = new List<SmallCloud>();

        //The texture of academy
        public static Texture2D Academy;
        public static Texture2D bloodImage;
        public static Texture2D sky;
        public static Texture2D people;
        public static Texture2D cheerpeople;
        public static Texture2D options;
        public static Song fightTheme;

        public static Texture2D highscoreTable;
        public static Texture2D newhighscoreTable;

        public static Song nonewh;

        List<Animation> xList = new List<Animation>();
        List<FlyingPeople> fList = new List<FlyingPeople>();

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

        Animation bum1 = new Animation();
        Animation bum2 = new Animation();
        Animation bum3 = new Animation();
        Animation bum4 = new Animation();
        Animation bum5 = new Animation();
        Animation bum6 = new Animation();
        Animation bum7 = new Animation();
        Animation bum8 = new Animation();
        Animation bum9 = new Animation();
        Animation bum10 = new Animation();


        public static Song band;

        List<Animation> sList = new List<Animation>();

        int elapsedTime;
        float opacity = 0;

        int setbigcloudTime;
        int setsmallcloudTime;

        int smallcloudTime;
        int bigcloudTime;

        Boolean newh = false;

        public static SoundEffect scream1, scream2, scream3;

        public static Song pressr;

        int peopleTime;

        public Playing()
        {

        }

        public static void LoadContent(ContentManager Content)
        {
            RyukoMatoi.LoadContent(Content);

            KiryuinSatsuki.LoadContent(Content);

            Academy = Content.Load<Texture2D>("Images//HonnojiAcademy");
            bloodImage = Content.Load<Texture2D>("Images//Blood");

            highscoreTable = Content.Load<Texture2D>("Images//Highscore");
            newhighscoreTable = Content.Load<Texture2D>("Images//newhighscore");
            people = Content.Load<Texture2D>("Images//crowd");

            sky = Content.Load<Texture2D>("Images//sky");

            options = Content.Load<Texture2D>("Images//Options");

            cheerpeople = Content.Load<Texture2D>("Images//crowdhappy");
            
            pressr = Content.Load<Song>("Sounds//pressr");
            nonewh = Content.Load<Song>("Sounds//nonewh");

            fightTheme = Content.Load<Song>("Sounds//FightingTheme");

            scream1 = Content.Load<SoundEffect>("Sounds//Scream1");
            scream2 = Content.Load<SoundEffect>("Sounds//Scream2");
            scream3 = Content.Load<SoundEffect>("Sounds//Scream3");

            band = Content.Load<Song>("Sounds//Band");

            Ground.LoadContent(Content);

            SoundWave.LoadContent(Content);

            FlyingPeople.LoadContent(Content);

            SmallCloud.LoadContent(Content);
            BigCloud.LoadContent(Content);
        }

        Boolean playing = false;

        public override void Initialize()
        {
            kiryuin = new KiryuinSatsuki();

            ryuko = new RyukoMatoi();

            ground = new Ground();

            xList.Add(num1);
            xList.Add(num2);
            xList.Add(num3);
            xList.Add(num4);
            xList.Add(num5);
            xList.Add(num6);
            xList.Add(num7);
            xList.Add(num8);
            xList.Add(num9);
            xList.Add(num10);

            sList.Add(bum1);
            sList.Add(bum2);
            sList.Add(bum3);
            sList.Add(bum4);
            sList.Add(bum5);
            sList.Add(bum6);
            sList.Add(bum7);
            sList.Add(bum8);
            sList.Add(bum9);
            sList.Add(bum10);

            num3.Initialize(bloodImage, new Vector2(150, 150), 800, 500, 1, 50, Color.White, 1, false);

            bList.Add(new BigCloud());
            cList.Add(new SmallCloud());

            setbigcloudTime = Game1.r.Next(1000, 6400);
            setsmallcloudTime = Game1.r.Next(1000, 6400);

            bigcloudTime = 0;
            smallcloudTime = 0;

            peopleTime = 0;
        }

        int e = 0;

        Boolean savedOnce = false;

        public override void Update(MouseState ms, GameTime gt)
        {
            smallcloudTime += gt.ElapsedGameTime.Milliseconds;
            bigcloudTime += gt.ElapsedGameTime.Milliseconds;

            if (bigcloudTime > setbigcloudTime)
            {
                bigcloudTime = 0;
                bList.Add(new BigCloud());
                setbigcloudTime = Game1.r.Next(1000, 6400);
            }

            if (smallcloudTime > setsmallcloudTime)
            {
                smallcloudTime = 0;
                cList.Add(new SmallCloud());
                setsmallcloudTime = Game1.r.Next(1000, 6400);
            }

            for (int i = 0; i < bList.Count; i++)
            {
                if (bList[i].removeFlag)
                {
                    bList.Remove(bList[i]);
                    i--;
                }
            }

            if(peopleON){
                peopleTime += gt.ElapsedGameTime.Milliseconds;
                if (peopleTime > e)
                {
                    e += 100;
                    for (int i = 0; i < 6; i++)
                    {
                        fList.Add(new FlyingPeople());
                        if(e == 100)
                            if (i == 1)
                            {
                                int y = Game1.r.Next(1, 4);
                                if (y == 1 && !Game1.mute)
                                    scream1.Play();
                                if (y == 2 && !Game1.mute)
                                    scream2.Play();
                                if (y == 3 && !Game1.mute)
                                    scream3.Play();

                            }

                    }
                }

                if (peopleTime > 500)
                {
                    peopleON = false;
                    e = 0;
                    peopleTime = 0;
                }
            }

            for (int i = 0; i < cList.Count; i++)
            {
                if (cList[i].removeFlag)
                {
                    cList.Remove(cList[i]);
                    i--;
                }
            }

            for (int i = 0; i < fList.Count; i++)
            {
                if (fList[i].removeflag)
                {
                    fList.Remove(fList[i]);
                    i--;
                }
            }

            ground.Update();

            foreach (SmallCloud sm in cList)
            {
                sm.Update();
            }

            foreach (BigCloud bm in bList)
            {
                bm.Update();
            }

            ryuko.Update(gt);

            kiryuin.Update(gt);

            if((Keyboard.GetState().IsKeyDown(Keys.R) && Game1.oldkb.IsKeyUp(Keys.R)) || (Game1.gp.Buttons.Y == ButtonState.Pressed && Game1.oldgp.Buttons.Y == ButtonState.Released))
            {
                savedOnce = false;
                Game1.PlayingLoop = true;
                Game1.gameList.CurrentScreen = new Playing();
                Game1.gameList.CurrentScreen.Initialize();
                Game1.gameList.CurrentScreen.kiryuin.startthetext = false;
                Game1.gameList.CurrentScreen.ryuko.showContinue = true;
                if (!Game1.mute)
                {
                    MediaPlayer.Play(pressr);
                    Game1.Movement.Play();
                }
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.M)) || (Game1.gp.Buttons.B == ButtonState.Pressed))
            {
                savedOnce = false;
                Game1.gameList.CurrentScreen = new MainMenu();
                Game1.gameList.CurrentScreen.Initialize();
                if (!Game1.mute)
                Game1.Movement.Play();
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.Escape)) || (Game1.gp.Buttons.Back == ButtonState.Pressed))
            {
                if(ryuko.dead)
                    Game1.gameList.CurrentScreen = new Exit();
                else
                    Game1.gameList.CurrentScreen = new MainMenu();
                savedOnce = false;
                Game1.gameList.CurrentScreen.Initialize();
                if (!Game1.mute)
                Game1.Movement.Play();
            }

            foreach (FlyingPeople f in fList)
            {
                f.Update();
            }

            if (ryuko.dead)
                elapsedTime += gt.ElapsedGameTime.Milliseconds;
            
            if (ryuko.dead)
            {
                if (ryuko.score > Game1.highScore && !savedOnce)
                {
                    Game1.highScore = ryuko.score;
                    newh = true;
                    Game1.save = true;
                    savedOnce = true;
                }
            }
            
            //Updates all the numbers
            foreach (Animation an in xList)
            {
                an.Update(gt);
            }

            foreach (Animation an in sList)
            {
                an.Update(gt);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sky, new Vector2(0, 0), Color.White);

            foreach (SmallCloud sm in cList)
            {
                sm.Draw(sb);
            }

            //Just drawing the academy because it isn't an object
            sb.Draw(Academy, new Vector2(0, 0), Color.White);

            foreach (BigCloud bm in bList)
            {
                bm.Draw(sb);
            }

            if (kiryuin.drawcheer)
            {
                sb.Draw(cheerpeople, new Vector2() + Ground.shaking, Color.White);
            }
            else
                sb.Draw(people, new Vector2() + Ground.shaking, Color.White);

            foreach (FlyingPeople f in fList)
            {
                f.Draw(sb);
            }

            ground.Draw(sb);

            sb.Draw(bloodImage, new Vector2(0, 0), Color.White);

            ryuko.Draw(sb);
            
            kiryuin.Draw(sb);

            if (elapsedTime > 3000)
                opacity += 0.01f;

            if (ryuko.dead && !newh)
            {
                sb.Draw(highscoreTable, new Vector2(0, 0), Color.White * opacity);
                if (!playing && !Game1.mute)
                    MediaPlayer.Play(nonewh);
                playing = true;
            }

            else if(ryuko.dead)
            {
                sb.Draw(newhighscoreTable, new Vector2(0, 0), Color.White * opacity);
                if (!playing && !Game1.mute)
                MediaPlayer.Play(band);
                playing = true;
            }

            int yplus = 0;
            if (newh)
            {
                yplus = 25;
            }


            if (ryuko.dead)
            {
                sb.Draw(options, new Vector2(202, 345), Color.White * opacity);
                for (int i = 0; i < Game1.highScore.ToString().Length; i++)
                {
                    //Intializes the animation texture
                    xList[i].Initialize(RyukoMatoi.numbers, new Vector2(150, 50 + yplus), 50, 50, 1, 50, Color.White * opacity, 1, false);

                    //Selects the position
                    xList[i].Position = new Vector2((387 - ((Game1.highScore.ToString().Length * 20) / 2) + (i * 20)), 153 + yplus) + Ground.shaking;

                    //Gets the frame
                    xList[i].currentFrame = Int32.Parse(Game1.highScore.ToString().Substring(i, 1));

                    //Draws
                    xList[i].Draw(sb);
                }
                if (!newh)
                {
                    for (int i = 0; i < ryuko.score.ToString().Length; i++)
                    {
                        //Intializes the animation texture
                        sList[i].Initialize(RyukoMatoi.numbers, new Vector2(150, 50 + yplus), 50, 50, 1, 50, Color.White * opacity, 1, false);

                        //Selects the position
                        sList[i].Position = new Vector2((387 - ((ryuko.score.ToString().Length * 20) / 2) + (i * 20)), 230) + Ground.shaking;

                        //Gets the frame
                        sList[i].currentFrame = Int32.Parse(ryuko.score.ToString().Substring(i, 1));

                        //Draws
                        sList[i].Draw(sb);
                    }
                }
            }
        }
    }

    public class Exit : Screens
    {
        public Exit()
        {

        }

        public override void Initialize()
        {

        }

        public override void Update(MouseState ms, GameTime gt)
        {

        }

        public override void Draw(SpriteBatch sb)
        {

        }
    }
}
