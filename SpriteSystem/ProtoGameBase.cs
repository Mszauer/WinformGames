using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class ProtoGameBase : GameBase{
        Sprite sprite = null;
        public ProtoGameBase(){
            sprite = new Sprite("Assets\\flappyBat.png");
        }

        public override void Render(Graphics g) {
            sprite.Draw(g, new Rect(new Point(0, 0), new Point(width, height)), new Rect(new Point(0, 0), new Point(285, 510)));
        }
    }
}
