﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Game;

namespace Game {
    class MainClass {
        [STAThread]
        static void Main(string[] args) {
            Application.Run(new GameWindow(new GameBase()));
        }
    }
}
