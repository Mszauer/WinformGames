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
        bool hp0 = false;
        bool noMoves = false;
        bool won = false;
        Sprite gameOverNoHealth = null;
        Sprite gameOverNoMoves = null;
        Sprite win = null;
        public ScreenWindow() {
            mockRPG = new MetaRPG();
            this.clearColor = Brushes.Black;
            background = new Sprite("Assets/background.png");
            gameOverNoHealth = new Sprite("Assets/gameover_no_health.png");
            gameOverNoMoves = new Sprite("Assets/gameover_no_move.png");
            win = new Sprite("Assets/win.png");
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
            bejeweled.OnNoMoves += this.DoNoMoves;
            mockRPG.OnWin += this.DoWin;
            bejeweled.OnSwap += graphics.DoSwap;
            bejeweled.OnDestroy += graphics.DoDestroy;
            bejeweled.OnDestroy += mockRPG.DoVisualAttack;
            graphics.SetExplosionFinishedCallback(bejeweled.TriggerAnimFinished);
            bejeweled.OnFall += graphics.DoFall;
            graphics.SetExplosionFinishedCallback(mockRPG.DoAttack);
            mockRPG.OnDeath += DoHPDeath;
        }

        public override void Update(float deltaTime) {
            if (hp0 || noMoves || won) {
                graphics = null;
                bejeweled = null;
                mockRPG = null;
                if (LeftMouseReleased) {
                    mockRPG = new MetaRPG();
                    graphics = new BejewledGraphics(60, 3, 223); //tilesize,xOffset,yOffset
                    bejeweled = new Bejewled(Guid.NewGuid().GetHashCode(), 60, 3, 223);

                    hp0 = false;
                    noMoves = false;
                    won = false;
                    Initialize();
                }
            }
            else {
                graphics.Update(deltaTime);
                bejeweled.Update(deltaTime, LeftMousePressed, MousePosition);
                mockRPG.Update(deltaTime);
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
        }

        public void DoNoMoves() {
            noMoves = true;
        }
            

        public void DoHPDeath() {
            hp0 = true;
        }

        public void DoWin() {
            won = true;
        }
        

        public override void Render(Graphics g) {
            if (hp0) {
                gameOverNoHealth.Draw(g, new Point(width / 2 - (int)gameOverNoHealth.W / 2, height / 2 - (int)gameOverNoHealth.H / 2));
            }
            else if (noMoves) {
                gameOverNoMoves.Draw(g, new Point(width / 2 - (int)gameOverNoMoves.W / 2, height / 2 - (int)gameOverNoMoves.H / 2));
            }
            else if (won) {
                win.Draw(g, new Point(width / 2 - (int)win.W / 2, height / 2 - (int)win.H / 2));
            }
            else {
                mockRPG.Render(g);
                background.Draw(g, new Point(0, 220));
                bejeweled.Render(g);
                graphics.Render(g);
            }
            

        }
    }
}
