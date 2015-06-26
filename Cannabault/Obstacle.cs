using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Obstacle {
        Rect building = null;
        public Obstacle lastBuilding = null;
        public float speed = 100;
        Random r = null;
        public float bldgSpacing = 0;
        Size windowWH = default(Size);
        public float X {
            get {
                return building.X;
            }
            set {
                building.X = value;
            }
        }
        public float W {
            get {
                return building.W;
            }
        }

        public Obstacle(Size window) {
            windowWH = window;
            r = new System.Random(Guid.NewGuid().GetHashCode());
        }

        public void Initialize(float x) {
            float bldgWidth = r.Next(150, windowWH.Width - 25); // 150min width, 300 max width
            float bldgHeight = r.Next(300, windowWH.Height - 250); //350min height, 300max height (in y terms)
            bldgSpacing = r.Next(50, 150);
#if DEBUG
            Console.WriteLine("Width: " + bldgWidth);
            Console.WriteLine("Height: " + bldgHeight);
            Console.WriteLine("Spacing: " + bldgSpacing);
#endif
            building = new Rect(x, bldgHeight, bldgWidth, windowWH.Height);
        }

        public void Update(float dTime) {
            building.X -= dTime * speed;
            if (building.X < 0 - building.W) {
                // dont set the building x just generate new one?
                //building.X = lastBuilding.X + lastBuilding.W + bldgSpacing ;
                Initialize(lastBuilding.X + lastBuilding.W + bldgSpacing);
            }


        }

        public void Render(Graphics g, Brush brushColor) {
#if DEBUG
            DebugRender(g, brushColor);
#endif
        }

        void DebugRender(Graphics g, Brush brushColor) {
            g.FillRectangle(brushColor, building.Rectangle);

        }
    }
}
