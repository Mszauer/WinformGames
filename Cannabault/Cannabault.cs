using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game {
    class Cannabault : GameBase{
        List<Obstacle> buildings = null;
        Player player = null;
        Background background = null;

        public Cannabault() {
            width = 400;
            height = 600;
        }

        public override void Initialize() {
            buildings = new List<Obstacle>();
            background = new Background(new Size(width, height));
            Obstacle bldg1 = new Obstacle(new Size(width, height));
            Obstacle bldg2 = new Obstacle(new Size(width, height));
            Obstacle bldg3 = new Obstacle(new Size(width, height));
            bldg1.lastBuilding = bldg3;
            bldg2.lastBuilding = bldg1;
            bldg3.lastBuilding = bldg2;

            background.Initialize();
            bldg1.Initialize(0);
            bldg2.Initialize(bldg2.lastBuilding.X+bldg2.lastBuilding.W +bldg2.bldgSpacing);
            bldg3.Initialize(bldg3.lastBuilding.X+bldg3.lastBuilding.W+bldg3.bldgSpacing);

            buildings.Add(bldg1);
            buildings.Add(bldg2);
            buildings.Add(bldg3);

            player = new Player(new Size(width,height));
            player.Initialize();
        }

        public override void Update(float dTime) {
            background.Update(dTime);
            if (KeyPressed(Keys.Up) || LeftMouseDown == true) {
                player.Jump();
            }
            if (KeyReleased(Keys.Up) || LeftMouseReleased) {
                player.InterruptJump();
            }
            for (int i = 0; i < buildings.Count; i++) {
                buildings[i].Update(dTime);
            }
                player.Update(dTime);
            Collision();
            
        }

        void Collision() {
            //add collision for top
            for (int i = 0; i < buildings.Count; i++) {
                if (buildings[i].type == Obstacle.ObstacleType.Normal || buildings[i].type == Obstacle.ObstacleType.Closed) {
                    if (buildings[i].building.Intersects(player.player)) {
                        Rect result = player.player.Intersection(buildings[i].building);
                        if (result.Left == buildings[i].building.Left && result.Right == player.player.Right) {
                            player.X -= result.W;
                        }
                        if (result.Top == buildings[i].building.Top && result.Bottom == player.player.Bottom) {
                            player.player.Y -= result.H;
                            player.Land();
                        }

                    }
                }
                if (buildings[i].type == Obstacle.ObstacleType.Closed) {
                    if (buildings[i].topBuilding.Intersects(player.player)) {
                        Rect result2 = player.player.Intersection(buildings[i].topBuilding);
                        if (result2.Bottom == buildings[i].topBuilding.Bottom && result2.Top == player.player.Top) {
                            player.StopJump();
                            player.player.Y += result2.H;
                        }
                        if (result2.Left == buildings[i].topBuilding.Left && result2.Right == player.player.Right) {
                            player.X -= result2.W;
                        }
                    }
                }
                if (buildings[i].type == Obstacle.ObstacleType.Cloud) {
                    Rect result = player.player.Intersection(buildings[i].building);
                    if (result.Top == buildings[i].building.Top && result.Bottom == player.player.Bottom) {
                        player.player.Y -= result.H;
                        player.Land();
                    }
                }
            }
        }

        public override void Render(Graphics g) {
            background.Render(g);
            buildings[0].Render(g,Brushes.Red);
            buildings[1].Render(g, Brushes.Green);
            buildings[2].Render(g, Brushes.Blue);
            player.Render(g);
        }

        void DebugControls() {
#if DEBUG
            
#endif
        }
    }
}
