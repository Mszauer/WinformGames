using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;


namespace Game {
    class BejewledGraphics {
        List<Sprite> icons = null;
        
        int [][] graphicsBoard = null;
        int tileSize = 0;
        int xOffset = 0;
        int yOffset = 0;

        public BejewledGraphics(int tileSize,int xOffset, int yOffset) {
            this.tileSize = tileSize;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            icons = new List<Sprite>();
            for (int i = 0; i < 8; i++) {
                icons.Add(new Sprite("Assets/icon_" +i+".png"));
            }
            
        }

        public void Initialize(int[][] board) {
            //Create graphic board
            graphicsBoard = new int[board.Length][];
            for (int i = 0; i < graphicsBoard.Length; i++) {
                graphicsBoard[i] = new int[board[i].Length];
            }
            //Copy values from logical to graphical
            for (int x = 0; x < graphicsBoard.Length; x++) {
                for (int y = 0; y < graphicsBoard[x].Length; y++) {
                    graphicsBoard[x][y] = board[x][y];
                }
            }
        }

        public void Update(float dTime){

        }

        public void Render(Graphics g) {
            for (int x = 0; x < graphicsBoard.Length; x++) {
                for (int y = 0; y < graphicsBoard[x].Length; y++) {
                    int index = graphicsBoard[x][y];
                    icons[index].Draw(g,new Point(x * tileSize + xOffset, y*tileSize + yOffset));
                }
            }
        }
    }
}
