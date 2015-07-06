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
        public delegate void OnDeathCallBack();
        public OnDeathCallBack OnDeath = null;
        public delegate void OnWinCallback();
        public OnWinCallback OnWin = null;
        List<Sprite> rpgBackGround = null;
        List<Sprite> enemies = null;
        List<Sprite> minimap = null;
        Sprite portrait = null;
        Sprite minimapBG = null;
        Sprite healthBar = null;
        List<Sprite> healthColors = null;
        FlipBook attack = null;
        FlipBook idle = null;
        Random r = null;
        int bgFrame = 0;
        float playerHealth = 1f;
        float enemyHealth = 1f;
        float attackTimer = 0f;
        float enemyTimer = 0f;
        float shakeTimer = 0f;
        float attackInterval = 3.0f;
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
            minimap = new List<Sprite>();
            enemies = new List<Sprite>();
            rpgBackGround = new List<Sprite>();
            healthColors = new List<Sprite>();
            r = new Random();
        }

        public void Initialize() {
            for (int  i = 1 ; i < 24 ; i++){ //23 is the number of backgrounds
                Sprite bg = new Sprite("Assets/w_" + i + ".png");
                rpgBackGround.Add(bg);
                minimap.Add(new Sprite("Assets/m_" + i + ".png"));
            }
            for (int i = 1; i < 7; i++) {
                enemies.Add(new Sprite("Assets/e" + i + ".png"));
            }
            attack = FlipBook.LoadCustom("Assets/attack.txt");
            idle = FlipBook.LoadCustom("Assets/idle.txt",1f);
            portrait = new Sprite("Assets/portrait.png");
            minimapBG = new Sprite("Assets/map_bg.png");
            healthBar = new Sprite("Assets/health.png");
            healthColors.Add(new Sprite("Assets/red.png"));
            healthColors.Add(new Sprite("Assets/yellow.png"));
            healthColors.Add(new Sprite("Assets/green.png"));

            attack.Anchor = FlipBook.AnchorPosition.BottomMiddle;
            idle.Anchor = FlipBook.AnchorPosition.BottomMiddle;

        }

        public void DoVisualAttack(List<Point> streakPos, int type) {
            if (type != 3) {
                attackTimer = 1.0f;
            }
        }

        public void Update(float dTime) {
            idle.Update(dTime);
            attack.Update(dTime);
            attackTimer -= dTime;
            shakeTimer -= dTime;
            if (shakeTimer < 0) {
                shakeTimer = 0;
            }
            if (enemyHealth > 0) {
                enemyTimer += dTime;
            }
            else {
                enemyTimer = 0;
            }
            if (enemyTimer > attackInterval) {
                enemyTimer = 0;
                playerHealth -= 0.05f;
                shakeTimer = 0.5f;
            }
            
            if (attackTimer < 0) {
                attackTimer = 0;
            }
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
            if (playerHealth <= 0) {
                OnDeath();
            }

        }
        public void DoAttack(int type) {
            if (type != 3) {
                enemyHealth -= 0.26f;
#if DEBUG
                enemyHealth -= 1.0f;
#endif
            }
            else if (type == 3) {
                playerHealth += 0.2f;
                if (playerHealth >= 1f) {
                    playerHealth = 1f;
                }
            }
            if (enemyHealth <= 0) {
                enemyHealth = 0;
            }
        }
        public void Render(Graphics g) {
            //Position the HUD
            if (shakeTimer > 0) {
                rpgBackGround[bgFrame].Draw(g, new Point(r.Next(-2,3), r.Next(-2,3)));
            }
            else {
                rpgBackGround[bgFrame].Draw(g, new Point(0, 0));
            }
            portrait.Draw(g, new Point(355, 0));
            minimapBG.Draw(g, new Point(355, 143));
            minimap[bgFrame].Draw(g, new Point(375, 154));
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
            else {
                if (bgFrame == rpgBackGround.Count - 1) {
                    OnWin();
                }
            }
            //idle or attack
            if (attackTimer > 0 && enemyHealth > 0) {
                attack.Render(g, new Point(354 / 2, 220));
            }
            else {
                idle.Render(g, new Point(354 / 2, 220));

            }
        }
    }
}
