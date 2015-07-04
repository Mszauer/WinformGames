using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    public class EaseAnimation {
        public delegate void FinishedAnimationCallback(Point cell, int value, EaseAnimation anim);
        public FinishedAnimationCallback OnFinished = null;
        public Point startPos = default(Point);
        public Point endPos = default(Point);
        public Point currentPosition = default(Point);
        public float time = 0f;
        public int cellValue = 0;
        public bool done = false;
        public float AnimationSpeed = 0.375f;
        public enum FallStyle { Linear, Bounce };
        public FallStyle FallType = FallStyle.Linear;

        public EaseAnimation(int value, Point posStart, Point posEnd) {
            cellValue = value;
            startPos = new Point(posStart.X, posStart.Y);
            endPos = new Point(posEnd.X, posEnd.Y);
            currentPosition = new Point(startPos.X, startPos.Y);
        }

        public void Update(float dTime) {
            if (done) {
                return;
            }
            time += dTime;
            if (time > AnimationSpeed) {
                time =  AnimationSpeed;
                done = true;
                if (FallType == FallStyle.Linear) {
                    currentPosition.X = (int)Easing.Linear(time, (float)startPos.X, (float)endPos.X, AnimationSpeed);
                    currentPosition.Y = (int)Easing.Linear(time, (float)startPos.Y, (float)endPos.Y, AnimationSpeed);
                }
                else if (FallType == FallStyle.Bounce) {
                    currentPosition.X = (int)Easing.BounceEaseOut(time, (float)startPos.X, (float)endPos.X, AnimationSpeed);
                    currentPosition.Y = (int)Easing.BounceEaseOut(time, (float)startPos.Y, (float)endPos.Y, AnimationSpeed);
                }
                if (OnFinished != null) {
                    OnFinished(currentPosition, cellValue, this);
                }
            }
            if (FallType == FallStyle.Linear) {
                currentPosition.X = (int)Easing.Linear(time, (float)startPos.X, (float)endPos.X, AnimationSpeed);
                currentPosition.Y = (int)Easing.Linear(time, (float)startPos.Y, (float)endPos.Y, AnimationSpeed);
            }
            else if (FallType == FallStyle.Bounce) {
                currentPosition.X = (int)Easing.BounceEaseOut(time, (float)startPos.X, (float)endPos.X, AnimationSpeed);
                currentPosition.Y = (int)Easing.BounceEaseOut(time, (float)startPos.Y, (float)endPos.Y, AnimationSpeed);
            }
        }
    }
}
