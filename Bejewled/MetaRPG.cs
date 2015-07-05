﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class MetaRPG {
        List<Sprite> rpgBackGround = null;
        Sprite portrait = null;
        Sprite navigation = null;
        Sprite healthBar = null;
        List<Sprite> healthColors = null;
        int bgFrame = 0;
        float health = 1f;
        float timeAccum = 0f;
        float walkFPS = 2f;
        int healthIndexer {
            get {
                if (health < 0.25) {
                    return 0;
                }
                else if (health < 0.5) {
                    return 1;
                }
                return 2;
            }
        }

        public MetaRPG() {
            rpgBackGround = new List<Sprite>();
            healthColors = new List<Sprite>();
        }

        public void Initialize() {
            for (int  i = 1 ; i < 24 ; i++){ //23 is the number of backgrounds
                Sprite bg = new Sprite("Assets/w_" + i + ".png");
                rpgBackGround.Add(bg);
            }
            portrait = new Sprite("Assets/portrait.png");
            navigation = new Sprite("Assets/navigation.png");
            healthBar = new Sprite("Assets/health.png");
            healthColors.Add(new Sprite("Assets/red.png"));
            healthColors.Add(new Sprite("Assets/yellow.png"));
            healthColors.Add(new Sprite("Assets/green.png"));
        }

        public void Update(float dTime) {
            timeAccum += dTime;
            if (walkFPS < timeAccum) {
                timeAccum = 0f;
                bgFrame++;
                if (bgFrame > rpgBackGround.Count - 1) {
                    bgFrame = rpgBackGround.Count - 1;
                }
            }
        }
        public void Render(Graphics g) {
            //Position the HUD
            rpgBackGround[bgFrame].Draw(g, new Point(0, 0));
            portrait.Draw(g, new Point(355, 0));
            navigation.Draw(g, new Point(355, 141));
            healthBar.Draw(g, new Point(355, 124));
            //health colors
            healthColors[healthIndexer].Draw_LeftScale(g, new Point(364, 127), 112f*health);
        }
    }
}
