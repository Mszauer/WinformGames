using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Game {
    class FlipBook {
        List<Rect> subSprites = null;
        Image spriteSheet = null;
        float timeAccum = 0;
        float updateRate = 0;
        int spriteIndex = 0;
        public Size spriteSize {
            get {
                return new Size((Int32)subSprites[spriteIndex].W,
                (Int32)subSprites[spriteIndex].H);

            }
        }
        public enum FlipStyle { None, Horizontal, Vertical, Both}
        public FlipStyle Flip = FlipStyle.None;
        public enum AnchorPosition { TopLeft, Center, BottomMiddle }
        public AnchorPosition Anchor = AnchorPosition.TopLeft;

        public FlipBook(string filePath, float fps) {
            updateRate = 1.0f / fps;
            subSprites = new List<Rect>();
            using (StreamReader sr = new StreamReader(filePath)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    if (line[0] == 'R') {
                        string[] split = line.Split(new Char[] { ' ' });
                        Rect r = new Rect(System.Convert.ToInt32(split[1]), System.Convert.ToInt32(split[2]), System.Convert.ToInt32(split[3]), System.Convert.ToInt32(split[4]));
                        subSprites.Add(r);
                    }
                    else if (line[0] == 'I') {
                        string[] split = line.Split(new Char[] { ' ' });
                        spriteSheet = Image.FromFile(split[1]);
                    }
                }
            }
        }

        public void Update(float dTime) {
            timeAccum += dTime;
            if (timeAccum >= updateRate) {
                spriteIndex++;
                if (spriteIndex >= subSprites.Count) {
                    spriteIndex = 0;

                }
                timeAccum -= updateRate;
            }

        }

        public void Render(Graphics g, Point p) {
            Rect screenRect = new Rect(p, new Size((Int32)subSprites[spriteIndex].W, (Int32)subSprites[spriteIndex].H));
            //g.DrawImage(spriteSheet, r.Rectangle, subSprites[spriteIndex].Rectangle, GraphicsUnit.Pixel);
            int sourceX = 0;
            int sourceY = 0;
            int sourceH= 0;
            int sourceW = 0;
            if (Flip == FlipStyle.None) {
                sourceX = (Int32)subSprites[spriteIndex].X;
                sourceY = (Int32)subSprites[spriteIndex].Y;
                sourceH = (Int32)subSprites[spriteIndex].H;
                sourceW = (Int32)subSprites[spriteIndex].W;
            }
            if (Flip == FlipStyle.Horizontal) {
                sourceX = (Int32)(subSprites[spriteIndex].X + subSprites[spriteIndex].W);
                sourceY = (Int32)subSprites[spriteIndex].Y;
                sourceH = (Int32)subSprites[spriteIndex].H;
                sourceW = -(Int32)subSprites[spriteIndex].W;
            }
            if (Flip == FlipStyle.Vertical) {
                sourceX = (Int32)subSprites[spriteIndex].X;
                sourceY = (Int32)(subSprites[spriteIndex].Y+subSprites[spriteIndex].H);
                sourceH = -(Int32)subSprites[spriteIndex].H;
                sourceW = (Int32)subSprites[spriteIndex].W;
            }
            if (Flip == FlipStyle.Both) {
                sourceX = (Int32)(subSprites[spriteIndex].X + subSprites[spriteIndex].W);
                sourceY = (Int32)(subSprites[spriteIndex].Y + subSprites[spriteIndex].H);
                sourceH = -(Int32)subSprites[spriteIndex].H;
                sourceW = -(Int32)subSprites[spriteIndex].W;
            }

            if (Anchor == AnchorPosition.Center) {
                screenRect.X -= screenRect.W / 2f;
                screenRect.Y -= screenRect.H / 2f;
            }

            if (Anchor == AnchorPosition.BottomMiddle) {
                screenRect.X -= screenRect.W / 2f;
                screenRect.Y -= screenRect.H;
            }
            g.DrawImage(spriteSheet, screenRect.Rectangle, sourceX,sourceY,sourceW,sourceH, GraphicsUnit.Pixel);


        }
    }
}
