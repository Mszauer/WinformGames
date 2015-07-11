using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class FallingPiece : GatePiece{
        public int VerticalOffset;
        public static int fallRate = 5;

        public FallingPiece(string pieceType,int verticalOffset) : base(pieceType) {
        VerticalOffset = verticalOffset;
        }
        

        public void UpdatePiece(float dTime) {
            VerticalOffset = Math.Max(0, (int)((float)VerticalOffset - fallRate * dTime));
        }
    }
}
