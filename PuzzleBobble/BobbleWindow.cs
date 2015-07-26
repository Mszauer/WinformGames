using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class BobbleWindow : GameBase{
        PuzzleBobble bobble = null;

        public BobbleWindow() {
            bobble = new PuzzleBobble(new Point(0,0),new Size(8,11));//Offset,Board Size (x*y), hexagonal radius
            width = (int)bobble.BoardArea.W;
            height = (int)bobble.BoardArea.H;
            title = "PuzzleBobble";
        }
        public override void Initialize() {
            bobble.Initialize();
        }
        public override void Update(float dTime) {
            bobble.Update(dTime,KeyDown(Keys.Right), KeyDown(Keys.D),KeyDown(Keys.Left),KeyDown(Keys.A),KeyPressed(Keys.Space), KeyPressed(Keys.R));
        }
        public override void Render(Graphics g) {
            bobble.Render(g);
        }
    }
}
