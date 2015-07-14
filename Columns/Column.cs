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
        int tileSize = 0; //set from ColumnWindow tileSize
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
        public Rect AABB { //Axis Aligned Boundry Block
            get {
                float minX = columnStates[currentState][0].X;
                float minY = columnStates[currentState][0].Y;
                float maxX = columnStates[currentState][0].X + columnStates[currentState][0].W;
                float maxY = columnStates[currentState][0].Y + columnStates[currentState][0].H;

            }
        }

        public Column(int size) {
            tileSize = size;
        }
        public void Initialize() {
            //create new list to use
            columnStates = new List<List<Rect>>();
        }
        public void CreateColumn(int[][] rowcol) {
            //create Individual columns
            List<Rect> shape = new List<Rect>();
            //loop through rows and columns passed in
            for (int col = 0; col < rowcol.Length; col++) {
                for (int row = 0; row < rowcol[col].Length; row++) {
                    //if value passed in >0 create a rect
                    if (rowcol[row][col] > 0) {
                        //create a rect in pixel space
                        Rect r = new Rect(col * tileSize, row * tileSize, tileSize, tileSize);
                        //add rect to shape (to form a shape)
                        shape.Add(r);
                    }//end if
                }//end row
            }//end col
            columnStates.Add(shape);
        }
        public void Switch() {
            //create a loop for switching rects in a column
            currentState++;
            if (currentState >= columnStates.Count) {
                currentState = 0;
            }
        }
    }
}