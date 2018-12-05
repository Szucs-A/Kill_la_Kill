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
    public class ParticleLauncher
    {
        public static Random r = new Random();

        //its list of particles
        public List<Particle> particles = new List<Particle>();

        //the obvious things
        Vector2 position;
        Color particleColor;
        int launchNumber;
        int ShootTime;

        //size of particle
        Vector2 size;
        
        //ygravity
        float gravity;

        //xgravity
        float sideways;

        //the how much is added to the y
        int ymultiplier;
        int xmultiplier;

        //cmon
        int elapsedTime;

        public Boolean active;

        public Boolean shootnow;

        int ground;

        Boolean disappear;

        Boolean randomground;

        //constructor
        public ParticleLauncher(int shootTIme, int numberof, Color parColor, Vector2 pos, Vector2 sizz, float side, float down, int ymul, Boolean actife, int g, Boolean d, int xmul, Boolean Randomground)
        {
            ShootTime = shootTIme;
            particleColor = parColor;
            position = pos;
            launchNumber = numberof;
            elapsedTime = 0;
            sideways = side;
            gravity = down;
            ymultiplier = ymul;
            xmultiplier = xmul;

            size = sizz;
            active = actife;
            shootnow = false;
            disappear = d;
            ground = g;
            randomground = Randomground;
        }

        public void Update(GameTime gt)
        {
            if (active)
            {
                //if ready to shoot
                if (elapsedTime > ShootTime)
                {
                    //for how many make some particles
                    for (int i = 0; i < launchNumber; i++)
                    {
                        particles.Add(new Particle((int)size.X, (int)size.Y, particleColor, position, new Vector2((float)(r.NextDouble() - r.NextDouble()) + xmultiplier, (float)(r.NextDouble() - r.NextDouble()) + ymultiplier), gravity, sideways,ground, disappear, randomground ));
                    }
                    elapsedTime = 0;
                }
                else
                {
                    elapsedTime += gt.ElapsedGameTime.Milliseconds;
                }

                if (shootnow)
                {
                    //for how many make some particles
                    for (int i = 0; i < launchNumber; i++)
                    {
                        particles.Add(new Particle((int)size.X, (int)size.Y, particleColor, position, new Vector2((float)(r.NextDouble() - r.NextDouble()) + xmultiplier, (float)(r.NextDouble() - r.NextDouble()) + ymultiplier), gravity, sideways, ground, disappear, randomground));
                    }
                    shootnow = false;
                }
            }
                //update these particles in list
                for (int i = 0; i < particles.Count; i++)
                {
                    //remove the ones that have remove flag
                    particles[i].Update();
                    if (particles[i].removeFlag)
                    {
                        particles.Remove(particles[i]);
                        i--;
                    }
                }
            
        }

        public void Draw(SpriteBatch sb)
        {
            //draw them particles
            foreach (Particle part in particles)
            {
                part.Draw(sb);
            }
        }
    }
}
