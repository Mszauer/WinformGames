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
        ProtoAnimation animatedSprite = null;
        ProtoAnimation marioRun = null;
        Point spritePosition = default(Point);

        public override void Initialize(){
            sprite = new Sprite("Assets\\flappyBat.png");
            animatedSprite = new ProtoAnimation("Assets/gohanrects.txt", 30f);
            marioRun = new ProtoAnimation("Assets/marioRun.txt",30f);
            spritePosition.X = width - marioRun.spriteSize.Width - 1;
        }

        public override void Update(float deltaTime) {
            animatedSprite.Update(deltaTime);
            marioRun.Update(deltaTime);
            if(spritePosition.X > 0){
                spritePosition.X -= (Int32)(30f * deltaTime);
            }
            
        }

        public override void Render(Graphics g) {
            sprite.Draw(g, new Rect(new Point(0, 0), new Point(width, height)), new Rect(new Point(0, 0), new Point(285, 510)));
            animatedSprite.Render(g, new Point(width / 2, height / 2));
            marioRun.Render(g, new Point(spritePosition.X, height - marioRun.spriteSize.Height));
        }
    }
}
