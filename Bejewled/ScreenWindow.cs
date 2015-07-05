//#define UNDO
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
        Sprite background = null;
        MetaRPG mockRPG = null;
        public ScreenWindow() {
            mockRPG = new MetaRPG();
            this.clearColor = Brushes.Black;
            background = new Sprite("Assets/background.png");
            width = 485;
            height = 705;
            
#if DEBUG
            width = 485;
            height = 705;
#endif
            graphics = new BejewledGraphics(60,3,223); //tilesize,xOffset,yOffset
#if UNDO
            bejeweled = new Bejewled(8, 60, 3, 223);// random seed, tilesize, xoffset, yoffset
#else
            bejeweled = new Bejewled(Guid.NewGuid().GetHashCode(),60,3,223);
#endif
        }

        public override void Initialize() {
            mockRPG.Initialize();
            bejeweled.OnSpawn += graphics.DoSpawn;
            bejeweled.OnSelection += graphics.DoSelection;
            bejeweled.Initialize(8);
            graphics.Initialize(bejeweled.logicBoard);
            //bejeweled.OnStreak += graphics.DoStreak;
            bejeweled.OnSwap += graphics.DoSwap;
            bejeweled.OnDestroy += graphics.DoDestroy;
            graphics.SetExplosionFinishedCallback(bejeweled.TriggerAnimFinished);
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
            mockRPG.Render(g);
            background.Draw(g, new Point(0,220));
            bejeweled.Render(g);
            graphics.Render(g);
        }
    }
}
