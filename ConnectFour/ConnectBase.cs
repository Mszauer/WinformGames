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
        Board gameBoard = null;

        public ConnectBase() {
            title = "Connect Four";
            width = 400;
            height = 600;

            gameBoard = new Board();

        }
        public override void Initialize() {
            gameBoard.Initialize();
        }
        public override void Update(float deltaTime) {
        
        }
        public override void Render(Graphics g) {
            gameBoard.Render(g);
        }
    }
}
