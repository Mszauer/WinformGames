using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;


namespace Game {
    class Tetris : GameBase {
        float deltaTime = 0;
        float moveAccum = 0;
        Size boardSize = default(Size);
        int size = 30;
        int currentSpeed = 1;
        int fastSpeed = 15;
        int speed = 1;
        Tetromino currentShape = null;
        Brush flashColor = default(Brush);
        List<Brush> tetrominoColors = null;
        bool gameOver = false;
        Random r = null;
        List<Tetromino> shapes = null;
        int[][] board = null;
        enum GameState { update, destroy, fall };
        GameState currentState = GameState.update;
        float timeAccum = 0;
        int flashes = 0;
        int speedCounter = 0;
        int lines = 0;
        int score = 0;

        public Tetris() {
            boardSize = new Size(10, 20);
            width = 500 / size * size;
            height = 600 / size * size;
            this.clearColor = Brushes.Black;
            this.title = "Tetris";
            r = new Random();

            board = new int[boardSize.Width][];
            for (int i = 0; i < board.Length; i++) {
                board[i] = new int[boardSize.Height];
            }
        }

        public override void Initialize() {
            this.width = width / size * size;
            this.height = height / size * size;

            tetrominoColors = new List<Brush>();
            shapes = new List<Tetromino>();
            Tetromino lShape = new Tetromino();
            shapes.Add(lShape);
            lShape.CreateShape(new int[][]{
                        new int[]{1,0,0,0},
                        new int[]{1,0,0,0},
                        new int[]{1,1,0,0},
                        new int[]{0,0,0,0}});
            lShape.CreateShape(new int[][]{
                        new int[]{0,0,1,0},
                        new int[]{1,1,1,0},
                        new int[]{0,0,0,0},
                        new int[]{0,0,0,0}});
            lShape.CreateShape(new int[][]{
                        new int[]{1,1,0,0},
                        new int[]{0,1,0,0},
                        new int[]{0,1,0,0},
                        new int[]{0,0,0,0}});
            lShape.CreateShape(new int[][]{
                        new int[]{1,1,1,0},
                        new int[]{1,0,0,0},
                        new int[]{0,0,0,0},
                        new int[]{0,0,0,0}});
            lShape.position = new Point(boardSize.Width / 2 * size, 0);
            Brush lShapeColor = Brushes.Orange;
            tetrominoColors.Add(lShapeColor);

            Tetromino oShape = new Tetromino();
            shapes.Add(oShape);
            oShape.CreateShape(new int[][]{
                        new int[]{2,2,0,0},
                        new int[]{2,2,0,0},
                        new int[]{0,0,0,0},
                        new int[]{0,0,0,0}});
            oShape.position = new Point(boardSize.Width / 2 * size, 0);
            Brush oShapeColor = Brushes.Yellow;
            tetrominoColors.Add(oShapeColor);

            Tetromino jShape = new Tetromino();
            shapes.Add(jShape);
            jShape.CreateShape(new int[][]{
                        new int[]{3,3,0,0},
                        new int[]{3,0,0,0},
                        new int[]{3,0,0,0},
                        new int[]{0,0,0,0}});
            jShape.CreateShape(new int[][]{
                        new int[]{3,3,3,0},
                        new int[]{0,0,3,0},
                        new int[]{0,0,0,0}});
            jShape.CreateShape(new int[][]{
                        new int[]{0,3,0,0},
                        new int[]{0,3,0,0},
                        new int[]{3,3,0,0},
                        new int[]{0,0,0,0}});
            jShape.CreateShape(new int[][]{
                        new int[]{3,0,0,0},
                        new int[]{3,3,3,0},
                        new int[]{0,0,0,0}});
            jShape.position = new Point(boardSize.Width / 2 * size, 0);
            Brush jShapeColor = Brushes.Blue;
            tetrominoColors.Add(jShapeColor);

            Tetromino tShape = new Tetromino();
            shapes.Add(tShape);
            tShape.CreateShape(new int[][]{
                        new int[]{4,4,4,0},
                        new int[]{0,4,0,0},
                        new int[]{0,0,0,0}});
            tShape.CreateShape(new int[][]{
                        new int[]{0,4,0,0},
                        new int[]{4,4,0,0},
                        new int[]{0,4,0,0},
                        new int[]{0,0,0,0}});
            tShape.CreateShape(new int[][]{
                        new int[]{0,4,0,0},
                        new int[]{4,4,4,0},
                        new int[]{0,0,0,0},
                        new int[]{0,0,0,0}});
            tShape.CreateShape(new int[][]{
                        new int[]{4,0,0,0},
                        new int[]{4,4,0,0},
                        new int[]{4,0,0,0},
                        new int[]{0,0,0,0}});
            tShape.position = new Point(boardSize.Width / 2 * size, 0);
            Brush tShapeColor = Brushes.Lavender;
            tetrominoColors.Add(tShapeColor);

            Tetromino zShape = new Tetromino();
            shapes.Add(zShape);
            zShape.CreateShape(new int[][]{
                        new int[]{0,5,0,0},
                        new int[]{5,5,0,0},
                        new int[]{5,0,0,0},
                        new int[]{0,0,0,0}});
            zShape.CreateShape(new int[][]{
                        new int[]{5,5,0,0},
                        new int[]{0,5,5,0},
                        new int[]{0,0,0,0}});
            zShape.position = new Point(boardSize.Width / 2 * size, 0);
            Brush zShapeColor = Brushes.Red;
            tetrominoColors.Add(zShapeColor);

            Tetromino sShape = new Tetromino();
            shapes.Add(sShape);
            sShape.position = new Point(boardSize.Width / 2 * size, 0);
            sShape.CreateShape(new int[][]{
                        new int[]{0,6,6,0},
                        new int[]{6,6,0,0},
                        new int[]{0,0,0,0}});
            sShape.CreateShape(new int[][]{
                        new int[]{6,0,0},
                        new int[]{6,6,0},
                        new int[]{0,6,0}});
            Brush sShapeColor = Brushes.LimeGreen;
            tetrominoColors.Add(sShapeColor);

            Tetromino iShape = new Tetromino();
            shapes.Add(iShape);
            iShape.CreateShape(new int[][]{
                        new int[]{7,0,0,0},
                        new int[]{7,0,0,0},
                        new int[]{7,0,0,0},
                        new int[]{7,0,0,0}});
            iShape.CreateShape(new int[][]{
                        new int[]{7,7,7,7},
                        new int[]{0,0,0,0},
                        new int[]{0,0,0,0}});
            iShape.position = new Point(boardSize.Width / 2 * size, 0);
            Brush iShapeColor = Brushes.Teal;
            tetrominoColors.Add(iShapeColor);

            currentShape = shapes[r.Next(0, shapes.Count)];
        }

        public override void Update(float dTime) {
            if (gameOver) {
                return;
            }
            deltaTime = dTime;
            if (currentState == GameState.destroy) {
                DestroyRow();
                return;
            }
            else if (currentState == GameState.fall) {
                RowFall();
                return;
            }
            else if (currentState == GameState.update) {
                if (KeyPressed(Keys.Up)) {
                    currentShape.Rotate(Tetromino.Direction.Left);
                    CheckBoundry();
                    if (CheckCollision()) {
                        currentShape.Rotate(Tetromino.Direction.Right);
                    }
                }
                /*if (KeyPressed(Keys.R)) {
                //currentShape = shapes[r.Next(0, shapes.Count)];
                int cIndex = shapes.IndexOf(currentShape);
                cIndex++;
                if (cIndex >= shapes.Count) {
                    cIndex = 0;
                }
                currentShape = shapes[cIndex];
            }
             */
                TetrominoMove();
            }

        }

        public bool FullRows() {
            for (int row = 0; row < boardSize.Height; row++) {
                bool fullRow = true;
                for (int col = 0; col < boardSize.Width; col++) {
                    if (board[col][row] == 0) {
                        fullRow = false;
                    }

                }
                if (fullRow == true) {
                    return true;
                }
            }
            return false;
        }

        public void DestroyRow() {
            timeAccum += deltaTime;
            flashColor = Brushes.Green;
            if (timeAccum > 0.3) {

                for (int y = 0; y < boardSize.Height; y++) {
                    bool FullRow = true;
                    for (int x = 0; x < boardSize.Width; x++) {
                        if (board[x][y] == 0) {
                            FullRow = false;
                        }
                    }

                    if (FullRow) {
                        
                        for (int x = 0; x < board.Length; x++) {
                            board[x][y] = -1;
                        }
                    }
                }
                flashColor = Brushes.Blue;
                flashes++;
                timeAccum -= 0.3f;
                if (flashes == 3) {
                    int numDestroyedThisFrame = 0;

                    for (int y = 0; y < boardSize.Height; y++) {
                        bool fullRow = false;
                        for (int x = 0; x < boardSize.Width; x++) {
                            if (board[x][y] == -1) {
                                board[x][y] = -2;
                                fullRow = true;
                            }
                        }
                        if (fullRow) {
                            lines++;
                            speedCounter++;
                            numDestroyedThisFrame++;
                            if (speedCounter >= 10) {
                                if (speed < 22) {
                                    speed++;
                                }
                                speedCounter = 0;
                            }
                        }
                    }
                    if (numDestroyedThisFrame > 0) {
                        if (numDestroyedThisFrame == 1) {
                            score += 40 * (speed);
                        }
                        else if (numDestroyedThisFrame == 2) {
                            score += 100 * (speed);
                        }
                        else if (numDestroyedThisFrame == 3) {
                            score += 300 * (speed);
                        }
                        else if(numDestroyedThisFrame == 4){
                            score += 1200 * (speed);
                        }
                    }
                    currentState = GameState.fall;
                    timeAccum = 0;
                }
            }
        }

        public void RowFall() {
            timeAccum += deltaTime;
            if (timeAccum > .5f){
                for (int x = 0; x < board.Length; x++ ) {
                    for (int y = board[0].Length-1; y >= 0; --y) {
                        if (board[x][y] == -2) {
                            for (int m = y ; m >= 0 ; --m){
                                if (m == 0) {
                                    board[x][m] = 0;
                                }
                                else {
                                    board[x][m] = board[x][m - 1];
                                }
                            }
                        }
                    }
                }
                timeAccum -= .5f;
                if (!FullRows()) {
                    currentState = GameState.update;
                }
            }
        }

        public void TetrominoMove() {
            moveAccum += deltaTime;
            if (KeyDown(Keys.Left)) {
                currentShape.position.X -= size;
                CheckBoundry();
                if (CheckCollision()) {
                    currentShape.position.X += size;
                }
            }
            if (KeyDown(Keys.Right)) {
                currentShape.position.X += size;
                CheckBoundry();
                if (CheckCollision()) {
                    currentShape.position.X -= size;
                }
            }
            if (KeyDown(Keys.Down)) {
                currentSpeed = fastSpeed;
            }
            else {
                currentSpeed = speed;
            }
            if (moveAccum > 1.0f / (float)currentSpeed) {
                currentShape.position.Y += size;
                CheckBoundry();
                if (CheckCollision()) {
                    currentShape.position.Y -= size;
                    StampStack();
                }
                moveAccum -= 1.0f / (float)currentSpeed;

            }
        }


        public bool CheckCollision() {
            foreach (Rect r in currentShape.ReturnRects()) {
                if (board[(Int32)r.X / size][(Int32)r.Y / size] > 0) {
                    return true;
                }
            }
            return false;
        }

        public void StampStack() {
            foreach (Rect r in currentShape.ReturnRects()) {
                Console.WriteLine("X: " + r.X + ", stamped at: " + ((Int32)r.X / size));
                Console.WriteLine("Y: " + r.Y + ", stamped at: " + ((Int32)r.Y / size) + "\n");
                board[(Int32)r.X / size][(Int32)r.Y / size] = shapes.IndexOf(currentShape)+1;
            }
            currentShape = shapes[this.r.Next(0, shapes.Count)];
            currentShape.position = new Point(boardSize.Width / 2 * size, 0);
            if (FullRows()) {
                timeAccum = 0;
                flashes = 0;
                currentState = GameState.destroy;
            }
        }


        public void CheckBoundry() {
            if (currentShape.position.X < 0) {
                currentShape.position.X = 0;
            }
            if (currentShape.position.X + currentShape.AABB.W > boardSize.Width* size) {
                currentShape.position.X = boardSize.Width*size - (Int32)currentShape.AABB.W;
            }
            if (currentShape.position.Y + currentShape.AABB.H > boardSize.Height * size) {
                currentShape.position.Y = boardSize.Height*size - (Int32)currentShape.AABB.H;
                StampStack();
            }
        }

        public override void Render(Graphics g) {
            DebugRender(g);
            currentShape.Draw(g,tetrominoColors[shapes.IndexOf(currentShape)]);
            for (int i = 0; i < board.Length; i++) { // i = row; row = y
                for (int j = 0; j < board[i].Length; j++) { // j = col; col = x
                    if (board[i][j] > 0) {
                        Rect block = new Rect(i * size, j * size, size, size);
                        g.FillRectangle(tetrominoColors[board[i][j]-1], block.Rectangle);
                    }
                    if (board[i][j] == -1) {
                        Rect block = new Rect(i * size, j * size, size, size);
                        g.FillRectangle(flashColor, block.Rectangle);
                    }
                }
            }
            g.DrawString("Level " + System.Convert.ToString(speed), new Font("Purisa", 20), Brushes.White, new Point(boardSize.Width * size + 35,50));
            g.DrawString("Lines " + System.Convert.ToString(lines), new Font("Purisa", 20), Brushes.White, new Point(boardSize.Width * size + 35, 100));
            g.DrawString("Score " + System.Convert.ToString(score), new Font("Purisa", 20), Brushes.White, new Point(boardSize.Width * size + 35, 150));

        }

        void DebugRender(Graphics g) {
            int tileCount = 0;
            for (int row = 0; row < boardSize.Height; row += 1) {
                tileCount = (row ) % 2 == 0 ? 1 : 0;//row/size=1,2,3... : %2 returns 1 or 0
                for (int col = 0; col < boardSize.Width; col += 1) {
                    Rect tile = new Rect(col*size, row*size, size, size);
                    g.FillRectangle(tileCount % 2 == 0 ? Brushes.Gray : Brushes.DarkGray, tile.Rectangle);
                    tileCount++;
                }
            }

        }
    }
}
