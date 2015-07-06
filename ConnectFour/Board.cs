using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Board {
        enum BoardState { Idle, Place}
        BoardState CurrentBoardState = BoardState.Idle;
        int[][] logicBoard = null;
        int tileSize = 0;
        int xIndexer = 0;
        int yIndexer = 0;

        public Board(int tileSize = 50) {
            this.tileSize = tileSize;
        }
        public void Initialize() {
            //initialize logic board,default values of 0
            logicBoard = new int[8][];
            for (int i = 0; i < logicBoard.Length; i++) {
                logicBoard[i] = new int[8];
            }
            

        }
        public void Update(float deltaTime,bool LeftMousePressed, Point MousePosition) {
            if (CurrentBoardState == BoardState.Idle) {
                if (LeftMousePressed) {
                    xIndexer = MousePosition.X / tileSize;
                    yIndexer = MousePosition.Y / tileSize;
                }
            }

        }
        public void Render(Graphics g,int xOffset = 0, int yOffset = 0) {
            //visually draw logic board
            using (Pen p = new Pen(Brushes.Green, 1.0f)) {
                for (int col = 0; col < logicBoard.Length; col++) {
                    g.DrawLine(p, new Point(col * tileSize + xOffset, yOffset), new Point(col * tileSize + xOffset, logicBoard.Length * tileSize + yOffset));
                    for (int row = 0; row < logicBoard[col].Length; row++) {
                        g.DrawLine(p, new Point(xOffset, row * tileSize + yOffset), new Point(400+xOffset, row * tileSize + yOffset));
                    }
                }
            }
        }
    }
}
