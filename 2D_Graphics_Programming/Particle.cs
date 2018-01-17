using Microsoft.Xna.Framework; // DEFAULT
using Microsoft.Xna.Framework.Graphics; // DEFAULT

namespace _2D_Graphics_Programming
{
    class Particle
    {
        public Sprite sprite;
        public int particleAge;
        public int particleFadeAge;
        public Color particleInitColor;
        public Color particleFinalColor;

        // Physics-related variables
        public Vector2 particleVelocity;
        public Vector2 particleAcceleration;
        public float particleVelFriction;
        public float particleRotVelocity;
        public float particleRotFriction;
        public float particleScaleVelocity;
        public float particleScaleAcceleration;
        public float particleMaxScale;

        public Particle()
        {
            sprite = new Sprite();
        }

        public void Create(Texture2D tex, int msAge, Vector2 pos, Vector2 vel, Vector2 acc, float velDamp, float initRot, float rotVel, float rotDamp, float initScale, float scaleVel, float scaleAcc, float maxScale, Color initColor, Color finalColor, int fadeAge)
        {
            sprite.Setup(tex, 0, 0, tex.Width, tex.Height);
            sprite.origin = new Vector2(sprite.rectangle.Width / 2, sprite.rectangle.Height / 2);
            sprite.position = pos;
            sprite.rotation = initRot;
            sprite.scale = initScale;

            particleAge = msAge;
            particleFadeAge = fadeAge;
            particleInitColor = initColor;
            particleFinalColor = finalColor;

            // Physics-related variables
            particleVelocity = vel;
            particleAcceleration = acc;
            particleVelFriction = velDamp;
            particleRotVelocity = rotVel;
            particleRotFriction = rotDamp;
            particleScaleVelocity = scaleVel;
            particleScaleAcceleration = scaleAcc;
            particleMaxScale = maxScale;
    }

        public void Update(GameTime gameTime)
        {
            if (particleAge < 0)
                return;
            particleAge -= gameTime.ElapsedGameTime.Milliseconds;

            // Physics-related code (velocity)
            particleVelocity *= (1 - particleVelFriction);
            particleVelocity += (particleAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);
            particleRotVelocity *= (1 - particleRotFriction);

            // Position
            sprite.hasMoved = true;
            sprite.Update(gameTime, particleVelocity);

            // Rotation
            sprite.rotation += (particleRotVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            // Scale
            particleScaleVelocity += (particleScaleAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);
            sprite.scale += (particleScaleVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (sprite.scale >= particleMaxScale)
                sprite.scale = particleMaxScale;
            if (sprite.scale <= 0.0f)
                sprite.scale = 0.0f;

            // Color
            if ((particleAge > particleFadeAge) && (particleFadeAge != 0))
                sprite.color = particleInitColor;
            else
            {
                float amtInit = (float)particleAge / particleFadeAge;
                float amtFinal = 1.0f - amtInit;
                sprite.color.R = (byte)((amtInit * particleInitColor.R) + (amtFinal * particleFinalColor.R));
                sprite.color.G = (byte)((amtInit * particleInitColor.G) + (amtFinal * particleFinalColor.G));
                sprite.color.B = (byte)((amtInit * particleInitColor.B) + (amtFinal * particleFinalColor.B));
                sprite.color.A = (byte)((amtInit * particleInitColor.A) + (amtFinal * particleFinalColor.A));
            }
        }

        public void Draw(SpriteBatch batch, Vector2 cameraPos)
        {
            if (particleAge < 0)
                return;
            sprite.Draw(batch, cameraPos);
        }
    }
}
