using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Graphics_Programming
{
    class Player
    {
        public Sprite sprite;
        public Animation animation;
        public Physics physics;
        public Controller controller;
        public float threshould;

        public Player()
        {
            sprite = new Sprite();
            animation = new Animation();
            physics = new Physics();
            controller = new Controller();
            threshould = 0.2f;
        }

        public void Setup(Texture2D tex, int rectX, int rectY, int rectWidth, int rectHeight, int numCels, int msCel)
        {
            sprite.Setup(tex, rectX, rectY, rectWidth, rectHeight);
            animation.Setup(numCels, msCel);
        }

        public void Action(GameTime gameTime, Controller.InputState state)
        {
            switch (state.action)
            {
                case Controller.Action.NONE:
                    break;
                case Controller.Action.MOVE_RIGHT:
                    physics.Accelerate(gameTime, new Vector2(state.jerk, 0.0f));
                    break;
                case Controller.Action.MOVE_DOWN:
                    physics.Accelerate(gameTime, new Vector2(0.0f, state.jerk));
                    break;
                case Controller.Action.MOVE_LEFT:
                    physics.Accelerate(gameTime, new Vector2(state.jerk, 0.0f));
                    break;
                case Controller.Action.MOVE_UP:
                    physics.Accelerate(gameTime, new Vector2(0.0f, state.jerk));
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            Action(gameTime, controller.Update());

            // If direction X OR Y changes AND above a threshould... OR if not accelerating, apply friction
            if ((physics.acceleration == Vector2.Zero) ||
                (((physics.velocity.X >= 0.0f) != (physics.acceleration.X >= 0.0f) || (physics.velocity.Y >= 0.0f) != (physics.acceleration.Y >= 0.0f)) &&
                ((((physics.velocity.X / physics.maxVelocity.X) < -threshould) || ((physics.velocity.X / physics.maxVelocity.X) > threshould)) ||
                (((physics.velocity.Y / physics.maxVelocity.Y) < -threshould) || ((physics.velocity.Y / physics.maxVelocity.Y) > threshould)))))
            {
                physics.applyFriction = true;
            }

            if (physics.acceleration.X < 0.0f && ((physics.velocity.X > 0.0f) != (physics.oldVelocity.X >= 0.0f)))
                sprite.flipHorizontally = true;
            else if (physics.acceleration.X > 0.0f && ((physics.velocity.X > 0.0f) != (physics.oldVelocity.X > 0.0f)))
                sprite.flipHorizontally = false;

            physics.Update(gameTime, sprite);
            sprite.Update(gameTime, physics.velocity);
            animation.Update(gameTime, sprite);
        }

        public void Draw(SpriteBatch batch, Vector2 cameraPosition)
        {
            sprite.Draw(batch, cameraPosition);
        }
    }
}