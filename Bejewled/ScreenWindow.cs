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
            bejeweled = new Bejewled(8, 50, 0, 0);// 3 + 4th argument determine offset (x/y) in pixels
#else
            bejeweled = new Bejewled(Guid.NewGuid().GetHashCode());
#endif
        }

        public override void Initialize() {
            bejeweled.Initialize(8);
        }

        public override void Update(float deltaTime) {
            bejeweled.Update(deltaTime, LeftMousePressed, MousePosition);
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
            bejeweled.Render(g);
        }
    }
}
