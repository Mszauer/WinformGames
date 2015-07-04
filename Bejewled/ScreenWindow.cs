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
        BejewledGraphics graphics = null;
        public ScreenWindow() {
            width = 400;
            height = 600;
            
#if DEBUG
            width = 800;
            height = 600;
#endif
            graphics = new BejewledGraphics(50,400,0); //tilesize,xOffset,yOffset
#if UNDO
            bejeweled = new Bejewled(8, 50, 0, 0);// random seed, tilesize, xoffset, yoffset
#else
            bejeweled = new Bejewled(Guid.NewGuid().GetHashCode());
#endif
        }

        public override void Initialize() {
            bejeweled.Initialize(8);
            graphics.Initialize(bejeweled.logicBoard);
            //bejeweled.OnStreak += graphics.DoStreak;
            bejeweled.OnSwap += graphics.DoSwap;
            bejeweled.OnDestroy += graphics.DoDestroy;
            graphics.SetExplosionFinishedCallback(bejeweled.DoExplosion);
            bejeweled.OnFall += graphics.DoFall;
        }

        public override void Update(float deltaTime) {
            graphics.Update(deltaTime);
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
            graphics.Render(g);
        }
    }
}
