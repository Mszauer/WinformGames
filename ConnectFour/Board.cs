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
        List<Brush> cellColors = new List<Brush>() {Brushes.Black, Brushes.Red,Brushes.Blue};
        int[][] logicBoard = null;
        int tileSize = 0;
        int xIndexer = 0;
        int yIndexer = 0;
        float xOffset = 0f;
        float yOffset = 0f;

        public LogicBoard(int tileSize, float xOffset, float yOffset) {
            this.tileSize = tileSize;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
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
                    //Set cell values of clicked to 1
                    if (logicBoard[xIndexer][yIndexer] >= 0) {
                        logicBoard[xIndexer][yIndexer] = 1;
                    }
                    //pieces.Add(new Rect(new Point(xIndexer * tileSize, yIndexer * tileSize), new Size(tileSize, tileSize)));
                    //CurrentBoardState = BoardState.Place;
                }
            }

        }


        public void Render(Graphics g) {
            //visually draw logic board
            using (Pen p = new Pen(Brushes.Green, 1.0f)) {
                for (int col = 0; col < logicBoard.Length; col++) {
                    g.DrawLine(p, new Point(col * tileSize + (int)xOffset, (int)yOffset), new Point(col * tileSize + (int)xOffset, logicBoard.Length * tileSize + (int)yOffset));
                    for (int row = 0; row < logicBoard[col].Length; row++) {
                        g.DrawLine(p, new Point((int)xOffset, row * tileSize + (int)yOffset), new Point(400 + (int)xOffset, row * tileSize + (int)yOffset));
                    }
                }
            }
            //Fills cells with color depending on value
            for (int col = 0; col < logicBoard.Length; col++) {
                for (int row = 0; row < logicBoard[col].Length; row++) {
                    if (logicBoard[col][row] > -1) {
                        Rect r = new Rect(new Point(col * tileSize + (int)xOffset, row * tileSize + (int)yOffset), new Size(tileSize, tileSize));
                        g.FillRectangle(cellColors[logicBoard[col][row]], r.Rectangle);
                    }
                }
            }
        }
    }
}
