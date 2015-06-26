using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Cannabault : GameBase{
        Obstacle bldg1 = null;
        Obstacle bldg2 = null;
        Obstacle bldg3 = null;
        

        public Cannabault() {
            width = 400;
            height = 600;
        }

        public override void Initialize() {
            bldg1 = new Obstacle(new Size(width, height));
            bldg2 = new Obstacle(new Size(width, height));
            bldg3 = new Obstacle(new Size(width, height));
            bldg1.lastBuilding = bldg3;
            bldg2.lastBuilding = bldg1;
            bldg3.lastBuilding = bldg2;

            bldg1.Initialize(0);
            bldg2.Initialize(bldg2.lastBuilding.X+bldg2.lastBuilding.W +bldg2.bldgSpacing);
            bldg3.Initialize(bldg3.lastBuilding.X+bldg3.lastBuilding.W+bldg3.bldgSpacing);
        }

        public override void Update(float dTime) {
            bldg1.Update(dTime);
            bldg2.Update(dTime);
            bldg3.Update(dTime);
        }

        public override void Render(Graphics g) {
            bldg1.Render(g,Brushes.Red);
            bldg2.Render(g, Brushes.Green);
            bldg3.Render(g, Brushes.Blue);
        }

        void DebugControls() {
            if (KeyPressed(Keys.R)) {
                bldg1.Initialize(0);
            }
        }
    }
}
