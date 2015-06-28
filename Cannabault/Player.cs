using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Player {
        public Rect player = null;
        float startX = 50.0f;// allows room for sprite movement
        Size windowWH = default(Size);
        float velocity = 125.0f; 
        float fallConstant = 300.0f;
        float jumpMax = -350f;
        float jumpMin = -100f;

        float deltaTime = 0f;
        bool canJump = true;
        public float X {
            get {
                return player.X;
            }
            set {
                player.X = value;
            }
        }
        public float W {
            get {
                return player.W;
            }
        }
        public float Y {
            get {
                return player.Y;
            }
            set {
                player.Y = value;
            }
        }

        public Player(Size window) {
            windowWH = window;
        }

        public void Initialize() {
            player = new Rect(new Point((Int32)startX, windowWH.Height / 2-15), new Size(15, 15));
        }
        public void Update(float dTime) {
            deltaTime = dTime;
            player.Y += velocity * dTime; //sets downward force on the player
            velocity += fallConstant * dTime; //goes up, then down as it approaches fallconstant
            if (velocity > fallConstant) { //sets limit to falling speed
                velocity = fallConstant;
            }
        }

        public void StopJump() {
            velocity = 0;
        }

        bool OutOfBounds(Size window) {
            if (player.Y < 0 || player.Y > windowWH.Height) {
                return true;
            }
            if (player.X < 0) {
                return true;
            }
            return false;
        }

        public void InterruptJump() {
            if (velocity < jumpMin) {
                velocity = jumpMin;
            }
        }

        public void Jump() {
            if (canJump) {
                canJump = false;
                velocity = jumpMax;
                Console.WriteLine("Player has jumped");
            }
        }

        public void Land() {
            canJump = true;
            Console.WriteLine("Player has landed");
        }

        public void Render(Graphics g) {
#if DEBUG
            DebugRender(g);
#endif
        }

        void DebugRender(Graphics g) {
            g.FillRectangle(Brushes.Purple, player.Rectangle);
            g.DrawLine(Pens.Black, player.Center.X, player.Center.Y, player.Center.X, player.Center.Y + velocity);
            g.DrawLine(Pens.Red, player.Center.X, player.Center.Y, player.Center.X, player.Center.Y + velocity * deltaTime);
        }
    }
}
