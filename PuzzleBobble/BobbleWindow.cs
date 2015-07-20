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
            width = 400;
            height = 600;
            bobble = new PuzzleBobble(new Point(0,0),new Size(8,11));//Offset,Board Size (x*y), hexagonal radius
        }
        public override void Initialize() {
            bobble.Initialize();
        }
        public override void Update(float dTime) {
            bobble.Update(dTime,RightMouseDown, KeyDown(Keys.D),LeftMouseDown,KeyDown(Keys.A),KeyPressed(Keys.Space));
        }
        public override void Render(Graphics g) {
            bobble.Render(g);
        }
    }
}
