using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class Hexagon {
        public float xIndexer = 0f; // indexers from main window
        public float yIndexer = 0f; // indexers from main window
        private float w = 0f;
        private float halfW = 0f;
        private float h = 0f;
        private float rowH = 0f;
        private float radius = 0f;
        float xOffset = 0f;
        float yOffset = 0f;
        public int Value = -1;
        //start getters
        public float W {
            get {
                return w;
            }
        }
        public float HalfW {
            get {
                return halfW;
            }
        }
        public float H {
            get {
                return h;
            }
        }
        public float RowH {
            get {
                return rowH;
            }
        }
        public float Radius {
            get {
                return radius;
            }
            set {
                radius = value;
                h = 2f * radius;
                rowH = 1.5f * radius;
                halfW = (float)Math.Sqrt((radius * radius) - ((radius / 2f) * (radius / 2f)));
                w = 2f * halfW;
            }
        }
        public Point Origin {
            get {
                //sets x to 0 or halfW(an offset) based on if Y is divisble by 2
                int x = (int)(xIndexer * w + xOffset) + (int)((yIndexer % 2 == 1) ? halfW : 0);
                int y = (int)(yIndexer * rowH + yOffset);
                return new Point(x, y);
            }
        }
        public Point Center {
            get {
                int x = (int)(xIndexer * W + halfW + xOffset) + (int)((yIndexer % 2 == 1) ? halfW : 0);
                int y = (int)(yIndexer * rowH + H / 2 + yOffset);
                return new Point(x, y);
            }
        }
        public enum Directions { NorthWest, NorthEast, East, SouthEast, SouthWest, West }
        //end member variables


        public Hexagon(float radius, float xOffset = 0, float yOffset = 0) {
            Radius = radius;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }

        public Point GetNeighborIndex(Directions direction) {
            if (yIndexer % 2 == 0) {//even row
                switch (direction) {
                    case Directions.NorthEast:
                        return new Point((int)xIndexer, (int)yIndexer - 1);
                    case Directions.NorthWest:
                        return new Point((int)xIndexer - 1, (int)yIndexer - 1);
                    case Directions.East:
                        return new Point((int)xIndexer + 1, (int)yIndexer);
                    case Directions.West:
                        return new Point((int)xIndexer - 1, (int)yIndexer);
                    case Directions.SouthEast:
                        return new Point((int)xIndexer, (int)yIndexer + 1);
                    case Directions.SouthWest:
                        return new Point((int)xIndexer - 1, (int)yIndexer + 1);
                }
            }
            else {
                switch (direction) { //odd row
                    case Directions.NorthEast:
                        return new Point((int)xIndexer + 1, (int)yIndexer - 1);
                    case Directions.NorthWest:
                        return new Point((int)xIndexer, (int)yIndexer - 1);
                    case Directions.East:
                        return new Point((int)xIndexer + 1, (int)yIndexer);
                    case Directions.West:
                        return new Point((int)xIndexer - 1, (int)yIndexer);
                    case Directions.SouthEast:
                        return new Point((int)xIndexer + 1, (int)yIndexer + 1);
                    case Directions.SouthWest:
                        return new Point((int)xIndexer, (int)yIndexer + 1);
                }
            }
            return new Point(-1, -1);
        }
        public List<Point> GetNeighborIndixes() {
            List<Point> result = new List<Point>();
            result.Add(GetNeighborIndex(Directions.NorthWest));
            result.Add(GetNeighborIndex(Directions.NorthEast));
            result.Add(GetNeighborIndex(Directions.East));
            result.Add(GetNeighborIndex(Directions.West));
            result.Add(GetNeighborIndex(Directions.SouthEast));
            result.Add(GetNeighborIndex(Directions.SouthWest));
            return result;
        }
        public static Point TileAt(Point worldCoordinate, float radius, float xOffset, float yOffset) {
            //adjust for offset
            worldCoordinate = new Point(worldCoordinate.X - (int)xOffset, worldCoordinate.Y - (int)yOffset);
            
            //static function so no access to member variables
            float height = 2f * radius;
            float rowHeight = 1.5f * radius;
            float halfWidth = (float)Math.Sqrt((radius * radius) - ((radius / 2f) * radius / 2f));
            float width = 2f * halfWidth;

            //First we will calculate a few constant to make the rest of code simpler
            float rise = height - rowHeight;
            float slope = rise / halfWidth;

            // Next we find our position in a square grid. This grid allows us to divide the hex map into two types of tiles.
            int X = (int)Math.Floor((worldCoordinate.X) / width);
            int Y = (int)Math.Floor((worldCoordinate.Y) / rowHeight);
            Point offset = new Point((int)(worldCoordinate.X - X  * width), (int)(worldCoordinate.Y- Y  * rowHeight));
           
            /* Looking at the diagram for section A, we can see that two hexagons poke into the bottom of the square.
              We plug the offset’s X value into the equation for the line of the top of those hexes at the bottom. 
              Recall from algerbra that the equation of a line like this is Y=MX+B. 
              That’s why we calculated the rise and slope earlier. 
              The line on the left of section A has a negative slope, and a Y intercept of rise. 
              The other has a positive slope, and a Y intercept of negative rise. 
              We adjust the X,Y tile coordinates if we discover that the point is below one of these lines. 
              This is the same adjustment used for finding neighbors. */
            if (Y % 2 == 0) { // if even row
                if (offset.Y < (-slope * offset.X + rise)) { //Point is below left line; inside SouthWest neighbor.
                    X -= 1;
                    Y -= 1;
                }
                else if(offset.Y < (slope*offset.X-rise)){//Point is below right line; inside SouthEast neighbor.
                    Y -= 1;
                }
            }
            else { //odd row
                /* Section B is slightly more complex. First we determine if the point is in the right or left section. 
                Since odd rows are offset by halfWidth, the large section on the right has the same coordinates as the square grid. */
                if (offset.X >= halfWidth) { // point on right side?
                    if (offset.Y < (-slope * offset.X + rise * 2.0f)) { //Point is below bottom line; inside SouthWest neighbor.
                        Y -= 1;
                    }
                }
                else { // Point on left side
                    if (offset.Y < (slope * offset.X)) {  //Point is below the bottom line; inside SouthWest neighbor.
                        Y -= 1;
                    }
                    else { //point is above the bottom line,inside west neighbor
                        X -= 1;
                    }
                }
            }
            return new Point(X, Y);
        }

        public void Draw(Graphics g, Pen p, Brushes[] b) {
            int centerHeight = (int)(RowH - (H - RowH));

            Point p1 = new Point(Center.X, Origin.Y);
            Point p2 = new Point(Origin.X, Origin.Y + (int)(H - RowH));
            g.DrawLine(p, p1, p2);

            p1 = new Point(Center.X, Origin.Y);
            p2 = new Point(Origin.X + (int)W, Origin.Y + (int)(H - RowH));
            g.DrawLine(p, p1, p2);

            p1 = new Point(Origin.X, Origin.Y + (int)(H - RowH));
            p2 = new Point(Origin.X, Origin.Y + (int)(H - RowH) + centerHeight);
            g.DrawLine(p, p1, p2);

            p1 = new Point(Origin.X + (int)W, Origin.Y + (int)(H - RowH));
            p2 = new Point(Origin.X + (int)W, Origin.Y + (int)(H - RowH) + centerHeight);
            g.DrawLine(p, p1, p2);

            p1 = new Point(Origin.X, Origin.Y + (int)(H - RowH) + centerHeight);
            p2 = new Point(Center.X, Origin.Y + (int)H);
            g.DrawLine(p, p1, p2);

            p1 = new Point(Origin.X + (int)W, Origin.Y + (int)(H - RowH) + centerHeight);
            p2 = new Point(Center.X, Origin.Y + (int)H);
            g.DrawLine(p, p1, p2);

            //Draw bubbles
            if (Value >= 0){
                g.FillEllipse(,new Rect(new Point((int)(Center.X-HalfW), (int)(Center.Y-HalfW)), new Size((int)HalfW*2,(int)HalfW*2)).Rectangle);
            }
        }
    }
}
