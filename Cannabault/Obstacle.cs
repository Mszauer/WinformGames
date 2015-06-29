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
        public float speed = 100.0f;
        public static float baseSpeed = 100.0f;
        public static float maxSpeed = 300.0f;
        public List<Sprite> buildingSprites = null;
        Random r = null;
        Sprite display = null;
        Sprite topBldg = null;
        public float bldgSpacing = 100; //sets initial spacing
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
            set {
                building.Y = value;
            }
        }

        public Obstacle(Size window) {
            windowWH = window;
            r = new System.Random(Guid.NewGuid().GetHashCode());
            buildingSprites = new List<Sprite>();
            Sprite bldg1 = new Sprite("Assets/building1.png");
            Sprite bldg2 = new Sprite("Assets/building2.png");
            Sprite bldg3 = new Sprite("Assets/building3.png");
            Sprite bldg4 = new Sprite("Assets/building4.png");
            Sprite bldg5 = new Sprite("Assets/building5.png");
            Sprite bldg6 = new Sprite("Assets/building6.png");
            Sprite bldg7 = new Sprite("Assets/building7.png");
            Sprite bldg8 = new Sprite("Assets/building8.png"); //cloud obstacle
            Sprite bldg9 = new Sprite("Assets/building9_bottom.png"); //bottom
            Sprite bldg10 = new Sprite("Assets/building9_top.png");//top
            buildingSprites.Add(bldg1);
            buildingSprites.Add(bldg2);
            buildingSprites.Add(bldg3);
            buildingSprites.Add(bldg4);
            buildingSprites.Add(bldg5);
            buildingSprites.Add(bldg6);
            buildingSprites.Add(bldg7);
            buildingSprites.Add(bldg8);
            buildingSprites.Add(bldg9);
            buildingSprites.Add(bldg10);
            topBldg = bldg10;

        }

        public void Initialize(float x) {
            float bldgHeight = r.Next(300, windowWH.Height - 250); //350min height, 300max height (in y terms)
            bldgSpacing = r.Next(75, 225);
#if DEBUG
            Console.WriteLine("Height: "+bldgHeight);
            Console.WriteLine("Spacing: " + bldgSpacing);
#endif
            display = buildingSprites[r.Next(0, 9)];
            building = new Rect(x, bldgHeight, display.W, windowWH.Height);
            if (display == buildingSprites[8]) { // bottom part of closed
                topBuilding = new Rect(x, 0, display.W, building.Y - closedOpening);
                type = ObstacleType.Closed;
            }
            else if (display == buildingSprites[7]) {// cloud
                building.Y -= 150;
                type = ObstacleType.Cloud;
            }
            else {
                type = ObstacleType.Normal;
            }
            Console.WriteLine(type);
            
        }

        public void Update(float dTime) {
            building.X -= dTime * speed;
            if (type == ObstacleType.Closed) {
                topBuilding.X -= dTime * speed;
            }
            if (building.X < 0 - building.W) {
                Initialize(lastBuilding.X + lastBuilding.W + bldgSpacing);
            }


        }

        public void Render(Graphics g, Brush brushColor) {
#if DEBUG
            DebugRender(g, brushColor);
#endif
#if !HIDESPRITE
            if (type == ObstacleType.Normal) {
                display.Draw(g, new Point((Int32)building.X, (Int32)building.Y));
            }
            else if (type == ObstacleType.Closed) {
                display.Draw(g, new Point((Int32)building.X, (Int32)building.Y));
                topBldg.Draw(g, new Point((Int32)topBuilding.X, (Int32)(topBuilding.H-topBldg.H)));
            }
            else if (type == ObstacleType.Cloud) {
                display.Draw(g, new Point((Int32)building.X, (Int32)building.Y));
            }
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
