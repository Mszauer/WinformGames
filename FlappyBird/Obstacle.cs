using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Obstacle {
        public float speed = 100;
        public Obstacle lastPipe = null; 
        public Rect topObstacle = null;
        public Rect bottomObstacle = null;
        public bool canScore = true;
        Sprite pipe = null;
        Random r = null;
        Size windowWH = default(Size);
        int startX = 0;
        float openingSize = 100; // size of the opening
        Size pipeSize = new Size(100, 0);
        float lastOpening = 0;
        public float X {
            get {
                return topObstacle.X;
            }
            set {
                topObstacle.X = value;
                bottomObstacle.X = value;
            }
        }

        public Obstacle(Size window) {
            startX = window.Width;
            windowWH = window;
            r = new System.Random(Guid.NewGuid().GetHashCode());
            pipe = new Sprite("Assets/pipe.png");
        }

        public void Update(float deltaTime) {
            topObstacle.X -= deltaTime * speed;
            bottomObstacle.X -= deltaTime * speed;
            if (topObstacle.X < 0 - pipeSize.Width && bottomObstacle.X < 0 - pipeSize.Width) {
                int randomY = r.Next(45,windowWH.Height-45);
                while (Math.Abs(lastPipe.lastOpening - randomY) > 150 ) {
                    randomY = r.Next(45, windowWH.Height - 45);
                }
                Generate(randomY);
                canScore = true;

            }            
        }

        public void Draw(Graphics g) {
#if DEBUG
            DebugRender(g);
#else
            pipe.Draw(g, topObstacle, new Rect(0,pipe.H - topObstacle.H,topObstacle.W,topObstacle.H));//topObstacle.X,topObstacle.Y,topObstacle.W,topObstacle.H);
            pipe.Draw(g, bottomObstacle, new Rect(0, 0, bottomObstacle.W, bottomObstacle.H));//bottomObstacle.X,bottomObstacle.Y,bottomObstacle.W,bottomObstacle.H);
#endif
        }

        public void Generate(float opening) {
            // opening = point where the gap begins, openingSize = distance of gap from both sides(gets doubled)
            lastOpening = opening;
            topObstacle = new Rect(new Point(startX, 0), new Size(pipeSize.Width, (Int32)opening - (Int32)openingSize));
            bottomObstacle = new Rect(new Point(startX, (Int32)opening + (Int32)openingSize), new Size(pipeSize.Width, windowWH.Height));
        }

        public void DebugRender(Graphics g) {
            g.FillRectangle(Brushes.White, topObstacle.Rectangle);
            g.FillRectangle(Brushes.Red, bottomObstacle.Rectangle);
        }
    }
}
