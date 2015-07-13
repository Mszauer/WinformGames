using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace Game {
    class Program {
        static void Main(string[] args) {
            Application.Run(new GameWindow(new BobbleWindow()));
        }
    }
}
