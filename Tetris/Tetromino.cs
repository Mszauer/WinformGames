using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class Tetromino {
        List<List<Rect>> states = null;
        int size = 30;
        int currentState = 0;
        public Point position = default(Point);
        public enum Direction { Left, Right };
        public Rect debugAABB = null;

        public Rect AABB {//axis aligned boundry blocks
            get {
                float minX = states[currentState][0].X;
                float minY = states[currentState][0].Y;
                float maxX = states[currentState][0].X + states[currentState][0].W;
                float maxY = states[currentState][0].Y + states[currentState][0].H;
                foreach (Rect r in states[currentState]) {
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
                }


                return new Rect(new Point((Int32)minX, (Int32)minY), new Point((Int32)maxX, (Int32)maxY));
            }
        }
        public Tetromino() {
            states = new List<List<Rect>>();

        }

        public List<Rect> ReturnRects() {
            List<Rect> returnShape = new List<Rect>();

            for (int i = 0; i < states[currentState].Count; i++) {
                Rect r = new Rect();
                r.X = states[currentState][i].X + position.X; // Offset for position
                r.Y = states[currentState][i].Y + position.Y;
                r.W = states[currentState][i].W;
                r.H = states[currentState][i].H;
                returnShape.Add(r);
            }
            return returnShape;
        }

        public void Draw(Graphics g, Brush color) {
            for (int i = 0; i < states[currentState].Count; i++) {
                g.FillRectangle(color, (Int32)(states[currentState][i].X + position.X), (Int32)(states[currentState][i].Y + position.Y), (Int32)states[currentState][i].W, (Int32)states[currentState][i].H);
            }
            using (Pen p = new Pen(Brushes.LimeGreen)) {
                g.DrawRectangle(p, AABB.X + position.X, AABB.Y + position.Y, AABB.W, AABB.H);

            }
        }

        public void CreateShape(int[][] rowcol) {
            List<Rect> shape = new List<Rect>();
            for (int row = 0; row < rowcol.Length; row++) {
                for (int col = 0; col < rowcol[row].Length; col++) {
                    if (rowcol[row][col] > 0) {
                        Rect r = new Rect(col * size, row * size, size, size);
                        shape.Add(r);
                    }
                }
            }
            states.Add(shape);
        }

        public void Rotate(Direction direction) {
            if (direction == Direction.Left) {
                currentState--;
                if (currentState < 0) {
                    currentState = states.Count - 1;
                }
            }
            if (direction == Direction.Right) {
                currentState++;
                if (currentState >= states.Count) {
                    currentState = 0;
                }
            }
        }
    }
}

