using System;
using System.Drawing;

namespace Game {

    class Rect {
        private float mx = 0;
        private float my = 0;
        private float mw = 0;
        private float mh = 0;

        public float X {
            get {
                return mx;
            }
            set {
                mx = value;
            }
        }

        public float Y {
            get {
                return my;
            }
            set {
                my = value;
            }
        }

        public float W {
            get {
                return mw;
            }
            set {
                if (value < 0) {
                    mw = Math.Abs(value);
                    mx = mx - mw;
                }
                else {
                    mw = value;
                }
            }
        }

        public float H {
            get {
                return mh;
            }
            set {
                if (value < 0) {
                    mh = Math.Abs(value);
                    my = my - mh;
                }
                else {
                    mh = value;
                }
            }
        }

        public float Left {
            get {
                return mx;
            }
        }

        public float Top {
            get {
                return my;
            }
        }

        public float Right {
            get {
                return mw + mx;
            }
        }

        public float Bottom {
            get {
                return mh + my;
            }
        }

        public Point Center {
            get {
                return new Point((Int32)(mx+ mw * 0.5f), (Int32)(my + mh * 0.5f));
            }
        }

        public float Area {
            get {
                return mw * mh;
            }
        }

        public override string ToString() {
            return "X: " + X + ", Y: " + Y + ", W: " + W + ", H: " + H;
        }

        public System.Drawing.Rectangle Rectangle {
            get {
                return new Rectangle(FloatClampToInt(mx), FloatClampToInt(my), FloatClampToInt(mw), FloatClampToInt(mh));
            }
        }

        private int FloatClampToInt(float f) {
            if (f < Int32.MaxValue && f > Int32.MinValue) {
                return System.Convert.ToInt32(f);
            }
            else if (f > Int32.MaxValue) {
                return Int32.MaxValue - 1;
            }
            else if (f < Int32.MinValue) {
                return Int32.MinValue + 1;
            }
            return 0;
        }

        public Rect() {
            mx = 0;
            my = 0;
            mw = 0;
            mh = 0;
        }

        public Rect(float x, float y, float w, float h) {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public Rect(Point position, Size size) {
            X = position.X;
            Y = position.Y;
            W = size.Width;
            H = size.Height;
        }

        public Rect(Point topLeft, Point bottomRight) {
            X = topLeft.X;
            Y = topLeft.Y;
            W = bottomRight.X - topLeft.X;
            H = bottomRight.Y - topLeft.Y;
        }

        public Rect(Rect rect) {
            X = rect.X;
            Y = rect.Y;
            W = rect.H;
            H = rect.W;
        }

        public bool Intersects(Rect r2) {
            if(this.Left < r2.Right && this.Right > r2.Left && this.Top < r2.Bottom && this.Bottom > r2.Top){
            //if (this.Contains(r2.Left, r2.Top) || this.Contains(r2.Left, r2.Bottom) || this.Contains(r2.Right, r2.Top) || this.Contains(r2.Right, r2.Bottom)) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool Contains(float x, float y) {
            if (this.Left < x && x < this.Right && this.Top < y && y < this.Bottom) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool Contains(Point p) {
            if (this.Left < p.X && p.X < this.Right && this.Top < p.Y && p.Y < this.Bottom) {
                return true;
            }
            else {
                return false;
            }
        }


        public Rect Intersection(Rect other) {
            if (Intersects(other)) {
                float resultLeft = Math.Max(this.Left, other.Left);
                float resultRight = Math.Min(this.Right, other.Right);
                float resultTop = Math.Max(this.Top, other.Top);
                float resultBottom = Math.Min(this.Bottom, other.Bottom);
                return new Rect(resultLeft, resultTop, resultRight - resultLeft, resultBottom - resultTop);
            }
            else {
                return new Rect(0, 0, 0, 0);
            }
        }
    }
}
