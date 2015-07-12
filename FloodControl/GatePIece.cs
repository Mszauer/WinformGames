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
        /*public static string[] Types = {"Left,Right", "Top,Bottom",
                                      "Left,Top", "Top,Right","Right,Bottom",
                                      "Bottom,Left", "Empty"};
         */
        public enum Types { LeftRight,TopBottom,LeftTop,TopRight,RightBottom,BottomLeft,Empty}
        public Types Type = Types.Empty;
        public const int H = 40;
        public const int W = 40;
        public const int MaxIndex = 5;
        public const int EmptyIndex = 6;
        private const int textureOffsetY = 1;
        private const int textureOffsetX = 1;
        private const int texturePaddingY = 1;
        private const int texturePaddingX = 1;
        //private string pieceType = "";
        //private string pieceSuffix = "";
        public bool IsFilled = false;
        /*public GetType {
            get {
                return Types.Type;
            }
        }
        public bool ReturnFull {
            get {
                return IsFilled;
            }
        }*/
        public GatePiece(Types type, bool filled) {
            Type = type;
            IsFilled = filled;
        }
        public GatePiece(Types type) {
            Type = type;
            IsFilled = false;
        }
        public void SetPiece(Types type, bool filled) {
            Type = type;
            IsFilled = filled;
        }
        public void SetPiece(bool filled) {
            SetPiece(Type, filled);
        }
        public void AddSuffix(string suffix) {
            //if piece already contains suffix do nothing, else change suffix
            if (!pieceSuffix.Contains(suffix)) {
                pieceSuffix += suffix;
            }
        }
        /* turn it to false
        public void RemoveSuffix(string suffix) {
            //removes passed in suffix
            pieceSuffix = pieceSuffix.Replace(suffix, "");
        }*/
        public void RotatePiece(bool Clockwise) {
            // takes in a piecetype, and rotates the openings (also depends if clockwise or not) 
            // IE openings that open left/right get switched to top/bottom
            switch (Type) {
                case Type = Types.LeftRight:
                    Type = Types.TopBottom;
                    break;
                case Type = Types.TopBottom:
                    Type = Types.LeftRight;
                    break;
                case Type = Types.LeftTop:
                    if (Clockwise) {
                        Type = Types.TopRight;
                    }
                    else {
                        Type = Types.BottomLeft; ;
                    }
                    break;
                case Type = Types.TopRight:
                    if (Clockwise) {
                        Type = Types.RightBottom;
                    }
                    else {
                        Type = Types.LeftTop;
                    }
                    break;
                case Type = Types.RightBottom:
                    if (Clockwise) {
                        Type = Types.BottomLeft;
                    }
                    else {
                        Type = Types.TopRight;
                    }
                    break;
                case Type = Types.BottomLeft:
                    if (Clockwise) {
                        Type = Types.LeftTop;
                    }
                    else {
                        Type = Types.RightBottom;
                    }
                    break;
                case Type = Types.Empty:
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
            if (IsFilled) {
                x += W + texturePaddingX;
            }
            //matches index of type and multiplies it by H+padding
            y += Array.IndexOf(Types, pieceType) * (H + texturePaddingY);
            return new Rect(x, y, W, H);
        }
    }//end class
}
