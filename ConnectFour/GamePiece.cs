using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class GamePiece  {
        Sprite graphic = null;
        Rect r = null;
        int tileSize = 0;
        float xOffset = 0f;
        float yOffset = 0f;

        public GamePiece(int tileSize, float xOffset, float yOffset) {
            this.tileSize = tileSize;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }
        public void Initialize() {
            //TODO
            r = new Rect()
        }
        public void Update(float deltaTime) {

        }
        public void Render(Graphics g) {
            g.FillRectangle(Brushes.Red, r.Rectangle);
        }
        public void DoPlace(int xIndexer, int yIndexer) {

        }
    }
}
