using Microsoft.Xna.Framework; // DEFAULT
using Microsoft.Xna.Framework.Graphics; // DEFAULT
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace _2D_Graphics_Programming
{
    class ParticleSystem
    {
        public List<ParticleEffect> particleEffects;

        public ParticleSystem()
        {
            particleEffects = new List<ParticleEffect>();
        }

        public void Setup(GraphicsDevice graphicsDevice)
        {
            ParticleEffect.Setup(graphicsDevice);
        }

        public void AddEffect(EffectType type)
        {
            ParticleEffect tempEffect = new ParticleEffect();
            tempEffect.Create(type);
            particleEffects.Add(tempEffect);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = particleEffects.Count() - 1; i >= 0; i--)
            {
                particleEffects[i].Update(gameTime);

                if (!particleEffects[i].IsAlive())
                    particleEffects.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch batch, Vector2 cameraLoc)
        {
            foreach (ParticleEffect e in particleEffects)
                e.Draw(batch, cameraLoc);
        }
    }
}
