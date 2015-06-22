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
        Random r = null;
        Size windowWH = default(Size);
        int startX = 0;
        float openingSize = 50; // size of the opening
        Size pipeSize = new Size(45, 0);
        public float X {
            set {
                collisionBoxTop.X = value;
                collisionBoxBottom.X = value;
            }
        }

        public Obstacles(Size window) {
            startX = window.Width;
            windowWH.Height = window.Height;
            r = new System.Random(Guid.NewGuid().GetHashCode()); ;
        }

        public void Initializer() {

        }

        public void Update(float deltaTime) {
            collisionBoxTop.X -= deltaTime * speed;
            collisionBoxBottom.X -= deltaTime * speed;
            if (collisionBoxTop.X < 0 - pipeSize.Width && collisionBoxBottom.X < 0 - pipeSize.Width) {
                Generate(r.Next(40,windowWH.Height-40)); // 40 is the amount it can move up or down
            }            
        }

        public void Draw(Graphics g) {
            DebugRender(g);
        }

        public void Generate(float opening) {
            // opening = point where the gap begins, openingSize = distance of gap from both sides(gets doubled)
            collisionBoxTop = new Rect(new Point(startX, 0), new Size(pipeSize.Width, (Int32)opening - (Int32)openingSize));
            collisionBoxBottom = new Rect(new Point(startX, (Int32)opening + (Int32)openingSize), new Size(pipeSize.Width, windowWH.Height));
        }

        public void DebugRender(Graphics g) {
            g.FillRectangle(Brushes.White, collisionBoxTop.Rectangle);
            g.FillRectangle(Brushes.Red, collisionBoxBottom.Rectangle);
        }
    }
}
