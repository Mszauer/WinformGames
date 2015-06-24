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
        Sprite hills = null;
        Sprite bldg1 = null;
        Sprite bldg2 = null;
        Size window = default(Size);
        float background0x = 0;
        float background1x = 0;
        float bldg1_0x = 0;
        float bldg1_1x = 0;
        float bldg2_0x = 0;
        float bldg2_1x = 0;
        float hills_0x = 0;
        float hills_1x = 0;


        public Background(Size window) {
            this.window = window;
            background0x = 0;
            background1x = window.Width;
            bldg1_0x = 0;
            bldg1_1x = window.Width;
            bldg2_0x = 0;
            bldg2_1x = window.Width;
            hills_0x = 0;
            hills_1x = window.Width;
        }

        public void Draw(Graphics g) {
            background.Draw(g, new Point((Int32)background0x, 0));
            background.Draw(g, new Point((Int32)background1x, 0));
            bldg1.Draw(g, new Point((Int32)bldg1_0x, 0));
            bldg1.Draw(g, new Point((Int32)bldg1_1x, 0));
            bldg2.Draw(g, new Point((Int32)bldg2_0x, 0));
            bldg2.Draw(g, new Point((Int32)bldg2_1x, 0));
            hills.Draw(g, new Point((Int32)hills_0x, 0));
            hills.Draw(g, new Point((Int32)hills_1x, 0));
        }

        public void Initialize() {
            background = new Sprite("Assets/paralax0.png");
            hills = new Sprite("Assets/paralax3.png");
            bldg1 = new Sprite("Assets/paralax1.png");
            bldg2 = new Sprite("Assets/paralax2.png");

        }

        public void Update(float dTime) {
            background0x -= 20.0f * dTime;
            background1x -= 20.0f * dTime;
            if (background0x <= -window.Width) {
                background0x = window.Width;
            }
            if (background1x <= -window.Width) {
                background1x = window.Width;
            }
            bldg1_0x -= 30.0f * dTime;
            bldg1_1x -= 30.0f * dTime;
            if (bldg1_0x <= -window.Width) {
                bldg1_0x = window.Width;
            }
            if (bldg1_1x <= -window.Width) {
                bldg1_1x = window.Width;
            } 
            bldg2_0x -= 50.0f * dTime;
            bldg2_1x -= 50.0f * dTime;
            if (bldg2_0x <= -window.Width) {
                bldg2_0x = window.Width;
            }
            if (bldg2_1x <= -window.Width) {
                bldg2_1x = window.Width;
            } 
            hills_0x -= 60.0f * dTime;
            hills_1x -= 60.0f * dTime;
            if (hills_0x <= -window.Width) {
                hills_0x = window.Width;
            }
            if (hills_1x <= -window.Width) {
                hills_1x = window.Width;
            }
        }
    }
}
