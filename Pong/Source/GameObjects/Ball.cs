using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MonoGame.Extended;
using MonoGame.Extended.Tweening;
using nkast.Aether.Physics2D.Dynamics;
using Plasma.Source.Engine;
using Plasma.Source.Engine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Source.GameObjects
{
    internal class Ball : GameObject2D
    {
        Vector2 speed;
        Texture2D pixel;

        public Ball(Vector2 position) : base(position, new Vector2(16, 16))
        {
            this.position = WindowManager.Screen2WorldPoint(position * Globals.gameScale);
            speed = new Vector2(900, 900);

            pixel = new Texture2D(Globals.graphicsDevice, 1, 1);

            Color[] colorData = { Color.White };
            pixel.SetData<Color>(colorData);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            body.LinearVelocity = speed * deltaTime;
        }

        public override void Draw(float deltaTime)
        {
            Globals.spriteBatch.Draw(pixel, GetPivotPosition(), null, Color.White, 0f, Vector2.Zero, size.X * scale, SpriteEffects.None, 0f);
        }

        public override void DrawDebug(float deltaTime)
        {
            CircleF circle = new(GetPosition(), (size.X / 2) * scale);

            Globals.spriteBatch.DrawCircle(circle, 20, Color.White, scale);
        }

        public override Body CreateBody()
        {
            return PhysicsBodyManager.CreateRect(this, size * scale, position);
        }

        public Vector2 GetPivotPosition()
        {
            return new Vector2(GetPosition().X - (pivot.X * scale), GetPosition().Y - (pivot.Y * scale));
        }
    }
}
