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
        public enum State { Aiming, Firing,Falling };
        Point boardOffset = default(Point);
        public float ShootAngle = 90f;
        public List<PointF> FallingAnimation = null;
        public List<int> FallingColors = null;
        public float RotationSpeed = 30f;
        int ShootingColor = 0; // set in constructor and on collision
        Random r = null;
        float fallSpeed = 100f;
        public PointF ShootingPosition = default(PointF);
        public PointF ShootingVelocity = default(PointF);
        Brush[] b = new Brush[] { Brushes.Yellow, Brushes.Green, Brushes.Red,Brushes.Blue, Brushes.Purple,Brushes.Silver,Brushes.Orange,Brushes.Black };
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
                if (FallingAnimation.Count > 0) {
                    return State.Falling;
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
            r = new Random();
            
            //set up board
            boardOffset = new Point(pos.X, pos.Y);
            BoardDimensions = new Size(siz.Width, siz.Height);
            Board = new Hexagon[BoardDimensions.Width][];
            for (int x = 0; x < Board.Length; x++) {
                //Create actual hexagons
                Board[x] = new Hexagon[BoardDimensions.Height];
                for (int y = 0; y < Board[x].Length; y++) {
                    //create hexagons then assign values
                    Board[x][y] = new Hexagon(rad, pos.X, pos.Y);
                    Board[x][y].Radius = rad;
                    Board[x][y].xIndexer = x;
                    Board[x][y].yIndexer = y;
                }
            }
#if DEBUG
            Board[0][0].Value = 1;
            Board[0][1].Value = 2;
            Board[0][2].Value = 0;
            Board[0][3].Value = 3;
            Board[1][1].Value = 1;
            Board[1][3].Value = 0;
            Board[2][1].Value = 3;
            Board[2][3].Value = 2;
            Board[3][1].Value = 0;
            Board[3][3].Value = 1;
            Board[3][4].Value = 2;
            Board[4][1].Value = 3;
            Board[4][3].Value = 3;
            Board[4][4].Value = 3;
            Board[5][1].Value = 1;
            Board[5][3].Value = 0;
            Board[6][1].Value = 2;
            Board[6][3].Value = 2;
            Board[7][0].Value = 1;
            Board[7][2].Value = 0;
#endif
            ShootingColor = GetNextShootingColor();
            FallingAnimation = new List<PointF>();
            FallingColors = new List<int>();
        }
        public void Initialize() {

        }
        public int GetNextShootingColor() {
            // Create a list of integers for all the colors on the board
            List<int> colors = new List<int>();

            // Collect that list
            for (int x = 0; x < BoardDimensions.Width; ++x) {
                for (int y = 0; y < BoardDimensions.Height; ++y) {
                    // Only if the cell being checked is not empty
                    if (Board[x][y].Value != -1) {
                        // Only if the colors list doesnt already have this color
                        if (!colors.Contains(Board[x][y].Value)) {
                            // Add the color to our (unique) list
                            colors.Add(Board[x][y].Value);
                        }
                    }
                }
            }

            // If there where colors on the board, they are now in the colors list
            // Next, if that list is empty, return default
            if (colors.Count == 0) {
                return r.Next(0, b.Length);
            }

            // Now generate a random color
            int randomColor = 0;
            do { // Generate a new random number
                randomColor = r.Next(0, b.Length);
                // until we get one that colors contains
            } while (!colors.Contains(randomColor));

            // Return our random color
            return randomColor;
        }

        public void Update(float deltaTime, bool rightDown,bool dDown, bool leftDown, bool aDown, bool spacePressed) {
            if (GameState == State.Falling) {
                for (int i = FallingAnimation.Count-1; i >= 0; i-- ) {
                    PointF point = FallingAnimation[i];
                    point.Y += fallSpeed * deltaTime;
                    FallingAnimation[i] = point;
                    if (FallingAnimation[i].Y - BallRadius > BoardArea.Bottom) {
                        FallingAnimation.RemoveAt(i);
                        FallingColors.RemoveAt(i);
                    }

                }
            }
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
                    Point p = Hexagon.TileAt(new Point((int)ShootingPosition.X,(int)ShootingPosition.Y), Board[0][0].Radius, (float)boardOffset.X, (float)boardOffset.Y);
                    if (p.X >= Board.Length) {
                        p.X = Board.Length-1;
                    }
                    StampBoard(p.X, p.Y);
                    return;
                }
                if (ShootingRect.Bottom > BoardArea.Bottom) {
                    ShootingPosition.Y = BoardArea.Bottom - BallRadius;
                    ShootingVelocity.Y *= -1.0f;
                }
                //Collision
                for (int x = 0; x < BoardDimensions.Width; x++) {
                    for (int y = 0; y < BoardDimensions.Height; y++) {
                        if (Board[x][y].Value != -1) {
                            if (Distance(Board[x][y].Center,new Point((int)ShootingPosition.X, (int)ShootingPosition.Y)) < BallDiameter) {
                                Point _p = Hexagon.TileAt(new Point((int)ShootingPosition.X, (int)ShootingPosition.Y), Board[x][y].Radius, boardOffset.X, boardOffset.Y);
                                if (_p.Y < BoardDimensions.Height && _p.X < BoardDimensions.Width) {
                                    StampBoard(_p.X, _p.Y);
                                    return;
                                }
                            }
                        }//end board value
                    }//end y
                }//end x
                
            }
        }

        void StampBoard(int x, int y) {
            //changing values before moving back into firing mode
            ShootingVelocity.X = 0;
            ShootingVelocity.Y = 0;
            Board[x][y].Value = ShootingColor;
            ShootingPosition.X = -Board[x][y].Radius * 2;
            ShootingPosition.Y = -Board[x][y].Radius * 2;
            List<Point> result = GetStreak(x, y);
            if (result.Count >= 3) {
                foreach (Point p in result) {
                    FallingAnimation.Add(Board[p.X][p.Y].Center);
                    FallingColors.Add(Board[p.X][p.Y].Value);
                    Board[p.X][p.Y].Value = -1;
                }
            }
            ShootingColor = GetNextShootingColor();
            for (int col = 0; col < Board.Length; col++) {
                for (int row = 0; row < Board[x].Length; row++) {
                    if (!IsAnchored(col,row)) {
                        FallingAnimation.Add(Board[col][row].Center);
                        FallingColors.Add(Board[col][row].Value);
                        Board[col][row].Value = -1;
                    }
                }
            }
        }

        public bool IsAnchored(int x, int y) {
            // Make list of connections
            List<Point> connections = new List<Point>();
            connections.Add(new Point(x, y));

            // Loop trough all connections
            for (int i = 0; i < connections.Count; i++) {
                // Get current hexagon
                Point current = connections[i];
                Hexagon hex = Board[current.X][current.Y];

                // If edge connection, return true
                if (current.Y == 0 || current.X == 0 || current.X == BoardDimensions.Width - 1) {
                    return true;
                }

                // Loop trough neighbors
                List<Point> neighbors = hex.GetNeighborIndixes();
                foreach (Point neighbor in neighbors) {
                    // Out of bounds
                    if (neighbor.X < 0 || neighbor.Y < 0) {
                        //exit loop
                        continue;
                    }
                    // Out of bounds
                    if (neighbor.X >= BoardDimensions.Width || neighbor.Y >= BoardDimensions.Height) {
                        //exit loop
                        continue;
                    }
                    // Empty neighbor
                    if (Board[neighbor.X][neighbor.Y].Value == -1) {
                        //exit loop
                        continue;
                    }

                    // Increase connection list if neighbor is not already in it
                    if (!connections.Contains(neighbor)) {
                        connections.Add(neighbor);
                    }
                }
            }

            return false;
        }

        public List<Point> GetStreak(int x, int y) {
            List<Point> result = new List<Point>();

            // Add the first point
            result.Add(new Point(x, y));

            // This loop works becuase the condition (i < result.Count) is
            // evaluated on each iteration. So if the first iteration (Count = 1)
            // adds 3 items to the list, Count will be 4. We loop forwards 
            // because the list grows, not shrinks.
            for (int i = 0; i < result.Count; i++) {
                // Get the current point and hexagon being processed
                Point current = result[i];
                Hexagon hex = Board[current.X][current.Y];

                // Get all neighbors
                List<Point> neighbors = hex.GetNeighborIndixes();
                foreach (Point neighbor in neighbors) {
                    // Dont process out of bounds neighbors
                    if (neighbor.X < 0 || neighbor.Y < 0) {
                        continue;
                    }
                    // Dont process out of bounds neighbors
                    if (neighbor.X >= BoardDimensions.Width || neighbor.Y >= BoardDimensions.Height) {
                        continue;
                    }

                    // If neighbor is same as the hex we are looking at
                    if (Board[neighbor.X][neighbor.Y].Value == hex.Value) {
                        // If neighbor is not yet in the result chain
                        if (!result.Contains(neighbor)) {
                            // Record neighbor as part of result
                            result.Add(neighbor);
                        }
                    }
                }
            }

            return result;
        }

        float Distance(Point p1, Point p2) {
            Point v = new Point(p1.X - p2.X, p1.Y - p2.Y);
            return (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }
        
        public void HighlightNeighbors(Graphics g, float xOffset, float yOffset, Hexagon[][] board, Point mouse) {
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
            g.FillEllipse(b[ShootingColor], ShootingRect.Rectangle);
            //Draw Falling bobbles
            if (FallingAnimation.Count > 0) {
                for (int i = 0; i < FallingAnimation.Count; i++) {
                    RectangleF r = new RectangleF(FallingAnimation[i],new SizeF(BallRadius,BallRadius));
                    g.FillEllipse(b[FallingColors[i]], r);
                }
            }
            int _x = (int)(Board[0][0].W*0.5f);
            int _y = (int)(Board[0][0].H*0.5f);
            RectangleF debug = new RectangleF(_x - BallRadius + boardOffset.X, _y - BallRadius+boardOffset.Y, BallDiameter, BallDiameter);
            g.DrawEllipse(Pens.Yellow, debug);
        }
    }
}
