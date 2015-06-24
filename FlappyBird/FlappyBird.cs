using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Game {
    class FlappyBird : GameBase{
        float timeAccum = 0;
        List<Obstacle> pipes = null;
        Player player = null;
        Random r = null;
        Rect pointCard = null;
        int score = 0;
        int bestScore = 0;
        enum GameState { Start, Play, Lose }
        GameState CurrentState = GameState.Start;
        Background background = null;

        public FlappyBird() {
            width = 400;
            height = 600;
            this.clearColor = Brushes.Black;
        }

        public override void Initialize(){
            r = new Random();
            pipes = new List<Obstacle>();
            score = 0;
            using (StreamReader loadScore = new StreamReader("Assets/score.txt")) {
                if (loadScore.ReadLine() != null) {
                    bestScore = System.Convert.ToInt32(loadScore.ReadLine());
                }
            }

            pointCard = new Rect(new Point(width / 2 - 150, height / 2 - 100), new Point(width / 2 + 150, height / 2 + 100));

            Obstacle pipe = new Obstacle(new Size(width, height));           
            pipe.Generate(200); // sets base point for opening            
            pipes.Add(pipe);
            Obstacle pipe2 = new Obstacle(new Size(width, height));
            pipe2.Generate(250);
            pipe2.lastPipe = pipe;
            pipe.lastPipe = pipe2;
            pipes.Add(pipe2);
            
            float pipeSpacing = 280.0f;
            pipe.X = width;
            pipe2.X = width + pipeSpacing;
            player = new Player(new Size(width, height));
            player.Generate();
            background = new Background(new Size(width,height));
            background.Initialize();
        }

        public override void Update(float dTime) {
            timeAccum += dTime;
            if (CurrentState == GameState.Start) {
                if (KeyPressed(Keys.Up)) {
                    CurrentState = GameState.Play;
                }
            }
            else if (CurrentState == GameState.Play) {
                Collision();
                foreach (Obstacle pipe in pipes) {
                    pipe.Update(dTime);
                    if (pipe.canScore && player.X > pipe.X ) {
                        if (pipe.canScore) {
                            score++;
                            pipe.canScore = false;
                        }
                    }
                }
                player.Update(dTime);
                if (KeyPressed(Keys.Up)) {
                    player.Jump();
                }
#if DEBUG
                if (KeyPressed(Keys.Down)) {
                    CurrentState = GameState.Start;
                }
#endif
            }
            else if (CurrentState == GameState.Lose) {
                if (KeyPressed(Keys.R)) {
                    CurrentState = GameState.Start;
                    Initialize();
                }
                if (score > bestScore) {
                    bestScore = score;
                    SaveScore();
                }
            }
            background.Update(dTime);
        }
        void SaveScore() {
            using (StreamWriter save = new StreamWriter("Assets/score.txt")) {
                save.Write(System.Convert.ToString(bestScore));
            }
        }

        public void Collision() {
            foreach (Obstacle pipe in pipes){
                if (player.player.Intersects(pipe.topObstacle) || player.player.Intersects(pipe.bottomObstacle)) {
                    CurrentState = GameState.Lose;
                }
            }
            if (player.OutOfBounds(new Size(width,height))) {
                    CurrentState = GameState.Lose;
            }
        }

        public override void Render(Graphics g){
            background.Draw(g);
            player.Draw(g);
            for (int i = 0; i < pipes.Count; i++) {
                pipes[i].Draw(g);
            }

            if (CurrentState == GameState.Start) {
                g.DrawString("Get Ready!", new Font("Purisa", 20), Brushes.Red, new Point(width / 2 - 50, 150));
            }
            else if (CurrentState == GameState.Play) {
                g.DrawString("Score: " + System.Convert.ToString(score), new Font("Purisa", 20), Brushes.Red, new Point(width / 2 - 50, 0));                
            }
            else if (CurrentState == GameState.Lose) {
#if DEBUG
                g.FillRectangle(Brushes.Blue, pointCard.Rectangle);
#endif
            }
        }


    }
}
