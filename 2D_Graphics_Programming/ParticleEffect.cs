using Microsoft.Xna.Framework; // DEFAULT
using Microsoft.Xna.Framework.Graphics; // DEFAULT
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

/*Other ideas for effects could be:
 * Update the effectOrigin through the game code to make the particles move, following (or running away from) the player, enemies or other objects.
 * This following effect can create puffs of dust when running, trails of blood when player/enemies are hurt, footsteps.
 * Copy and leave behind the player, enemy or a moving object's own sprite as they move to create a blur effect.*/

namespace _2D_Graphics_Programming
{
    public enum EffectType
    {
        SPIRAL, SMOKE, POINT_FIRE, WALL_FIRE, MOVING_FIRE, EXPLOSION, SNOW
    }

    // Effect textures and loading content are done only once.
    class ParticleEffect
    {
        public EffectType effectType;

        static Texture2D particleTexture;
        static Texture2D snowflakeTexture;
        static Texture2D circleTexture;
        static Texture2D starTexture;

        public Vector2 effectOrigin;
        public int effectRadius;
        public int effectDuration;
        public int newParticleAmount;
        public int burstFrequency;
        public int burstCountdown;
        public bool isRunning;
        public BlendState blendType;
        public Random random;
        public List<Particle> particles;

        public ParticleEffect()
        {
            particles = new List<Particle>();
            isRunning = false;
            random = new Random();
        }

        public bool IsAlive()
        {
            if (effectDuration > 0)
                return true;
            if (particles.Count() > 0)
                return true;
            return false;
        }

        static public void Setup(GraphicsDevice graphicsDevice)
        {
            FileStream stream = new FileStream("snowflake.png", FileMode.Open);
            snowflakeTexture = Texture2D.FromStream(graphicsDevice, stream);
            stream.Close();

            stream = new FileStream("whiteCircle.png", FileMode.Open);
            circleTexture = Texture2D.FromStream(graphicsDevice, stream);
            stream.Close();

            stream = new FileStream("whiteStar.png", FileMode.Open);
            starTexture = Texture2D.FromStream(graphicsDevice, stream);
            stream.Close();

            stream.Dispose();
        }

        public void Create(EffectType type)
        {
            effectType = type;

            switch (effectType)
            {
                case EffectType.SPIRAL:
                    Spiral();
                    break;
                case EffectType.SMOKE:
                    Smoke();
                    break;
                case EffectType.POINT_FIRE:
                case EffectType.WALL_FIRE:
                case EffectType.MOVING_FIRE:
                    Fire();
                    break;
                case EffectType.EXPLOSION:
                    Explosion();
                    break;
                case EffectType.SNOW:
                    Snowfall();
                    break;
            }
        }

        public void CreateParticle()
        {
            switch (effectType)
            {
                case EffectType.SPIRAL:
                    CreateSpiral();
                    break;
                case EffectType.SMOKE:
                    CreateSmoke();
                    break;
                case EffectType.POINT_FIRE:
                    CreatePointFire();
                    break;
                case EffectType.WALL_FIRE:
                    CreateWallFire();
                    break;
                case EffectType.MOVING_FIRE:
                    CreateMovingFire();
                    break;
                case EffectType.EXPLOSION:
                    CreateExplosion();
                    break;
                case EffectType.SNOW:
                    CreateSnowfall();
                    break;
            }
        }

        public void Spiral()
        {
            effectDuration = 10000;
            newParticleAmount = 1;
            burstFrequency = 16;
            burstCountdown = burstFrequency;
            effectOrigin = Vector2.Zero;
            effectRadius = 0;
            blendType = BlendState.AlphaBlend;
        }

        public void Smoke()
        {
            effectDuration = 60000;
            newParticleAmount = 4;
            burstFrequency = 16;
            burstCountdown = burstFrequency;
            effectOrigin = Vector2.Zero;
            effectRadius = 50;
            blendType = BlendState.Additive;
        }

        public void Fire()
        {
            effectDuration = 60000;
            newParticleAmount = 10;
            burstFrequency = 16;
            burstCountdown = burstFrequency;
            effectOrigin = Vector2.Zero;
            effectRadius = 50;
            blendType = BlendState.Additive;
        }

        public void Explosion()
        {
            effectDuration = 16;
            newParticleAmount = 800;
            burstFrequency = 16;
            burstCountdown = burstFrequency;
            effectOrigin = Vector2.Zero;
            effectRadius = 20;
            blendType = BlendState.NonPremultiplied;
        }

        public void Snowfall()
        {
            effectDuration = 60000;
            newParticleAmount = 1;
            burstFrequency = 64;
            burstCountdown = burstFrequency;
            effectOrigin = Vector2.Zero;
            effectRadius = 50;
            blendType = BlendState.NonPremultiplied;
        }

        public void CreateSpiral()
        {
            particleTexture = starTexture;
            int initAge = 3000;

            Vector2 initPos;
            Vector2 offset;
            if (effectRadius == 0)
            {
                initPos = effectOrigin;
                offset = Vector2.Zero;
            }
            else
            {
                offset = new Vector2(
                (float)(random.Next(effectRadius) * Math.Cos(random.Next(360))),
                (float)(random.Next(effectRadius) * Math.Sin(random.Next(360))));
                initPos = effectOrigin + offset;
            }

            // Spiraling particle effect
            Vector2 initVel = new Vector2(
                (float)(100.0f * Math.Cos(effectDuration)),
                (float)(100.0f * Math.Sin(effectDuration)));
            Vector2 initAcc = new Vector2(0, 0);
            float velFric = 0.0f;

            float initRot = 0.0f;
            float initRotVel = 2.0f;
            float rotFric = 0.01f;

            float initScale = 0.1f;
            float initScaleVel = 0.4f;
            float initScaleAcc = -0.2f;
            float maxScale = 1.0f;

            Color initColor = Color.Black;
            Color finalColor = Color.Yellow;
            finalColor.A = 0;
            int fadeAge = initAge;

            Particle particle = new Particle();
            particle.Create
                (particleTexture, initAge, initPos,
                initVel, initAcc, velFric,
                initRot, initRotVel, rotFric,
                initScale, initScaleVel, initScaleAcc, maxScale,
                initColor, finalColor, fadeAge);

            particles.Add(particle);
        }

        public void CreateSmoke()
        {
            particleTexture = circleTexture;
            int initAge = 5000 + random.Next(5000);
            int fadeAge = initAge - random.Next(5000);

            Vector2 initPos;
            Vector2 offset;
            Vector2 offset2;
            if (effectRadius == 0)
            {
                initPos = effectOrigin;
                offset = Vector2.Zero;
            }
            else
            {
                offset = new Vector2(
                (float)(random.Next(effectRadius) * Math.Cos(random.Next(360))),
                (float)(random.Next(effectRadius) * Math.Sin(random.Next(360))));
                offset2 = new Vector2((float)(400 * Math.Cos(effectDuration / 500.0f)), 0.0f);
                initPos = effectOrigin + offset + offset2;
            }

            // Smoke Effect. Could also be used for Fog or Clouds.
            // For better-looking effect, consider using a puff texture.
            Vector2 initVel = new Vector2(0.0f, -30 - random.Next(30));
            Vector2 initAcc = new Vector2(10 + random.Next(10), 0.0f);
            float velFric = 0.00f;

            float initRot = 10 + random.Next(10);
            float initRotVel = 0.05f;
            float rotFric = 0.0f;

            float initScale = 0.6f;
            float initScaleVel = random.Next(10) / 50.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 3.0f;

            Color initColor = Color.Black;
            initColor.A = 128;
            Color finalColor = new Color(32, 32, 32, 0);

            Particle particle = new Particle();
            particle.Create
                (particleTexture, initAge, initPos,
                initVel, initAcc, velFric,
                initRot, initRotVel, rotFric,
                initScale, initScaleVel, initScaleAcc, maxScale,
                initColor, finalColor, fadeAge);

            particles.Add(particle);
        }

        public void CreatePointFire()
        {
            particleTexture = circleTexture;
            int initAge = 3000;

            Vector2 initPos;
            Vector2 offset;
            if (effectRadius == 0)
            {
                initPos = effectOrigin;
                offset = Vector2.Zero;
            }
            else
            {
                offset = new Vector2(
                (float)(random.Next(effectRadius) * Math.Cos(random.Next(360))),
                (float)(random.Next(effectRadius) * Math.Sin(random.Next(360))));
                initPos = effectOrigin + offset;
            }

            // Fire Effect
            Vector2 initVel = new Vector2(-offset.X * 0.5f, 0.0f);
            Vector2 initAcc = new Vector2(0.0f, -random.Next(200));
            float velFric = 0.04f;

            float initRot = 0.0f;
            float initRotVel = 0.0f;
            float rotFric = 0.0f;

            float initScale = 0.5f;
            float initScaleVel = -0.1f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            Color initColor = Color.Red;
            Color finalColor = Color.Yellow;
            finalColor.A = 0;
            int fadeAge = 2750;

            Particle particle = new Particle();
            particle.Create
                (particleTexture, initAge, initPos,
                initVel, initAcc, velFric,
                initRot, initRotVel, rotFric,
                initScale, initScaleVel, initScaleAcc, maxScale,
                initColor, finalColor, fadeAge);

            particles.Add(particle);
        }

        public void CreateWallFire()
        {
            particleTexture = circleTexture;
            int initAge = 3000;

            Vector2 initPos;
            Vector2 offset;
            if (effectRadius == 0)
            {
                initPos = effectOrigin;
                offset = Vector2.Zero;
            }
            else
            {
                offset = new Vector2((float)(200 * Math.Cos(effectDuration)), 0.0f);
                initPos = effectOrigin + offset;
            }

            // Fire Effect
            Vector2 initVel = new Vector2(-offset.X * 0.5f, 0.0f);
            Vector2 initAcc = new Vector2(0.0f, -random.Next(200));
            float velFric = 0.04f;

            float initRot = 0.0f;
            float initRotVel = 0.0f;
            float rotFric = 0.0f;

            float initScale = 0.5f;
            float initScaleVel = -0.1f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            Color initColor = Color.Red;
            Color finalColor = Color.Yellow;
            finalColor.A = 0;
            int fadeAge = 2750;

            Particle particle = new Particle();
            particle.Create
                (particleTexture, initAge, initPos,
                initVel, initAcc, velFric,
                initRot, initRotVel, rotFric,
                initScale, initScaleVel, initScaleAcc, maxScale,
                initColor, finalColor, fadeAge);

            particles.Add(particle);
        }

        public void CreateMovingFire()
        {
            particleTexture = circleTexture;
            int initAge = 500 + random.Next(500); // 3 seconds
            int fadeAge = initAge - random.Next(100);

            Vector2 initPos;
            Vector2 offset;
            if (effectRadius == 0)
            {
                initPos = effectOrigin;
                offset = Vector2.Zero;
            }
            else
            {
                offset = new Vector2((float)(200 * Math.Cos(effectDuration / 500.0f)), 0.0f);
                initPos = effectOrigin + offset;
            }

            // Moving Fire Effect
            Vector2 initVel = new Vector2(0.0f, -500.0f);
            Vector2 initAcc = new Vector2(0.0f, -random.Next(300));
            float velFric = 0.04f;

            float initRot = 0.0f;
            float initRotVel = 0.0f;
            float rotFric = 0.0f;

            float initScale = 0.5f;
            float initScaleVel = -0.1f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            Color initColor = Color.DarkBlue;
            Color finalColor = Color.DarkOrange;
            finalColor.A = 0;

            Particle particle = new Particle();
            particle.Create
                (particleTexture, initAge, initPos,
                initVel, initAcc, velFric,
                initRot, initRotVel, rotFric,
                initScale, initScaleVel, initScaleAcc, maxScale,
                initColor, finalColor, fadeAge);

            particles.Add(particle);
        }

        public void CreateExplosion()
        {
            particleTexture = starTexture;
            int initAge = 3000 + random.Next(5000);
            int fadeAge = initAge / 2;

            Vector2 initPos;
            Vector2 offset;
            if (effectRadius == 0)
            {
                initPos = effectOrigin;
                offset = Vector2.Zero;
            }
            else
            {
                offset = new Vector2(
                (float)(random.Next(effectRadius) * Math.Cos(random.Next(360))),
                (float)(random.Next(effectRadius) * Math.Sin(random.Next(360))));
                initPos = effectOrigin + offset;
            }

            // Explosion Effect. Could also be used for impacts or fireworks.
            Vector2 initVel = new Vector2(random.Next(500) + (offset.X * 30.0f), -60.0f * Math.Abs(offset.Y));
            Vector2 initAcc = new Vector2(0.0f, 400.0f);
            float velFric = 0.0f;

            float initRot = 0.0f;
            float initRotVel = initVel.X / 50.0f;
            float rotFric = 0.03f;

            float initScale = 0.1f + (random.Next(10) / 50.0f);
            float initScaleVel = (random.Next(10) - 5) / 50.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            byte randomRed = (byte)(random.Next(128) + 128);
            Color initColor = new Color(randomRed, 0, 0);
            Color finalColor = new Color(32, 32, 32);

            Particle particle = new Particle();
            particle.Create
                (particleTexture, initAge, initPos,
                initVel, initAcc, velFric,
                initRot, initRotVel, rotFric,
                initScale, initScaleVel, initScaleAcc, maxScale,
                initColor, finalColor, fadeAge);

            particles.Add(particle);
        }

        public void CreateSnowfall()
        {
            particleTexture = snowflakeTexture;
            Vector2 initPos;
            Vector2 offset;
            Vector2 offset2;
            if (effectRadius == 0)
            {
                initPos = effectOrigin;
                offset = Vector2.Zero;
            }
            else
            {
                offset = new Vector2(
                    (float)(random.Next(effectRadius) * Math.Cos(random.Next(360))),
                    (float)(random.Next(effectRadius) * Math.Sin(random.Next(360))));
                offset2 = new Vector2((float)(600 * Math.Cos(effectDuration / 500.0f)), 0.0f);
                initPos = effectOrigin + offset + offset2;
            }

            // Snow Effect. Could also be used for rain or falling leaves.
            // For better-looking results, use the appropriate textures for each case.
            float initScale = 0.1f + (random.Next(10) / 20.0f);
            float initScaleVel = 0.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            int initAge = (int)(10000 / initScale);
            int fadeAge = initAge;

            Vector2 initVel = new Vector2(random.Next(10) - 5, 100 * initScale);
            Vector2 initAcc = Vector2.Zero;
            float velFric = 0.0f;

            float initRot = 0.0f;
            float initRotVel = initVel.X / 5.0f; // Rain or leaves do not rotate here.
            float rotFric = 0.0f;

            Color initColor = Color.White;
            Color finalColor = Color.White;
            finalColor.A = 0;

            Particle particle = new Particle();
            particle.Create
                (particleTexture, initAge, initPos,
                initVel, initAcc, velFric,
                initRot, initRotVel, rotFric,
                initScale, initScaleVel, initScaleAcc, maxScale,
                initColor, finalColor, fadeAge);

            particles.Add(particle);
        }

        public void Update(GameTime gameTime)
        {
            effectDuration -= gameTime.ElapsedGameTime.Milliseconds;
            burstCountdown -= gameTime.ElapsedGameTime.Milliseconds;

            if ((burstCountdown <= 0) && (effectDuration >= 0))
            {
                for (int i = 0; i < newParticleAmount; i++)
                    CreateParticle();
                burstCountdown = burstFrequency;
            }

            for (int i = particles.Count() - 1; i >= 0; i--)
            {
                particles[i].Update(gameTime);

                if (particles[i].particleAge <= 0)
                    particles.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch batch, Vector2 cameraPos)
        {
            batch.Begin(SpriteSortMode.FrontToBack, blendType);
            foreach (Particle p in particles)
                p.Draw(batch, cameraPos);
            batch.End();
        }
    }
}
