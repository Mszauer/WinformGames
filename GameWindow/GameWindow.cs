using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Game {
    class GameWindow : Form {
        bool leftMouseDown = false;
        bool rightMouseDown = false;
        bool middleMouseDown = false;
        bool runOnce = true;
        bool frameQueued = false;
        Dictionary<Keys, bool> keysDown = null;
        Image bufferA = null;
        Image bufferB = null;
        Image frontBuffer = null;
        Image backBuffer = null;
        System.DateTime thisTime = default(System.DateTime);
        System.DateTime lastTime = default(System.DateTime);

        GameBase gameInstance = null;

        private void OnGameWindowClosed(object sender, System.EventArgs e) {
            gameInstance.ShutDown();
        }

        public GameWindow(GameBase game) {
            gameInstance = game;


            Text = gameInstance.title;
            Size = new Size(gameInstance.width, gameInstance.height);
            Application.Idle += new EventHandler(OnIdle);
            bufferA = new Bitmap(Width, Height);
            bufferB = new Bitmap(Width, Height);
            gameInstance.width = this.ClientRectangle.Width;
            gameInstance.height = this.ClientRectangle.Height;

            gameInstance.Initialize();

            // new Dictionary here!
            keysDown = new Dictionary<Keys, bool>();
            foreach (Keys key in Enum.GetValues(typeof(Keys))) {
                if (!keysDown.ContainsKey(key)) {
                    keysDown.Add(key, false);
                }
            }

            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.KeyUp += new KeyEventHandler(OnKeyUp);
            this.MouseClick += new MouseEventHandler(OnMouseDown);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseUp += new MouseEventHandler(OnMouseUp);
            //if the mouse leaves the window, reset it's state
            this.MouseLeave += new EventHandler(ResetMouse);
            this.ResizeEnd += new EventHandler(WindowResizeEnd);
            this.FormClosed += new FormClosedEventHandler(OnGameWindowClosed);

            thisTime = System.DateTime.Now;
            lastTime = System.DateTime.Now;

            BufferedPaint();
            BufferedPaint();
            CenterToScreen();

        }

        private void WindowResizeEnd(object sender, EventArgs e) {
            gameInstance.width = this.ClientRectangle.Width;
            gameInstance.height = this.ClientRectangle.Height;
            thisTime = System.DateTime.Now;
            lastTime = System.DateTime.Now;
            BufferedPaint();
        }

        private void BufferedPaint() {
            if (bufferA.Width != Width || bufferA.Height != Height) {
                bufferA = new Bitmap(Width, Height);
            }
            if (bufferB.Width != Width || bufferB.Height != Height) {
                bufferB = new Bitmap(Width, Height);
            }
            if (frontBuffer == bufferA) {
                frontBuffer = bufferB;
                backBuffer = bufferA;
            }
            else {
                frontBuffer = bufferA;
                backBuffer = bufferB;
            }

            using (Graphics g = Graphics.FromImage(backBuffer)) {
                g.FillRectangle(gameInstance.clearColor, 0, 0, Width, Height);

                // This is where the game will do it's painting
                gameInstance.Render(g);
            }

            if (!frameQueued) {
                frameQueued = true;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawImage(frontBuffer, 0, 0);
            g.Dispose();
            frameQueued = false;
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            if (runOnce) {
                e.Graphics.FillRectangle(gameInstance.clearColor, 0, 0, Width, Height);
                runOnce = false;
            }
        }

        void OnKeyDown(object sender, KeyEventArgs e) {
            keysDown[e.KeyCode] = true;
            //Console.WriteLine(e.KeyCode + "is down");
        }

        void OnKeyUp(object sender, KeyEventArgs e) {
            keysDown[e.KeyCode] = false;
            //Console.WriteLine(e.KeyCode + "is up");
        }

        void ResetMouse(object sender, EventArgs e) {
            leftMouseDown = false;
            rightMouseDown = false;
            middleMouseDown = false;
        }

        void OnMouseDown(object sender, MouseEventArgs e) {
            // The == evaluates to true or false
            leftMouseDown = e.Button == MouseButtons.Left;
            middleMouseDown = e.Button == MouseButtons.Middle;
            rightMouseDown = e.Button == MouseButtons.Right;
        }

        void OnMouseUp(object sender, MouseEventArgs e) {
            //This is the long form of what's going in OnMouseDown
            if (e.Button == MouseButtons.Left) {
                leftMouseDown = false;
            }
            if (e.Button == MouseButtons.Right) {
                rightMouseDown = false;
            }
            if (e.Button == MouseButtons.Middle) {
                middleMouseDown = false;
            }
            //Console.WriteLine("Mouse is up: " + e.Button);
        }

        //uint idleCounter = 0;
        void OnIdle(object sender, EventArgs a) {
            while (IsApplicationIdle()) {
                thisTime = System.DateTime.Now;
                System.TimeSpan deltaTime = thisTime - lastTime;
                Rectangle winRect = RectangleToScreen(this.ClientRectangle);
                Point mousePosition = new Point();
                //Get mouse position relative to window
                mousePosition.X = Cursor.Position.X - winRect.X;
                mousePosition.Y = Cursor.Position.Y - winRect.Y;
                //Clamp Mouse position to window coords/size
                mousePosition.X = Math.Max(0, mousePosition.X);
                mousePosition.X = Math.Min(winRect.Width, mousePosition.X);
                mousePosition.Y = Math.Max(0, mousePosition.Y);
                mousePosition.Y = Math.Min(winRect.Height, mousePosition.Y);

                gameInstance.UpdateMouse(mousePosition, leftMouseDown, middleMouseDown, rightMouseDown);
                gameInstance.UpdateKeyboard(keysDown);
                //DebugLog
                //Console.WriteLine("Mouse position: " + Cursor.Position);
                //Console.WriteLine("Processing Idle Event: " + (idleCounter++));
                float fDelta = System.Convert.ToSingle(deltaTime.TotalSeconds);
                gameInstance.Update(fDelta);
                lastTime = thisTime;
                //Update Game Here
                BufferedPaint();
                System.Threading.Thread.Sleep(1);

            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        [DllImport("user32.dll")]
        public static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

        bool IsApplicationIdle() {
            NativeMessage res;
            return PeekMessage(out res, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        }


    }
}
