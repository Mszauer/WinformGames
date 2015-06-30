using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Bejewled : GameBase {
        int[][] logicBoard = null;
        int tileSize = 50;
        Random r = null;
        Brush[] debugJewels = new Brush[] { Brushes.Red, Brushes.Salmon, Brushes.Teal, Brushes.Black, Brushes.White, Brushes.Purple, Brushes.Green, Brushes.Blue };

#if VISUALIZE
        int gen_row = 0;
        int gen_col = -1;
#endif

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

            for (int col = 0; col < logicBoard.Length; col++) {
                for (int row = 0; row < logicBoard[col].Length; row++) {
#if !VISUALIZE
                    logicBoard[col][row] = r.Next(0, 8);
                    //Checks to make sure there aren't 3 in a row
                    if (CheckNeighbors(col, row)) {
                        logicBoard[col][row] = r.Next(0, 8);
                    }
#else
                    logicBoard[col][row] = -1;
#endif
                }
            }
        }

        bool CheckNeighbors(int col, int row) {
            if (col - 1 > 0 && col - 2 > 0) {
                if (logicBoard[col - 1][row] == logicBoard[col][row] && logicBoard[col - 2][row] == logicBoard[col][row]) {
                    return true;
                }
            }
            else if (col + 1 < logicBoard.Length && col + 2 < logicBoard.Length) {
                if (logicBoard[col + 1][row] == logicBoard[col][row] && logicBoard[col + 2][row] == logicBoard[col][row]) {
                    return true;
                }
            }
            if (row - 1 > 0 && row - 2 > 0) {
                if (logicBoard[col][row - 1] == logicBoard[col][row] && logicBoard[col][row - 2] == logicBoard[col][row]) {
                    return true;
                }
            }
            else if (row + 1 < logicBoard[col].Length && row + 2 < logicBoard[col].Length) {
                if (logicBoard[col][row + 1] == logicBoard[col][row] && logicBoard[col][row + 2] == logicBoard[col][row]) {
                    return true;
                }
            }
            return false;
        }

        public override void Update(float deltaTime) {
            //TODO CHECK IF 3 IN A ROW (CHECKNEIGHBOR())
            //DESTROY THOSE 3
            //MOVE JEWELS DOWN
            //GENERATE JEWELS
            //MOVE GENERATE JEWELS DOWN
            //CLICK MOVING LOGIC
#if VISUALIZE
            if (KeyPressed(Keys.Space)) {

                gen_col += 1;
                if (gen_col >= logicBoard.Length) {
                    gen_row += 1;
                    gen_col = 0;
                }
                if (gen_row < logicBoard[gen_col].Length) {
                    logicBoard[gen_col][gen_row] = r.Next(0, 8);
                    //Checks to make sure there aren't 3 in a row
                    if (CheckNeighbors(gen_col, gen_row)) {
                        bool breakHere = CheckNeighbors(gen_col, gen_row);
                        Console.WriteLine("CheckNeighbors(" + gen_col + ", " + gen_row + ") == true");
                        Console.WriteLine("old logicBoard[" + gen_col + "][" + gen_row + "] == " + logicBoard[gen_col][gen_row] + " | " + debugJewels[logicBoard[gen_col][gen_row]]);
                        logicBoard[gen_col][gen_row] = r.Next(0, 8);
                        Console.WriteLine("new logicBoard[" + gen_col + "][" + gen_row + "] == " + logicBoard[gen_col][gen_row] + " | " + debugJewels[logicBoard[gen_col][gen_row]]);
                        Console.Write("\n");
                    }
                }
            }
#endif
        }

        public override void Render(Graphics g) {
            //visually draw logic board
            using (Pen p = new Pen(Brushes.Green, 1.0f)) {
                for (int col = 0; col < logicBoard.Length; col++) {
                    g.DrawLine(p, new Point(col * tileSize, 0), new Point(col * tileSize, height));
                    for (int row = 0; row < logicBoard[col].Length; row++) {
                        g.DrawLine(p, new Point(0, row * tileSize), new Point(width, row * tileSize));
                    }
                }
            }

            //draws jewels depending on cell value
            for (int col = 0; col < logicBoard.Length; col++) {
                for (int row = 0; row < logicBoard[col].Length; row++) {
                    // checks values and assigns corresponding brush
#if VISUALIZE
                    if (logicBoard[col][row] == -1) {
                        continue;
                    }
#endif
                    Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
                    g.FillRectangle(debugJewels[logicBoard[col][row]], r.Rectangle);
                }
            }
        }


    }
}
