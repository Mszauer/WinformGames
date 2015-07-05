using System;
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
        List<Sprite> enemies = null;
        Sprite portrait = null;
        Sprite navigation = null;
        Sprite healthBar = null;
        List<Sprite> healthColors = null;
        int bgFrame = 0;
        float playerHealth = 1f;
        float enemyHealth = 1f;
        int enemyHealthIndexer {
            get {
                if (enemyHealth < 0.25) {
                    return 0;
                }
                else if (enemyHealth < 0.5) {
                    return 1;
                }
                return 2;
            }
        }
        float timeAccum = 0f;
        float walkFPS = 2f;
        int playerHealthIndexer {
            get {
                if (playerHealth < 0.25) {
                    return 0;
                }
                else if (playerHealth < 0.5) {
                    return 1;
                }
                return 2;
            }
        }

        public MetaRPG() {
            enemies = new List<Sprite>();
            rpgBackGround = new List<Sprite>();
            healthColors = new List<Sprite>();
        }

        public void Initialize() {
            for (int  i = 1 ; i < 24 ; i++){ //23 is the number of backgrounds
                Sprite bg = new Sprite("Assets/w_" + i + ".png");
                rpgBackGround.Add(bg);
            }
            for (int i = 1; i < 7; i++) {
                enemies.Add(new Sprite("Assets/e" + i + ".png"));
            }
            portrait = new Sprite("Assets/portrait.png");
            navigation = new Sprite("Assets/navigation.png");
            healthBar = new Sprite("Assets/health.png");
            healthColors.Add(new Sprite("Assets/red.png"));
            healthColors.Add(new Sprite("Assets/yellow.png"));
            healthColors.Add(new Sprite("Assets/green.png"));
        }

        public void Update(float dTime) {
            if (enemyHealth <= 0) {
                timeAccum += dTime;
            }
            if (walkFPS < timeAccum) {
                timeAccum = 0f;
                bgFrame++;
                if (bgFrame % 5 == 0 || bgFrame == rpgBackGround.Count-1) {
                    enemyHealth = 1;
                }
                if (bgFrame > rpgBackGround.Count - 1) {
                    bgFrame = rpgBackGround.Count - 1;
                }
            }

        }
        public void DoAttack(int type) {
            Console.WriteLine(type);
            enemyHealth -= 0.2f;
            if (enemyHealth < 0) {
                enemyHealth = 0;
            }
        }
        public void Render(Graphics g) {
            //Position the HUD
            rpgBackGround[bgFrame].Draw(g, new Point(0, 0));
            portrait.Draw(g, new Point(355, 0));
            navigation.Draw(g, new Point(355, 141));
            healthBar.Draw(g, new Point(355, 124));
            //health colors
            healthColors[playerHealthIndexer].Draw_LeftScale(g, new Point(364, 127), 112f*playerHealth);
            //Enemy Health
            if (enemyHealth > 0) {
                if (bgFrame % 5 == 0) {

                    enemies[bgFrame / 5].Draw(g, new Point(354 / 2 - (int)(enemies[bgFrame / 5].W / 2), 220 - (int)enemies[bgFrame / 5].H));
                    healthBar.Draw(g, new Point(113, 197));
                    healthColors[enemyHealthIndexer].Draw_LeftScale(g, new Point(122, 200), 112f * enemyHealth);
                }

                else if (bgFrame == rpgBackGround.Count - 1) {
                    enemies[enemies.Count - 1].Draw(g, new Point(354 / 2 - ((int)enemies[enemies.Count - 1].W / 2), 220 - (int)enemies[enemies.Count - 1].H));
                    healthBar.Draw(g, new Point(113, 197));
                    healthColors[enemyHealthIndexer].Draw_LeftScale(g, new Point(122, 200), 112f * enemyHealth);
                }
            }
        }
    }
}
