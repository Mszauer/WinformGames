using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using Game;
using System.Drawing.Drawing2D;

namespace Game {
    class Snake : GameBase{
        Rect food = null;
        Random r = null;
        List<Rect> snake = null;
        int speed = 1;
        float deltaTime = 0;
        int size = 30;
        float moveAccum = 0;
        enum Direction {Up,Down,Left,Right};
        Direction currentDirection = Direction.Right;

        public Snake() {
            this.title = "Snake";
            this.clearColor = Brushes.Black;
            r = new Random();
            snake = new List<Rect>();
        }

        public override void Initialize() {
            Rect head = new Rect(new Point(width/2/size * size, height/2/size * size), new Size(size, size));
            snake.Add(head);
            GenerateFood();
        }

        public override void Render(Graphics g) {
            DebugRender(g);
            for (int i = 0; i < snake.Count; i++) {
                g.FillRectangle(Brushes.White,snake[i].Rectangle);
            }
            if (food != null) {
                g.FillRectangle(Brushes.Green, food.Rectangle);
            }
        }

        void DebugRender(Graphics g) {
            int tileCount = 0;
            //Pen dashedPen = new Pen(Color.Green, 3);
            //dashedPen.DashStyle = DashStyle.Dash;
            for (int row = 0; row < height; row += size) {
                tileCount = (row/size)%2 == 0 ? 1 : 0; // row / size = 1,2,3,4,5  : %2 returns 1 or 0
                for (int col = 0; col < width; col += size) {
                    Rect tile = new Rect(col, row, size, size);
                    g.FillRectangle(tileCount%2 == 0 ? Brushes.Gray : Brushes.DarkGray, tile.Rectangle);
                    tileCount++;
                }
            }
            //g.DrawLine(dashedPen, new Point(width / 2, 0), new Point(width / 2, height));
            //g.DrawLine(dashedPen, new Point(0, height/2), new Point(width, height/2));
        }

        public override void Update(float dTime) {
            deltaTime = dTime;
            if (KeyPressed(Keys.R)) {
                GenerateFood();
            }
            if (snake[0].X + snake[0].W > width || snake[0].X + snake[0].W < 0 || snake[0].Y < 0 || snake[0].Y + snake[0].H > height) {

            }
            SnakeMove();
        }
        public override void ShutDown() {

        }
        public bool CheckBoundry (){
            if (snake[0].X <= 0 || snake[0].X + snake[0].W >= width || snake[0].Y <= 0 || snake[0].Y + snake[0].H > height) {
                return false;
            }
            return true;
        }

        public void GenerateBody(Rect r) {

        }
        public Rect SnakeMove() {
            Rect r = snake[0];
            moveAccum += deltaTime;

            if (KeyDown(Keys.Up)) {
                if (currentDirection != Direction.Down) {
                    currentDirection = Direction.Up;
                }
            }
            else if (KeyDown(Keys.Down)) {
                if (currentDirection != Direction.Up) {
                    currentDirection = Direction.Down;
                }
            }
            else if (KeyDown(Keys.Left)) {
                if (currentDirection != Direction.Right) {
                    currentDirection = Direction.Left;
                }
            }
            else if (KeyDown(Keys.Right)) {
                if (currentDirection != Direction.Left) {
                    currentDirection = Direction.Right;
                }
            }

            if (moveAccum > 1.0f / (float)speed) {
                if (currentDirection == Direction.Up) {
                    r.Y -= size;
                }
                if (currentDirection == Direction.Down) {
                    r.Y += size;
                }
                if (currentDirection == Direction.Left) {
                    r.X -= size;
                }
                if (currentDirection == Direction.Right) {
                    r.X += size;
                }
                moveAccum -= 1.0f / (float)speed;
            }

            return new Rect(r) ;
        }

        public Rect GenerateFood() {
            Point foodLocation = new Point(r.Next(0, width / size)*size, r.Next(0, height / size)*size);
            food = new Rect(foodLocation, new Size(size, size));
            return food;
        }
    }
}
