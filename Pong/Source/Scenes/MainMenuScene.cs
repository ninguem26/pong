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
using Plasma.Source.Game.GameObjects;
using Plasma.Source.Engine.Managers;
using System.Diagnostics;
using System.Numerics;
#endregion

namespace Pong.Source.Scenes
{
    public class MainMenuScene : Scene
    {
        public override void Initialize()
        {
            Globals.uiSystem.Add("OptionsBox", UI.MainMenu.Load());
        }

        public override void Hide()
        {
            Globals.uiSystem.Remove("OptionsBox");
        }
    }
}
