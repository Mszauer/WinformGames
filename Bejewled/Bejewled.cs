#define UNDO
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
#if UNDO
        int[][] undoBoard = null;

        void RecordUndo() {
            undoBoard = new int[logicBoard.Length][];
            for (int i = 0; i < logicBoard.Length; ++i) {
                undoBoard[i] = new int[logicBoard[i].Length];
            }

            for (int x = 0; x < logicBoard.Length; ++x) {
                for (int y = 0; y < logicBoard[x].Length; ++y) {
                    undoBoard[x][y] = logicBoard[x][y];
                }
            }
        }

        void PerformUndo() {
            for (int x = 0; x < logicBoard.Length; ++x) {
                for (int y = 0; y < logicBoard[x].Length; ++y) {
                    logicBoard[x][y] = undoBoard[x][y];
                }
            }
        }
#endif

        int[][] logicBoard = null;
        int tileSize = 50;
        Random r = null;
        Brush[] debugJewels = new Brush[] { Brushes.Red, Brushes.Salmon, Brushes.Teal, Brushes.Black, Brushes.White, Brushes.Purple, Brushes.Green, Brushes.Blue };

        int xIndex1 = -1;
        int yIndex1 = -1;
        int xIndex2 = -1;
        int yIndex2 = -1;

        bool oneSelected {
            get {
                if (xIndex1 != -1 && yIndex1 != -1) {
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
            get {
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
                    while (CheckStreak(col, row).Count > 0) {
                        logicBoard[col][row] = r.Next(0, 8);
                    }
                }
            }
        }

        List<Point> CheckStreak(int col, int row) {
            if (logicBoard[col][row] == -1) {
                return new List<Point>() ;
            }

            // >= to include 0 index
            List <Point> horizontalStreak = new List<Point>();
            //Add starting block
            horizontalStreak.Add(new Point(col,row));
            //Check blocks to the left
            int logicalX = col - 1;
            if (logicalX >= 0) {
                while (logicBoard[logicalX][row] == logicBoard[col][row]) {
                    horizontalStreak.Add(new Point(logicalX, row));
                    logicalX -= 1;
                    if (logicalX < 0) {
                        break;
                    }
                }
            }
            //check blocks to the right
            logicalX = col + 1;
            if (logicalX < logicBoard.Length) {
                while (logicBoard[logicalX][row] == logicBoard[col][row]) {
                    horizontalStreak.Add(new Point(logicalX, row));
                    logicalX += 1;
                    if (logicalX == logicBoard.Length) {
                        break;
                    }
                }
            }
            //returns the list if streak
            if (horizontalStreak.Count >= 3) {
                return horizontalStreak;
            }
            //Check vertical
            List<Point> verticalStreak = new List<Point>();
            verticalStreak.Add(new Point(col,row));
            int logicalY = row - 1;
            if (logicalY >= 0) {
                while (logicBoard[col][logicalY] == logicBoard[col][row]) {
                    verticalStreak.Add(new Point(col, logicalY));
                    logicalY -= 1;
                    if (logicalY < 0) {
                        break;
                    }
                }
            }
            logicalY = row + 1;
            if (logicalY < logicBoard[col].Length) {
                while (logicBoard[col][logicalY] == logicBoard[col][row]) {
                    verticalStreak.Add(new Point(col, logicalY));
                    logicalY += 1;
                    if (logicalY >= logicBoard[col].Length) {
                        break;
                    }
                }
            }
            if (verticalStreak.Count >= 3) {
                return verticalStreak;
            }
            /*if (col - 1 >= 0 && col + 1 < logicBoard.Length) {
                if (logicBoard[col - 1][row] == logicBoard[col][row] && logicBoard[col + 1][row] == logicBoard[col][row]) {
                    return new List<Point>() { new Point(col - 1, row), new Point(col, row), new Point(col + 1, row) };
                }
            }
            if (row - 1 >= 0 && row + 1 < logicBoard[col].Length) {
                if (logicBoard[col][row - 1] == logicBoard[col][row] && logicBoard[col][row + 1] == logicBoard[col][row]) {
                    return new List<Point>() { new Point(col, row - 1), new Point(col, row), new Point(col, row + 1) };
                }
            }
            if (col - 1 >= 0 && col - 2 >= 0) {
                if (logicBoard[col - 1][row] == logicBoard[col][row] && logicBoard[col - 2][row] == logicBoard[col - 1][row]) {
                    return new List<Point>() { new Point(col - 2, row), new Point(col - 1, row), new Point(col, row) };
                }
            }
             
            if (col + 1 < logicBoard.Length && col + 2 < logicBoard.Length) {
                if (logicBoard[col + 1][row] == logicBoard[col][row] && logicBoard[col + 2][row] == logicBoard[col + 1][row]) {
                    return new List<Point>() { new Point(col, row), new Point(col + 1, row), new Point(col + 2, row) };
                }
            }
             
            if (row - 1 >= 0 && row - 2 >= 0) {
                if (logicBoard[col][row - 1] == logicBoard[col][row] && logicBoard[col][row - 2] == logicBoard[col][row - 1]) {
                    return new List<Point>() { new Point(col, row - 2), new Point(col, row - 1), new Point(col, row) };
                }
            }
            if (row + 1 < logicBoard[col].Length && row + 2 < logicBoard[col].Length) {
                if (logicBoard[col][row + 1] == logicBoard[col][row] && logicBoard[col][row + 2] == logicBoard[col][row + 1]) {
                    return new List<Point>() { new Point(col, row), new Point(col, row + 1), new Point(col, row + 2) };
                }
            }*/
            return new List<Point>();
             
        }

        public override void Update(float deltaTime) {
            for (int x = 0; x < logicBoard.Length; x++) {
                for (int y = 0; y < logicBoard[x].Length; y++) {
                    DestroyStreak(CheckStreak(x, y));
                    for (int n = 0; n < logicBoard[0].Length; n++) {
                        Movedown();
                    }
                    //GenerateJewels();
                }
            }
                //CLICK MOVING LOGIC
                if (LeftMousePressed && !oneSelected) {
                    xIndex1 = (MousePosition.X / tileSize);
                    xIndex1 = xIndex1 < logicBoard.Length ? MousePosition.X / tileSize : -1;
                    yIndex1 = (MousePosition.Y / tileSize);
                    yIndex1 = yIndex1 < logicBoard[0].Length ? MousePosition.Y / tileSize : -1;

                }
                else if (LeftMousePressed && !twoSelected) {
                    xIndex2 = (MousePosition.X / tileSize);
                    xIndex2 = xIndex1 < logicBoard.Length ? MousePosition.X / tileSize : -1;
                    yIndex2 = (MousePosition.Y / tileSize);
                    yIndex2 = yIndex2 < logicBoard[0].Length ? MousePosition.Y / tileSize : -1;

                    //checks to see if its a valid move
                    if (SelectionNeighbors()) {
                        //Swap logic
                        // swap
#if UNDO
                        RecordUndo();
#endif
                        int _value = logicBoard[xIndex1][yIndex1];
                        logicBoard[xIndex1][yIndex1] = logicBoard[xIndex2][yIndex2];
                        logicBoard[xIndex2][yIndex2] = _value;
                        //streak?
                        if (CheckStreak(xIndex2, yIndex2).Count > 0 || CheckStreak(xIndex1, yIndex1).Count > 0) {
                            //Destroy Row
                            DestroyStreak(CheckStreak(xIndex1, yIndex1));
                            DestroyStreak(CheckStreak(xIndex2, yIndex2));
                            //Move jewels down
                            for (int n = 0; n < logicBoard[0].Length; n++) {
                                Movedown();
                                
                            }
                            //Generate new Jewels
                            //GenerateJewels();

                            //Deselect
                            xIndex1 = xIndex2 = -1;
                            yIndex1 = yIndex2 = -1;
                        }
                        //not streak
                        else {
                            //  swap back to original
                            int _value2 = logicBoard[xIndex1][yIndex1];
                            logicBoard[xIndex1][yIndex1] = logicBoard[xIndex2][yIndex2];
                            logicBoard[xIndex2][yIndex2] = _value2;

                            xIndex1 = xIndex2 = -1;
                            yIndex1 = yIndex2 = -1;
                        }
                    }
                    else {
                        xIndex1 = xIndex2;
                        yIndex1 = yIndex2;
                        xIndex2 = -1;
                        yIndex2 = -1;
                    }

                }
#if DEBUG
            if (KeyPressed(Keys.R)) {
#if UNDO
                RecordUndo();
#endif
                for (int col = 0; col < logicBoard.Length; col++) {
                    for (int row = 0; row < logicBoard[col].Length; row++) {
                        logicBoard[col][row] = r.Next(0, 8);
                        while (CheckStreak(col, row).Count > 0) {
                            logicBoard[col][row] = r.Next(0, 8);
                        }
                    }
                }
            }
#endif
#if UNDO
            if (KeyPressed(Keys.U)) {
                PerformUndo();
            }
#endif
        }

        void Movedown() {
            for (int x = logicBoard.Length - 1; x >= 0; x--) { // columns
                for (int y = logicBoard[x].Length - 1; y >= 0; y--) { // rows
                    if (logicBoard[x][y] == -1) {
                        //Stores value, swaps value upwards
                        int _value = logicBoard[x][y];
                        if (y != 0) {
                            logicBoard[x][y] = logicBoard[x][y - 1];
                            logicBoard[x][y - 1] = _value;
                        }
                    }
                }
            }
        }

        void GenerateJewels() {
            for (int x = logicBoard.Length - 1; x >= 0; x--) { // columns
                for (int y = logicBoard[x].Length - 1; y >= 0; y--) { // rows
                        if (logicBoard[x][y] == -1) {
                            logicBoard[x][y] = r.Next(0, 8);
                        
                    }
                }
            }
        }

        void DestroyStreak(List<Point> locations) {
            //Sets cell value to -1
            foreach (Point p in locations) {
                logicBoard[p.X][p.Y] = -1;
            }
        }

        bool SelectionNeighbors() {
            if ((xIndex2 == xIndex1 - 1 || xIndex2 == xIndex1 + 1) && yIndex2 == yIndex1) {
                return true;
            }
            if ((yIndex2 == yIndex1 - 1 || yIndex2 == yIndex1 + 1) && xIndex2 == xIndex1) {
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
                    if (logicBoard[col][row] > -1) {
                        Rect r = new Rect(new Point(col * tileSize, row * tileSize), new Size(tileSize, tileSize));
#if DEBUG
                        g.FillRectangle(debugJewels[logicBoard[col][row]], r.Rectangle);
#endif
                    }
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
