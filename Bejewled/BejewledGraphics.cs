using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

//explode / remove cell
//drop down
//generate new
namespace Game {
    class BejewledGraphics {
        List<EaseAnimation> lerp = null; 
        List<Sprite> icons = null;
        List<Point> destroyPos = null;
        FlipBook explosionAnim = null;
        FlipBook selectionAnim = null;
        
        int [][] graphicsBoard = null;
        int tileSize = 0;
        int xOffset = 0;
        int yOffset = 0;
        Point selection = default(Point);

        public BejewledGraphics(int tileSize,int xOffset, int yOffset) {
            lerp = new List<EaseAnimation>();
            destroyPos = new List<Point>();
            icons = new List<Sprite>();
            explosionAnim = FlipBook.LoadCustom("Assets/explosion.txt",60f);
            selectionAnim = FlipBook.LoadCustom("Assets/sparkles.txt");
            
            selectionAnim.Playback = FlipBook.PlaybackStyle.Loop;
            explosionAnim.Playback = FlipBook.PlaybackStyle.Single;
            selectionAnim.Anchor = FlipBook.AnchorPosition.TopLeft;
            explosionAnim.Anchor = FlipBook.AnchorPosition.Center;

            for (int i = 0; i < 180; i++) {
                explosionAnim.Update(1.0f / 30f);
            }
            this.tileSize = tileSize;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            for (int i = 0; i < 8; i++) {
                icons.Add(new Sprite("Assets/icon_" +i+".png"));
            }
            explosionAnim.AnimationFinished += this.DoExplosionFinished;
        }

        public void DoFall(Dictionary<Point, int> result, EaseAnimation.FinishedAnimationCallback finished) {
            foreach (KeyValuePair<Point, int> kvp in result) {
                int value = graphicsBoard[kvp.Key.X][kvp.Key.Y];
                Point startPos = new Point(kvp.Key.X*tileSize, kvp.Key.Y*tileSize);
                Point endPos = new Point(kvp.Key.X * tileSize, (kvp.Key.Y + kvp.Value) * tileSize);
                lerp.Add(new EaseAnimation(value, startPos, endPos));
                lerp[lerp.Count - 1].FallType = EaseAnimation.FallStyle.Bounce;
                lerp[lerp.Count - 1].AnimationSpeed = 0.75f;
                lerp[lerp.Count - 1].OnFinished = DoFinished;
                graphicsBoard[kvp.Key.X][kvp.Key.Y] = -1;
            }
            lerp[lerp.Count - 1].OnFinished += finished;
        }

        public void SetExplosionFinishedCallback(FlipBook.AnimationFinishedCallback Callback) {
            explosionAnim.AnimationFinished += Callback;
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
                    graphicsBoard[x][y] = -1;
                }
            }
        }

        public void DoSpawn(Dictionary<Point, int> spawnLoc) {
            foreach (KeyValuePair<Point, int> kvp in spawnLoc) {
                lerp.Add(new EaseAnimation(kvp.Value, new Point(kvp.Key.X*tileSize,kvp.Key.Y*tileSize), new Point(0,1)));
                lerp[lerp.Count - 1].AnimationSpeed = 0.75f;
                lerp[lerp.Count - 1].FallType = EaseAnimation.FallStyle.Scale;
                lerp[lerp.Count - 1].OnFinished += DoFinished;
            }
        }

        public void DoExplosionFinished(int ed){            
            foreach (Point p in destroyPos) {
                graphicsBoard[p.X][p.Y] = -1;
            }
            destroyPos = new List<Point>();
        }

        public void DoSwap(Point a, Point b, EaseAnimation.FinishedAnimationCallback onDone, int aVal, int bVal) {
            lerp.Add(new EaseAnimation(aVal, new Point(a.X * tileSize, a.Y * tileSize), new Point(b.X * tileSize, b.Y * tileSize)));
            lerp[lerp.Count - 1].OnFinished += DoFinished;
            lerp[lerp.Count - 1].OnFinished += onDone;

            lerp.Add(new EaseAnimation(bVal, new Point(b.X * tileSize, b.Y * tileSize), new Point(a.X * tileSize, a.Y * tileSize)));
            lerp[lerp.Count - 1].OnFinished += DoFinished;


            graphicsBoard[a.X][a.Y] = -1;
            graphicsBoard[b.X][b.Y] = -1;

        }

        public void DoSelection(int xIndex, int yIndex){
            selection = new Point(xIndex*tileSize, yIndex*tileSize);
        }


        public void DoDestroy(List<Point> streak, int value){
            explosionAnim.Reset(value);
            destroyPos = streak;
        }

        void DoFinished(Point cell, int value, EaseAnimation anim) {
            graphicsBoard[cell.X/tileSize][cell.Y/tileSize] = value;
            lerp.Remove(anim);
        }
        
        public void Update(float dTime){
            for (int i = lerp.Count - 1; i >= 0; i--) {
                lerp[i].Update(dTime);
            }
            explosionAnim.Update(dTime);
            selectionAnim.Update(dTime);
        }

        public void Render(Graphics g) {
            //Draw Icons
            for (int x = 0; x < graphicsBoard.Length; x++) {
                for (int y = 0; y < graphicsBoard[x].Length; y++) {
                    int index = graphicsBoard[x][y];
                    if (index >= 0) {
                        icons[index].Draw(g, new Point(x * tileSize + xOffset, y * tileSize + yOffset));
                    }
                }
            }
            //Animations for spawn
            //Console.WriteLine("num lerp: " + lerp.Count);
            foreach (EaseAnimation l in lerp) {
                if (l.cellValue >= 0) {
                    if (l.FallType == EaseAnimation.FallStyle.Scale) {
                        icons[l.cellValue].Draw(g, new Point(l.currentPosition.X + xOffset, l.currentPosition.Y + yOffset), l.currentScale);                        
                    }
                    else {
                        icons[l.cellValue].Draw(g, new Point(l.currentPosition.X + xOffset, l.currentPosition.Y + yOffset));
                    }
                }
            }
            // Animation for destroy
            foreach (Point p in destroyPos){
                explosionAnim.Render(g, new Point(p.X*tileSize + xOffset + (int)(explosionAnim.W/2f)-2, p.Y*tileSize+yOffset + (int)(explosionAnim.H/2f-4)));
            }
            //Anim for selection
            selectionAnim.Render(g, new Point(selection.X + xOffset +3, selection.Y +yOffset+ 2));
            selectionAnim.Render(g, new Point(selection.X + xOffset +15, selection.Y + yOffset+30));
            selectionAnim.Render(g, new Point(selection.X + xOffset +34, selection.Y + yOffset+12));

        }
    }
}
