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
        int size = 30;
        int currentSpeed = 1;
        int fastSpeed = 15;
        int speed = 1;
        Tetromino currentShape = null;
        bool gameOver = false;
        Random r = null;
        List<Tetromino> shapes = null;
        int[][] board = null;

        public Tetris() {
            width = 800 / size * size;
            height = 600 / size * size;
            this.clearColor = Brushes.Black;
            this.title = "Tetris";
            r = new Random();

            board = new int[height / size][];
            for (int i = 0; i < board.Length; i++) {
                board[i] = new int[width / size];
            }
            board[0][0] = 1;
            board[2][2] = 1;
            board[5][7] = 1;

        }

        public override void Initialize() {
            this.width = width / size * size;
            this.height = height / size * size;

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
            lShape.position = new Point(width / 2 / size * size, 0);

            Tetromino oShape = new Tetromino();
            shapes.Add(oShape);
            oShape.CreateShape(new int[][]{
                        new int[]{1,1,0,0},
                        new int[]{1,1,0,0},
                        new int[]{0,0,0,0},
                        new int[]{0,0,0,0}});
            oShape.position = new Point(width / 2 / size * size, 0);

            Tetromino jShape = new Tetromino();
            shapes.Add(jShape);
            jShape.CreateShape(new int[][]{
                        new int[]{1,1,0,0},
                        new int[]{1,0,0,0},
                        new int[]{1,0,0,0},
                        new int[]{0,0,0,0}});
            jShape.CreateShape(new int[][]{
                        new int[]{0,0,0,0},
                        new int[]{1,1,1,0},
                        new int[]{0,0,1,0},
                        new int[]{0,0,0,0}});
            jShape.CreateShape(new int[][]{
                        new int[]{1,0,0,0},
                        new int[]{1,1,1,0},
                        new int[]{0,0,0,0},
                        new int[]{0,0,0,0}});
            jShape.CreateShape(new int[][]{
                        new int[]{0,1,0,0},
                        new int[]{0,1,0,0},
                        new int[]{1,1,0,0}});
            jShape.position = new Point(width / 2 / size * size, 0);

            Tetromino tShape = new Tetromino();
            shapes.Add(tShape);
            tShape.CreateShape(new int[][]{
                        new int[]{1,1,1,0},
                        new int[]{0,1,0,0},
                        new int[]{0,0,0,0}});
            tShape.CreateShape(new int[][]{
                        new int[]{0,1,0,0},
                        new int[]{1,1,0,0},
                        new int[]{0,1,0,0},
                        new int[]{0,0,0,0}});
            tShape.CreateShape(new int[][]{
                        new int[]{0,1,0,0},
                        new int[]{1,1,1,0},
                        new int[]{0,0,0,0},
                        new int[]{0,0,0,0}});
            tShape.CreateShape(new int[][]{
                        new int[]{1,0,0,0},
                        new int[]{1,1,0,0},
                        new int[]{1,0,0,0},
                        new int[]{0,0,0,0}});
            tShape.position = new Point(width / 2 / size * size, 0);

            Tetromino zShape = new Tetromino();
            shapes.Add(zShape);
            zShape.CreateShape(new int[][]{
                        new int[]{0,1,0,0},
                        new int[]{1,1,0,0},
                        new int[]{1,0,0,0},
                        new int[]{0,0,0,0}});
            zShape.CreateShape(new int[][]{
                        new int[]{1,1,0,0},
                        new int[]{0,1,1,0},
                        new int[]{0,0,0,0}});
            zShape.position = new Point(width / 2 / size * size, 0);

            Tetromino sShape = new Tetromino();
            shapes.Add(sShape);
            sShape.CreateShape(new int[][]{
                        new int[]{0,1,1,0},
                        new int[]{1,1,0,0},
                        new int[]{0,0,0,0}});
            sShape.CreateShape(new int[][]{
                        new int[]{0,1,0,0},
                        new int[]{0,1,1,0},
                        new int[]{0,0,1,0}});
            sShape.position = new Point(width / 2 / size * size, 0);

            Tetromino iShape = new Tetromino();
            shapes.Add(iShape);
            iShape.CreateShape(new int[][]{
                        new int[]{1,0,0,0},
                        new int[]{1,0,0,0},
                        new int[]{1,0,0,0},
                        new int[]{1,0,0,0}});
            iShape.CreateShape(new int[][]{
                        new int[]{1,1,1,1},
                        new int[]{0,0,0,0},
                        new int[]{0,0,0,0}});
            iShape.position = new Point(width / 2 / size * size, 0);

            currentShape = shapes[r.Next(0, shapes.Count)];
        }

        public override void Update(float dTime) {
            if (gameOver) {
                return;
            }
            deltaTime = dTime;
            if (KeyPressed(Keys.Up)) {
                currentShape.Rotate(Tetromino.Direction.Left);
            }
            if (KeyPressed(Keys.R)) {
                //currentShape = shapes[r.Next(0, shapes.Count)];
                int cIndex = shapes.IndexOf(currentShape);
                cIndex++;
                if (cIndex >= shapes.Count) {
                    cIndex = 0;
                }
                currentShape = shapes[cIndex];
            }
            TetrominoMove();
        }

        public void TetrominoMove() {
            moveAccum += deltaTime;
            if (KeyDown(Keys.Left)) {
                currentShape.position.X -= size;
            }
            if (KeyDown(Keys.Right)) {
                currentShape.position.X += size;
            }
            if (KeyDown(Keys.Down)) {
                currentSpeed = fastSpeed;
            }
            else {
                currentSpeed = speed;
            }
            if (moveAccum > 1.0f / (float)currentSpeed) {
                currentShape.position.Y += size;
                moveAccum -= 1.0f / (float)currentSpeed;
            }
            CheckBoundry();
        }


        public bool CheckStack() {

            return true;
        }

        public void SetStack() {

        }


        public void CheckBoundry() {
            if (currentShape.position.X < 0) {
                currentShape.position.X = 0;
            }
            if (currentShape.position.X + currentShape.AABB.W > width) {
                currentShape.position.X = width - (Int32)currentShape.AABB.W;
            }
            if (currentShape.position.Y + currentShape.AABB.H > height) {
                currentShape.position.Y = height - (Int32)currentShape.AABB.H;

                currentShape = shapes[r.Next(0, shapes.Count)];
            }
        }

        public override void Render(Graphics g) {
            DebugRender(g);
            currentShape.Draw(g);
            for (int i = 0; i < board.Length; i++) { // i = row; row = y
                for (int j = 0; j < board[i].Length; j++) { // j = col; col = x
                    if (board[i][j] == 1) {
                        Rect block = new Rect(j * size, i * size, size, size);
                        g.FillRectangle(Brushes.Red, block.Rectangle);
                    }
                }
            }
        }

        void DebugRender(Graphics g) {
            int tileCount = 0;
            for (int row = 0; row < height; row += size) {
                tileCount = (row / size) % 2 == 0 ? 1 : 0;//row/size=1,2,3... : %2 returns 1 or 0
                for (int col = 0; col < width; col += size) {
                    Rect tile = new Rect(col, row, size, size);
                    g.FillRectangle(tileCount % 2 == 0 ? Brushes.Gray : Brushes.DarkGray, tile.Rectangle);
                    tileCount++;
                }
            }

        }
    }
}
