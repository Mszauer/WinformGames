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
        enum BoardState { Player_Turn, Player_Anim, Ai_Turn, Ai_Anim, Lost, Draw, Won }
        BoardState CurrentBoardState = BoardState.Player_Turn;
        //elipse instead of rect
        Rect lerp = null;
        Rect lerp2 = null;
        List<Brush> cellColors = new List<Brush>() { Brushes.BlanchedAlmond, Brushes.Red, Brushes.Yellow };
        Sprite visualBoard = null;
        int[][] logicBoard = null;
        int tileSize = 0;
        int xIndexer = 0;
        int yIndexer = 0;
        float xOffset = 0f;
        float yOffset = 0f;
        int boardSize = 0;
        float timeAccum = 0;

        public LogicBoard(int tileSize, float xOffset, float yOffset) {
            lerp = new Rect(new Point(tileSize * -1, tileSize * -1), new Size(tileSize, tileSize));
            lerp2 = new Rect(new Point(tileSize * -1, tileSize * -1), new Size(tileSize, tileSize));
            this.tileSize = tileSize;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }
        public void Initialize(int size) {
            this.boardSize = size; // size of the grid x*x
            //initialize logic board,default values of 0
            logicBoard = new int[boardSize][];
            for (int i = 0; i < logicBoard.Length; i++) {
                logicBoard[i] = new int[boardSize];
            }
            //set up sprite
            visualBoard = new Sprite("Assets/board.png");

        }
        public void Update(float deltaTime, bool LeftMousePressed, Point MousePosition, bool reset, bool forceAI) {
            MousePosition = new Point(MousePosition.X, MousePosition.Y);

            if (CurrentBoardState == BoardState.Player_Turn) {
                if (LeftMousePressed) {
                    if (!WithinGameBoundry(MousePosition)) {
                        return;
                    }
                    CurrentBoardState = BoardState.Player_Anim;
                    //transform mouse position into valid index
                    MousePosition.X -= (int)xOffset;
                    MousePosition.Y -= (int)yOffset;
                    //Get the x/y indexers
                    xIndexer = MousePosition.X / tileSize;
                    xIndexer = xIndexer < logicBoard.Length ? (MousePosition.X / tileSize) : -1;
                    Console.WriteLine("x Indexer: " + xIndexer);
                    yIndexer = YPosition(xIndexer);
                    Console.WriteLine("yIndexer: " + yIndexer);
                    //Checks to see if it's within boundries
                    if (xIndexer >= 0 && xIndexer <= logicBoard.Length) {
                        lerp.X = xIndexer * tileSize + xOffset;
                        lerp.Y = yIndexer * tileSize + yOffset;
                        //Set cell values of clicked to 1
                        if (yIndexer >= 0 && logicBoard[xIndexer][yIndexer] >= 0) {
                            logicBoard[xIndexer][yIndexer] = -1;

                        }
                    }
                    timeAccum = 1.0f;
                }
            }
            else if (CurrentBoardState == BoardState.Player_Anim) {
                timeAccum -= deltaTime;

                if (timeAccum > 0) {
                    Point lerpanim = lerpAnimation(new Point(xIndexer * tileSize, 0), new Point(xIndexer * tileSize, yIndexer * tileSize), 1.0f - timeAccum);
                    lerp.X = lerpanim.X;
                    lerp.Y = lerpanim.Y;
                }
                else {
                    lerp.X = tileSize * -1;
                    lerp.Y = tileSize * -1;
                    CurrentBoardState = BoardState.Ai_Turn;
                    logicBoard[xIndexer][yIndexer] = 1;
                    //Checks to see if 4 in a row
                    if (CheckStreak(new Point(xIndexer, yIndexer))) {
                        //player win
                        CurrentBoardState = BoardState.Won;
                    }
                    if (IsDraw()) {
                        CurrentBoardState = BoardState.Draw;
                    }

                }
            }
            else if (CurrentBoardState == BoardState.Ai_Turn) {
                AITurn();
                //Check if 4 in a row
                timeAccum = 0.0f;
                CurrentBoardState = BoardState.Ai_Anim;
                timeAccum = 1.0f;
            }
            else if (CurrentBoardState == BoardState.Ai_Anim) {
                timeAccum -= deltaTime;

                if (timeAccum > 0) {
                    Point lerpanim = lerpAnimation(new Point(xIndexer * tileSize, 0), new Point(xIndexer * tileSize, yIndexer * tileSize), 1.0f - timeAccum);
                    lerp2.X = lerpanim.X;
                    lerp2.Y = lerpanim.Y;
                }
                else {
                    lerp2.X = tileSize * -1;
                    lerp2.Y = tileSize * -1;
                    CurrentBoardState = BoardState.Player_Turn;
                    logicBoard[xIndexer][yIndexer] = 2;
                    //Checks to see if 4 in a row
                    if (CheckStreak(new Point(xIndexer, yIndexer))) {
                        //player win
                        CurrentBoardState = BoardState.Won;
                        return;
                    }
                    if (IsDraw()) {
                        CurrentBoardState = BoardState.Draw;
                        return;
                    }
                }
            }
            else if (CurrentBoardState == BoardState.Draw) {
                Console.WriteLine("Draw");
                if (LeftMousePressed || reset) {
                    Reset();
                    CurrentBoardState = BoardState.Player_Turn;
                }
            }
            else if (CurrentBoardState == BoardState.Lost) {
                Console.WriteLine("Lost");
                if (LeftMousePressed || reset) {
                    Reset();
                    CurrentBoardState = BoardState.Player_Turn;
                }
            }
            else if (CurrentBoardState == BoardState.Won) {
                Console.WriteLine("Won");
                if (LeftMousePressed || reset) {
                    Reset();
                    CurrentBoardState = BoardState.Player_Turn;
                }
            }
#if DEBUG
            if (reset) {
                Reset();
            }

            if (forceAI) {
                AITurn();
                if (CheckStreak(new Point(xIndexer, yIndexer))) {
                    //ai win
                    Console.WriteLine("4 in a row");
                }
            }
#endif
        }

        void Reset() {
            for (int x = 0; x < logicBoard.Length; x++) {
                for (int y = 0; y < logicBoard[x].Length; y++) {
                    logicBoard[x][y] = 0;
                }
            }
        }

        bool IsDraw() {
            int fullCells = 0;
            for (int x = 0; x < logicBoard.Length; x++) {
                for (int y = 0; y < logicBoard[x].Length; y++) {
                    if (logicBoard[x][y] > 0) {
                        fullCells++;
                    }
                }
            }
            if (fullCells == boardSize * boardSize) {
                return true;
            }
            return false;
        }

        Point lerpAnimation(Point startPos, Point endPos, float dTime) {
            float time = 0;
            time += dTime;
            Point currentPosition = new Point(startPos.X + (int)xOffset, startPos.Y + (int)yOffset);
            currentPosition.X = (int)Easing.BounceEaseOut(time, (float)startPos.X + xOffset, (float)endPos.X + xOffset, 1.0f);
            currentPosition.Y = (int)Easing.BounceEaseOut(time, (float)startPos.Y + yOffset, (float)endPos.Y + yOffset, 1.0f);
            return currentPosition;
        }

        void AITurn() {
            Random r = new Random();
            xIndexer = r.Next(0, boardSize);
            yIndexer = YPosition(xIndexer);
            while (yIndexer == -1 && !IsDraw()) {
                xIndexer = r.Next(0, boardSize);
                yIndexer = YPosition(xIndexer);
            }
            // Can still potentially be -1, if IsDraw was true
            if (yIndexer != -1) {
                logicBoard[xIndexer][yIndexer] = -2;
            }
        }

        bool WithinGameBoundry(Point MousePosition) {
            if (MousePosition.X >= xOffset && MousePosition.Y >= yOffset && MousePosition.X <= xOffset + boardSize * tileSize && MousePosition.Y <= yOffset + boardSize * tileSize) {
                return true;
            }
            return false;
        }

        int YPosition(int x) {
            int yCount = 0;
            //Checks to see if it's a move within bounds
            if (x >= 0 && x <= logicBoard.Length) {
                // loop through Y unil it reaches a fille cell
                for (int y = 0; y < logicBoard[x].Length; y++) {
                    // if filled break
                    if (logicBoard[x][y] > 0) {
                        break;
                    }
                    //if not filled counter++
                    yCount++;
                }
                //return index
                return yCount - 1;
            }
            return yCount;
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
                        g.DrawLine(p, new Point((int)xOffset, row * tileSize + (int)yOffset), new Point(row * tileSize + (int)xOffset, row * tileSize + (int)yOffset));
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

            //fall animation
            g.FillRectangle(Brushes.Red, lerp.Rectangle);
            g.FillRectangle(Brushes.Yellow, lerp2.Rectangle);
            //Draw the visual board
            for (int x = 0; x < logicBoard.Length; x++) {
                for (int y = 0; y < logicBoard[x].Length; y++) {
                    visualBoard.Draw(g, new Point(x * tileSize + (int)xOffset, y * tileSize + (int)yOffset), 1.0f);
                }
            }
            //Write out win/lose/draw
            if (CurrentBoardState == BoardState.Won) {
                g.DrawString("You have Won!", new Font("Purisia", 20, FontStyle.Bold), Brushes.Black, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 100, (int)yOffset + 50)); //draw in the middle of screen offsetted a bit
                g.DrawString("You have Won!", new Font("Purisia", 20, FontStyle.Bold), Brushes.DarkViolet, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 101, (int)yOffset + 49)); //dropshadow
                g.DrawString("Click to Play Again", new Font("Purisia", 20, FontStyle.Bold), Brushes.Black, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 101, (int)yOffset + 100));
                g.DrawString("Click to Play Again", new Font("Purisia", 20, FontStyle.Bold), Brushes.DarkViolet, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 101, (int)yOffset + 99));
            } if (CurrentBoardState == BoardState.Lost) {
                g.DrawString("You have Lost!", new Font("Purisia", 20, FontStyle.Bold), Brushes.Black, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 100, (int)yOffset + 50)); //draw in the middle of screen offsetted a bit
                g.DrawString("You have Lost!", new Font("Purisia", 20, FontStyle.Bold), Brushes.DarkViolet, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 101, (int)yOffset + 49)); //dropshadow
                g.DrawString("Click to Play Again", new Font("Purisia", 20, FontStyle.Bold), Brushes.Black, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 101, (int)yOffset + 100));
                g.DrawString("Click to Play Again", new Font("Purisia", 20, FontStyle.Bold), Brushes.DarkViolet, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 101, (int)yOffset + 99));
            } if (CurrentBoardState == BoardState.Draw) {
                g.DrawString("You have a Draw!", new Font("Purisia", 20, FontStyle.Bold), Brushes.Black, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 100, (int)yOffset + 50)); //draw in the middle of screen offsetted a bit
                g.DrawString("You have a Draw!", new Font("Purisia", 20, FontStyle.Bold), Brushes.DarkViolet, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 101, (int)yOffset + 49)); //dropshadow
                g.DrawString("Click to Play Again", new Font("Purisia", 20, FontStyle.Bold), Brushes.Black, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 101, (int)yOffset + 100));
                g.DrawString("Click to Play Again", new Font("Purisia", 20, FontStyle.Bold), Brushes.DarkViolet, new Point(logicBoard.Length * tileSize / 2 + (int)xOffset - 101, (int)yOffset + 99));
            }
        }
    }
}
