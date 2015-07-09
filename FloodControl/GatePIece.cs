using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class GatePiece {
        public static string[] Types = {"Left, Right", "Top,Bottom",
                                      "Left,Top", "Top,Right","Right,Bottom",
                                      "Bottom,Left", "Empty"};

        public const int H = 40;
        public const int W = 40;
        public const int MaxIndex = 5;
        public const int EmptyIndex = 6;
        private const int textureOffsetY = 1;
        private const int textureOffsetX = 1;
        private const int texturePaddingY = 1;
        private const int texturePaddingX = 1;
        private string pieceType = "";
        private string pieceSuffix = "";
        public string GetType {
            get {
                return pieceType;
            }
        }
        public string GetSuffic {
            get {
                return pieceSuffix;
            }
        }
        public GatePiece(string type, string suffix) {
            pieceType = type;
            pieceSuffix = suffix;
        }
        public GatePiece(string type) {
            pieceType = type;
            pieceSuffix = "";
        }
        public void SetPiece(string type, string suffix) {
            pieceType = type;
            pieceSuffix = suffix;
        }
        public void SetPiece(string type) {
            SetPiece(type, "");
        }
        public void AddSuffix(string suffix) {
            //if piece already contains suffix do nothing, else change suffix
            if (!pieceSuffix.Contains(suffix)) {
                pieceSuffix += suffix;
            }
        }
        public void RemoveSuffix(string suffix) {
            //removes passed in suffix
            pieceSuffix = pieceSuffix.Replace(suffix, "");
        }
        public void RotatePiece(bool Clockwise) {
            // takes in a piecetype, and rotates the openings (also depends if clockwise or not) 
            // IE openings that open left/right get switched to top/bottom
            switch (pieceType) {
                case "Left,Right":
                    pieceType = "Top,Bottom";
                    break;
                case "Top,Bottom":
                    pieceType = "Left,Right";
                    break;
                case "Left,Top":
                    if (Clockwise) {
                        pieceType = "Top,Right";
                    }
                    else {
                        pieceType = "Bottom,Left";
                    }
                    break;
                case "Top,Right":
                    if (Clockwise) {
                        pieceType = "Right,Bottom";
                    }
                    else {
                        pieceType = "Left,Top";
                    }
                    break;
                case "Right,Bottom":
                    if (Clockwise) {
                        pieceType = "Bottom,Left";
                    }
                    else {
                        pieceType = "Top,Right";
                    }
                    break;
                case "Bottom,Left":
                    if (Clockwise) {
                        pieceType = "Left,Top";
                    }
                    else {
                        pieceType = "Right,Bottom";
                    }
                    break;
                case "Empty":
                    break;
            }//end switch
        }//end rotate piece
        public string[] GetOtherEnds(string startingEnd) {
            //create a list and loop through it
            List<string> opposites = new List<string>();
            foreach (string end in pieceType.Split(',')) {
                //returns a list of openings for this piece
                //does not return opening passed into here
                if (end != startingEnd) {
                    opposites.Add(end);
                }
            }
            //convert to an array because the end types is also an array
            return opposites.ToArray();
        }
        public bool HasConnection(string direction){
            //checks to see if openings match
            return pieceType.Contains(direction);
        }
        public Rect SubSprite() {
            //returns part of spritesheet to draw
            int x = textureOffsetX;
            int y = textureOffsetX;

            //if it has a "w" then move add W to x
            if (pieceSuffix.Contains("W")) {
                x += W + texturePaddingX;
            }
            //matches index of type and multiplies it by H+padding
            y += Array.IndexOf(Types, pieceType) * (H + texturePaddingY);
            return new Rect(x, y, W, H);
        }
    }//end class
}
