using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class FlappyBird : GameBase{
        float deltaTime = 0;
        List<Obstacles> pipes = null;
        float pipeSpeed = 0;
        Player player = null;
        bool playerCollision = false;
        bool gameOver = false;

        public FlappyBird() {
            width = 400;
            height = 600;
            this.clearColor = Brushes.Black;
        }

        public override void Initialize(){
            pipes = new List<Obstacles>();
            Obstacles pipe = new Obstacles(new Size(width, height));
            pipe.Generate(250); // height of pipes, opening == windowHeight - height*2
            pipes.Add(pipe);
        }

        public override void Update(float dTime) {
            deltaTime += dTime;
            pipes[0].Update(dTime);
        }

        public override void Render(Graphics g){
            for (int i = 0; i < pipes.Count; i++) {
                pipes[i].Draw(g);
            }

        }


    }
}
