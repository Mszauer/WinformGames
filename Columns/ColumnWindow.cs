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
        List<Column> columns = null;
        List<Brush> gems = null;
        List<Rect> currentJewels = null;
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
        int nextShape = 0;
        float dTime = 0f;
        float moveAccum = 0f;
        float sideAccum = 0f;
#if DEBUG
        bool StateDisplayed = true;
#endif


        public ColumnWindow(int w=8,int h=10, int xOffset = 0, int yOffset = 0) {
            width = 400;
            height = 600;
            title = "Columns";
            boardW = w;
            boardH = h;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            gems = new List<Brush>() { Brushes.Red, Brushes.Blue, Brushes.Green, Brushes.Black, Brushes.Orange };
            currentJewels = new List<Rect>();
        }
        public void Reset() {
            //set up logic board size and set values to default(0);
            logicBoard = new int[boardW][];
            for (int i = 0 ; i < logicBoard.Length ; i++){
                logicBoard[i] = new int[boardH];
            }
            r = new Random();
            columns = new List<Column>();
            
        }
        public override void Initialize() {
            this.width = width / tileSize * tileSize;
            this.height = height / tileSize * tileSize;
            Reset();
            //Generate 3 random jewels
            int jewel1 = r.Next(1, 5);
            int jewel2 = r.Next(1, 5);
            int jewel3 = r.Next(1, 5);
            Column jewelStack = new Column(tileSize);
            columns.Add(jewelStack);

            jewelStack.CreateColumn(new int[][]{
                                    new int[] {0,jewel1,0},
                                    new int[] {0,jewel2,0},
                                    new int[] {0,jewel3,0}});
            jewelStack.CreateColumn(new int[][]{
                                    new int[] {0,jewel3,0},
                                    new int[] {0,jewel2,0},
                                    new int[] {0,jewel1,0}});
            jewelStack.CreateColumn(new int[][]{
                                    new int[] {0,jewel1,0},
                                    new int[] {0,jewel3,0},
                                    new int[] {0,jewel2,0}});
            jewelStack.Position = new Point((boardW-1) / 2 * tileSize + xOffset,yOffset);
            currentColumn = jewelStack;
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
#if DEBUG
                    Console.WriteLine("Moved Left");
#endif

                }
                if (KeyDown(Keys.Right)) {
                    currentColumn.Position.X += tileSize;
                    CheckBoundry();
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

        bool CheckCollision() {
            //todo check against other pieces on the field
            return false;
        }

        void CheckBoundry() {
            /*
            if (currentColumn.Position.X < xOffset) {
                currentColumn.Position.X = xOffset; // This is your first mistake. You don't know where 
                // if currentX is -7, and xOffset is 5, this will put the column at 2, not 5
                // dont add, just assign
            }

            // This if check is broken in a few places. It takes the right side of the column, and adds xOffset
            // to it, meaning an already world space variable is being moved again, further out. Yeah, get rid
            // of that. This bit, (boardW * tileSize + xOffset) is fine the way it is.
            if (currentColumn.Position.X + currentColumn.AABB.W > (boardW-1) * tileSize + xOffset) {

                // Next up this is broken. the right side of the board is represented by the equasion in the above
                // if statement. (boardW * tileSize + xOffset), should be an addition to move boardW * tileSize INTO
                // world space. Subtracting the column width is fine.
                currentColumn.Position.X = (boardW-1) * tileSize + xOffset - (int)currentColumn.AABB.W;
            }

            // Like the above function, you are taking a world space variable and re-moving it into world space.
            // Now, why is this boardH - 1 in the if statemtnt, but boardH in the body of the loop. Stay consistent
            // if you are checking against something, offset to the thing youre checking. So the question is,
            // should the if statement have (boardH -1) or BoardH??? And of course why?

            if (currentColumn.Position.Y + currentColumn.AABB.H > (boardH - 1) * tileSize + yOffset) {
                currentColumn.Position.Y = (boardH - 1) * tileSize + yOffset - (int)currentColumn.AABB.H;
                StampStack();
            }
             * */
        }

        void StampStack() {

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

            //Draw each jewel a different color (check value?)
            foreach (Rect r in currentColumn.ReturnRects()) {
                g.FillRectangle(Brushes.Red, r.Rectangle);

            }//end foreach

            /*color stamped values
            for(int col = 0 ; col < logicBoard.Length ; col++){
                for (int row = 0 ; row < logicBoard[col].Length ; row++){
                    if (logicBoard[col][row] > 0) {
                        Rect fill = new Rect(col * tileSize, row * tileSize, tileSize, tileSize);
                        g.FillRectangle(gems[logicBoard[col][row] - 1], fill.Rectangle);
                    }
                }
            }*/
        }
        
    }
}
