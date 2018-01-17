using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Graphics_Programming
{
    class Sprite
    {
        public Texture2D texture;
        public Vector2 origin;
        public Rectangle rectangle;
        public Vector2 position;
        public Color color;
        public float rotation;
        public float scale;
        public float depth;
        public SpriteEffects spriteEffect;
        public bool flipHorizontally;
        public bool hasMoved;
        public Vector2 cameraOffset;

        public Sprite()
        {
            position = Vector2.Zero;
            color = Color.White;
            spriteEffect = SpriteEffects.None;
            rotation = 0.0f;
            scale = 1.0f;
            origin = Vector2.Zero;
            rectangle = new Rectangle(0, 0, 0, 0);
            depth = 0.0f;
            flipHorizontally = false;
            hasMoved = false;
            cameraOffset = new Vector2(Game.SCREEN_WIDTH / 2, Game.SCREEN_HEIGHT / 2);
        }

        public void Setup(Texture2D tex, int rectX, int rectY, int rectWidth, int rectHeight)
        {
            texture = tex;
            rectangle = new Rectangle(rectX, rectY, rectWidth, rectHeight);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
        }

        public void Update(GameTime gameTime, Vector2 velocity)
        {
            if (hasMoved == true)
            {
                position.X += (velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
                position.Y += (velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            hasMoved = false;
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            depth = (position.Y + rectangle.Height / 2) / Game.WORLD_HEIGHT;

            if (flipHorizontally == true)
                spriteEffect = SpriteEffects.FlipHorizontally;
            else
                spriteEffect = SpriteEffects.None;
        }

        public void Draw(SpriteBatch batch, Vector2 cameraPosition)
        {
            batch.Draw(texture, position - cameraPosition + cameraOffset, rectangle, color, rotation, origin, scale, spriteEffect, depth);
        }
    }
}