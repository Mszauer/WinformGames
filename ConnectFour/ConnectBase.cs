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
        LogicBoard gameBoard2 = null;

        public ConnectBase() {
            title = "Connect Four";
            width = 700;
            height = 600;

            gameBoard = new LogicBoard(50, 0, 0); // tile size, xOffset, yOffset
            gameBoard2 = new LogicBoard(50, 250, 250); // tile size, xOffset, yOffset
        }
        public override void Initialize() {
            gameBoard.Initialize(4); // board size x*x
            gameBoard2.Initialize(6); // board size x*x
        }
        public override void Update(float deltaTime) {
            //TODO
            //UPDATE ONLY IF MOUSECLICK IS IN THE BOUNDRIES OF ONE INSTANCE
            gameBoard.Update(deltaTime, LeftMousePressed, MousePosition, KeyPressed(Keys.R));
            gameBoard2.Update(deltaTime, LeftMousePressed, MousePosition, KeyPressed(Keys.R));
        }
        public override void Render(Graphics g) {
            gameBoard.Render(g);
            gameBoard2.Render(g);
        }
    }
}
