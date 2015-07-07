using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class LogicBoard {
        enum BoardState { Idle, Place }
        BoardState CurrentBoardState = BoardState.Idle;
        List<Brush> cellColors = new List<Brush>() { Brushes.Black, Brushes.Red, Brushes.Blue };
        int[][] logicBoard = null;
        int tileSize = 0;
        int xIndexer = 0;
        int yIndexer = 0;
        float xOffset = 0f;
        float yOffset = 0f;

        public LogicBoard(int tileSize, float xOffset, float yOffset) {
            this.tileSize = tileSize;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }
        public void Initialize(int boardSize) {
            //initialize logic board,default values of 0
            logicBoard = new int[boardSize][];
            for (int i = 0; i < logicBoard.Length; i++) {
                logicBoard[i] = new int[boardSize];
            }

        }
        public void Update(float deltaTime, bool LeftMousePressed, Point MousePosition, bool reset) {
            if (CurrentBoardState == BoardState.Idle) {
                if (LeftMousePressed) {
                    //Get the x/y indexers
                    xIndexer = MousePosition.X / tileSize;
                    xIndexer = xIndexer < logicBoard.Length ? (MousePosition.X / tileSize) : -1;
                    Console.WriteLine("x Indexer: " + xIndexer);
                    yIndexer = YPosition(xIndexer);
                    Console.WriteLine("yIndexer: " + yIndexer);
                    //Set cell values of clicked to 1
                    if (yIndexer >= 0 && logicBoard[xIndexer][yIndexer] >= 0) {
                        logicBoard[xIndexer][yIndexer] = 1;
                        if (CheckStreak(new Point(xIndexer,yIndexer))) {
                            Console.WriteLine("4 in a row");
                        }
                        //if 4 win
                        //AI takes turn
                        //Check if 4 in a row
                        // if 4 lose
                    }
                }
            }
            if (reset) {
                for (int x = 0; x < logicBoard.Length; x++) {
                    for (int y = 0; y < logicBoard[x].Length; y++) {
                        logicBoard[x][y] = 0;
                    }
                }
            }
        }

        int YPosition(int x) { 
            int yCount = 0;
            // loop through Y unil it reaches a fille cell
            for (int y = 0; y < logicBoard[x].Length; y++) {
                // if filled break
                if (logicBoard[x][y] == 1) {
                    break;
                }
                //if not filled counter++
                yCount++;
            }
            //return index
            return yCount - 1;
        }

        bool CheckStreak(Point p) {
            //side to side
            int left = StreakCounter(p, new Point(-1, 0)); 
            int right = StreakCounter(p, new Point(1, 0));
            int horizontal = left + right + 1;
            //Up and Down
            int vertical = StreakCounter(p, new Point(0, 1)) + StreakCounter(p, new Point(0, -1)) + 1; //Did math inline
            //Upwards Diag
            int diag1 = StreakCounter(p, new Point(1, 1)) + StreakCounter(p, new Point(-1, -1)) + 1;
            //Downwards diag
            int diag2 = StreakCounter(p, new Point(1, -1)) + StreakCounter(p, new Point(-1, 1)) + 1;

            return horizontal >= 4 || vertical >= 4 || diag1 >= 4 || diag2 >= 4; //inline return true or false
        }

        int StreakCounter(Point pos, Point posDiff) {
            //Store original value and create counter
            int value = logicBoard[pos.X][pos.Y];
            int streak = 0;
            //DOWHILE
            do {
                //Change x/y pos based on input
                pos.X += posDiff.X;
                pos.Y += posDiff.Y;
                //Checks bounds
                if (pos.X >= 0 && pos.Y >= 0 && pos.X < logicBoard.Length && pos.Y < logicBoard[pos.X].Length) {
                    //Check to see if they are a streak
                    if (logicBoard[pos.X][pos.Y] == value) {
                        //increase counter
                        streak++;
                    }
                }
            //Do until no longer a streak and within bounds
            } while (pos.X >= 0 && pos.Y >= 0 && pos.X < logicBoard.Length && pos.Y < logicBoard[pos.X].Length && logicBoard[pos.X][pos.Y] == value);

            return streak;
        }

        public void Render(Graphics g) {
            //visually draw logic board
            using (Pen p = new Pen(Brushes.Green, 1.0f)) {
                for (int col = 0; col < logicBoard.Length; col++) {
                    g.DrawLine(p, new Point(col * tileSize + (int)xOffset, (int)yOffset), new Point(col * tileSize + (int)xOffset, logicBoard.Length * tileSize + (int)yOffset));
                    for (int row = 0; row < logicBoard[col].Length; row++) {
                        g.DrawLine(p, new Point((int)xOffset, row * tileSize + (int)yOffset), new Point(row*tileSize + (int)xOffset, row * tileSize + (int)yOffset));
                    }
                }
            }
            //Fills cells with color depending on value
            for (int col = 0; col < logicBoard.Length; col++) {
                for (int row = 0; row < logicBoard[col].Length; row++) {
                    if (logicBoard[col][row] > -1) {
                        Rect r = new Rect(new Point(col * tileSize + (int)xOffset, row * tileSize + (int)yOffset), new Size(tileSize, tileSize));
                        g.FillRectangle(cellColors[logicBoard[col][row]], r.Rectangle);
                    }
                }
            }
        }
    }
}
