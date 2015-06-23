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
        float velocity = 125.0f; // Same as constant for now
        float deltaTime = 0;

        public Player(Size window) {
            playerPosition = new Point(window.Width / 2, window.Height / 2);
        }

        public void Generate() {
            player = new Rect(playerPosition, new Size(50, 50));
        }


        public void Update(float dTime) {
            deltaTime = dTime;
            player.Y += velocity * dTime;
            velocity += fallConstant * dTime;
            if (velocity > fallConstant) {
                velocity = fallConstant;
            }
        }

        public void Jump() {
            velocity = -fallConstant;
            // if (fallRate < -fallConstant)
            //    Don't let fall rate go out of constant bounds.
        }

        public void Draw(Graphics g) {
#if DEBUG
            DebugRender(g);
#endif
        }

        public void DebugRender(Graphics g) {
            g.FillRectangle(Brushes.Green, player.Rectangle);
            g.DrawLine(Pens.Blue, player.Center.X, player.Center.Y,player.Center.X, player.Center.Y + velocity);
            g.DrawLine(Pens.Red, player.Center.X, player.Center.Y, player.Center.X, player.Center.Y + velocity*deltaTime);
        }

    }
}
