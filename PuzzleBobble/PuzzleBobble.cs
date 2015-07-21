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
        public enum State { Aiming, Firing };
        Point boardOffset = default(Point);
        public float ShootAngle = 90f;
        public float RotationSpeed = 30f;
        public PointF ShootingPosition = default(PointF);
        public PointF ShootingVelocity = default(PointF);
        Brush[] b = new Brush[] { Brushes.Yellow, Brushes.Green, Brushes.Red };
        public float BallRadius {
            get {
                return Board[0][0].HalfW;
            }
        }
        public float BallDiameter {
            get {
                return BallRadius * 2;
            }
        }
        public Rect BoardArea {
            get {
                return new Rect(boardOffset.X, boardOffset.Y, BoardDimensions.Width * BallDiameter+BallRadius, BoardDimensions.Height * BallDiameter+BallDiameter*2);
            }
        }
        public Point BoardCenter{ //returns center in world space / pixel space
            get{
                return new Point((int)(BoardDimensions.Width*Board[0][0].W) / 2 , (int)(BoardDimensions.Height*Board[0][0].Radius) / 2 );
            }
        }
        public float AimRadians {
            get {
                return ShootAngle * 0.0174532925f; //Convert degree to radians constant
            }
        }
        public State GameState {
            get {
                if (ShootingVelocity.X == 0 && ShootingVelocity.Y == 0) {
                    return State.Aiming;
                }
                return State.Firing;
            }
        }
        public Rect ShootingRect {
            get {
                return new Rect(new Point((int)(ShootingPosition.X - BallRadius), (int)(ShootingPosition.Y - BallRadius)), new Size((int)BallDiameter, (int)BallDiameter));
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
        public void Update(float deltaTime, bool rightDown,bool dDown, bool leftDown, bool aDown, bool spacePressed) {
            if (GameState == State.Aiming) {
                //Rotate Aim Left or Right
                if (rightDown || dDown) {
                    ShootAngle -= RotationSpeed * deltaTime;
                    if (ShootAngle < 0f) {
                        ShootAngle = 0f;
                    }
                }
                if (leftDown || aDown) {
                    ShootAngle += RotationSpeed * deltaTime;
                    if (ShootAngle > 180f) {
                        ShootAngle = 180f;
                    }
                }
                //Find the center point of aim indicator
                PointF end = new PointF();
                end.X = BoardCenter.X + (50.0f * (float)Math.Cos(AimRadians));
                end.Y = BoardArea.Bottom - (50.0f * (float)Math.Sin(AimRadians));
                ShootingPosition.X = (int)(BoardCenter.X + (end.X - BoardCenter.X) * 0.5f) + boardOffset.X;
                ShootingPosition.Y = (int)(BoardArea.Bottom + (end.Y - BoardArea.Bottom) * 0.5f);
                //Shoot!
                //250 = speed of moving ball / second
                if (spacePressed) {
                    ShootingVelocity.X = (float)Math.Cos(AimRadians) * 250.0f;
                    ShootingVelocity.Y = -(float)Math.Sin(AimRadians) * 250.0f;
                }
            }
            else if (GameState == State.Firing) {
                ShootingPosition.X += ShootingVelocity.X * deltaTime;
                ShootingPosition.Y += ShootingVelocity.Y * deltaTime;
                if (ShootingRect.Right > BoardArea.Right) {
                    ShootingPosition.X = BoardArea.Right - BallRadius;
                    ShootingVelocity.X *= -1.0f;
                }
                if (ShootingRect.Left < BoardArea.Left) {
                    ShootingPosition.X = BoardArea.Left + BallRadius;
                    ShootingVelocity.X *= -1.0f;
                }
                if (ShootingRect.Top < BoardArea.Top) {
                    Point p = Hexagon.TileAt(new Point((int)ShootingPosition.X,(int)ShootingPosition.Y), BallRadius, (float)boardOffset.X, (float)boardOffset.Y);
                    if (p.X >= Board.Length) {
                        p.X = Board.Length-1;
                    }
                    Board[p.X][p.Y].Value = 0;
                    ShootingVelocity.X = 0;
                    ShootingVelocity.Y = 0;
                }
                if (ShootingRect.Bottom > BoardArea.Bottom) {
                    ShootingPosition.Y = BoardArea.Bottom - BallRadius;
                    ShootingVelocity.Y *= -1.0f;
                }
            }
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
                        board[p.X][p.Y].Draw(g, Pens.Green,b);

                        Point nw = board[mouseIndex.X][mouseIndex.Y].GetNeighborIndex(Hexagon.Directions.NorthWest);
                        if (p == nw) {
                            board[nw.X][nw.Y].Draw(g, Pens.Yellow,b);
                        }
                    }
                }
                board[mouseIndex.X][mouseIndex.Y].Draw(g, Pens.Red,b);
            }
        }

        public void Render(Graphics g){
            //Draw Board
            for (int x = 0; x < Board.Length; x++) {
                for (int y = 0; y < Board[x].Length; y++) {
                    Board[x][y].Draw(g, Pens.Blue,b);
                }
            }
            //Draw Playing Area
            g.DrawRectangle(Pens.Green,BoardArea.Rectangle);
            //Draw Aimer
            Point end = new Point();
            end.X = (int)(50.0f * (float)Math.Cos(AimRadians));
            end.Y = (int)(50.0f * (float)Math.Sin(AimRadians));
            g.DrawLine(Pens.Red, new Point((int)BoardCenter.X+boardOffset.X, (int)BoardArea.Bottom), new Point((int)(BoardCenter.X + end.X+boardOffset.X), (int)(BoardArea.Bottom - end.Y)));
            g.DrawEllipse(Pens.Red, ShootingRect.Rectangle);
            
            int _x = (int)(Board[0][0].W*0.5f);
            int _y = (int)(Board[0][0].H*0.5f);
            RectangleF debug = new RectangleF(_x - BallRadius + boardOffset.X, _y - BallRadius+boardOffset.Y, BallDiameter, BallDiameter);
            g.DrawEllipse(Pens.Yellow, debug);
        }
    }
}
