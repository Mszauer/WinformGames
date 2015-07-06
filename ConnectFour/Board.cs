using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class LogicBoard {
        public delegate void OnPlaceCallBack (int xIndexer, int yIndexer);
        public OnPlaceCallBack OnPlace = null;
        enum BoardState { Idle, Place}
        BoardState CurrentBoardState = BoardState.Idle;
        List<Rect> pieces = null;
        int[][] logicBoard = null;
        int tileSize = 0;
        int xIndexer = 0;
        int yIndexer = 0;
        float xOffset = 0f;
        float yOffset = 0f;

        public LogicBoard(int tileSize = 50, float xOffset, float yOffset) {
            this.tileSize = tileSize;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            pieces = new List<Rect>();
        }
        public void Initialize() {
            //initialize logic board,default values of 0
            logicBoard = new int[8][];
            for (int i = 0; i < logicBoard.Length; i++) {
                logicBoard[i] = new int[8];
            }
            

        }
        public void Update(float deltaTime,bool LeftMousePressed, Point MousePosition) {
            if (CurrentBoardState == BoardState.Idle) {
                if (LeftMousePressed) {
                    //Get the x/y indexers
                    xIndexer = MousePosition.X / tileSize;
                    xIndexer = xIndexer < logicBoard.Length ? (MousePosition.X / tileSize) : -1;
                    yIndexer = MousePosition.Y / tileSize;
                    yIndexer = yIndexer < logicBoard[0].Length ? (MousePosition.Y / tileSize) : -1;
                    //Place gamepiece object
                    if (OnPlace != null) {
                        OnPlace(xIndexer, yIndexer);
                    }
                    //pieces.Add(new Rect(new Point(xIndexer * tileSize, yIndexer * tileSize), new Size(tileSize, tileSize)));
                    //CurrentBoardState = BoardState.Place;
                }
            }

        }


        public void Render(Graphics g,int xOffset = 0, int yOffset = 0) {
            //visually draw logic board
            using (Pen p = new Pen(Brushes.Green, 1.0f)) {
                for (int col = 0; col < logicBoard.Length; col++) {
                    g.DrawLine(p, new Point(col * tileSize + xOffset, yOffset), new Point(col * tileSize + xOffset, logicBoard.Length * tileSize + yOffset));
                    for (int row = 0; row < logicBoard[col].Length; row++) {
                        g.DrawLine(p, new Point(xOffset, row * tileSize + yOffset), new Point(400+xOffset, row * tileSize + yOffset));
                    }
                }
            }
            //Draw placed pieces
            foreach (Rect r in pieces) {
                g.FillRectangle(Brushes.Red, r.Rectangle);
            }
        }
    }
}
