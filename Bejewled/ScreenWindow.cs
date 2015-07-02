#define UNDO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class ScreenWindow : GameBase {
        Bejewled bejeweled = null;

        public ScreenWindow() {
            width = 400;
            height = 600;
            
#if DEBUG
            width = 800;
            height = 600;
#endif

#if UNDO
            bejeweled = new Bejewled(8,60);
#else
            bejeweled = new Bejewled(Guid.NewGuid().GetHashCode());
#endif
        }

        public override void Initialize() {
            bejeweled.Initialize(7);
        }

        public override void Update(float deltaTime) {
            bejeweled.Update(deltaTime, LeftMousePressed, MousePosition, 50, 50);// 4 + 5th argument determine offset (x/y) in pixels
#if UNDO
            if (KeyPressed(Keys.U)) {
                bejeweled.PerformUndo();
            }
#endif
#if DEBUG
            if (KeyPressed(Keys.R)) {
                bejeweled.Reset();
            }
#endif
        }
        

        public override void Render(Graphics g) {
            bejeweled.Render(g,50,50); // second + third argument determine offset (x/y) in pixels
        }
    }
}
