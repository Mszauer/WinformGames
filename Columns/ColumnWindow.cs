﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class ColumnWindow {
        Column currentColumn = null;
        List<Brush> gemColors = null;
        int[][] logicBoard = null;
        enum GameState { TitleScreen, Playing, Lost, Fall, Pause}
        GameState CurrentState = GameState.TitleScreen;
        int boardW = 0; // board width, set inside constructor
        int boardH = 0; // board Height , set inside constructor
        int tileSize = 40; //size of tiles
        int xOffset = 0;
        int yOffset = 0;
        int fallSpeed = 1;
        int fastFall = 15;
        int currentSpeed = 1;
        float timeAccum = 0f;
        Random r = null;
        int score = 0;
        float moveAccum = 0f;
        float sideAccum = 0f;


        public ColumnWindow(int w=8,int h=10, int xOffset = 20, int yOffset = 20) {
            
            boardW = w;
            boardH = h;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            gemColors = new List<Brush>() { Brushes.Red, Brushes.Blue, Brushes.Green, Brushes.Black, Brushes.Orange };
        }
        public void Reset() {
            //set up logic board size and set values to default(0);
            logicBoard = new int[boardW][];
            for (int i = 0 ; i < logicBoard.Length ; i++){
                logicBoard[i] = new int[boardH];
            }
            r = new Random();
            
        }
        public void Initialize() {
            Reset();
            //Generate 3 random jewels
            NewPiece();
        }

    
        public void Update(float deltaTime, bool pPressed, bool lPressed, bool spacePressed, bool lmPressed,bool upPressed, bool wPressed,bool leftPressed,bool aPressed,bool rightPressed, bool dPressed, bool downPressed, bool sPressed) {
#if DEBUG
            if (pPressed) {
                CurrentState = GameState.Pause;
            }
            if (lPressed) {
                CurrentState = GameState.Playing;
            }
#endif

            if (CurrentState == GameState.TitleScreen) {
                if (spacePressed || lmPressed) {
                    CurrentState = GameState.Playing;
                }
            }
            if (CurrentState == GameState.Playing) {
                if (logicBoard[(boardW - 1) / 2][2] > 0) {
                    CurrentState = GameState.Lost;
                }
                if (upPressed || wPressed) {
                    currentColumn.Switch();
                }
                MoveDown(deltaTime, leftPressed, aPressed, rightPressed, dPressed, downPressed, sPressed);
                //Checks to see if anything fell into a streak
                for (int x = boardW - 1; x >= 0; x--) {
                    for (int y = boardH-1 ; y >= 0 ; y--){
                        List<Point> streak = CheckStreak(x, y);
                        if (streak.Count >= 3 && DestroyStreak(streak)) {
                            CurrentState = GameState.Fall;
                        }
                    }
                }

            }

            if (CurrentState == GameState.Lost) {
                if (spacePressed || lmPressed) {
                    Reset();
                    NewPiece();
                    CurrentState = GameState.Playing;
                }
            }
            if (CurrentState == GameState.Fall) {
                RowFall(deltaTime);
                Console.WriteLine("dTime: "+deltaTime);
                Console.WriteLine("MoveAccum: " + moveAccum);
            }
        }

        void MoveDown(float dTime, bool leftPressed, bool aPressed, bool rightPressed, bool dPressed, bool downPressed, bool sPressed) {
            moveAccum += dTime;
            sideAccum += dTime;
            if (sideAccum > 0.1f) {
                if (leftPressed || aPressed) {
                    currentColumn.Position.X -= tileSize;
                    CheckBoundry();
                    if (CheckStackingCollision()) {		
                       currentColumn.Position.X += tileSize;		
#if DEBUG		
                       Console.WriteLine("Adjusted Position");		
#endif		
                    }
#if DEBUG
                    Console.WriteLine("Moved Left");
#endif

                }
                if (rightPressed || dPressed) {
                    currentColumn.Position.X += tileSize;
                    CheckBoundry(); 
                    if (CheckStackingCollision()) {
                        currentColumn.Position.X -= tileSize;
#if DEBUG
                        Console.WriteLine("Adjusted Position");
#endif
                    }
#if DEBUG
                    Console.WriteLine("Moved Right");
#endif
                }
                sideAccum -= 0.1f;
            }
            if (downPressed || sPressed) {
                currentSpeed = fastFall;
            }
            else {
                currentSpeed = fallSpeed;
            }

            if (moveAccum > 1.0f) {
                currentColumn.Position.Y += tileSize;
                CheckBoundry();
                if (CheckStackingCollision()) {
                    currentColumn.Position.Y -= tileSize;
                    StampStack();
                    
                }
                moveAccum -= 1.0f / (float)currentSpeed;
            }
        }

        bool CheckStackingCollision() {
            foreach (Rect r in currentColumn.ReturnRects()) {
                if (logicBoard[(int)(r.X - xOffset)/ tileSize][(int)(r.Y-yOffset) / tileSize] > 0) {
                    return true;
                }
            }
            return false;
        }

        void CheckBoundry() {
            if (currentColumn.Position.X < xOffset) {
                currentColumn.Position.X = xOffset;
            }
            if (currentColumn.Position.X + currentColumn.AABB.W > (boardW) * tileSize + xOffset) {
                currentColumn.Position.X = (boardW) * tileSize + xOffset - (int)currentColumn.AABB.W;
            }
            if (currentColumn.Position.Y + currentColumn.AABB.H > (boardH) * tileSize + yOffset) {
                currentColumn.Position.Y = (boardH) * tileSize + yOffset - (int)currentColumn.AABB.H;
                StampStack();
                
            }
        }

        void StampStack() {
            //copy values ontop logic board
            List<Rect> r = currentColumn.ReturnRects();
            for (int i = 0 ; i < r.Count ; i++) {
#if DEBUG
                Console.WriteLine("X: " + r[i].X + ", stamped at: " + ((Int32)r[i].X / tileSize + 1));
                Console.WriteLine("Y: " + r[i].Y + ", stamped at: " + ((Int32)r[i].Y / tileSize + 1) + "\n");
#endif
                logicBoard[(int)(r[i].X - xOffset)/ tileSize][(int)(r[i].Y-yOffset) / tileSize] = currentColumn.Values[i];
                CheckStreak((int)r[i].X / tileSize, (int)r[i].Y / tileSize);
            }
            //Generate new column
            NewPiece();
        }

        void RowFall(float dTime) {
            timeAccum += dTime;
            if (timeAccum > 0.5f) {
                for (int col =0; col < boardW; col++) {
                    for (int row = boardH - 1; row >= 0; row--) {
                        if (logicBoard[col][row] == -1) {
                            for (int move = row; move >= 0; move--) {
                                if (move == 0) {
                                    logicBoard[col][move] = 0;
                                }
                                else {
                                    logicBoard[col][move] = logicBoard[col][move - 1];
                                }
                            }//end move
                        }
                    }//end row
                }//end col
                timeAccum -= 0.5f;
                bool blankCell = false; // No, you don't have a streak by default
                for (int col = 0; col < boardW; col++) {
                    for (int row = boardH - 1; row >= 0; row--) {
                        if (logicBoard[col][row] == -1) {
                            blankCell = true; // If the streak count is >= 3, then you have a streak
                        }
                    }
                }
                if (!blankCell) {
                    CurrentState = GameState.Playing;
                }
            }
        }

        bool DestroyStreak(List<Point> locations) {
            bool destroyed = false;
            foreach (Point p in locations) {
                logicBoard[p.X][p.Y] = -1;
                destroyed = true;
#if DEBUG
                Console.WriteLine("Cell Removed at X: " + p.X + " Y: " + p.Y);
#endif
            }
            float multiplier = locations.Count / 1.5f;
            PointTracker(multiplier);
            return destroyed;
        }

        void PointTracker(float multiplier) {
            score += (int)(40 * (fallSpeed) * (float)multiplier);
            if (score%500 > 0){
                fallSpeed++;
            }
        }

        List<Point> CheckStreak(int col, int row) {

            //HORIZONTAL
            List<Point> horizontalStreak = new List<Point>();
            horizontalStreak.Add(new Point(col, row));
            //LEFT
            int logicalX = col -1; //temporary indexer
            if (logicalX >= 0) {
                while (logicBoard[logicalX][row] > 0 && logicBoard[logicalX][row] == logicBoard[col][row]) {
                    //while the cells are equal, keep moving left until they become unequal or boundry is reached
                    horizontalStreak.Add(new Point(logicalX, row));
                    logicalX -= 1;
                    if (logicalX < 0) {
                        break;
                    }
                }//endwhile
            }//endif
            //RIGHT
            logicalX = col + 1;
            if (logicalX < boardW) {
                //while the cells are equal, keep moving left until they become unequal or boundry is reached
                while (logicBoard[logicalX][row] > 0 && logicBoard[logicalX][row] == logicBoard[col][row]) {
                    horizontalStreak.Add(new Point(logicalX, row));
                    logicalX++;
                    if (logicalX > boardW-1) {
                        break;
                    }
                }//endwhile
            }//end if
            //return list if there is horizontal streak
            if (horizontalStreak.Count >= 3) {
#if DEBUG
                Console.WriteLine("Horizontal Streak found, Length: " + horizontalStreak.Count);
#endif
                return horizontalStreak;
            }

            //VERTICAL
            List<Point> verticalStreak = new List<Point>();
            verticalStreak.Add(new Point(col, row));
            //UPWARDS
            int logicalY = row - 1;
            if (logicalY >= 0) {
                while (logicBoard[col][logicalY] > 0 && logicBoard[col][logicalY] == logicBoard[col][row]) {
                    //same as horizontal but why Y variables
                    verticalStreak.Add(new Point(col, logicalY));
                    logicalY -= 1;
                    if (logicalY < 0) {
                        break;
                    }
                }
            }
            //DOWNWARDS
            logicalY = row + 1;
            if (logicalY < boardH) {
                while (logicBoard[col][logicalY] > 0 && logicBoard[col][logicalY] == logicBoard[col][row]) {
                    verticalStreak.Add(new Point(col, logicalY));
                    logicalY++;
                    if (logicalY > boardH-1) {
                        break;
                    }
                }
            }
            if (verticalStreak.Count >= 3) {
#if DEBUG
                Console.WriteLine("Vertical Streak found, Length: " + verticalStreak.Count);
#endif
                return verticalStreak;
            }

            //DIAGONAL
            List<Point> leftDiag = new List<Point>();
            leftDiag.Add(new Point(col, row));
            List<Point> rightDiag = new List<Point>();
            rightDiag.Add(new Point(col, row));
            //LEFT UP
            logicalX = col - 1;
            logicalY = row - 1;
            if (logicalX >= 0 && logicalY >= 0) {
                while (logicBoard[logicalX][logicalY] > 0 && logicBoard[logicalX][logicalY] == logicBoard[col][row]) {
                    leftDiag.Add(new Point(logicalX, logicalY));
                    logicalX -= 1;
                    logicalY -= 1;
                    if (logicalX < 0 || logicalY < 0) {
                        break;
                    }
                }
            }
            //RIGHT UP
            logicalX = col + 1;
            logicalY = row - 1;
            if (logicalY >= 0 && logicalX < boardW) {
                while (logicBoard[logicalX][logicalY] > 0 && logicBoard[logicalX][logicalY] == logicBoard[col][row]) {
                    rightDiag.Add(new Point(logicalX, logicalY));
                    logicalX++;
                    logicalY -= 1;
                    if (logicalY < 0 || logicalX > boardW-1) {
                        break;
                    }
                }
            }
            //LEFT DOWN
            logicalX = col - 1;
            logicalY = row + 1;
            if (logicalX >= 0 && logicalY < boardH) {
                while (logicBoard[logicalX][logicalY] > 0 && logicBoard[logicalX][logicalY] == logicBoard[col][row]) {
                    leftDiag.Add(new Point(logicalX, logicalY));
                    logicalX -= 1;
                    logicalY++;
                    if (logicalX < 0 || logicalY > boardH-1) {
                        break;
                    }
                }
            }
            //RIGHT DOWN
            logicalX = col + 1;
            logicalY = row + 1;
            if (logicalX < boardW && logicalY < boardH) {
                while (logicBoard[logicalX][logicalY] > 0 && logicBoard[logicalX][logicalY] == logicBoard[col][row]) {
                    rightDiag.Add(new Point(logicalX, logicalY));
                    logicalX++;
                    logicalY++;
                    if (logicalX > boardW-1 || logicalY > boardH-1) {
                        break;
                    }
                }
            }

            if (leftDiag.Count >= 3) {
#if DEBUG
                Console.WriteLine("Left Diagonal Streak found, Length: " + leftDiag.Count);
#endif
                return leftDiag;
            }
            if (rightDiag.Count >= 3) {
#if DEBUG
                Console.WriteLine("Right Diagonal Streak found, Length: " + rightDiag.Count);
#endif
                return rightDiag;
            }

            return new List<Point>();
        }

        void NewPiece() {
            int jewel1 = r.Next(1, 5);
            int jewel2 = r.Next(1, 5);
            int jewel3 = r.Next(1, 5);
            Column jewelStack = new Column(tileSize);

            jewelStack.CreateColumn(jewel1, jewel2, jewel3);
            jewelStack.Position = new Point((boardW - 1) / 2 * tileSize + xOffset, yOffset);
            currentColumn = jewelStack;
        }

        public void Render(Graphics g) {
            //draw logic board
            using (Pen p = new Pen(Brushes.Black, 1f)) {
                for (int col = 0; col < logicBoard.Length; col++) {
                    //Draw columns
                    g.DrawLine(p, new Point(col * tileSize+xOffset, yOffset), new Point(col * tileSize+xOffset, logicBoard[col].Length * tileSize+yOffset));
                    
                    for (int row = 0; row < logicBoard[col].Length; row++) {
                        //draw rows
                        g.DrawLine(p, new Point(xOffset, row * tileSize+yOffset), new Point(xOffset+boardW*tileSize, row*tileSize+yOffset));
                    }//end col
                }//end row
                //Draw last lines to close the square
                g.DrawLine(p, new Point(xOffset, (boardH) * tileSize + yOffset), new Point(boardW*tileSize+xOffset,(boardH) * tileSize + yOffset));
                g.DrawLine(p, new Point(xOffset + boardW * tileSize, yOffset), new Point(xOffset + boardW * tileSize, boardH * tileSize+yOffset));
            }//end using pen p

            currentColumn.Render(g, gemColors);

           // color stamped values
            for(int col = 0 ; col < logicBoard.Length ; col++){
                for (int row = 0 ; row < logicBoard[col].Length ; row++){
                    if (logicBoard[col][row] > 0) {
                        Rect fill = new Rect(col * tileSize + xOffset, row * tileSize + yOffset, tileSize, tileSize);
                        g.FillRectangle(gemColors[logicBoard[col][row]], fill.Rectangle);
                    }
                }
            }

            //Display Score to the left of playing board
            g.DrawString("Score: " + System.Convert.ToString(score), new Font("Purisia", 30), Brushes.Black, new Point(boardW * (3 / 2) * tileSize,0));
            g.DrawString("Level: " + System.Convert.ToString(fallSpeed), new Font("Purisia", 30), Brushes.Black, new Point(boardW * (3 / 2) * tileSize, 60));

            if (CurrentState == GameState.Lost) {
                g.DrawString("You have lost", new Font("Purisia", 15), Brushes.Black, new Point(boardW * (3 / 2) * tileSize, 120));
                g.DrawString("Left Click or press Space to start again", new Font("Purisia", 15), Brushes.Black, new Point(boardW * (3 / 2) * tileSize, 180));
            }
            if (CurrentState == GameState.TitleScreen) {
                g.DrawString("Left Click or press Space to start", new Font("Purisia", 15), Brushes.Black, new Point(boardW * (3 / 2) * tileSize, 180));

            }
        }
        
    }
}
