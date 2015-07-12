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
        Point gameBoardDisplayOrigin = new Point(70, 89);
        int playerScore = 0;
        enum GameStates { TitleScreen, Playing, GameOver}
        GameStates gameState = GameStates.TitleScreen;
        Rectangle EmptyPiece = new Rectangle(1, 247, 40, 40);
        Sprite titleScreenSprite = null;
        Sprite gameSprites = null;
        Sprite gameBg = null;
        Point gameOverLocation = new Point(200, 260);
        float gameOverTimer = 0;
        const float MaxFloodCounter = 100.0f;
        public float floodCount = 0.0f;
        float timeSinceLastFloodIncrease = 0.0f;
        float timeBetweenFloodIncreases = 1.0f;
        float floodIncreaseAmount = 10;// 0.5f;
        const int MaxWaterHeight = 244;
        const int WaterWidth = 297;
        Point waterOverlayStart = new Point(85, 245);
        Point waterPosition = new Point(478, 338);
        int currentLevel = 0;
        int linesCompletedThisLevel = 0;
        const float floodAccelerationPerLevel = 0.5f;
        Point levelTextPosition = new Point(512, 225);

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
                        floodCount = 0;
                        gameState = GameStates.Playing;
                    }
                    break;
                case GameStates.Playing:
                    if (gameBoard.ArePiecesAnimating()) {
                        gameBoard.UpdateAnimatedPieces(dTime);
                    }
                    else {
                        DoLeftMouseUp(LeftMousePressed, RightMousePressed);

                        gameBoard.ResetWater();
                        for (int y = 0; y < gameBoard.BoardHeight; y++) {
                            CheckScoringChain(gameBoard.GetWaterChain(y));
                        }
                        gameBoard.GenerateNewPieces(true);
                    }
                    timeSinceLastFloodIncrease += dTime;
                    if (timeSinceLastFloodIncrease >= timeBetweenFloodIncreases) {
                        floodCount += floodIncreaseAmount;
                        timeSinceLastFloodIncrease = 0.0f;
                        if (floodCount >= MaxFloodCounter) {
                            gameOverTimer = 8.0f;
                            gameState = GameStates.GameOver;
                        }
                    }

                    break;
                case GameStates.GameOver:
                    gameOverTimer -= dTime;
                    if (gameOverTimer <= 0) {
                        gameState = GameStates.TitleScreen;
                    }
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
                    linesCompletedThisLevel++;
                    //Clamp floodCount to 0 and max flood counter
                    if ((floodCount - (DetermineScore(WaterChain.Count) / 10) > MaxFloodCounter)) {
                        floodCount = 100.0f;
                    }
                    else if ((floodCount - (DetermineScore(WaterChain.Count) / 10) < 0.0f)) {
                        floodCount = 0.0f;
                    }
                    foreach (Point ScoringSquare in WaterChain) {
                        gameBoard.SetType(ScoringSquare.X, ScoringSquare.Y, "Empty");
                    }
                    if (linesCompletedThisLevel >= 10) {
                        StartNewLevel();
                    }
                }
            }
        }

        private void StartNewLevel() {
            currentLevel++;
            floodCount = 0.0f;
            linesCompletedThisLevel = 0;
            floodIncreaseAmount += floodAccelerationPerLevel;
            gameBoard.ClearBoard();
            gameBoard.GenerateNewPieces(false);
        }
        private void Draw(Graphics g) {
            int waterHeight = (int)(MaxWaterHeight * (floodCount / 100));
            Rect screen = new Rect(waterPosition.X, waterPosition.Y + (MaxWaterHeight - waterHeight), WaterWidth, waterHeight);
            Rect texture = new Rect(waterOverlayStart.X, waterOverlayStart.Y + (MaxWaterHeight - waterHeight), WaterWidth, waterHeight);

            gameSprites.Draw(g, screen, texture);
        }
        private void DrawEmptyPiece(Graphics g, int pixelX, int pixelY) {
            Rect empty = new Rect(1, 247, 40, 40);
            Rect screen = new Rect(pixelX, pixelY, GatePiece.W, GatePiece.H);
            gameSprites.Draw(g, screen, empty);
        }
        private void DrawStandardPiece(Graphics g, int x, int y, int pixelX, int pixelY) {
            Rect screen = new Rect(pixelX, pixelY, GatePiece.W, GatePiece.H);
            Rect texture = gameBoard.GetSubSprite(x, y);
            gameSprites.Draw(g, screen, texture);
        }
        private void DrawFallingPiece(Graphics g, int pixelX, int pixelY, Point position) {
            pixelY -= gameBoard.fallingPieces[position].VerticalOffset;

            Rect screen = new Rect(pixelX, pixelY, GatePiece.W, GatePiece.H);
            Rect texture = gameBoard.GetSubSprite(position.X,position.Y);
            gameSprites.Draw(g, screen, texture);
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
                currentLevel = 0;
                floodIncreaseAmount = 0.0f;
                StartNewLevel();
            }
            else if (gameState == GameStates.Playing || gameState == GameStates.GameOver){
                if (gameState == GameStates.GameOver) {
                    g.DrawString("GAME OVER", new Font("Purisia", 20), Brushes.Red, gameOverLocation);
                }
                gameBg.Draw(g,0,0,width,height);
                for (int x = 0 ; x < gameBoard.BoardWidth ; x++){
                    for (int y=0; y < gameBoard.BoardHeight ; y++){
                        int pixelX = gameBoardDisplayOrigin.X + (x*GatePiece.W);
                        int pixelY = gameBoardDisplayOrigin.Y + (y*GatePiece.H);

                        DrawEmptyPiece(g, pixelX, pixelY);
                        Point position = new Point(x, y);

                        if (gameBoard.fallingPieces.ContainsKey(position)) {
                            DrawFallingPiece(g, pixelX, pixelY, position);
                        }
                        else {
                            DrawStandardPiece(g, x, y, pixelX, pixelY);
                        }
                    }//end y
                }// end x
                Draw(g);
                g.DrawString(System.Convert.ToString(playerScore), new Font("Purisa", 20), Brushes.Black, new Point(625, 225));
                g.DrawString(System.Convert.ToString(currentLevel), new Font("Purisa", 20), Brushes.Black, levelTextPosition);
            }//end playing
        }
    }
}
