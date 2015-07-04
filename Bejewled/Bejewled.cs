using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Bejewled {
        public delegate void StreakCallback(List<Point> removedCells);
        public StreakCallback OnStreak = null;
        public delegate void SwapCallback(Point a, Point b, LerpAnimation.FinishedAnimationCallback finished, int aVal, int bVal);
        public SwapCallback OnSwap = null;
        public delegate void DestroyCallBack(List<Point> streakPos);
        public DestroyCallBack OnDestroy = null;
        enum State { Idle, WaitSwap1, WaitSwap2}
        State gameState = State.Idle;
        int[][] undoBoard = null;

        public void RecordUndo() {
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

        public void PerformUndo() {
            for (int x = 0; x < logicBoard.Length; ++x) {
                for (int y = 0; y < logicBoard[x].Length; ++y) {
                    logicBoard[x][y] = undoBoard[x][y];
                }
            }
        }

        public int[][] logicBoard = null;
        int tileSize = 50;
        Random r = null;
        int xOffset = 0;
        int yOffset = 0;
#if DEBUG
        Brush[] debugJewels = new Brush[] { Brushes.Red, Brushes.Salmon, Brushes.Teal, Brushes.Black, Brushes.White, Brushes.Purple, Brushes.Green, Brushes.Blue };
#endif

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

        public Bejewled(int seed,int tileSize=50,int xOffset = 0,int yOffset = 0) {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.tileSize = tileSize;
            r = new Random(seed);
        }

        public void Initialize(int boardSize = 8) {

            //create logical board
            logicBoard = new int[boardSize][];
            for (int i = 0; i < logicBoard.Length; i++) {
                logicBoard[i] = new int[boardSize];
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
            RecordUndo();
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
            return new List<Point>();
             
        }

        public void Update(float deltaTime, bool LeftMousePressed,Point MousePosition) {
            if (gameState == State.Idle) {
                MousePosition = new Point(MousePosition.X, MousePosition.Y);
                MousePosition.X = MousePosition.X - xOffset;
                MousePosition.Y = MousePosition.Y - yOffset;
                for (int x = 0; x < logicBoard.Length; x++) {
                    for (int y = 0; y < logicBoard[x].Length; y++) {
                        DestroyStreak(CheckStreak(x, y));
                        for (int n = 0; n < logicBoard[0].Length; n++) {
                            Movedown();
                        }
                        GenerateJewels();
                    }
                }

                // if mouse is out of bounds
                if (MousePosition.X < 0 || MousePosition.Y < 0) {
                    return;
                }
                if (MousePosition.X > logicBoard.Length * tileSize || MousePosition.Y > logicBoard[0].Length * tileSize) {
                    return;
                }

                //CLICK MOVING LOGIC
                if (LeftMousePressed && !oneSelected) {
                    xIndex1 = ((MousePosition.X) / tileSize);
                    xIndex1 = xIndex1 < logicBoard.Length ? MousePosition.X / tileSize : -1;
                    yIndex1 = (MousePosition.Y / tileSize);
                    yIndex1 = yIndex1 < logicBoard[0].Length ? MousePosition.Y / tileSize : -1;

                }
                else if (LeftMousePressed && !twoSelected) {
                    xIndex2 = ((MousePosition.X) / tileSize);
                    xIndex2 = xIndex1 < logicBoard.Length ? MousePosition.X / tileSize : -1;
                    yIndex2 = (MousePosition.Y / tileSize);
                    yIndex2 = yIndex2 < logicBoard[0].Length ? MousePosition.Y / tileSize : -1;

                    //checks to see if its a valid move
                    if (SelectionNeighbors()) {
                        //Swap logic
                        // swap
                        RecordUndo();
                        //visual swap
                        OnSwap(new Point(xIndex1, yIndex1), new Point(xIndex2, yIndex2), AnimationFinished, logicBoard[xIndex1][yIndex1], logicBoard[xIndex2][yIndex2]);
                        //logical swap
                        int _value = logicBoard[xIndex1][yIndex1];
                        logicBoard[xIndex1][yIndex1] = logicBoard[xIndex2][yIndex2];
                        logicBoard[xIndex2][yIndex2] = _value;

                        //State switches to waitSwap1
                        gameState = State.WaitSwap1;
                        //streak?
                       
                    }
                    else {
                        xIndex1 = xIndex2;
                        yIndex1 = yIndex2;
                        xIndex2 = -1;
                        yIndex2 = -1;
                    }

                }
            } //end idle
        }

        void AnimationFinished(Point cell, int value, LerpAnimation anim) {
            if (gameState == State.WaitSwap2) {
                gameState = State.Idle;
            }
            else if (gameState == State.WaitSwap1) {
                if (CheckStreak(xIndex2, yIndex2).Count > 0 || CheckStreak(xIndex1, yIndex1).Count > 0) {
                    //Call animation to remove removed cells
                    OnStreak(CheckStreak(xIndex1, yIndex1));
                    OnStreak(CheckStreak(xIndex2, yIndex2));

                    //Play Destroyed Animation
                    if (OnDestroy != null) {
                        OnDestroy(CheckStreak(xIndex1, yIndex1));
                        OnDestroy(CheckStreak(xIndex2, yIndex2));

                    }

                    //Destroy Row
                    DestroyStreak(CheckStreak(xIndex1, yIndex1));
                    DestroyStreak(CheckStreak(xIndex2, yIndex2));

                    //Move jewels down
                    for (int n = 0; n < logicBoard[0].Length; n++) {
                        Movedown();

                    }
                    //Generate new Jewels
                    GenerateJewels();

                    //Deselect
                    xIndex1 = xIndex2 = -1;
                    yIndex1 = yIndex2 = -1;

                    //Put into idle state
                    gameState = State.Idle;
                }
                //not streak
                else {
                    //Visual Swap
                    if (OnSwap != null) {
                        OnSwap(new Point(xIndex1, yIndex1), new Point(xIndex2, yIndex2), AnimationFinished, logicBoard[xIndex1][yIndex1], logicBoard[xIndex2][yIndex2]);
                    }
                    //  swap back to original
                    int _value2 = logicBoard[xIndex1][yIndex1];
                    logicBoard[xIndex1][yIndex1] = logicBoard[xIndex2][yIndex2];
                    logicBoard[xIndex2][yIndex2] = _value2;
                    
                    gameState = State.WaitSwap2;
                    xIndex1 = xIndex2 = -1;
                    yIndex1 = yIndex2 = -1;
                }
            }
        }
        public void Reset(){
            
            RecordUndo();
            for (int col = 0; col < logicBoard.Length; col++) {
                for (int row = 0; row < logicBoard[col].Length; row++) {
                    logicBoard[col][row] = r.Next(0, 8);
                    while (CheckStreak(col, row).Count > 0) {
                        logicBoard[col][row] = r.Next(0, 8);
                    }
                }
            }
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

        public void Render(Graphics g) {
#if DEBUG
            //visually draw logic board
            using (Pen p = new Pen(Brushes.Green, 1.0f)) {
                for (int col = 0; col < logicBoard.Length ; col++) {
                    g.DrawLine(p, new Point(xOffset + col * tileSize, yOffset), new Point(xOffset + col * tileSize, logicBoard[col].Length * tileSize));
                    for (int row = 0; row < logicBoard[col].Length ; row++) {
                        g.DrawLine(p, new Point(xOffset, yOffset + row * tileSize), new Point(logicBoard.Length * tileSize, yOffset + row * tileSize));
                    }
                }
            }

            //draws jewels depending on cell value

            for (int col = 0; col < logicBoard.Length; col++) {
                for (int row = 0; row < logicBoard[col].Length; row++) {
                    // checks values and assigns corresponding brush
                    if (logicBoard[col][row] > -1) {
                        Rect r = new Rect(new Point(xOffset + col * tileSize, yOffset + row * tileSize), new Size(tileSize, tileSize));
                        g.FillRectangle(debugJewels[logicBoard[col][row]], r.Rectangle);

                    }
                }
            }

            //Outlines selected jewels
            using (Pen p = new Pen(Brushes.Crimson, 1.0f)) {
                g.DrawRectangle(p, new Rectangle(new Point(xIndex1 * tileSize +xOffset, yIndex1 * tileSize + yOffset), new Size(tileSize, tileSize)));
                g.DrawRectangle(p, new Rectangle(new Point(xIndex2 * tileSize + xOffset, yIndex2 * tileSize +yOffset), new Size(tileSize, tileSize)));
            }
        }
#endif
    }
}
