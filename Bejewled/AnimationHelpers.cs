using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    public class LerpAnimation {
        public delegate void FinishedAnimationCallback(Point cell, int value, LerpAnimation anim);
        public FinishedAnimationCallback OnFinished = null;
        public Point startPos = default(Point);
        public Point endPos = default(Point);
        public Point currentPosition = default(Point);
        public float time = 0f;
        public int cellValue = 0;
        public bool done = false;
        float animationSpeed = 0.375f;

        public LerpAnimation(int value, Point posStart, Point posEnd) {
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
            if (time > animationSpeed) {
                time =  animationSpeed;
                done = true;
                currentPosition.X = (int)Easing.Linear(time, (float)startPos.X, (float)endPos.X, animationSpeed);
                currentPosition.Y = (int)Easing.Linear(time, (float)startPos.Y, (float)endPos.Y, animationSpeed);
                if (OnFinished != null) {
                    OnFinished(currentPosition, cellValue, this);
                }
            }
            currentPosition.X = (int)Easing.Linear(time, (float)startPos.X, (float)endPos.X, animationSpeed);
            currentPosition.Y = (int)Easing.Linear(time, (float)startPos.Y, (float)endPos.Y, animationSpeed);
        }
    }
}
