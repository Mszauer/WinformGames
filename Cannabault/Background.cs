using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Background {
        Sprite background = null;
        Sprite paralax1 = null;
        Sprite paralax2 = null;
        Size window = default(Size);
        float para1_0X = 0f;
        float para1_1X = 0f;
        float para2_0X = 0f;
        float para2_1X = 0f;

        public Background(Size window) {
            this.window = window;
            para1_1X = window.Width;
            para2_1X = window.Width;
        }

        public void Initialize() {
            background = new Sprite("Assets/layer_0.png");
            paralax1 = new Sprite("Assets/layer_1.png");
            paralax2 = new Sprite("Assets/layer_2.png");
        }

        public void Update(float dTime) {
            para1_0X -= 20.0f * dTime;
            para1_1X -= 20.0f * dTime;
            if (para1_0X <= -paralax1.W) {
                para1_0X = window.Width;
            }
            if (para1_1X <= -paralax1.W) {
                para1_1X = window.Width;
            }
            para2_0X -= 45.0f * dTime;
            para2_1X -= 45.0f * dTime;
            if (para2_0X <= -paralax2.W) {
                para2_0X = window.Width;
            }
            if (para2_1X <= -paralax2.W) {
                para2_1X = window.Width;
            }
        }

        public void Render(Graphics g) {
#if !HIDESPRITE
            background.Draw(g, new Point(0, 0));
            paralax1.Draw(g, new Point((Int32)para1_0X, 0));
            paralax1.Draw(g, new Point((Int32)para1_1X, 0));
            paralax2.Draw(g, new Point((Int32)para2_0X, 0));
            paralax2.Draw(g, new Point((Int32)para2_1X, 0));
#endif

        }

    }
}
