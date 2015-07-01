using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;


/* WE ARE ABOUT TO GENERATE A NEW ONE
q a b r 
w d b c 
a a 
*/

/* GENERATED A B, IF STATEMENT EXECUTES
q a b r 
w d b c 
a a b
*/

/* IT REPLACED B WITH A :X
q a b r 
w d b c 
a a a
*/

/* AND THE NEXT ONE IS GENERATED, BUT WE HAVE A STREAK
q a b r 
w d b c 
a a a e
*/

namespace Game {
    class Bejewled : GameBase {
        int[][] logicBoard = null;
        int tileSize = 50;
        Random r = null;
        Brush[] debugJewels = new Brush[] { Brushes.Red, Brushes.Salmon, Brushes.Teal, Brushes.Black, Brushes.White, Brushes.Purple, Brushes.Green, Brushes.Blue };
        
        int xIndex1 = -1;
        int yIndex1 = -1;
        int xIndex2 = -1;
        int yIndex2 = -1;
        
        bool oneSelected {
            get{
                if (xIndex1 != -1 && yIndex1 != 1){
                    return true;
                }
                return false;
            }
        }
        
        bool twoSelected {
            get {
               return xIndex2 != -1 && yIndex2 != -1;
            }
        }
        
        bool hasSelection {
            get{
                return oneSelected || twoSelected;
            }
        }
        
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

            //Assigns values to cells, if it's 3in a row it will reassign a new value
            for (int col = 0; col < logicBoard.Length; col++) {
                for (int row = 0; row < logicBoard[col].Length; row++) {
                    logicBoard[col][row] = r.Next(0, 8);
                    while (CheckNeighbors(col, row)) {
                        logicBoard[col][row] = r.Next(0, 8);
                    }
                }
            }
        }

        bool CheckNeighbors(int col, int row) {
            // >= to include 0 index
            if (col - 1 >= 0 && col - 2 >= 0) {
                if (logicBoard[col - 1][row] == logicBoard[col][row] && logicBoard[col - 2][row] == logicBoard[col - 1][row]) {
                    return true;
                }
            }
            if (col + 1 < logicBoard.Length && col + 2 < logicBoard.Length) {
                if (logicBoard[col + 1][row] == logicBoard[col][row] && logicBoard[col + 2][row] == logicBoard[col + 1][row]) {
                    return true;
                }
            }
            if (row - 1 >= 0 && row - 2 >= 0) {
                if (logicBoard[col][row - 1] == logicBoard[col][row] && logicBoard[col][row - 2] == logicBoard[col][row - 1]) {
                    return true;
                }
            }
            if (row + 1 < logicBoard[col].Length && row + 2 < logicBoard[col].Length) {
                if (logicBoard[col][row + 1] == logicBoard[col][row] && logicBoard[col][row + 2] == logicBoard[col][row + 1]) {
                    return true;
                }
            }
            return false;
        }

        public override void Update(float deltaTime) {
            //CLICK MOVING LOGIC

            if (LeftMousePressed && !oneSelected) {
                xIndex1 = (MousePosition.X / tileSize);
                yIndex1 = (MousePosition.Y / tileSize);
                Console.WriteLine("xIndex1 :" + xIndex1 + "yIndex1: " + yIndex1);
            }
            else if (LeftMousePressed && !twoSelected) {
                xIndex2 = (MousePosition.X / tileSize);
                yIndex2 = (MousePosition.Y / tileSize);
                
                //does not work
                if (!SelectionNeighbors()) {
                    xIndex1 = xIndex2;
                    yIndex1 = yIndex2;
                    xIndex2 = -1;
                    yIndex2 = -1;
                }

                Console.WriteLine("xIndex2 :" + xIndex2 + "yIndex2: " + yIndex2);

            }
            //TODO CHECK IF 3 IN A ROW (CHECKNEIGHBOR())
            //DESTROY THOSE 3
            //MOVE JEWELS DOWN
            //GENERATE JEWELS
            if (KeyPressed(Keys.R)) {
                for (int col = 0; col < logicBoard.Length; col++) {
                    for (int row = 0; row < logicBoard[col].Length; row++) {
                        logicBoard[col][row] = r.Next(0, 8);
                        while (CheckNeighbors(col, row)) {
                            logicBoard[col][row] = r.Next(0, 8);
                        }
                    }
                }
            }
        }

        bool SelectionNeighbors() {
            if ((xIndex2 == xIndex1 - 1 || xIndex2 == xIndex1 +1) && yIndex2 == yIndex1) {
                return true;
            }
            if ((yIndex2 == yIndex1 - 1 || yIndex2 == yIndex1 + 1) && xIndex2 == xIndex1 ) {
                return true;
            }
            return false;
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
                    Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
                    g.FillRectangle(debugJewels[logicBoard[col][row]], r.Rectangle);
                }
            }

            //Outlines selected jewels
            using (Pen p = new Pen(Brushes.Crimson, 1.0f)) {
                g.DrawRectangle(p, new Rectangle(new Point(xIndex1 * tileSize, yIndex1 * tileSize), new Size(tileSize, tileSize)));
                g.DrawRectangle(p, new Rectangle(new Point(xIndex2 * tileSize, yIndex2 * tileSize), new Size(tileSize, tileSize)));
            }
        }
    }
}
