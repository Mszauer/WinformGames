using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace Game {
    class Pong : GameBase {
        Rect ball = null;
        Rect player1 = null;
        Rect player2 = null;
        float deltaTime = 0;
        float horizontalSpeed = 0;
        float verticalSpeed = 0;
        float paddleOffset = 0;
        int paddleSpeed = 0;
        //float debugD = 0;
        int p1Score = 0;
        int p2Score = 0;
        Pen dashedPen = null;
        enum EnemyAi {none,Up,Down };
        EnemyAi ai;
        Random r = null;

        public Pong() {
            this.title = "Pong";
            this.clearColor = Brushes.Black;
            horizontalSpeed = 150f;
            verticalSpeed = 150f;
            paddleSpeed = 300;
            r = new Random();
        }

        public override void Update(float dTime) {
            deltaTime = dTime;
            /*if(KeyPressed(Keys.D)){
                debugD =dTime;
            }
            else if(KeyDown(Keys.F)){
                debugD = dTime;
            }
            else {
                debugD = 0;
            }*/
            float[] result = UpdateBall(ball.X, ball.Y);
            ball.X = result[0];
            ball.Y = result[1];
            UpdatePaddle();
        }



        public override void Initialize() {
            ball = new Rect(new Point(width /2 -13, height /2 - 13), new Size(25, 25));
            player1 = new Rect(new Point(0, 0), new Point(25, 100));
            player2 = new Rect(new Point(width-25, 0), new Point(width,100));
            dashedPen = new Pen(Color.White,3);
            dashedPen.DashStyle = DashStyle.Dash;
            ai = EnemyAi.none;
            paddleOffset = r.Next(0,(Int32)player2.H/2);

        }

        public override void Render(Graphics g)
        {
            g.FillRectangle(Brushes.White, ball.Rectangle);
            g.FillRectangle(Brushes.White, player1.Rectangle);
            g.FillRectangle(Brushes.White, player2.Rectangle);
            g.DrawLine(dashedPen,new Point(width/2,0), new Point(width/2,height));
            g.DrawString(System.Convert.ToString(p1Score),new Font("Purisa", 40),Brushes.White,new Point(width/2 -70,0));
            g.DrawString(System.Convert.ToString(p2Score), new Font("Purisa", 40), Brushes.White, new Point(width / 2 + 30, 0));

        }

        private void UpdatePaddle() {
            if (KeyDown(Keys.Up)) {
                if (player1.Y - paddleSpeed * deltaTime > 0) {
                    player1.Y = player1.Y - paddleSpeed * deltaTime;
                }
                else {

                    player1.Y = 0;
                }
            }
            if (KeyDown(Keys.Down)) {
                if ((player1.H + player1.Y)  + paddleSpeed * deltaTime < height) {
                    player1.Y = player1.Y + paddleSpeed * deltaTime;
                }
                else {
                    player1.Y = height - player1.H;
                }
            }
            if (ball.Center.Y  < player2.Top+paddleOffset) {
                ai = EnemyAi.Up;
            }
            else if (ball.Center.Y > player2.Bottom-paddleOffset) {
                ai = EnemyAi.Down;
            }
            else  {
                ai = EnemyAi.none;
            }
            if (ai == EnemyAi.Down) {
                if ((player2.H + player2.Y) + paddleSpeed * deltaTime < height) {
                    player2.Y = player2.Y + paddleSpeed * deltaTime;
                }
                else {
                    player2.Y = height - player2.H;
                }
            }
            if (ai == EnemyAi.Up) {
                if (player2.Y - paddleSpeed * deltaTime > 0) {
                    player2.Y = player2.Y - paddleSpeed * deltaTime;
                }
                else {
                    player2.Y = 0;
                }
            }
            
        }

        public float[] UpdateBall(float _x, float _y) {
            float[] posXY = new float[2];
            posXY[0] = _x + horizontalSpeed * deltaTime;

            if (ball.W+_x > width) {
                horizontalSpeed = horizontalSpeed * -1f;
                posXY[0] = width - ball.W;
                p1Score++;
                Console.WriteLine("player 1 Score" + p1Score);
            }
            else if (ball.X < 0) {
                horizontalSpeed = horizontalSpeed * -1f;
                posXY[0] = 0;
                p2Score++;
                Console.WriteLine("player 2 score" + p2Score);
            }        
            posXY[1] = _y + verticalSpeed *deltaTime;

            if (ball.H + _y > height) {
                verticalSpeed = verticalSpeed * -1f;
                posXY[1] = height - ball.H;
            }
            else if (ball.Y < 0) {
                verticalSpeed = verticalSpeed * -1f;
                posXY[1] = 0;
            }
            if (ball.Intersects(player1)){
                horizontalSpeed = horizontalSpeed * -1f;
                posXY[0] = player1.W+1;
                paddleOffset = r.Next(0, (Int32)player2.H/2);
            }
            if (ball.Intersects(player2)) {
                horizontalSpeed = horizontalSpeed * -1f;
                posXY[0] = width - player2.W -ball.W - 1;
            }
            
            return posXY;
        }


    }
}
