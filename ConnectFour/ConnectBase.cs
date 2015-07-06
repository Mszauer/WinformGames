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
        GamePiece piece = null;
        List<GamePiece> pieces = null;

        public ConnectBase() {
            title = "Connect Four";
            width = 400;
            height = 600;

            gameBoard = new LogicBoard(50,0,0);
            piece = new GamePiece(50,0,0);
            pieces = new List<GamePiece>();
        }
        public override void Initialize() {
            gameBoard.Initialize();
            gameBoard.OnPlace += piece.DoPlace;
        }
        public override void Update(float deltaTime) {
            gameBoard.Update(deltaTime, LeftMousePressed, MousePosition);
        }
        public override void Render(Graphics g) {
            gameBoard.Render(g);
        }
    }
}
