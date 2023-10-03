#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Plasma.Source.Engine;
using System.Diagnostics;
using nkast.Aether.Physics2D.Dynamics;
using MonoGame.Extended;
using MonoGame.Extended.Tweening;
using Plasma.Source.Game.GameObjects;
using Plasma.Source.Engine.Managers;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using MLEM.Ui.Elements;
using MLEM.Extensions;
using MLEM.Ui;
#endregion

namespace Pong.Source.UI
{
    public static class MainMenu
    {
        public static Panel Load()
        {
            Panel box = new(Anchor.Center, new Vector2(480, 1), Vector2.Zero, setHeightBasedOnChildren: true);

            box.AddChild(new Paragraph(Anchor.AutoCenter, 1, "Pong!"));
            box.AddChild(new Button(Anchor.AutoCenter, new Vector2(0.5F, 20), "Play")
            {
                OnPressed = element => Play(),
                PositionOffset = new Vector2(0, 1)
            });
            box.AddChild(new Button(Anchor.AutoCenter, new Vector2(0.5F, 20), "Quit")
            {
                OnPressed = element => Globals.uiSystem.Game.Exit(),
                PositionOffset = new Vector2(0, 2)
            });

            return box;
        }

        private static void Play()
        {
            Globals.world.SetCurrentScene(new Scenes.GameScene());
        }
    }
}
