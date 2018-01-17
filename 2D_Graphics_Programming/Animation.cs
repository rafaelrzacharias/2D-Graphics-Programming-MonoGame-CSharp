using Microsoft.Xna.Framework;

namespace _2D_Graphics_Programming
{
    class Animation
    {
        public int currentCel;
        public int numberOfCels;
        public int msUntilNextCel;
        public int msPerCel;

        public Animation()
        {
            currentCel = 0;
            numberOfCels = 0;
            msUntilNextCel = 0;
            msPerCel = 0;
        }

        public void Setup(int numCels, int msCel)
        {
            numberOfCels = numCels;
            msPerCel = msCel;
        }

        public void Update(GameTime gameTime, Sprite sprite)
        {
            msUntilNextCel -= gameTime.ElapsedGameTime.Milliseconds;

            if (msUntilNextCel <= 0)
            {
                currentCel++;
                msUntilNextCel = msPerCel;
            }

            if (currentCel >= numberOfCels)
                currentCel = 0;

            sprite.rectangle.X = sprite.rectangle.Width * currentCel;
        }
    }
}