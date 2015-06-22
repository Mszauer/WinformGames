using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Obstacles {
        public float speed = 100;
        Point pipePosition = default(Point);
        Rect collisionBoxTop = null;
        Rect collisionBoxBottom = null;
        Sprite pipeTop = null;
        Sprite pipeBottom = null;
        float obstacleOpening = 0;
        Random r = null;
        Size windowWH = default(Size);
        int startX = 0;
        int finishX = 0;
        Size pipeSize = new Size(45,0);

        public Obstacles(Size window) {
            startX = window.Width;
            windowWH.Height = window.Height;
            r = new Random();
        }

        public void Initializer() {

        }

        public void Update(float deltaTime) {
            collisionBoxTop.X -= deltaTime * speed;
            collisionBoxBottom.X -= deltaTime * speed;
            if (collisionBoxTop.X < 0-pipeSize.Width) {
                collisionBoxTop.X = startX;
            }
            if (collisionBoxBottom.X < 0 - pipeSize.Width) {
                collisionBoxBottom.X = startX;
            }
        }

        public void Draw(Graphics g) {
            DebugRender(g);
        }

        public void Generate(int y) {
            pipeSize.Height = y;
            collisionBoxTop = new Rect(new Point(startX,0), pipeSize);
            collisionBoxBottom = new Rect(new Point(startX, windowWH.Height -pipeSize.Height), pipeSize);
        }

        public void DebugRender(Graphics g) {
            g.FillRectangle(Brushes.White, collisionBoxTop.Rectangle);
            g.FillRectangle(Brushes.Red, collisionBoxBottom.Rectangle);
        }
    }
}
