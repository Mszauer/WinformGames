using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class PuzzleBobble {
        public Hexagon[][] Board = null;
        public Size BoardDimensions = default(Size);
        Point boardOffset = default(Point);
        float BallRadius {
            get {
                return Board[0][0].HalfW;
            }
        }
        float BallDiameter {
            get {
                return BallRadius * 2;
            }
        }
        Rect BoardArea {
            get {
                return new Rect(boardOffset.X, boardOffset.Y, BoardDimensions.Width * BallDiameter, BoardDimensions.Height * BallDiameter+BallDiameter*2);
            }
        }
        Point BoardCenter{
            get{
                return new Point(BoardDimensions.Width / 2 , BoardDimensions.Height / 2 );
            }
        }

        public PuzzleBobble(Point pos, Size siz, float rad = 20f) {
            
            //set up board
            boardOffset = new Point(pos.X, pos.Y);
            BoardDimensions = new Size(siz.Width, siz.Height);
            Board = new Hexagon[BoardDimensions.Width][];
            for (int x = 0; x < Board.Length; x++) {
                Board[x] = new Hexagon[BoardDimensions.Height];
                for (int y = 0; y < Board[x].Length; y++) {
                    Board[x][y] = new Hexagon(rad, pos.X, pos.Y);
                    Board[x][y].Radius = rad;
                    Board[x][y].xIndexer = x;
                    Board[x][y].yIndexer = y;
                }
            }

        }
        public void Initialize() {

        }
        public void Update(float deltaTime) {

        }

        public void HighlightNeighbors(Graphics g,float xOffset, float yOffset, Hexagon[][] board,Point mouse) {
            Point MousePosition = new Point(mouse.X, mouse.Y);
            // Get the X, Y (board indices) of the mouse position
            Point mouseIndex = Hexagon.TileAt(new Point(MousePosition.X,MousePosition.Y), BallRadius,xOffset,yOffset);
             // Make sure mouseIndex is in bounds
            if (mouseIndex.X >= 0 && mouseIndex.Y >= 0 && mouseIndex.X < BoardDimensions.Width && mouseIndex.Y < BoardDimensions.Height) {
                // Get all neighbors
                List<Point> neighbors = board[mouseIndex.X][mouseIndex.Y].GetNeighborIndixes();
                // Loop trough all neighbors
                foreach (Point p in neighbors) {
                    // If neightbor is valid
                    if (p.X >= 0 && p.Y >= 0 && p.X < BoardDimensions.Width && p.Y < BoardDimensions.Height) {
                        // Draw neighbor
                        board[p.X][p.Y].Draw(g, Pens.Green);

                        Point nw = board[mouseIndex.X][mouseIndex.Y].GetNeighborIndex(Hexagon.Directions.NorthWest);
                        if (p == nw) {
                            board[nw.X][nw.Y].Draw(g, Pens.Yellow);
                        }
                    }
                }
                board[mouseIndex.X][mouseIndex.Y].Draw(g, Pens.Red);
            }
        }

        public void Render(Graphics g){
            //Draw Board
            for (int x = 0; x < Board.Length; x++) {
                for (int y = 0; y < Board[x].Length; y++) {
                    Board[x][y].Draw(g, Pens.Blue);
                }
            }
            g.DrawRectangle(Pens.Green,BoardArea.Rectangle);
            int _x = (int)(Board[0][0].W*0.5f);
            int _y = (int)(Board[0][0].H*0.5f);
            RectangleF debug = new RectangleF(_x - BallRadius + boardOffset.X, _y - BallRadius+boardOffset.Y, BallDiameter, BallDiameter);
            g.DrawEllipse(Pens.Yellow, debug);
        }
    }
}
