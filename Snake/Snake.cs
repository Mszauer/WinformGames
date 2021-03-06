﻿using System;
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
        int speed = 3;
        int foodEaten = 0;
        float deltaTime = 0;
        int size = 30;
        float moveAccum = 0;
        enum Direction {Up,Down,Left,Right};
        Direction currentDirection = Direction.Right;
        bool gameOver = false;

        public Snake() {
            width = 800 / size * size;
            height = 600 / size * size;
            this.title = "Snake";
            this.clearColor = Brushes.Black;
            r = new Random();
            snake = new List<Rect>();
        }

        public override void Initialize() {
            this.width = width / size * size;
            this.height = height / size * size;
            Rect head = new Rect(new Point(width/2/size * size, height/2/size * size), new Size(size, size));
            Rect body1 = new Rect(new Point(width / 2 / size * size-size, height / 2 / size * size), new Size(size, size));
            Rect body2 = new Rect(new Point(width / 2 / size * size-size-size, height / 2 / size * size), new Size(size, size));
            snake.Add(head);
            snake.Add(body1);
            snake.Add(body2);
            GenerateFood();
        }

        public override void Render(Graphics g) {
            //DebugRender(g);
            for (int i = 0; i < snake.Count; i++) {
                g.FillRectangle(Brushes.White,snake[i].Rectangle);
            }
            if (food != null) {
                g.FillRectangle(Brushes.Green, food.Rectangle);
            }
            if (gameOver) {
                g.DrawString("Game Over!", new Font("Purisa", 40), Brushes.White, new Point(width / 2 - 120, height / 2 - 30));
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
            if (gameOver) {
                return;
            }
            deltaTime = dTime;
            if (KeyPressed(Keys.R)) {
                GenerateFood();
            }
            if (CheckBoundry()) {
                Rect tail = SnakeMove();
                GenerateBody(tail);
            }
            else {
                gameOver = true;
            }
        }
        public override void ShutDown() {

        }

        public bool CheckBoundry (){
            if (snake[0].X + snake[0].W > width / size * size || snake[0].X < 0 || snake[0].Y < 0 || snake[0].Y + snake[0].H > height) {
                return false;
            }
            for (int i = snake.Count-1; i >= 1; i--) {
                if (snake[0].X == snake[i].X && snake[0].Y == snake[i].Y) {
                    return false;
                }
            }
            return true;
        }

        public void GenerateBody(Rect r) {
            if (snake[0].X == food.X && snake[0].Y == food.Y) {
                snake.Add(new Rect(r));
                GenerateFood();
                foodEaten++;
                if (foodEaten >= 3) {
                    speed += 1;
                    foodEaten = 0;
                }
            }
        }
        public Rect SnakeMove() {
            Rect r = new Rect(snake[0]);
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
            Rect tail = new Rect(snake[snake.Count - 1]);
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
                
                for (int i = snake.Count - 1; i >= 1; i--) {
                    snake[i] = new Rect(snake[i - 1]);
                }
                snake[0] = r;
            }     

            return new Rect(tail);
        }
            

        public Rect GenerateFood() {
            Point foodLocation = new Point(r.Next(0, width / size)*size, r.Next(0, height / size)*size);
           
            while (true) {
                bool unique = true;
                food = new Rect(foodLocation, new Size(size, size));
                for (int i = 0; i < snake.Count; i++) {
                    if (food.X == snake[i].X && food.Y == snake[i].Y) {
                        unique = false;
                    }
                }
                if (unique) {
                    break;
                }
            }

            return food;
        }
    }
}
