using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class MetaRPG {
        List<Sprite> rpgBackGround = null;

        public MetaRPG() {
            rpgBackGround = new List<Sprite>();
        }

        public void Initialize() {
            for (int  i = 1 ; i < 24 ; i++){ //23 is the number of backgrounds
                Sprite bg = new Sprite("Assets/w_" + i + ".png");
                rpgBackGround.Add(bg);
            }
        }

        public void Update(float dTime) {

        }
        public void Render(Graphics g) {
            rpgBackGround[0].Draw(g, new Point(0, 0));
        }
    }
}
