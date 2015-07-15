using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class Column {
        float tileSize = 0; //set from ColumnWindow tileSize
        List<List<Rect>> columnStates = null;
        public Point Position = default(Point); //gets set in ColumnWindow and accounts for offset
        //checks to see what order the column is in, and can set or return it
        int currentState = 0;
        public int CurrentState {
            get {
                return currentState;
            }
            set {
                currentState = value;
            }
        }

        //FIX THIS TO GET CHECKBOUNDRY TO WORK
        public Rect AABB { //Axis Aligned Boundry Blocks AKA visualization of boundry
            get {
                //set min and max X/Y according to first rect in currentState
                float minX = columnStates[currentState][0].X;
                float minY = columnStates[currentState][0].Y;
                float maxX = columnStates[currentState][0].X + columnStates[currentState][0].W;
                float maxY = columnStates[currentState][0].Y + columnStates[currentState][0].H;
                //Loop through each rect in currentState
                foreach (Rect r in columnStates[currentState]) {
                    //Adjust min/max X/Y accordingly
                    if (r.X < minX) {
                        minX = r.X;
                    }
                    if (r.Y < minY) {
                        minY = r.Y;
                    }
                    if (r.X + r.W > maxX) {
                        maxX = r.X + r.W;
                    }
                    if (r.Y + r.H > maxY) {
                        maxY = r.Y + r.H;
                    }
                }//end foreach
                return new Rect(new Point((int)minX, (int)minY), new Point((int)maxX, (int)maxY));

            }
        }

        public Column(int size) {
            tileSize = size;
            columnStates = new List<List<Rect>>();

        }

        public List<Rect> ReturnRects() {
            //Returns a list of new rects based on current rect displayed
            List<Rect> returnShape = new List<Rect>();
            for (int i = 0; i < columnStates[currentState].Count; i++) {
                Rect r = new Rect();
                r.X = columnStates[currentState][i].X + Position.X; //account for offset
                r.Y = columnStates[currentState][i].Y + Position.Y;
                r.W = columnStates[currentState][i].W;
                r.H = columnStates[currentState][i].H;
                returnShape.Add(r);
            }
            return returnShape;
        }

        public void CreateColumn(int one, int two, int three) {
            //create Individual columns
            List<Rect> shape = new List<Rect>();
            Rect ichi = new Rect(0f,0f,tileSize,tileSize);
            Rect ni = new Rect(0f, tileSize, tileSize, tileSize);
            Rect san = new Rect(0f, tileSize*2, tileSize, tileSize);
            shape.Add(ichi);
            shape.Add(ni);
            shape.Add(san);
            columnStates.Add(shape);
        }
        public void Switch() {
            //create a loop for switching rects in a column
            currentState++;
            if (currentState >= columnStates.Count) {
                currentState = 0;
            }
#if DEBUG
            Console.WriteLine("ColumnState: "+currentState);
#endif
        }
        public void Render(Graphics g, List<Brush> color) {
#if DEBUG
            //Not working? Don't even need AABB probably
            //draw AABB 
            using (Pen p = new Pen(Brushes.Green, 1.0f)) {
                g.DrawRectangle(p, AABB.X+Position.X, AABB.Y+Position.Y,AABB.W,AABB.H);
            }
#endif
            //Draw each rectangle in currentState while accounted for offset
            for (int i = 0; i < columnStates[currentState].Count; i++) {
                g.FillRectangle(color[0], (int)Position.X, (int)Position.Y, (int)columnStates[currentState][i].W, (int)columnStates[currentState][i].H);
            }

        }
    }
}