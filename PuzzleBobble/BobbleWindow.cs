using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class BobbleWindow : GameBase {
        List<Hexagon> Hexagons = null;
        int[][] board = null;
        float hexRadius = 20.0f; // set size of hexagons
        int boardWidth = 5; // # of hexagons on x axis
        int boardHeight = 5; // # of hexagons on y axis

        public BobbleWindow() {
            width = 400;
            height = 600;
            //set up board
            board = new int[boardWidth][];
            for (int i = 0; i < board.Length; i++) {
                board[i] = new int[boardHeight];
            }
        }
        public override void Initialize() {
            Hexagons = new List<Hexagon>();
            for (int x = 0; x < board.Length; x++) {
                for (int y = 0; y < board[x].Length; y++) {
                    Hexagon H = new Hexagon(hexRadius,50f,50f);
                    H.xIndexer = x;
                    H.yIndexer = y;
                    Hexagons.Add(H);
                }
            }
        }
        public override void Update(float deltaTime) {
            base.Update(deltaTime);
        }
        public override void Render(Graphics g) {
            foreach (Hexagon H in Hexagons) {
                H.Draw(g, Pens.Blue);
            }
        }
    }
}
