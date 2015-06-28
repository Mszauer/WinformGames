using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Obstacle {
        public Rect building = null;
        public Rect topBuilding = null;
        public Obstacle lastBuilding = null;
        public float speed = 100;
        Random r = null;
        public float bldgSpacing = 50; //sets initial spacing
        Size windowWH = default(Size);
        public enum ObstacleType { Normal, Cloud, Closed }
        public ObstacleType type = ObstacleType.Normal;
        float closedOpening = 100f;
        public float X {
            get {
                return building.X;
            }
        }
        public float W {
            get {
                return building.W;
            }
        }
        public float Y {
            get {
                return building.Y;
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
            Console.WriteLine("Width: "+bldgWidth);
            Console.WriteLine("Height: "+bldgHeight);
            Console.WriteLine("Spacing: " + bldgSpacing);
#endif
            building = new Rect(x, bldgHeight, bldgWidth, windowWH.Height);

            if (type == ObstacleType.Cloud) {
                building.Y -= 150;
            }
            topBuilding = new Rect(x, 0, bldgWidth, building.Y - closedOpening);
            
        }

        public void Update(float dTime) {
            building.X -= dTime * speed;
            topBuilding.X -= dTime * speed;
            if (building.X < 0 - building.W) {
                Initialize(lastBuilding.X + lastBuilding.W + bldgSpacing);
            }


        }

        public void Render(Graphics g, Brush brushColor) {
#if DEBUG
            DebugRender(g, brushColor);
#endif
        }

        void DebugRender(Graphics g, Brush brushColor) {
            if (type == ObstacleType.Normal) {
                g.FillRectangle(brushColor, building.Rectangle);
            }
            else if (type == ObstacleType.Closed) {
                g.FillRectangle(brushColor, building.Rectangle);
                g.FillRectangle(brushColor, topBuilding.Rectangle);

            }
            else if (type == ObstacleType.Cloud) {
                g.FillRectangle(brushColor, building.Rectangle);
            }
        }
    }
}
