using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Game {
    class GameBase {
        protected bool lastLeftMouseDown = false;
        protected bool lastRightMouseDown = false;
        protected bool lastMiddleMouseDown = false;
        protected Point lastMousePosition = default(Point);
        protected Dictionary<Keys, bool> lastKeysDown = null;

        protected bool currentLeftMouseDown = false;
        protected bool currentRightMouseDown = false;
        protected bool currentMiddleMouseDown = false;
        protected Point currentMousePosition = default(Point);
        protected Dictionary<Keys, bool> currentKeysDown = null;
        public string title = "Game";
        public int width = 800;
        public int height = 600;
        public Brush clearColor = Brushes.LightBlue;


        public GameBase() {
            lastKeysDown = new Dictionary<Keys, bool>();
            currentKeysDown = new Dictionary<Keys, bool>();
            foreach (Keys key in Enum.GetValues(typeof(Keys))) {
                if (!lastKeysDown.ContainsKey(key)) {
                    lastKeysDown.Add(key, false);
                }
                if (!currentKeysDown.ContainsKey(key)) {
                    currentKeysDown.Add(key, false);
                }
            }
            lastMousePosition = new Point(0, 0);
            currentMousePosition = new Point(0, 0);
        }

        public Point MousePosition {
            get {
                return new Point(currentMousePosition.X, currentMousePosition.Y);
            }
        }

        //Get how much the mouse moved between frames
        public Point MousePositionData {
            get {
                return new Point(

                    currentMousePosition.X - lastMousePosition.X,
                    currentMousePosition.Y - lastMousePosition.Y);
            }
        }

        //Will be true so long as the left mouse button is down
        public bool LeftMouseDown {
            get {
                return currentLeftMouseDown;
            }
        }

        //will be true so long as the left mouse button is up
        public bool LeftMouseUp {
            get {
                return currentLeftMouseDown;
            }
        }

        //will be true only the first frame that the left mosue button is down
        public bool LeftMousePressed {
            get { // currently down, was up last frame
                return (currentLeftMouseDown) && (!lastLeftMouseDown);
            }
        }

        public bool LeftMouseReleased {
            get { // currently up, was down last frame
                return (!currentLeftMouseDown) && (lastLeftMouseDown);
            }
        }

        public bool MiddleMouseDown {
            get {
                return currentMiddleMouseDown;
            }
        }

        public bool MiddleMouseUp {
            get {
                return !currentMiddleMouseDown;
            }
        }

        public bool MiddleMousePressed {
            get {
                return (currentMiddleMouseDown) && (!lastMiddleMouseDown);
            }
        }

        public bool MiddleMouseReleased {
            get {
                return (!currentMiddleMouseDown) && (lastMiddleMouseDown);
            }
        }

        public bool RightMouseDown {
            get {
                return currentRightMouseDown;
            }
        }

        public bool RightMouseUp {
            get {
                return currentRightMouseDown;
            }
        }

        public bool RightMousePressed {
            get {
                return (currentRightMouseDown) && (!lastRightMouseDown);
            }
        }

        public bool RightMouseReleased {
            get {
                return (!currentRightMouseDown) && (lastRightMouseDown);
            }
        }

        public bool KeyDown(Keys key) {
            return currentKeysDown[key];
        }

        public bool KeyUp(Keys key) {
            return !currentKeysDown[key];
        }

        public bool KeyPressed(Keys key) {
            return (currentKeysDown[key]) && (!lastKeysDown[key]);
        }

        public bool KeyReleased(Keys key) {
            return (!currentKeysDown[key]) && (lastKeysDown[key]);
        }

        public void UpdateMouse(Point position, bool leftDown, bool middleDown, bool rightDown) {
            //First, buffer the current values into previous frames
            lastMousePosition.X = currentMousePosition.X;
            lastMousePosition.Y = currentMousePosition.Y;
            lastLeftMouseDown = currentLeftMouseDown;
            lastRightMouseDown = currentRightMouseDown;
            lastMiddleMouseDown = currentMiddleMouseDown;

            //then update the current mosue with new data
            currentMousePosition.X = position.X;
            currentMousePosition.Y = position.Y;
            currentLeftMouseDown = leftDown;
            currentRightMouseDown = rightDown;
            currentMiddleMouseDown = middleDown;
        }

        public void UpdateKeyboard(Dictionary<Keys, bool> keyboard) {
            //First, buffer current values into previous frame
            foreach (KeyValuePair<Keys, bool> kvp in currentKeysDown) {
                lastKeysDown[kvp.Key] = kvp.Value;
            }
            //then update the current keyboard with new data
            foreach (KeyValuePair<Keys, bool> kvp in keyboard) {
                if (kvp.Value) {
                    //Console.WriteLine("PRESS" + kvp.Key);
                }
                currentKeysDown[kvp.Key] = kvp.Value;
            }
        }

        public virtual void Initialize() {

        }

        public virtual void Update(float deltaTime) {

        }

        public virtual void Render(Graphics g) {

        }

        public virtual void ShutDown() {

        }
    }
}
