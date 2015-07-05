using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Game {
    class FlipBook {
        public delegate void AnimationFinishedCallback(int data);
        public AnimationFinishedCallback AnimationFinished = null;
        List<Rect> subSprites = null;
        Image spriteSheet = null;
        float timeAccum = 0;
        float updateRate = 0;
        int spriteIndex = 0;
        int extraData = 0;
        public Size spriteSize {
            get {
                return new Size((Int32)subSprites[spriteIndex].W,
                (Int32)subSprites[spriteIndex].H);

            }
        }
        public float W {
            get {
                return subSprites[spriteIndex].W;
            }
        }
        public float H {
            get {
                return subSprites[spriteIndex].H;
            }
        }
        public enum FlipStyle { None, Horizontal, Vertical, Both}
        public FlipStyle Flip = FlipStyle.None;
        public enum AnchorPosition { TopLeft, Center, BottomMiddle }
        public AnchorPosition Anchor = AnchorPosition.TopLeft;
        public enum PlaybackStyle { Single, Loop}
        public PlaybackStyle Playback = PlaybackStyle.Loop;
        bool didFinish = false;

        protected FlipBook() {

        }

        public static FlipBook LoadXML(string filePath, float fps = 30f) {
            //https://msdn.microsoft.com/en-us/library/cc189056(v=vs.95).aspx
            FlipBook result = new FlipBook();
            result.updateRate = 1f / fps;
            result.subSprites = new List<Rect>();
            float sourceX = 0;
            float sourceY = 0;
            float sourceW = 0;
            float sourceH = 0;
            using (XmlReader reader = XmlReader.Create(new StreamReader(filePath))) {
                while (reader.Read()) {
                    if (reader.NodeType == XmlNodeType.Element) {
                        if (reader.Name == "Texture") {
                            reader.MoveToFirstAttribute();
                            string texturePath = reader.Value;
                            result.spriteSheet = Image.FromFile(texturePath); // was texture path
                        }
                        if (reader.Name == "X") {
                            reader.Read();
                            sourceX = System.Convert.ToInt32(reader.Value);
                        }
                        if (reader.Name == "Y") {
                            reader.Read();
                            sourceY = System.Convert.ToInt32(reader.Value);
                        }
                        if (reader.Name == "Width") {
                            reader.Read();
                            sourceW = System.Convert.ToInt32(reader.Value);
                        }
                        if (reader.Name == "Height") {
                            reader.Read();
                            sourceH = System.Convert.ToInt32(reader.Value);
                            Rect r = new Rect(sourceX, sourceY, sourceW, sourceH);
                            result.subSprites.Add(r);
                        }
                    }
                }
            }
            
            return result;
        }

        public static FlipBook LoadCustom(string filePath, float fps = 30f) {
            FlipBook result = new FlipBook();
            result.updateRate = 1.0f / fps;
            result.subSprites = new List<Rect>();
            using (StreamReader sr = new StreamReader(filePath)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    if (line[0] == 'R') {
                        string[] split = line.Split(new Char[] { ' ' });
                        Rect r = new Rect(System.Convert.ToInt32(split[1]), System.Convert.ToInt32(split[2]), System.Convert.ToInt32(split[3]), System.Convert.ToInt32(split[4]));
                        result.subSprites.Add(r);
                    }
                    else if (line[0] == 'I') {
                        string[] split = line.Split(new Char[] { ' ' });
                        result.spriteSheet = Image.FromFile(split[1]);
                    }
                }
            }
            return result;
        }

        public void Update(float dTime) {
            timeAccum += dTime;
            if (timeAccum >= updateRate) {
                spriteIndex++;
                if (spriteIndex >= subSprites.Count) {
                    if (Playback == PlaybackStyle.Loop) {
                        spriteIndex = 0;
                        if (AnimationFinished != null) {
                            AnimationFinished(extraData);
                        }
                    }
                    else if (Playback == PlaybackStyle.Single){
                        spriteIndex = subSprites.Count - 1;
                        if (!didFinish) {
                            if (AnimationFinished != null) {
                                didFinish = true;                        
                                AnimationFinished(extraData);
                            }
                            else {
                                didFinish = true;
                            }
                        }
                    }
                }
                timeAccum -= updateRate;
            }
        }

        public void Reset(int ed) {
            extraData = ed;
            spriteIndex = 0;
            didFinish = false;
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
