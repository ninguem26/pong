using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Plasma.Source;
using Plasma.Source.Engine;
using Plasma.Source.Engine.Managers;
using Plasma.Source.Game.UI;
using System;
using System.Diagnostics;


namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private RenderTarget2D renderTarget;

        // Simple camera controls
        private Vector3 _cameraPosition = new(0, 0, 0); // camera is 1.7 meters above the ground
        readonly float cameraViewWidth = 100f; // camera is 12.5 meters wide.

        private Vector2 renderTargetRes = new(640, 360);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.HardwareModeSwitch = false;
            graphics.IsFullScreen = Globals.isFullScreen;

            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 540;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Globals.windowHeight = GraphicsDevice.Viewport.Bounds.Height;
            Globals.windowWidth = GraphicsDevice.Viewport.Bounds.Width;

            base.Initialize();

            WindowManager.UpdateViewProjection(_cameraPosition, cameraViewWidth, Globals.mainSpriteBatchEffect);
            WindowManager.UpdateViewProjectionOffCenter(Vector3.Zero, Globals.currentRenderTarget.Width, Globals.renderTargetSpriteBatchEffect);

            // Calculating world scale
            Vector2 worldTopLeftCorner = WindowManager.Screen2WorldPoint(new Vector2(0, 0));
            Vector2 worldBottomRightCorner = WindowManager.Screen2WorldPoint(new Vector2(Globals.windowWidth, Globals.windowHeight));
            Vector2 worldSize = new(MathF.Round(MathF.Abs(worldBottomRightCorner.X - worldTopLeftCorner.X), 3), MathF.Round(MathF.Abs(worldBottomRightCorner.Y - worldTopLeftCorner.Y), 3));

            Globals.worldScale = new Vector2(worldSize.X / renderTargetRes.X, worldSize.Y / renderTargetRes.Y);
            Globals.worldViewport = GraphicsDevice.Viewport;

            Scene newScene = new Source.Scenes.MainMenuScene();
            Globals.world = new GameWorld(newScene);

            Globals.world.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.content = Content;
            Globals.graphicsDevice = GraphicsDevice;
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

            // We use a BasicEffect to pass our view/projection in _spriteBatch
            Globals.mainSpriteBatchEffect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true
            };
            Globals.renderTargetSpriteBatchEffect = new BasicEffect(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Globals.font = Content.Load<SpriteFont>("font");

            renderTarget = new RenderTarget2D(GraphicsDevice, (int)renderTargetRes.X, (int)renderTargetRes.Y);
            Globals.currentRenderTarget = renderTarget;
            Globals.renderTargetViewport = new Viewport(0, 0, renderTarget.Width, renderTarget.Height);
            Globals.gameWidth = renderTarget.Width;
            Globals.gameHeight = renderTarget.Height;
            Globals.gameScale = (float)Globals.windowHeight / renderTarget.Height;
            Globals.offsetRenderTarget = Vector2.Zero;

            UntexturedStyle uiStyle = new(Globals.spriteBatch)
            {
                Font = new GenericSpriteFont(Globals.font)
            };
            Globals.uiSystem = new UiSystem(this, uiStyle);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Globals.world.Update(deltaTime);
            Globals.uiSystem.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            Globals.worldViewport = GraphicsDevice.Viewport;

            // TODO: Add your drawing code here

            // Update camera View and Projection.
            WindowManager.UpdateViewProjection(_cameraPosition, cameraViewWidth, Globals.mainSpriteBatchEffect);
            WindowManager.UpdateViewProjectionOffCenter(Vector3.Zero, renderTargetRes.X, Globals.renderTargetSpriteBatchEffect);

            // Draw world
            Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullClockwise, Globals.mainSpriteBatchEffect);
            Globals.world.Draw(deltaTime);
            Globals.spriteBatch.End();

            // Draw UI
            Globals.spriteBatch.Begin();
            //Globals.spriteBatch.DrawString(Globals.font, Globals.worldScale.ToString(), Vector2.Zero, Color.White);
            Globals.spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            if (Globals.isFullScreen)
            {
                int currentWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                int currentHeight = (int)(currentWidth / Globals.renderTargetViewport.AspectRatio);

                Globals.offsetRenderTarget.X = 0;
                Globals.offsetRenderTarget.Y = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - currentHeight) / 2;
                Globals.offsetRenderTarget = Plasma.Source.Engine.Math.Floor(Globals.offsetRenderTarget);

                Globals.gameScale = (float)currentWidth / Globals.currentRenderTarget.Width;
            }

            Globals.spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            Globals.spriteBatch.Draw(renderTarget, Globals.offsetRenderTarget, null, Color.White, 0f, Vector2.Zero, Globals.gameScale, SpriteEffects.None, 0f);
            Globals.spriteBatch.DrawString(Globals.font, MathF.Ceiling(1 / deltaTime).ToString(), Vector2.Zero, Color.White);
            Globals.spriteBatch.End();

            Globals.uiSystem.Draw(gameTime, Globals.spriteBatch);

            base.Draw(gameTime);
        }
    }
}
