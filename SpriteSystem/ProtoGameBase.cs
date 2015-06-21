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
        Sprite background = null;
        FlipBook gohanJump = null;
        ProtoAnimation marioRun = null;
        Point marioPosition = default(Point);
        ProtoAnimation marioStanding = null;
        ProtoAnimation mario = null;
        FlipBook gohanFull = null;

        public override void Initialize(){
            background = new Sprite("Assets\\flappyBat.png");
            gohanJump = FlipBook.LoadCustom("Assets/gohanrects.txt", 30);
            marioStanding = new ProtoAnimation("Assets/marioStanding.txt", 30f);
            marioRun = new ProtoAnimation("Assets/marioRun.txt",30f);
            mario = marioStanding;
            marioPosition.X = width - mario.spriteSize.Width - 1;
            gohanJump.Flip = FlipBook.FlipStyle.Horizontal;
            gohanFull = FlipBook.LoadXML("Assets\\gohanAlpha.xml");
        }

        public override void Update(float deltaTime) {
            gohanJump.Update(deltaTime);
            marioRun.Update(deltaTime);
            gohanFull.Update(deltaTime);
            if (KeyDown(Keys.Left)) {
                if (marioPosition.X > 0) {
                    marioPosition.X -= (Int32)(30f * deltaTime);
                }
                mario = marioRun;
            }

            else {
                mario = marioStanding;
            }

            if (KeyPressed(Keys.D)) {
                gohanJump.Flip = FlipBook.FlipStyle.Horizontal;
            }
            if (KeyPressed(Keys.A)) {
                gohanJump.Flip = FlipBook.FlipStyle.None;
            }
            if (KeyPressed(Keys.W)) {
                gohanJump.Flip = FlipBook.FlipStyle.Vertical;
            }
            if (KeyPressed(Keys.S)) {
                gohanJump.Flip = FlipBook.FlipStyle.Both;
            }
            if (KeyPressed(Keys.D1)) {
                gohanJump.Anchor = FlipBook.AnchorPosition.Center;
            }
            if (KeyPressed(Keys.D2)) {
                gohanJump.Anchor = FlipBook.AnchorPosition.TopLeft;
            }
            if (KeyPressed(Keys.D3)) {
                gohanJump.Anchor = FlipBook.AnchorPosition.BottomMiddle;
            }
        }

        public override void Render(Graphics g) {
            background.Draw(g, new Rect(new Point(0, 0), new Point(width, height)), new Rect(new Point(0, 0), new Point(285, 510))); //background
            gohanJump.Render(g, new Point(width / 2, height / 2)); //gohan
            mario.Render(g, new Point(marioPosition.X, height - mario.spriteSize.Height)); //mario
            gohanFull.Render(g, new Point(0, 0));
        }
    }
}
