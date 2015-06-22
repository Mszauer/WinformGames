using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Player {
        Rect player = null;
        FlipBook playerSprite = null;
        Point playerPosition = default(Point);
        float fallConstant = 155.0f; //Jump height?
        float fallRate = 125.0f; // Same as constant for now

        public Player(Size window) {
            playerPosition = new Point(window.Width / 2, window.Height / 2);
        }

        public void Generate() {
            player = new Rect(playerPosition, new Size(50, 50));
        }


        public void Update(float dTime) {
            player.Y += fallRate * dTime;
            fallRate += fallConstant * dTime;
            if (fallRate > fallConstant) {
                fallRate = fallConstant;
            }
        }

        public void Jump() {
            fallRate = -fallConstant;
            // if (fallRate < -fallConstant)
            //    Don't let fall rate go out of constant bounds.
        }

        public void Draw(Graphics g) {
            DebugRender(g);
        }

        public void DebugRender(Graphics g) {
            g.FillRectangle(Brushes.Green, player.Rectangle);
        }

    }
}
