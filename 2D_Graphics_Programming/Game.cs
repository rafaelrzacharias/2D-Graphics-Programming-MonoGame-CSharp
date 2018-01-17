using Microsoft.Xna.Framework; // DEFAULT
using Microsoft.Xna.Framework.Graphics; // DEFAULT
using Microsoft.Xna.Framework.Input; // DEFAULT
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.IO; // For FileStream
using System;

namespace _2D_Graphics_Programming
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics; // DEFAULT
        SpriteBatch spriteBatch; // DEFAULT

        // Video
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;
        public const float BASE_FRICTION = 0.05f;
        public const float SCREENS_PER_SEC = 0.24f;
        public const int WORLD_WIDTH = 2000;
        public const int WORLD_HEIGHT = 1200;

        // Textures
        private Texture2D snowAssetsTexture;
        private Texture2D backgroundTexture;
        private Texture2D playerTexture;

        // Objects
        Player player;
        Sprite background;
        Sprite snowman;

        ParticleSystem particleSystem;

        // Shader effects
        Effect uniformBlur;
        Effect radialBlur;
        Effect darken;
        // Shader parameter name
        EffectParameter darkenBrightness;

        // Game state
        public enum GameStates
        {
            NULL, SETUP, FRONT_END, WRAP_TO, MAIN_GAME, LEVEL_END, PAUSE, EXIT
        }
        public enum TransitionStates
        {
            FADE_IN, FULL, FADE_OUT
        }
        private GameStates stateNext;
        private GameStates currentState;
        private TransitionStates transState;
        private double timeNext;
        private double timeMax = 0.5f; // Half a second for the transition effect
        public byte fadeAlpha;

        // List of control points to be used by the spline
        List<Vector2> controlPoints;
        Texture2D whiteCircle;

        // Camera
        public Vector2 cameraPosition;
        public Vector2 cameraOffset;

        /*Sprite floor;
        Sprite plains;
        Sprite mountains;
        Sprite hills;
        Sprite water;
        Sprite forest;
        private Texture2D tilesTexture;
        private Texture2D mapTexture;
        private Color[] gameMap = new Color[MAP_WIDTH * MAP_HEIGHT];

        private Vector2 playerMapPosition;
        private Vector2 playerScreenPosition;

        private const int SPRITE_WIDTH = 32;
        private const int SPRITE_HEIGHT = 32;
        private const int MAP_WIDTH = 256;
        private const int MAP_HEIGHT = 256;*/

        public Game()
        {
            graphics = new GraphicsDeviceManager(this); // DEFAULT
            Content.RootDirectory = "Content"; // DEFAULT
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            player = new Player();
            background = new Sprite();
            snowman = new Sprite();
            particleSystem = new ParticleSystem();
            controlPoints = new List<Vector2>();
            /*plains = new Sprite();
            mountains = new Sprite();
            hills = new Sprite();
            water = new Sprite();
            forest = new Sprite();*/
        }

        protected override void Initialize()
        {
            cameraOffset = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);

            /*// Control points to be used for the spline
            controlPoints.Add(new Vector2(100, 300));
            controlPoints.Add(new Vector2(300, 600));
            controlPoints.Add(new Vector2(500, 550));
            controlPoints.Add(new Vector2(700, 350));
            controlPoints.Add(new Vector2(900, 150));
            controlPoints.Add(new Vector2(1100, 700));*/

            base.Initialize(); // DEFAULT
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice); // DEFAULT

            // Never have a static screen. Show a visual feedback, such as a percentage bar
            // For every asset loaded, draw a percentage bar slightly larger until 100% is reached

            // Load textures
            FileStream stream1 = new FileStream("run_cycle.png", FileMode.Open);
            playerTexture = Texture2D.FromStream(GraphicsDevice, stream1);
            stream1.Dispose();
            FileStream stream2 = new FileStream("snow_assets.png", FileMode.Open);
            snowAssetsTexture = Texture2D.FromStream(GraphicsDevice, stream2);
            stream2.Dispose();
            FileStream stream3 = new FileStream("snow_background.png", FileMode.Open);
            backgroundTexture = Texture2D.FromStream(GraphicsDevice, stream3);
            stream3.Dispose();
            FileStream stream4 = new FileStream("whiteCircle.png", FileMode.Open);
            whiteCircle = Texture2D.FromStream(GraphicsDevice, stream4);
            stream4.Dispose();

            player.Setup(playerTexture, 0, 0, 128, 114, 12, 50);
            snowman.Setup(snowAssetsTexture, 56, 160, 132, 174);
            background.Setup(backgroundTexture, 0, 0, backgroundTexture.Width, backgroundTexture.Height);
            particleSystem.Setup(GraphicsDevice);

            // Load the shaders
            //uniformBlur = Content.Load<Effect>("uniformBlur");
            //radialBlur = Content.Load<Effect>("radialBlur");
            //darken = Content.Load<Effect>("darken");

            // Link the parameter inside darken shader with the variable here
            //darkenBrightness = darken.Parameters["brightness"]; // String name must match parameter name inside shader

            /*FileStream stream4 = new FileStream("tile_sprites.png", FileMode.Open);
            tilesTexture = Texture2D.FromStream(GraphicsDevice, stream4);
            stream4.Dispose();*/
            /*FileStream stream5 = new FileStream("map01.png", FileMode.Open);
            mapTexture = Texture2D.FromStream(GraphicsDevice, stream5);
            stream5.Dispose();

            plains.Initialize(tilesTexture, SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2, 0, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            mountains.Initialize(tilesTexture, SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2, 0, SPRITE_HEIGHT, SPRITE_WIDTH, SPRITE_HEIGHT);
            hills.Initialize(tilesTexture, SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2, 2 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            water.Initialize(tilesTexture, SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2, SPRITE_WIDTH, SPRITE_HEIGHT, SPRITE_WIDTH, SPRITE_HEIGHT);
            forest.Initialize(tilesTexture, SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2, SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT);;
            player.Initialize(tilesTexture, SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2, 2 * SPRITE_WIDTH, SPRITE_HEIGHT, SPRITE_WIDTH, SPRITE_HEIGHT);
            
            // Copy the data in the texture directly into the array of colors
            mapTexture.GetData(gameMap);*/
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                snowman.position.Y += 2;
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
                snowman.position.Y -= 2;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                snowman.position.X -= 2;
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
                snowman.position.X += 2;

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad0))
                particleSystem.AddEffect(EffectType.EXPLOSION);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                particleSystem.AddEffect(EffectType.MOVING_FIRE);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                particleSystem.AddEffect(EffectType.POINT_FIRE);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                particleSystem.AddEffect(EffectType.SMOKE);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                particleSystem.AddEffect(EffectType.SNOW);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
                particleSystem.AddEffect(EffectType.SPIRAL);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                particleSystem.AddEffect(EffectType.WALL_FIRE);

            /*// Update game state and transitions
            timeNext -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timeNext <= 0)
            {
                if (transState == TransitionStates.FADE_IN)
                    transState = TransitionStates.FULL;
                else if (transState == TransitionStates.FADE_OUT)
                {
                    transState = TransitionStates.FADE_IN;
                    currentState = stateNext;
                    timeNext = timeMax;
                }
                else
                {
                    // Do nothing, timer is only used when transition is occuring
                }
            }

            if (transState == TransitionStates.FADE_IN)
            {
                if (currentState != GameStates.LEVEL_END)
                    fadeAlpha = (byte)((timeNext / timeMax) * 255); // Alpha 0->255
            }
            else if (transState == TransitionStates.FADE_OUT)
            {
                if (stateNext != GameStates.LEVEL_END)
                    fadeAlpha = (byte)(((-timeNext / timeMax) * 255) + 255); // Alpha 255->0
            }
            else
            {
                fadeAlpha = 0;
            }*/

            cameraPosition = player.sprite.position;
            // Restrict the camera within the world boundaries
            //cameraPosition.X = MathHelper.Clamp(cameraPosition.X, cameraOffset.X, WORLD_WIDTH - cameraOffset.X);
            //cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, cameraOffset.Y, WORLD_HEIGHT - cameraOffset.Y);

            /*// Update player position on the map
            playerMapPosition.X += (float)(playerForwardVelocity * Math.Cos(playerRotation) * gameTime.ElapsedGameTime.TotalSeconds);
            playerMapPosition.Y += (float)(playerForwardVelocity * Math.Sin(playerRotation) * gameTime.ElapsedGameTime.TotalSeconds);

            // Convert from map to screen coordinates
            playerScreenPosition.X = playerMapPosition.X * SPRITE_WIDTH;
            playerScreenPosition.Y = playerMapPosition.Y * SPRITE_HEIGHT;

            // Top-down tiled: Limit the camera movement to the world boundaries
            if (cameraPosition.X > worldEdge.Width - 2 * SPRITE_WIDTH - cameraOffset.X + SPRITE_WIDTH / 2)
                cameraPosition.X = worldEdge.Width - 2 * SPRITE_WIDTH - cameraOffset.X + SPRITE_WIDTH / 2;
            if (cameraPosition.X < worldEdge.X + cameraOffset.X - SPRITE_WIDTH / 2)
                cameraPosition.X = worldEdge.X + cameraOffset.X - SPRITE_WIDTH / 2;
            if (cameraPosition.Y > worldEdge.Height - 2 * SPRITE_HEIGHT - cameraOffset.Y + SPRITE_HEIGHT / 2)
                cameraPosition.Y = worldEdge.Height - 2 * SPRITE_HEIGHT - cameraOffset.Y + SPRITE_HEIGHT / 2;
            if (cameraPosition.Y < worldEdge.Y + cameraOffset.Y - SPRITE_HEIGHT / 2)
                cameraPosition.Y = worldEdge.Y + cameraOffset.Y - SPRITE_HEIGHT / 2;*/

            // Update brightness variable to update darken parameter inside shader
            //float pulse = Math.Abs((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 500.0f));
            //darkenBrightness.SetValue(pulse);

            // Update all game objects.
            // Update background tiles if they should to be in front of the player.
            player.Update(gameTime);
            snowman.Update(gameTime, Vector2.Zero);
            particleSystem.Update(gameTime);
            base.Update(gameTime); // DEFAULT
        }

        protected override void Draw(GameTime gameTime)
        {
            RenderTargetBinding[] binding = GraphicsDevice.GetRenderTargets();
            RenderTarget2D tempRenderTarget = new RenderTarget2D(GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);
            GraphicsDevice.SetRenderTarget(tempRenderTarget);
            GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);

            background.Draw(spriteBatch, cameraPosition);
            snowman.Draw(spriteBatch, cameraPosition);
            player.Draw(spriteBatch, cameraPosition);

            /*for (int x = 0; x < SCREEN_WIDTH; x += 50)
            {
                float y = (int)QuadraticInterp(x, controlPoints);
                spriteBatch.Draw(whiteCircle, new Vector2(x, y), Color.White);
            }*/    
            
            spriteBatch.End();
            particleSystem.Draw(spriteBatch, cameraPosition);
            GraphicsDevice.SetRenderTargets(binding);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            // Apply one of the shaders
            //uniformBlur.CurrentTechnique.Passes[0].Apply();
            //radialBlur.CurrentTechnique.Passes[0].Apply();
            //darken.CurrentTechnique.Passes[0].Apply();

            // Draw previous render target to screen with shader applied
            spriteBatch.Draw(tempRenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
            tempRenderTarget.Dispose();

            /*Vector2 screenLocation;
            Color mapLocation;
            int xOffset = SCREEN_WIDTH / SPRITE_WIDTH;
            int yOffset = SCREEN_HEIGHT / SPRITE_HEIGHT;

            int iStart = (int)(playerMapPosition.X - xOffset);
            if (iStart < 0)
                iStart = 0;

            int iEnd = (int)(playerMapPosition.X + xOffset);
            if (iEnd >= MAP_WIDTH)
                iEnd = MAP_WIDTH - 1;

            int jStart = (int)(playerMapPosition.Y - yOffset);
            if (jStart < 0)
                jStart = 0;

            int jEnd = (int)(playerMapPosition.Y + yOffset);
            if (jEnd >= MAP_HEIGHT)
                jEnd = MAP_HEIGHT - 1;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, samplerType, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(zoomLevel));
            for (int i = iStart; i < iEnd; i++)
            {
                for (int j = jStart; j < jEnd; j++)
                {
                    screenLocation = new Vector2(i * SPRITE_WIDTH, j * SPRITE_HEIGHT);
                    mapLocation = gameMap[i + (j * MAP_HEIGHT)];

                    if (mapLocation.R == 255)
                        mountains.Draw(spriteBatch, screenLocation - drawLocation, 0.0f, 1.0f);
                    else if (mapLocation.R == 127)
                        hills.Draw(spriteBatch, screenLocation - drawLocation, 0.0f, 1.0f);
                    else if (mapLocation.G == 255)
                        forest.Draw(spriteBatch, screenLocation - drawLocation, 0.0f, 1.0f);
                    else if (mapLocation.B == 255)
                        water.Draw(spriteBatch, screenLocation - drawLocation, 0.0f, 1.0f);
                    else
                        plains.Draw(spriteBatch, screenLocation - drawLocation, 0.0f, 1.0f);
                }
            }
            player.Draw(spriteBatch, playerScreenPosition - drawLocation, (float)playerRotation, 1.0f);
            spriteBatch.End();*/

            base.Draw(gameTime); // DEFAULT
        }

        /*public void Set(GameStates next)
        {
            if (next == GameStates.NULL)
                return;

            if (stateNext != next)
            {
                stateNext = next;
                transState = TransitionStates.FADE_OUT;
                timeNext = timeMax;
            }
        }

        public void SetImmediate(GameStates nextState, TransitionStates nextTransition)
        {
            if (nextState == GameStates.NULL)
                return;

            stateNext = GameStates.NULL;
            currentState = nextState;
            transState = nextTransition;
            timeNext = 0.0f;
        }*/

        /*// Research about Spline movement. Implement a system using XNA/C# Hermite and Catmull-Rom. Compare.
        private float QuadraticInterp(int xValue, List<Vector2> controlPoints)
        {
            float percentX = (xValue - controlPoints[0].X) / (controlPoints[(controlPoints.Count - 1)].X - controlPoints[0].X);

            float sum = 0;
            for (int i = 0; i < controlPoints.Count; i++)
            {
                float tempValue;
                if (i == 0 || i == (controlPoints.Count - 1))
                    tempValue = 1;
                else
                    tempValue = 1.5f * (controlPoints.Count - 1);

                sum += (float)(Math.Pow((1.0f - percentX), (controlPoints.Count - (i + 1)))) * (float)(Math.Pow((percentX), i)) * tempValue * (float)(controlPoints[i].Y);
            }

            return sum;
        }*/
    }
}
 