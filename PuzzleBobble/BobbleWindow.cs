using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class BobbleWindow : GameBase {
        Hexagon[][] board = null;
        Hexagon[][] board2 = null;
        float hexRadius = 20.0f; // set size of hexagons
        int boardWidth = 5; // # of hexagons on x axis
        int boardHeight = 5; // # of hexagons on y axis
        float xOffset = 0f;
        float yOffset = 0f;
        float xOffset2 = 0f;
        float yOffset2 = 300f;

        public BobbleWindow() {
            width = 400;
            height = 600;
            //set up board
            board = new Hexagon[boardWidth][];
            for (int i = 0; i < board.Length; i++) {
                board[i] = new Hexagon[boardHeight];
            } 
            board2 = new Hexagon[boardWidth][];
            for (int i = 0; i < board.Length; i++) {
                board2[i] = new Hexagon[boardHeight];
            }
        }
        public override void Initialize() {
            for (int x = 0; x < board.Length; x++) {
                for (int y = 0; y < board[x].Length; y++) {
                    Hexagon H = new Hexagon(hexRadius,xOffset,yOffset);
                    H.xIndexer = x;
                    H.yIndexer = y;
                    board[x][y] = H;
                }
            }
            for (int x = 0; x < board2.Length; x++) {
                for (int y = 0; y < board2[x].Length; y++) {
                    Hexagon H = new Hexagon(hexRadius, xOffset2, yOffset2);
                    H.xIndexer = x;
                    H.yIndexer = y;
                    board2[x][y] = H;
                }
            }
        }
        public override void Update(float deltaTime) {

        }

        public void HighlightNeighbors(Graphics g,float xOffset, float yOffset, Hexagon[][] board) {
            // Get the X, Y (board indices) of the mouse position
            Point mouseIndex = Hexagon.TileAt(new Point(MousePosition.X,MousePosition.Y), hexRadius,xOffset,yOffset);
             // Make sure mouseIndex is in bounds
            if (mouseIndex.X >= 0 && mouseIndex.Y >= 0 && mouseIndex.X < boardWidth && mouseIndex.Y < boardHeight) {
                // Get all neighbors
                List<Point> neighbors = board[mouseIndex.X][mouseIndex.Y].GetNeighborIndixes();
                // Loop trough all neighbors
                foreach (Point p in neighbors) {
                    // If neightbor is valid
                    if (p.X >= 0 && p.Y >= 0 && p.X < boardWidth && p.Y < boardHeight) {
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

        public override void Render(Graphics g){
            //Draw Board
            for (int x = 0; x < board.Length; x++) {
                for (int y = 0; y < board[x].Length; y++) {
                    board[x][y].Draw(g, Pens.Blue);
                }
            }
            HighlightNeighbors(g, xOffset, yOffset, board);
        }
    }
}
