using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class ConnectBase : GameBase{
        LogicBoard gameBoard = null;

        public ConnectBase() {
            this.clearColor = Brushes.BlanchedAlmond;
            this.WindowIcon = new Icon("Assets/icon.ico");
            title = "Connect Four";
            width = 300;
            height = 300;

            gameBoard = new LogicBoard(50, 0, 0); // tile size, xOffset, yOffset
        }
        public override void Initialize() {
            gameBoard.Initialize(6); // board size x*x
        }
        public override void Update(float deltaTime) {
            gameBoard.Update(deltaTime, LeftMousePressed, MousePosition, KeyPressed(Keys.R), KeyPressed(Keys.D));
        }
        public override void Render(Graphics g) {
            gameBoard.Render(g);
        }
    }
}
