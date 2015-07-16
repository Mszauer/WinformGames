using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class ColumnWindow : GameBase{
        Column currentColumn = null;
        List<Brush> gemColors = null;
        int[][] logicBoard = null;
        enum GameState { TitleScreen, Playing, Lost, Destroy, Fall}
        GameState CurrentState = GameState.TitleScreen;
        int boardW = 0; // board width, set inside constructor
        int boardH = 0; // board Height , set inside constructor
        int tileSize = 40; //size of tiles
        int xOffset = 0;
        int yOffset = 0;
        int fallSpeed = 1;
        int fastFall = 15;
        int currentSpeed = 1;
        Random r = null;
        bool isGameOver = false;
        float dTime = 0f;
        float moveAccum = 0f;
        float sideAccum = 0f;
#if DEBUG
        bool StateDisplayed = true;
#endif


        public ColumnWindow(int w=8,int h=10, int xOffset = 20, int yOffset = 20) {
            width = 400;
            height = 600;
            title = "Columns";
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
        public override void Initialize() {
            this.width = width / tileSize * tileSize;
            this.height = height / tileSize * tileSize;
            Reset();
            //Generate 3 random jewels
            NewPiece();
        }
        public void DebugStateStatus(){
            //Doesnt work, wont display anything  past title screen
            //Update calls it to much to set true need help
#if DEBUG
            if (StateDisplayed) {
                Console.WriteLine(CurrentState);
                StateDisplayed = false;
            }
#endif
        }
    
        public override void Update(float deltaTime) {
#if DEBUG
            if (KeyPressed(Keys.P)) {
                CurrentState = GameState.Lost;
            }
            if (KeyPressed(Keys.L)) {
                CurrentState = GameState.Playing;
            }
#endif

            if (CurrentState == GameState.TitleScreen) {
                DebugStateStatus();
                if (KeyPressed(Keys.Space) || LeftMousePressed) {
                    CurrentState = GameState.Playing;
                }
            }
            if (CurrentState == GameState.Playing) {
                DebugStateStatus();
                if (KeyPressed(Keys.Up)) {
                    currentColumn.Switch();
                }
                MoveDown(deltaTime);
            }
            if (CurrentState == GameState.Destroy) {
                DebugStateStatus();
                //DestroyColumn();
                CurrentState = GameState.Fall;
            }
            if (CurrentState == GameState.Fall) {
                DebugStateStatus();
                //ColumnFall();
                CurrentState = GameState.Playing;
            }
            if (CurrentState == GameState.Lost) {
                DebugStateStatus();
                isGameOver = true;
                if (KeyPressed(Keys.Space) || LeftMousePressed) {
                    Reset();
                    NewPiece();
                    isGameOver = false;
                    CurrentState = GameState.Playing;
                }
            }
        }

        void MoveDown(float dTime) {
            moveAccum += dTime;
            sideAccum += dTime;
            if (sideAccum > 0.1f) {
                if (KeyDown(Keys.Left)){
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
                if (KeyDown(Keys.Right)) {
                    currentColumn.Position.X += tileSize;
                    CheckBoundry(); 
                    if (CheckStackingCollision()) {
                        currentColumn.Position.X += tileSize;
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
            if (KeyDown(Keys.Down)) {
                currentSpeed = fastFall;
            }
            else {
                currentSpeed = fallSpeed;
            }

            if (moveAccum > 1.0f) {
                currentColumn.Position.Y += tileSize;
                CheckBoundry();
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
            }
            //Generate new column
            NewPiece();
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

        public override void Render(Graphics g) {
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
        }
        
    }
}
