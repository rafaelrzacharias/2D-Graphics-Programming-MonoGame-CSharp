using Microsoft.Xna.Framework;

namespace _2D_Graphics_Programming
{
    class Physics
    {
        public Vector2 velocity;
        public Vector2 maxVelocity;
        public Vector2 acceleration;
        public Vector2 maxAcceleration;
        public Vector2 oldVelocity;
        public float friction;
        public bool applyFriction;

        public Physics()
        {
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            oldVelocity = Vector2.Zero;

            friction = Game.BASE_FRICTION;
            maxAcceleration = new Vector2(Game.SCREEN_WIDTH / (8.0f * (1.0f + friction)), Game.SCREEN_HEIGHT / (8.0f * (1.0f + friction)));
            maxVelocity = maxAcceleration / Game.SCREENS_PER_SEC;
            applyFriction = false;
        }

        public void Setup(Vector2 maxVel, Vector2 maxAcc, float fric)
        {
            friction = fric;
            maxAcceleration = maxAcc;
            maxVelocity = maxVel;
        }

        public void Accelerate(GameTime gameTime, Vector2 jerk)
        {
            acceleration.X = jerk.X;
            acceleration.Y = jerk.Y;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity.X += acceleration.X * maxAcceleration.X * dt;
            if (velocity.X <= -maxVelocity.X)
                velocity.X = -maxVelocity.X;
            if (velocity.X >= maxVelocity.X)
                velocity.X = maxVelocity.X;

            velocity.Y += acceleration.Y * maxAcceleration.Y * dt;
            if (velocity.Y <= -maxVelocity.Y)
                velocity.Y = -maxVelocity.Y;
            if (velocity.Y >= maxVelocity.Y)
                velocity.Y = maxVelocity.Y;
        }

        public void Update(GameTime gameTime, Sprite sprite)
        {
            if (velocity != Vector2.Zero)
            {
                if (applyFriction == true)
                {
                    velocity.X *= (1 - friction);
                    if (((velocity.X / maxVelocity.X) <= 0.05f) && ((velocity.X / maxVelocity.X) >= -0.05f))
                        velocity.X = 0.0f;

                    velocity.Y *= (1 - friction);
                    if (((velocity.Y / maxVelocity.Y) <= 0.05f) && ((velocity.Y / maxVelocity.Y) >= -0.05f))
                        velocity.Y = 0.0f;
                }
                sprite.hasMoved = true;
            }

            acceleration = Vector2.Zero;
            oldVelocity.X = velocity.X;
            applyFriction = false;
        }         
    }
}