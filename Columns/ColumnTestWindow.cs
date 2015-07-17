using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class ColumnTestWindow : GameBase{

        ColumnWindow game = null;

        public ColumnTestWindow() {
            width = 800;
            height = 600;
            title = "Columns";
            game = new ColumnWindow(6, 12, 0, 0); //W,H,xOffset,yOffset
        }
        public override void Initialize() {
            game.Initialize();
        }
        public override void Update(float deltaTime) {
            game.Update(deltaTime, KeyPressed(Keys.P), KeyPressed(Keys.L), KeyPressed(Keys.Space), LeftMousePressed, KeyPressed(Keys.Up), KeyPressed(Keys.W), KeyDown(Keys.Left), KeyDown(Keys.A), KeyDown(Keys.Right), KeyDown(Keys.D), KeyDown(Keys.Down), KeyDown(Keys.S));
        }
        public override void Render(Graphics g) {
            game.Render(g);
        }
    }
}
