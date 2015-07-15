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
        List<Rect> position = null;
        public Point Position = default(Point); 
        //gets set in ColumnWindow and accounts for offset
        //checks to see what order the column is in, and can set or return it


        //FIX THIS TO GET CHECKBOUNDRY TO WORK
        public Rect AABB { //Axis Aligned Boundry Blocks AKA visualization of boundry
            get {
                //set min and max X/Y according to first rect in currentState
                float minX = position[0].X;
                float minY = position[0].Y;
                float maxX = position[0].X + position[0].W;
                float maxY = position[0].Y + position[0].H;
                //Loop through each rect in currentState
                foreach (Rect r in position) {
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
            position = new List<Rect>();

        }

        public List<Rect> ReturnRects() {
            //Returns a list of new rects based on current rect displayed
            List<Rect>result = new List<Rect>();
            foreach (Rect r in position) {
                Rect x = new Rect(r.X + Position.X,r.Y + Position.Y,r.W,r.H);
                result.Add(x);
            }
            return result;
        }

        public void CreateColumn(int one, int two, int three) {
            //create Individual columns
            Rect ichi = new Rect(0f,0f,tileSize,tileSize);
            Rect ni = new Rect(0f, tileSize, tileSize, tileSize);
            Rect san = new Rect(0f, tileSize*2, tileSize, tileSize);
            position.Add(ichi);
            position.Add(ni);
            position.Add(san);
        }
        public void Switch() {
            //create a loop for switching rects in a column
            
        }
        public void Render(Graphics g, List<Brush> color) {
#if DEBUG
            //draw AABB 
            using (Pen p = new Pen(Brushes.Green, 1.0f)) {
                g.DrawRectangle(p, AABB.X+Position.X, AABB.Y+Position.Y,AABB.W,AABB.H);
            }
#endif
            //Draw each rectangle in currentState while accounted for offset
            foreach (Rect r in position) {
                g.FillRectangle(color[0], (int)Position.X, (int)Position.Y, (int)r.W, (int)r.H);
            }

        }
    }
}