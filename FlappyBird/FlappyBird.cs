using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class FlappyBird : GameBase{
        float timeAccum = 0;
        List<Obstacles> pipes = null;
        float pipeSpeed = 0;
        Player player = null;
        bool playerCollision = false;
        bool gameOver = false;
        Random r = null;

        public FlappyBird() {
            width = 400;
            height = 600;
            this.clearColor = Brushes.Black;
        }

        public override void Initialize(){
            r = new Random();
            pipes = new List<Obstacles>();
            Obstacles pipe = new Obstacles(new Size(width, height));
            pipe.Generate(100); // sets base point for opening
            pipes.Add(pipe);
            Obstacles pipe2 = new Obstacles(new Size(width, height));
            pipe2.Generate(200);
            pipes.Add(pipe2);
            Obstacles pipe3 = new Obstacles(new Size(width, height));
            pipe3.Generate(300);
            pipes.Add(pipe3);
            float pipeSpacing = 150;//((45 / width) / 3) * 45;
            pipe2.X = width + pipeSpacing;
            pipe3.X = width + pipeSpacing * 2;
        }

        public override void Update(float dTime) {
            timeAccum += dTime;
            foreach (Obstacles pipe in pipes) {
                pipe.Update(dTime);
            }
            /*if (timeAccum ==  1){
                pipes[1].Update(dTime);
            }
            if (timeAccum == 2) {
                pipes[2].Update(dTime);
                timeAccum -= 2.0f;
            }
             */

            
        }

        public override void Render(Graphics g){
            for (int i = 0; i < pipes.Count; i++) {
                pipes[i].Draw(g);
            }

        }


    }
}
