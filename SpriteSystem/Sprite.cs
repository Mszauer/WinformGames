using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Game {
    class Sprite{
        Image spriteSheet = null;

        public Sprite(string filepath) {
            spriteSheet = Image.FromFile(filepath);
        }
        public void Draw(Graphics g,Point p) {
            g.DrawImage(spriteSheet, p);
        }

        public void Draw(Graphics g,Point p, Rect subSprite) {
            Rect r = new Rect(p,new Size((Int32)subSprite.W,(Int32)subSprite.H));
            g.DrawImage(spriteSheet, r.Rectangle, subSprite.Rectangle, GraphicsUnit.Pixel);
        }

        public void Draw(Graphics g, Rect screen, Rect subSprite) {
            g.DrawImage(spriteSheet, screen.Rectangle, subSprite.Rectangle, GraphicsUnit.Pixel);
        }

    }
}
