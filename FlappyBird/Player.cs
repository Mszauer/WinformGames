﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Player {
        public Rect player = null;
        FlipBook playerSprite = null;
        Point playerPosition = default(Point);
        float fallConstant = 155.0f; 
        float velocity = 125.0f; 
        float deltaTime = 0;
        public float X {
            get {
                return player.X;
            }
        }

        public Player(Size window) {
            playerPosition = new Point(window.Width / 2, window.Height / 2);
            playerSprite = FlipBook.LoadXML("Assets/birdie.xml", 30.0f);
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
            playerSprite.Update(dTime);
        }

        public void Jump() {
            velocity = -fallConstant;
            // if (fallRate < -fallConstant)
            //    Don't let fall rate go out of constant bounds.
        }

        public bool OutOfBounds(Size window) {
            if (player.Y > window.Height || player.Y < 0){
                return true;
            }
            return false;
        }

        public void Draw(Graphics g) {
#if DEBUG
            DebugRender(g);
#endif
            playerSprite.Render(g,new Point((Int32)player.X,(Int32)player.Y));
        }

        public void DebugRender(Graphics g) {
            g.FillRectangle(Brushes.Green, player.Rectangle);
            g.DrawLine(Pens.Blue, player.Center.X, player.Center.Y,player.Center.X, player.Center.Y + velocity);
            g.DrawLine(Pens.Red, player.Center.X, player.Center.Y, player.Center.X, player.Center.Y + velocity*deltaTime);
        }

    }
}
