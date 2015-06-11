using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class BreakOut : GameBase {
        Rect ball = null;
        Rect player = null;
        float deltaTime = 0;
        float ballXSpeed = 0;
        float ballYSpeed = 0;
        float ballMaxSpeed = 300;
        int paddleSpeed = 0;
        int numRow = 0;
        int numCol = 0;
        int lives = 5;
        List<int> difficultyLevel = null;
        //float debugDTime = 0;
        List<Rect> obstacles = null;
        Rect debugRect = null;

        public BreakOut() {
            this.title = "BreakOut";
            this.clearColor = Brushes.Black;
            ballXSpeed = 50f;
            ballYSpeed = 150f;
            paddleSpeed = 500;
            numRow = 5;
            numCol = 5;
        }

        public float ballPositionNormalization() {
            float normalization = ball.X - player.Center.X;
            normalization = (normalization / player.W)* 2.0f;
            if (normalization < -1.0f) {
                normalization = -1.0f;
            }
            else if (normalization > 1.0f) {
                normalization = 1.0f;
            }
            return Math.Abs(normalization);
        }

        public override void Update(float dTime) {
            if (obstacles.Count == 0) {
                return;
            }
            if (lives == 0) {
                return;
            }
            deltaTime = dTime;
            //debug controls
            /*if (KeyDown(Keys.D)) {
                debugDTime = deltaTime;
            }
            else if (KeyPressed(Keys.F)) {
                debugDTime = deltaTime;
            }
            else {
                debugDTime = 0;
            }
            if (KeyPressed(Keys.R)) {
                obstacles.RemoveAt(0);
                difficultyLevel.RemoveAt(0);
            }*/
            float[] result = UpdateBall(ball.X, ball.Y);
            ball.X = result[0];
            ball.Y = result[1];
            UpdatePaddle();
        }

        public override void Initialize() {
            player = new Rect(new Point(width/2-75, height - 25), new Point(width/2 + 75, height));
            ball = new Rect(new Point(width / 2 - 13, height / 2 - 13), new Size(25, 25));
            obstacles = new List<Rect>();
            difficultyLevel = new List<int>();
            for (int row = 0; row < numRow;  row++) { 
                for (int col = 0; col < numCol; col++) { 
                    Rect r = new Rect();
                    r.Y = row * 25;
                    r.X = col * (width / numCol + 1);
                    r.W = width / numCol;
                    r.H = 24.0f;
                    obstacles.Add(r);
                    if (row == numRow - 1) {
                        difficultyLevel.Add(3);
                    }
                    else if (row == numRow - 2) {
                        difficultyLevel.Add(2);
                    }
                    else {
                        difficultyLevel.Add(1);
                    }
                }
            }
            
        }
        public override void Render(Graphics g) {
            g.FillRectangle(Brushes.White, player.Rectangle);
            g.FillRectangle(Brushes.White ,ball.Rectangle);
            for (int i = 0; i < obstacles.Count; i++) {
                if (difficultyLevel[i] == 3) {
                    g.FillRectangle(Brushes.Red, obstacles[i].Rectangle);
                }
                else if (difficultyLevel[i] == 2) {
                    g.FillRectangle(Brushes.Yellow, obstacles[i].Rectangle);
                }
                else if (difficultyLevel[i] == 1) {
                    g.FillRectangle(Brushes.Green, obstacles[i].Rectangle);
                }
            }

            if (obstacles.Count == 0) {
                g.DrawString("Winner!", new Font("Purisa", 40), Brushes.White, new Point(width / 2 - 120, height/2 - 30));
            }
            if (lives == 0) {
                g.DrawString("Game Over!", new Font("Purisa", 40), Brushes.White, new Point(width / 2 - 120, height / 2 - 30));
            }
            //Collison debug
            /*if (debugRect != null) {
                g.FillRectangle(Brushes.Green, debugRect.Rectangle);
            }*/
        }
        void UpdatePaddle() {
            
            if (KeyDown(Keys.Right)) {
                if (player.X +player.W + paddleSpeed * deltaTime  < width) {
                    player.X = player.X  + paddleSpeed * deltaTime;
                }
                else {
                    player.X = width - player.W;
                }
            }
            if (KeyDown(Keys.Left)) {
                if (player.X - paddleSpeed * deltaTime > 0) {
                    player.X = player.X - paddleSpeed * deltaTime;
                }
                else {
                    player.X = 0;
                }
            }

        }
        public float[] UpdateBall(float x, float y) {
            float[] posXY = new float[2];
            posXY[0] = x + ballXSpeed * deltaTime;
            posXY[1] = y + ballYSpeed * deltaTime;
            if (ball.X <= 0) {
                ballXSpeed = ballXSpeed * -1.0f;
                posXY[0] = 0 + 1;
            }
            if ((ball.W + x) >= width) {
                ballXSpeed = ballXSpeed * -1.0f;
                posXY[0] = width - ball.W - 1;
            }
            if (ball.Y <= 0) {
                ballYSpeed = ballYSpeed * -1.0f;
                posXY[1] = 0 + 1;
            }

            if ((ball.H + y) >= height) {
                ballYSpeed = ballYSpeed * -1.0f;
                posXY[1] = height - ball.H - 1;
                lives--;
            }
            if (ball.Intersects(player)) {
                ballYSpeed = ballYSpeed * -1.0f;
                float ballDirection = -1.0f;
                if (ballXSpeed > 0){
                    ballDirection = 1.0f;
                }

                ballXSpeed = ballMaxSpeed * ballPositionNormalization() * ballDirection;
                posXY[1] = height - player.H - ball.H - 1;
                //posXY[1] = height / 2;
                
            }
            bool isHit = false;
            Rect tempBall = new Rect(posXY[0], posXY[1], ball.W, ball.H);
            for (int i = obstacles.Count-1; i >= 0; i--) {
                if (tempBall.Intersects(obstacles[i])) {
                    Rect obstacle = obstacles[i];
                    if (difficultyLevel[i] == 3 ){
                        difficultyLevel[i] = difficultyLevel[i] - 1;
                    }
                    else if ( difficultyLevel[i] == 2){
                        difficultyLevel[i] = difficultyLevel[i] - 1;
                    }
                    else if (difficultyLevel[i] == 1){
                       obstacles.RemoveAt(i);
                       difficultyLevel.RemoveAt(i);
                    }
                    if (isHit) {
                        continue;
                    }
                    Rect diff = debugRect = tempBall.Intersection(obstacle);
                    if (obstacle.Bottom == diff.Bottom) {
                        //posXY[1] = height / 2;
                        posXY[1] = posXY[1] + diff.H + 1;
                    }
                    else if (obstacle.Top == diff.Top) {
                        posXY[1] = posXY[1] - diff.H - 1;
                    }

                    if (obstacle.Left == diff.Left) {
                        posXY[0] = posXY[0] - diff.W - 1;
                    }
                    else if (obstacle.Right == diff.Right){
                        posXY[0] = posXY[0] + diff.W + 1;
                    }

                    ballYSpeed = ballYSpeed * -1.0f;
                    isHit = true;
                }
            }
                
            return posXY;
        }
        
    }
}
