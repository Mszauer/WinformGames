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
        private GatePiece[][] gates = null;
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
                    gates[col][row] = new GatePiece(Empty);
                }
            }
        }
        public void RotatePiece(int x, int y, bool clockwise){
            gates[x][y].RotatePiece(clockwise);
        }
        public Rect GetSubSprite(int x, int y){
            return gates[x][y].SubSprite();
        }
        public string GetType(int x, int y){
            return gates[x][y].GetSquare;
        }
        public void SetType(int x, int y, string pieceName){
            gates[x][y].SetPiece(pieceName);
        }
        public bool HasConnector(int x, int y, string direction){
            return gates[x][y].HasConnection(direction);
        }
        public void RandomPiece(int x, int y){
            gates[x][y].SetPiece(GatePiece.Types[r.Next(0,GatePiece.MaxIndex+1)]);
        }
        public void DropDown (int x, int y){
            //Get amount to move down
            int dropAmt = y-1;
            while (dropAmt >=0){
                //if piece isn't null / empty then move down
                if (GetType(x,dropAmt) != "Empty"){
                    SetType(x,y,GetType(x,dropAmt));
                    //set moved down to empty/null
                    SetType(x,dropAmt, "Empty");
                    AddFallingPiece(x, y, GetType(x, y),GatePiece.H *(y-dropAmt));
                    //breaks loop if not empty
                    dropAmt = -1;
                }
                dropAmt--;
            }
        }
        public void AddFallingPiece(int x, int y, string PieceName, int VerticalOffset) {
            fallingPieces[new Point(x, y)] = new FallingPiece(PieceName, VerticalOffset);
        }
        public bool ArePiecesAnimating() {
            return fallingPieces.Count != 0;
        }
        public void GenerateNewPieces(bool dropSquares) {
            if (dropSquares) {
                for (int x = 0; x < gates.Length; x++) {
                    for (int y = gates[x].Length - 1; y >= 0; y--) {
                        if (GetType(x, y) == "Empty") {
                            DropDown(x, y);
                        }
                    }
                }
            }

            for (int y = 0; y < BoardHeight; y++) {
                for (int x = 0; x < gates.Length; x++) {
                    if (GetType(x,y) == "Empty") {
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
        public void PropagateWater(int x, int y, string fromDirection) {
            if (y >= 0 && y < BoardHeight && x >= 0 && x < BoardWidth) {
                if (gates[x][y].HasConnection(fromDirection) && !gates[x][y].IsFilled) {
                    FillPiece(x, y);
                    WaterTracker.Add(new Point(x, y));
                    foreach (string end in gates[x][y].GetOtherEnds(fromDirection)) {
                        //determine where opening is, find next opening
                        switch (end) {
                            case "Left": PropagateWater(x - 1, y, "Right");
                                break;
                            case "Right": PropagateWater(x + 1, y, "Left");
                                break;
                                //doesn't take into account hooked pieces?
                            case "Top": PropagateWater(x, y - 1, "Bottom");
                                break;
                            case "Bottom": PropagateWater(x, y + 1, "Top");
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
            PropagateWater(0, y, "Left");
            return WaterTracker;
        }
    }
}
