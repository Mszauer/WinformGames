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
        public void ClearBoard(){
            //create board
            gates = new GatePiece[BoardWidth][];
            for (int col = 0 ; col < gates.Length ; ++col){
               gates[col] = new GatePiece[BoardHeight];
                for (int row = 0 ; row < gates[col].Length ; row++){
                    gates[col][row] = new GatePiece("Empty");
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
                    //breaks loop if not empty
                    dropAmt = -1;
                }
                dropAmt--;
            }
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
                    }
                }
            }
        }//end new pieces
        public void ResetWater() {
            for (int y = 0; y < BoardHeight; y++) {
                for (int x = 0; x < gates.Length; x++) {
                    gates[x][y].RemoveSuffix("W");
                }
            }
        }
        public void FillPiece(int x, int y) {
            gates[x][y].AddSuffix("W");
        }
        public void PropagateWater(int x, int y, string fromDirection) {
            if (y >= 0 && y < BoardHeight && x >= 0 && x < BoardWidth) {
                if (gates[x][y].HasConnection(fromDirection) && !gates[x][y].GetSuffix.Contains("W")) {
                    FillPiece(x, y);
                    WaterTracker.Add(new Point(x, y));
                    foreach (string end in gates[x][y].GetOtherEnds(fromDirection)) {
                        //determine where opening is, find next opening
                        switch (end) {
                            case "Left": PropagateWater(x - 1, y, "Right");
                                break;
                            case "Right": PropagateWater(x + 1, y, "Left");
                                break;
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
