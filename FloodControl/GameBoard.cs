using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class GameBoard {
        public Dictionary<Point, FallingPiece> fallingPieces = new Dictionary<Point, FallingPiece>();
        Random r = new Random();
        public int BoardWidth = 0;
        public int BoardHeight = 0;
        public GatePiece[][] gates = null;
        private List<Point> WaterTracker = new List<Point>();

        public GameBoard(int width = 8, int height = 10){
            //set board size
            BoardHeight = height;
            BoardWidth = width;
            //Generate board
           
            ClearBoard();
        }
        public bool ArePiecesAnimation() {
            return fallingPieces.Count == 0;
        }
        public void UpdateAnimatedPieces(float dTime) {
            Queue<Point> remove = new Queue<Point>();
            foreach (Point key in fallingPieces.Keys) {
                fallingPieces[key].UpdatePiece(dTime);

                if (fallingPieces[key].VerticalOffset == 0) {
                    remove.Enqueue(key);
                }
            }
            while (remove.Count > 0) {
                fallingPieces.Remove(remove.Dequeue());
            }
        }
        public void ClearBoard(){
            //create board
            gates = new GatePiece[BoardWidth][];
            for (int col = 0 ; col < gates.Length ; ++col){
               gates[col] = new GatePiece[BoardHeight];
                for (int row = 0 ; row < gates[col].Length ; row++){
                    gates[col][row] = new GatePiece(GatePiece.Types.Empty, false);
                }
            }
        }
        public void RotatePiece(int x, int y, bool clockwise){
            gates[x][y].RotatePiece(clockwise);
        }
        public Rect GetSubSprite(int x, int y){
            return gates[x][y].SubSprite();
        }
        public GatePiece.Types GetType(int x, int y){
            return gates[x][y].Type;
        }
        public void SetType(int x, int y, GatePiece.Types pieceType){
            gates[x][y].SetPiece(pieceType, false);
        }
        public bool HasConnector(int x, int y, GatePiece.Ends direction){
            return gates[x][y].HasConnection(direction);
        }
        public void RandomPiece(int x, int y){
            gates[x][y].SetPiece(GatePiece.RandomInt(r.Next(0,GatePiece.MaxIndex)),false);
        }
        public void DropDown (int x, int y){
            //Get amount to move down
            int dropAmt = y-1;
            while (dropAmt >=0){
                //if piece isn't null / empty then move down
                if (GetType(x,dropAmt) != GatePiece.Types.Empty){
                    SetType(x,y,GetType(x,dropAmt));
                    //set moved down to empty/null
                    SetType(x, dropAmt, GatePiece.Types.Empty);
                    AddFallingPiece(x, y, GetType(x, y),GatePiece.H *(y-dropAmt));
                    //breaks loop if not empty
                    dropAmt = -1;
                }
                dropAmt--;
            }
        }
        public void AddFallingPiece(int x, int y, GatePiece.Types PieceType, int VerticalOffset) {
            fallingPieces[new Point(x, y)] = new FallingPiece(PieceType, VerticalOffset);
        }
        public bool ArePiecesAnimating() {
            return fallingPieces.Count != 0;
        }
        public void GenerateNewPieces(bool dropSquares) {
            if (dropSquares) {
                for (int x = 0; x < gates.Length; x++) {
                    for (int y = gates[x].Length - 1; y >= 0; y--) {
                        if (GetType(x, y) == GatePiece.Types.Empty) {
                            DropDown(x, y);
                        }
                    }
                }
            }

            for (int y = 0; y < BoardHeight; y++) {
                for (int x = 0; x < gates.Length; x++) {
                    if (GetType(x,y) == GatePiece.Types.Empty) {
                        RandomPiece(x, y);
                        AddFallingPiece(x, y, GetType(x, y), GatePiece.H * BoardHeight);
                    }
                }
            }
        }//end new pieces
        public void ResetWater() {
            for (int y = 0; y < BoardHeight; y++) {
                for (int x = 0; x < gates.Length; x++) {
                    gates[x][y].IsFilled = false;
                }
            }
        }
        public void FillPiece(int x, int y) {
            gates[x][y].IsFilled = true;
        }
        public void PropagateWater(int x, int y, GatePiece.Ends fromDirection) {
            if (y >= 0 && y < BoardHeight && x >= 0 && x < BoardWidth) {
                if (gates[x][y].HasConnection(fromDirection) && !gates[x][y].IsFilled) {
                    FillPiece(x, y);
                    WaterTracker.Add(new Point(x, y));
                    foreach (GatePiece.Ends end in gates[x][y].GetOtherEnds(fromDirection)) {
                        //determine where opening is, find next opening
                        switch (end) {
                            case GatePiece.Ends.Left: PropagateWater(x - 1, y, GatePiece.Ends.Right);
                                break;
                            case GatePiece.Ends.Right: PropagateWater(x + 1, y, GatePiece.Ends.Left);
                                break;
                            case GatePiece.Ends.Top: PropagateWater(x, y - 1, GatePiece.Ends.Bottom);
                                break;
                            case GatePiece.Ends.Bottom: PropagateWater(x, y + 1, GatePiece.Ends.Top);
                                break;
                        }//end switch
                    }//end foreach
                }//end if loop
            }//end y loop
        }//end propagate
        public List<Point> GetWaterChain(int y) {
            //calls once for each row
            //once filled calls move water for next square
            WaterTracker.Clear();
            PropagateWater(0, y, GatePiece.Ends.Left);
            return WaterTracker;
        }
    }
}
