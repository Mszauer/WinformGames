using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Bejewled : GameBase{
        int[][] logicBoard = null;
        int tileSize = 50;
        Random r = null;

        public Bejewled() {
            width = 400;
            height = 600;
            r = new Random();
        }

        public override void Initialize() {

            //create logical board
            logicBoard = new int[8][];
            for (int i = 0; i < logicBoard.Length; i++) {
                logicBoard[i] = new int[8];
            }
            for (int col = 0; col < logicBoard.Length;col++) {
                for (int row = 0; row < logicBoard[col].Length; row++){
                    //TODO CHECK NEIGHBORS SO 3 DON'T SPAWN IN A ROW PS:CHECKNEIGHBOR() LIKE IN CONWAY GAME OF LIFE
                    logicBoard[col][row] = r.Next(0,8);
                }
            }
        }

        public override void Update(float deltaTime) {
            //TODO CHECK IF 3 IN A ROW (CHECKNEIGHBOR())
            //DESTROY THOSE 3
            //MOVE JEWELS DOWN
            //GENERATE JEWELS
            //MOVE GENERATE JEWELS DOWN
            //CLICK MOVING LOGIC
        }

        public override void Render(Graphics g) {
            //visually draw logic board
            using (Pen p = new Pen(Brushes.Green, 1.0f)) {
                for (int col = 0; col < logicBoard.Length; col ++) {
                    g.DrawLine(p, new Point(col * tileSize, 0), new Point(col * tileSize, height));
                    for (int row = 0; row < logicBoard[col].Length; row ++) {
                        g.DrawLine(p, new Point(0, row * tileSize), new Point(width, row * tileSize));
                    }
                }
            }

            //draws jewels depending on cell value
            for (int col = 0; col < logicBoard.Length; col++) {
                for (int row = 0; row < logicBoard[col].Length; row++) {
                    if (logicBoard[col][row] == 0) {
                        Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
                        g.FillRectangle(Brushes.Red, r.Rectangle);
                    }
                    else if (logicBoard[col][row] == 1) {
                        Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
                        g.FillRectangle(Brushes.Salmon, r.Rectangle);
                    }
                    else if (logicBoard[col][row] == 2) {
                        Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
                        g.FillRectangle(Brushes.Teal, r.Rectangle);
                    }
                    else if (logicBoard[col][row] == 3) {
                        Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
                        g.FillRectangle(Brushes.Black, r.Rectangle);
                    }
                    else if (logicBoard[col][row] == 4) {
                        Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
                        g.FillRectangle(Brushes.White, r.Rectangle);
                    }
                    else if (logicBoard[col][row] == 5) {
                        Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
                        g.FillRectangle(Brushes.Purple, r.Rectangle);
                    }
                    else if (logicBoard[col][row] == 6) {
                        Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
                        g.FillRectangle(Brushes.Green, r.Rectangle);
                    }
                    else if (logicBoard[col][row] == 7) {
                        Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
                        g.FillRectangle(Brushes.Blue, r.Rectangle);
                    }

                }
            }
        }

    }
}
