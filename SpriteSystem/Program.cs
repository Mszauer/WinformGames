﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Game {
    class Program {
        static void Main(string[] args) {
            Application.Run(new GameWindow(new ProtoGameBase()));
        }
    }
}
