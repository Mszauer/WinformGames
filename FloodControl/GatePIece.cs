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
        public enum Types { LeftRight, TopBottom, LeftTop, TopRight, RightBottom, BottomLeft, Empty }
        public Types Type = Types.Empty;
        public enum Ends { Left, Right, Bottom, Top, Empty }
        //public Ends Opening = Ends.Empty;
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
        /*public void AddSuffix(string suffix) {
            //if piece already contains suffix do nothing, else change suffix
            if (!pieceSuffix.Contains(suffix)) {
                pieceSuffix += suffix;
            }
        }
         turn it to false
        public void RemoveSuffix(string suffix) {
            //removes passed in suffix
            pieceSuffix = pieceSuffix.Replace(suffix, "");
        }*/
        public static Types RandomInt(int index) {
            if (index == 0) {
                return Types.LeftRight;
            }
            else if (index == 1) {
                return Types.TopBottom;
            }
            else if (index == 2) {
                return Types.LeftTop;
            }
            else if (index == 3) {
                return Types.TopRight;
            }
            else if (index == 4) {
                return Types.RightBottom;
            }
            else if (index == 5) {
                return Types.BottomLeft;
            }
            return Types.Empty;
        }

        public void RotatePiece(bool Clockwise) {
            // takes in a piecetype, and rotates the openings (also depends if clockwise or not) 
            // IE openings that open left/right get switched to top/bottom
            switch (Type) {
                case Types.LeftRight:
                    Type = Types.TopBottom;
                    break;
                case Types.TopBottom:
                    Type = Types.LeftRight;
                    break;
                case Types.LeftTop:
                    if (Clockwise) {
                        Type = Types.TopRight;
                    }
                    else {
                        Type = Types.BottomLeft; ;
                    }
                    break;
                case Types.TopRight:
                    if (Clockwise) {
                        Type = Types.RightBottom;
                    }
                    else {
                        Type = Types.LeftTop;
                    }
                    break;
                case Types.RightBottom:
                    if (Clockwise) {
                        Type = Types.BottomLeft;
                    }
                    else {
                        Type = Types.TopRight;
                    }
                    break;
                case Types.BottomLeft:
                    if (Clockwise) {
                        Type = Types.LeftTop;
                    }
                    else {
                        Type = Types.RightBottom;
                    }
                    break;
                case Types.Empty:
                    break;
            }//end switch
        }//end rotate piece

        public Ends[] GetOtherEnds(Ends startingEnd) {
            List<Ends> result = new List<Ends>();

            // Basically, this returns an array with the connections in the piece
            // So if the piece is top left, this should return Top and Left
            // But it should NOT return whatever startingEnd is.

            if (Type == Types.LeftRight) { // Type is LeftRight, so try to add Left and Right
                if (startingEnd != Ends.Left) { // If the startingEnd arg is not left
                    result.Add(Ends.Left); // Add Left
                }
                if (startingEnd != Ends.Right) { // If the starting End arg is not right
                    result.Add(Ends.Right); // Add Right
                }
            }
            if (Type == Types.TopBottom) {
                if (startingEnd != Ends.Top) {
                    result.Add(Ends.Top);
                }
                if (startingEnd != Ends.Bottom) {
                    result.Add(Ends.Bottom);
                }
            }

            if (Type == Types.LeftTop) {
                if (startingEnd != Ends.Left) {
                    result.Add(Ends.Left);
                }
                if (startingEnd != Ends.Top) {
                    result.Add(Ends.Top);
                }
            }
            if (Type == Types.TopRight) {
                if (startingEnd != Ends.Top) {
                    result.Add(Ends.Top);
                }
                if (startingEnd != Ends.Right) {
                    result.Add(Ends.Right);
                }
            }
            if (Type == Types.RightBottom) {
                if (startingEnd != Ends.Right) {
                    result.Add(Ends.Right);
                }
                if (startingEnd != Ends.Bottom) {
                    result.Add(Ends.Bottom);
                }
            }
            if (Type == Types.BottomLeft) {
                if (startingEnd != Ends.Bottom) {
                    result.Add(Ends.Bottom);
                } // Not else
                if (startingEnd != Ends.Left) {
                    result.Add(Ends.Left);
                }
            }

            return result.ToArray();
        }

        public bool HasConnection(Ends direction) {

            if (Type == Types.LeftRight && (direction == Ends.Left || direction == Ends.Right)) {
                return true;
            }
            if (Type == Types.TopBottom && (direction == Ends.Top || direction == Ends.Bottom)) {
                return true;
            }
            if (Type == Types.LeftTop && (direction == Ends.Left || direction == Ends.Top)) {
                return true;
            }
            if (Type == Types.TopRight && (direction == Ends.Top || direction == Ends.Right)) {
                return true;
            }
            if (Type == Types.RightBottom && (direction == Ends.Right || direction == Ends.Bottom)) {
                return true;
            }
            if (Type == Types.BottomLeft && (direction == Ends.Bottom || direction == Ends.Left)) {
                return true;
            }
            return false;

        }

        private int IndexOf(Types type) {
            if (type == Types.LeftRight) {
                return 0;
            }
            else if (type == Types.TopBottom) {
                return 1;
            }
            else if (type == Types.LeftTop) {
                return 2;
            }
            else if (type == Types.TopRight) {
                return 3;
            }
            else if (type == Types.RightBottom) {
                return 4;
            }
            else if (type == Types.BottomLeft) {
                return 5;
            }
            else if (type == Types.Empty) {
                return 6;
            }

            return -1; // Default
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
            y += IndexOf(Type) * (H + texturePaddingY);
            return new Rect(x, y, W, H);
        }
    }//end class
}
