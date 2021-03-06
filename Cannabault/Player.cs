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
        float startX = 50.0f;// allows room for sprite movement
        Size windowWH = default(Size);
        FlipBook batmanState = null;
        FlipBook batmanIdle = null;
        FlipBook batmanRun = null;
        FlipBook batmanJump = null;
        float velocity = 125.0f; 
        float fallConstant = 300.0f;
        float jumpMax = -350f;
        float jumpMin = -100f;
        public enum BatmanState { Idle, Run}
        public BatmanState currentState = BatmanState.Idle;

        float deltaTime = 0f;
        bool canJump = false;
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
            batmanIdle = FlipBook.LoadCustom("Assets/batmanIdle.txt");
            batmanRun = FlipBook.LoadXML("Assets/newrun.xml", 12);
            batmanJump = FlipBook.LoadCustom("Assets/batmanJump.txt",2);
            batmanRun.Anchor = FlipBook.AnchorPosition.BottomMiddle;
            batmanIdle.Anchor = FlipBook.AnchorPosition.BottomMiddle;
            batmanJump.Anchor = FlipBook.AnchorPosition.BottomMiddle;
            batmanState = batmanIdle;
        }
        public void Update(float dTime) {
            deltaTime = dTime;
            if (currentState == BatmanState.Idle) {
                batmanState = batmanIdle;
            }
            else if (currentState == BatmanState.Run) {
                batmanState = batmanRun;
            }
            if (!canJump) {
                batmanState = batmanJump;
            }
            batmanState.Update(dTime);
            player.Y += velocity * dTime; //sets downward force on the player
            if (canJump) {
                if (player.X < windowWH.Width / 2) {
                    player.X += 10.0f * dTime;
                }
            }
            velocity += fallConstant * dTime; //goes up, then down as it approaches fallconstant
            if (velocity > fallConstant) { //sets limit to falling speed
                velocity = fallConstant;
            }
        }

        public void StopJump() {
            velocity = 0;
        }

        public bool OutOfBounds(Size window) {
            if (player.Y > windowWH.Height) {
                return true;
            }
            if (player.X+player.W < 0) {
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
#if !HIDESPRITE
            batmanState.Render(g, new Point((Int32)player.X + (Int32)player.W/2, (Int32)player.Y+(Int32)player.H));
#endif
        }

        void DebugRender(Graphics g) {
            g.FillRectangle(Brushes.Purple, player.Rectangle);
            g.DrawLine(Pens.Black, player.Center.X, player.Center.Y, player.Center.X, player.Center.Y + velocity);
            g.DrawLine(Pens.Red, player.Center.X, player.Center.Y, player.Center.X, player.Center.Y + velocity * deltaTime);
        }
    }
}
