using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class FloodWindow : GameBase {
        GameBoard gameBoard;
        GatePiece pipe;
        Point gameBoardDisplayOrigin = new Point(70, 89);
        int playerScore = 0;
        enum GameStates { TitleScreen, Playing }
        GameStates gameState = GameStates.TitleScreen;
        Rectangle EmptyPiece = new Rectangle(1, 247, 40, 40);
        Sprite titleScreenSprite = null;
        Sprite gameSprites = null;
        Sprite gameBg = null;

        public FloodWindow() {
            width = 800;
            height = 600;
            titleScreenSprite = new Sprite("Assets/TitleScreen.png");
            gameSprites = new Sprite("Assets/Tile_Sheet.png");
            gameBg = new Sprite("Assets/Background.png");
            Initialize();
        }
        public override void Initialize() {
            gameBoard = new GameBoard();
        }
        public override void Update(float dTime) {
            switch (gameState) {
                case GameStates.TitleScreen:
                    if (KeyPressed(Keys.Space)) {
                        gameBoard.ClearBoard();
                        gameBoard.GenerateNewPieces(false);
                        playerScore = 0;
                        gameState = GameStates.Playing;
                    }
                    break;
                case GameStates.Playing:
                    DoLeftMouseUp(LeftMousePressed,RightMousePressed);

                    gameBoard.ResetWater();
                    for (int y = 0; y < gameBoard.BoardHeight; y++) {
                        CheckScoringChain(gameBoard.GetWaterChain(y));
                    }
                    gameBoard.GenerateNewPieces(true);
                    break;
            }
        }
        private int DetermineScore(int SquareCount) {
            return (int)((Math.Pow((SquareCount / 5), 2) + SquareCount) * 10);
        }
        private void CheckScoringChain(List<Point> WaterChain) {
            if (WaterChain.Count > 0) {
                Point LastPipe = WaterChain[WaterChain.Count - 1];
                if (LastPipe.X == gameBoard.BoardWidth - 1) {
                    playerScore += DetermineScore(WaterChain.Count);
                    foreach (Point ScoringSquare in WaterChain) {
                        gameBoard.SetType(ScoringSquare.X, ScoringSquare.Y, "Empty");
                    }
                }
            }
        }
        private void DoLeftMouseUp(bool LeftMousePressed, bool RightMousePressed) {
            if (!LeftMousePressed && !RightMousePressed) {
                return;
            }
            int x = ((MousePosition.X - gameBoardDisplayOrigin.X) / GatePiece.W);
            int y = ((MousePosition.Y - gameBoardDisplayOrigin.Y) / GatePiece.H);
            if ((x >= 0) && (x < gameBoard.BoardWidth) && (y >= 0) && (y < gameBoard.BoardHeight)) {
                if (LeftMousePressed) {
                    gameBoard.RotatePiece(x, y, false);
                }
                if (RightMousePressed) {
                    gameBoard.RotatePiece(x, y, true);
                }
            }
        }
        public override void Render(Graphics g) {
            if (gameState == GameStates.TitleScreen){
                titleScreenSprite.Draw(g,0,0,width,height);
            }
            else if (gameState == GameStates.Playing){
                gameBg.Draw(g,0,0,width,height);
                for (int x = 0 ; x < gameBoard.BoardWidth ; x++){
                    for (int y=0; y < gameBoard.BoardHeight ; y++){
                        int pixelX = gameBoardDisplayOrigin.X + (x*GatePiece.W);
                        int pixelY = gameBoardDisplayOrigin.Y + (y*GatePiece.H);

                        Rect screen = new Rect(pixelX,pixelY,GatePiece.W,GatePiece.H);
                        Rect texture = gameBoard.GetSubSprite(x,y);
                        Rect empty = new Rect(1,247,40,40);

                        //draw empty cell, background for game piece
                        gameSprites.Draw(g,screen,empty);
                        //draw actual cell
                        gameSprites.Draw(g,screen,texture);
                    }//end y
                }// end x
                g.DrawString("Score: " + System.Convert.ToString(playerScore),new Font("Purisa", 20),Brushes.White,new Point(width/2 - 75,50));
            }//end playing
        }
    }
}
