using Plasma.Source.Engine;
using Plasma.Source.Game.GameObjects;
using Plasma.Source.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pong.Source.GameObjects;

namespace Pong.Source.Scenes
{
    internal class GameScene : Scene
    {
        public override void Initialize()
        {
            Ball ball = new(new Vector2(Globals.gameWidth / 2, Globals.gameHeight / 2));
            AddGameObject(ball);

            // Scene bounds
            int wallSize = 64;
            int wallOffset = 32;

            AddGameObject(new Wall(new Vector2(Globals.gameWidth + wallOffset, Globals.gameHeight / 2), new Vector2(wallSize, Globals.gameHeight)));
            AddGameObject(new Wall(new Vector2(-wallOffset, Globals.gameHeight / 2), new Vector2(wallSize, Globals.gameHeight)));
            AddGameObject(new Wall(new Vector2(Globals.gameWidth / 2, -wallOffset), new Vector2(Globals.gameWidth, wallSize)));
            AddGameObject(new Wall(new Vector2(Globals.gameWidth / 2, Globals.gameHeight + wallOffset), new Vector2(Globals.gameWidth, wallSize)));

            base.Initialize();
        }
    }
}
