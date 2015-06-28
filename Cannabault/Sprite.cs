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
        public float H {
            get {
                return spriteSheet.Height;
            }
        }
        public float W {
            get {
                return spriteSheet.Width;
            }
        }

        public Sprite(string filepath) {
            spriteSheet = Image.FromFile(filepath);
        }

        public void Draw(Graphics g, Point p) {
            g.DrawImage(spriteSheet, p.X, p.Y, spriteSheet.Width, spriteSheet.Height);
        }
        public void Draw(Graphics g, float x1, float y1, float w, float h) {
            g.DrawImage(spriteSheet, x1,y1,w,h);
        }

        public void Draw(Graphics g,Point p, Rect subSprite) {
            Rect r = new Rect(p,new Size((Int32)subSprite.W,(Int32)subSprite.H));
            g.DrawImage(spriteSheet, r.Rectangle, subSprite.Rectangle, GraphicsUnit.Pixel);
        }

        public void Draw(Graphics g, Rect screen, Rect texture) { //screen = where to draw, subsprite what to draw
            g.DrawImage(spriteSheet, screen.Rectangle, texture.Rectangle, GraphicsUnit.Pixel);
        }

    }
}
