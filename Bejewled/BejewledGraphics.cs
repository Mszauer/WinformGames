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
        List<LerpAnimation> lerp = null; 
        List<Sprite> icons = null;
        
        int [][] graphicsBoard = null;
        int tileSize = 0;
        int xOffset = 0;
        int yOffset = 0;

        public BejewledGraphics(int tileSize,int xOffset, int yOffset) {
            lerp = new List<LerpAnimation>();
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

        public void DoStreak(List<Point> removedCells){
            /*List<Point> removeThis = new List<Point>(removedCells);
            foreach (Point p in removedCells) {
                graphicsBoard[p.X][p.Y] = -1;
            }*/
        }

        public void DoSwap(Point a, Point b, LerpAnimation.FinishedAnimationCallback onDone, int aVal, int bVal) {
            lerp.Add(new LerpAnimation(aVal, new Point(a.X * tileSize, a.Y * tileSize), new Point(b.X * tileSize, b.Y * tileSize)));
            lerp[lerp.Count - 1].OnFinished += OnFinished;
            lerp[lerp.Count - 1].OnFinished += onDone;

            lerp.Add(new LerpAnimation(bVal, new Point(b.X * tileSize, b.Y * tileSize), new Point(a.X * tileSize, a.Y * tileSize)));
            lerp[lerp.Count - 1].OnFinished += OnFinished;


            graphicsBoard[a.X][a.Y] = -1;
            graphicsBoard[b.X][b.Y] = -1;

        }

        void OnFinished(Point cell, int value, LerpAnimation anim) {
            graphicsBoard[cell.X/tileSize][cell.Y/tileSize] = value;
            lerp.Remove(anim);
        }

        public void Update(float dTime){
            for (int i = lerp.Count - 1; i >= 0; i--) {
                lerp[i].Update(dTime);
            }
        }

        public void Render(Graphics g) {
            for (int x = 0; x < graphicsBoard.Length; x++) {
                for (int y = 0; y < graphicsBoard[x].Length; y++) {
                    int index = graphicsBoard[x][y];
                    if (index >= 0) {
                        icons[index].Draw(g, new Point(x * tileSize + xOffset, y * tileSize + yOffset));
                    }
                }
            }
            //Console.WriteLine("num lerp: " + lerp.Count);
            foreach (LerpAnimation l in lerp) {
                icons[l.cellValue].Draw(g, new Point(l.currentPosition.X  + xOffset, l.currentPosition.Y + yOffset));
            }
        }
    }
}
