using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Game;
using System.Windows.Forms;

namespace Game {
    class Program {
        static void Main(string[] args) {
            Application.Run(new GameWindow(new Pong()));
        }
    }
}
