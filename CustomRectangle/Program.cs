using System;
using Game;
using System.Drawing;

namespace CustomRectangle {
    class Program {

        static void Wait() {
            Console.WriteLine("");
            Console.ReadKey();
        }

        static void Main(string[] args) {
            // Constructor: default
            {
                Rect def = new Rect();
                if (def.X != 0 || def.Y != 0 || def.W != 0 || def.H != 0) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect();");
                    Console.WriteLine("def.X != 0 || def.Y != 0 || def.W != 0 || def.H != 0");
                    Console.WriteLine(def.X + " != 0 || " + def.Y + " != 0 || " + def.W + " != 0 || " + def.H + " != 0");
                    Wait();
                    return;
                }
                if (def.Left != 0 || def.Top != 0 || def.Right != 0 || def.Bottom != 0) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect();");
                    Console.WriteLine("def.Left != 0 || def.Top != 0 || def.Right != 0 || def.Bottom != 0");
                    Console.WriteLine(def.Left + " != 0 || " + def.Top + " != 0 || " + def.Right + " != 0 || " + def.Bottom + " != 0");
                    Wait();
                    return;
                }
                if (def.Area != 0) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect();");
                    Console.WriteLine("def.Area != 0");
                    Console.WriteLine(def.Area + " != 0");
                    Wait();
                    return;
                }
            }

            // Constructor: int x, int y, int w, int h
            {
                Rect def = new Rect(-20, 6, 4, 7);
                if (def.X != -20 || def.Y != 6 || def.W != 4 || def.H != 7) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect(-20, 6, 4, 7);");
                    Console.WriteLine("def.X != -20 || def.Y != 6 || def.W != 4 || def.H != 7");
                    Console.WriteLine(def.X + " != -20 || " + def.Y + " != 6 || " + def.W + " != 4 || " + def.H + " != 7");
                    Wait();
                    return;
                }
                if (def.Left != -20 || def.Top != 6 || def.Right != -16 || def.Bottom != 13) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect(-20, 6, 4, 7);");
                    Console.WriteLine("def.Left != -20 || def.Top != 6 || def.Right != -16 || def.Bottom != 13");
                    Console.WriteLine(def.Left + " != -20 || " + def.Top + " != 6 || " + def.Right + " != -16 || " + def.Bottom + " != 13");
                    Wait();
                    return;
                }
                if (def.Area != 28) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect(-20, 6, 4, 7);");
                    Console.WriteLine("def.Area != 28");
                    Console.WriteLine(def.Area + " != 28");
                    Wait();
                    return;
                }
            }

            // Constructor: int x, int y, int w, int h
            {
                Rect def = new Rect(5, 5, -5, -10);
                if (def.X != 0 || def.W != 5 || def.Y != -5 || def.H != 10) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect(5, 5, -5, -10);");
                    Console.WriteLine("def.X != 0 || def.W != 5 || def.Y != -5 || def.H != 10");
                    Console.WriteLine(def.X + " != 0 || " + def.Y + " != 5 || " + def.W + " != -5 || " + def.H + " != 10");
                    Wait();
                    return;
                }
                if (def.Left != 0 || def.Top != -5 || def.Right != 5 || def.Bottom != 5) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect(5, 5, -5, -10);");
                    Console.WriteLine("def.Left != 0 || def.Top != -5 || def.Right != 5 || def.Bottom != 5");
                    Console.WriteLine(def.Left + " != 0 || " + def.Top + " != -5 || " + def.Right + " != 5 || " + def.Bottom + " != 5");
                    Wait();
                    return;
                }
                if (def.Area != 50) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect(5, 5, -5, -10);");
                    Console.WriteLine("def.Area != 50");
                    Console.WriteLine(def.Area + " != 50");
                    Wait();
                    return;
                }
            }

            // X, Y, W, H Setters and getters
            {
                Rect rect = new Rect();
                rect.X = 5;
                if (rect.X != 5 || rect.Y != 0 || rect.W != 0 || rect.H != 0) {
                    Console.WriteLine("Error");
                    Console.WriteLine("rect.X = 5;");
                    Console.WriteLine("rect.X != 5 || rect.Y != 0 || rect.W != 0 || rect.H != 0");
                    Console.WriteLine(rect.X + " != 5 || " + rect.Y + " != 0 || " + rect.W + " != 0 || " + rect.H + " != 0");
                    Wait();
                    return;
                }
                if (rect.Left != 5 || rect.Top != 0 || rect.Right != 5 || rect.Bottom != 0) {
                    Console.WriteLine("Error");
                    Console.WriteLine("rect.X = 5;");
                    Console.WriteLine("rect.Left != 5 || rect.Top != 0 || rect.Right != 5 || rect.Bottom != 0");
                    Console.WriteLine(rect.Left + " != 5 || " + rect.Top + " != 0 || " + rect.Right + " != 5 || " + rect.Bottom + " != 0");
                    Wait();
                    return;
                }

                rect = new Rect();
                rect.Y = 5;
                if (rect.X != 0 || rect.Y != 5 || rect.W != 0 || rect.H != 0) {
                    Console.WriteLine("Error");
                    Console.WriteLine("rect.Y = 5;");
                    Console.WriteLine("rect.X != 0 || rect.Y != 5 || rect.W != 0 || rect.H != 0");
                    Console.WriteLine(rect.X + " != 0 || " + rect.Y + " != 5 || " + rect.W + " != 0 || " + rect.H + " != 0");
                    Wait();
                    return;
                }
                if (rect.Left != 0 || rect.Top != 5 || rect.Right != 0 || rect.Bottom != 5) {
                    Console.WriteLine("Error");
                    Console.WriteLine("rect.X = 5;");
                    Console.WriteLine("rect.Left != 0 || rect.Top != 5 || rect.Right != 0 || rect.Bottom != 5");
                    Console.WriteLine(rect.Left + " != 0 || " + rect.Top + " != 5 || " + rect.Right + " != 0 || " + rect.Bottom + " != 5");
                    Wait();
                    return;
                }

                rect = new Rect();
                rect.W = 5;
                if (rect.X != 0 || rect.Y != 0 || rect.W != 5 || rect.H != 0) {
                    Console.WriteLine("Error");
                    Console.WriteLine("rect.W = 5;");
                    Console.WriteLine("rect.X != 0 || rect.Y != 0 || rect.W != 5 || rect.H != 0");
                    Console.WriteLine(rect.X + " != 0 || " + rect.Y + " != 0 || " + rect.W + " != 5 || " + rect.H + " != 0");
                    Wait();
                    return;
                }

                rect = new Rect();
                rect.H = 5;
                if (rect.X != 0 || rect.Y != 0 || rect.W != 0 || rect.H != 5) {
                    Console.WriteLine("Error");
                    Console.WriteLine("rect.W = 5;");
                    Console.WriteLine("rect.X != 0 || rect.Y != 0 || rect.W != 0 || rect.H != 5");
                    Console.WriteLine(rect.X + " != 0 || " + rect.Y + " != 0 || " + rect.W + " != 0 || " + rect.H + " != 5");
                    Wait();
                    return;
                }

                rect = new Rect();
                rect.X = 2;
                rect.W = -5;
                if (rect.X != -3 || rect.Y != 0 || rect.W != 5 || rect.H != 0) {
                    Console.WriteLine("Error");
                    Console.WriteLine("rect.X = 2; rect.W = -5;");
                    Console.WriteLine("rect.X != -3 || rect.Y != 0 || rect.W != 5 || rect.H != 0");
                    Console.WriteLine(rect.X + " != -3 || " + rect.Y + " != 0 || " + rect.W + " != 5 || " + rect.H + " != 0");
                    Wait();
                    return;
                }

                rect = new Rect();
                rect.Y = 2;
                rect.H = -5;
                if (rect.X != 0 || rect.Y != -3 || rect.W != 0 || rect.H != 5) {
                    Console.WriteLine("Error");
                    Console.WriteLine("rect.Y = 2; rect.H = -5;");
                    Console.WriteLine("rect.X != 0 || rect.Y != -3 || rect.W != 0 || rect.H != 5");
                    Console.WriteLine(rect.X + " != 0 || " + rect.Y + " != -3 || " + rect.W + " != 0 || " + rect.H + " != 5");
                    Wait();
                    return;
                }
            }
            // Rectangle getter function
            {
                Rect r1 = new Rect(5, 10, 15, 20);
                System.Drawing.Rectangle r2 = r1.Rectangle;

                if (r1.X != r2.X || r1.Y != r2.Y || r1.W != r2.Width || r1.H != r2.Height) {
                    Console.WriteLine("Error");
                    Console.WriteLine("Rect r1 = new Rect(5, 10, 15, 20);");
                    Console.WriteLine("System.Drawing.Rectangle r2 = r1.Rectangle;");
                    Console.WriteLine("r1.X != r2.X || r1.Y != r2.Y || r1.W != r2.Width || r1.H != r2.Height");
                    Console.WriteLine(r1.X + " != " + r2.X + " || " + r1.Y + " != " + r2.Y + " || " + r1.W + " != " + r2.Width + " || " + r1.H + " != " + r2.Height);
                    Wait();
                    return;
                }
            }

            // Constructor: Point, Size
            {
                Rect def = new Rect(new System.Drawing.Point(5, 5), new System.Drawing.Size(-5, -10));
                if (def.X != 0 || def.W != 5 || def.Y != -5 || def.H != 10) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect(new .Point(5, -5), new Size(5, -10));");
                    Console.WriteLine("def.X != 0 || def.W != 5 || def.Y != -5 || def.H != 10");
                    Console.WriteLine(def.X + " != 0 || " + def.Y + " != 5 || " + def.W + " != -5 || " + def.H + " != 10");
                    Wait();
                    return;
                }
                if (def.Left != 0 || def.Top != -5 || def.Right != 5 || def.Bottom != 5) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect(new .Point(5, -5), new Size(5, -10));");
                    Console.WriteLine("def.Left != 0 || def.Top != -5 || def.Right != 5 || def.Bottom != 5");
                    Console.WriteLine(def.Left + " != 0 || " + def.Top + " != -5 || " + def.Right + " != 5 || " + def.Bottom + " != 5");
                    Wait();
                    return;
                }
                if (def.Area != 50) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect(new .Point(5, -5), new Size(5, -10));");
                    Console.WriteLine("def.Area != 50");
                    Console.WriteLine(def.Area + " != 50");
                    Wait();
                    return;
                }
            }
            // Constructor point, point
            {
                Rect def = new Rect(new Point(0, 0), new Point(5, 5));

                if (def.X != 0 || def.Y != 0 || def.W != 5 || def.H != 5) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect def = new Rect(new Point(0, 0), new Point(5, 5));");
                    Console.WriteLine("def.X != 0 || def.Y != 0 || def.W != 5 || def.H != 5");
                    Console.WriteLine(def.X + " != 0 || " + def.Y + " != 0 || " + def.W + " != 5 || " + def.H + " != 5");
                    Wait();
                    return;
                }

                def = new Rect(new Point(2, 2), new Point(-5, -5));
                if (def.X != -5 || def.Y != -5 || def.W != 7 || def.H != 7) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("def = new Rect(new Point(2, 2), new Point(-5, -5));");
                    Console.WriteLine("def.X != -5 || def.Y != -5 || def.W != 7 || def.H != 7");
                    Console.WriteLine(def.X + " != -5 || " + def.Y + " != -5 || " + def.W + " != 7 || " + def.H + " != 7");
                    Wait();
                    return;
                }
            }
            // Intersects
            {
                Rect r1 = new Rect(new Point(1, 1), new Point(2, 2));
                Rect r2 = new Rect(new Point(3, 3), new Point(4, 4));
                if (r1.Intersects(r2)) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("Rect r1 = new Rect(new Point(1, 1), new Point(2, 2));");
                    Console.WriteLine("Rect r2 = new Rect(new Point(3, 3), new Point(4, 4));");
                    Console.WriteLine("Rectangles should not intersect");
                    Wait();
                    return;
                }

                r1 = new Rect(new Point(1, 1), new Point(5, 5));
                r2 = new Rect(new Point(2, 2), new Point(3, 4));
                if (!r1.Intersects(r2)) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("r1 = new Rect(new Point(1, 1), new Point(5, 5));");
                    Console.WriteLine("r2 = new Rect(new Point(2, 2), new Point(3, 4));");
                    Console.WriteLine("r2 is fully contained inside of r1");
                    Console.WriteLine("The rectangles should be intersecting");
                    Wait();
                    return;
                }

                r1 = new Rect(new Point(1, 1), new Point(3, 3));
                r2 = new Rect(new Point(2, 2), new Point(4, 4));
                if (!r1.Intersects(r2)) {
                    Console.WriteLine("Error:");
                    Console.WriteLine("r1 = new Rect(new Point(1, 1), new Point(3, 3));");
                    Console.WriteLine("r2 = new Rect(new Point(2, 2), new Point(4, 4));");
                    Console.WriteLine("The rectangles should be intersecting");
                    Wait();
                    return;
                }
            }
            // Contains
            {
                Rect rect = new Rect(new Point(1, 1), new Point(3, 3));
                Point point = new Point(2, 2);
                if (!rect.Contains(point)) {
                    Console.WriteLine("Error");
                    Console.WriteLine("rect = new Rect(new Point(1, 1), new Point(3, 3));");
                    Console.WriteLine("point = new Point(2, 2);");
                    Console.WriteLine("Rect should contain point");
                    Wait();
                    return;
                }

                rect = new Rect(new Point(1, 1), new Point(3, 3));
                point = new Point(7, 7);
                if (rect.Contains(point)) {
                    Console.WriteLine("Error");
                    Console.WriteLine("rect = new Rect(new Point(1, 1), new Point(3, 3));");
                    Console.WriteLine("point = new Point(7, 7);");
                    Console.WriteLine("Rect should NOT contain point");
                    Wait();
                    return;
                }
            }
            // Intersection
            {
                Rect r1 = new Rect(new Point(1, 1), new Point(2, 2));
                Rect r2 = new Rect(new Point(3, 3), new Point(4, 4));
                Rect result = r1.Intersection(r2);
                if (result.X != 0 || result.Y != 0 || result.W != 0 || result.H != 0) {
                    Console.WriteLine("Error, (They don't touch)");
                    Console.WriteLine("Rect r1 = new Rect(new Point(1, 1), new Point(2, 2));");
                    Console.WriteLine("Rect r2 = new Rect(new Point(3, 3), new Point(4, 4));");
                    Console.WriteLine("Rect result = r1.Intersection(r2);");
                    Console.WriteLine("result.X != 0 || result.Y != 0 || result.W != 0 || result.H != 0");
                    Console.WriteLine(result.X + " != 0 || " + result.Y + " != 0 || " + result.W + " != 0 || " + result.H + " != 0");
                    Wait();
                    return;
                }

                r1 = new Rect(new Point(1, 1), new Point(5, 5));
                r2 = new Rect(new Point(2, 2), new Point(4, 4));
                result = r1.Intersection(r2);
                if (result.X != 2 || result.Y != 2 || result.W != 2 || result.H != 2) {
                    Console.WriteLine("Error, (R2 is fully in R1)");
                    Console.WriteLine("Rect r1 = new Rect(new Point(1, 1), new Point(5, 5));");
                    Console.WriteLine("Rect r2 = new Rect(new Point(2, 2), new Point(4, 4));");
                    Console.WriteLine("Rect result = r1.Intersection(r2);");
                    Console.WriteLine("result.X != 2 || result.Y != 2 || result.W != 2 || result.H != 2");
                    Console.WriteLine(result.X + " != 2 || " + result.Y + " != 2 || " + result.W + " != 2 || " + result.H + " != 2");
                    Wait();
                    return;
                }

                r1 = new Rect(new Point(1, 1), new Point(3, 3));
                r2 = new Rect(new Point(2, 2), new Point(4, 4));
                result = r1.Intersection(r2);
                if (result.X != 2 || result.Y != 2 || result.W != 1 || result.H != 1) {
                    Console.WriteLine("Error, (They intersect)");
                    Console.WriteLine("Rect r1 = new Rect(new Point(1, 1), new Point(3, 3));");
                    Console.WriteLine("Rect r2 = new Rect(new Point(2, 2), new Point(4, 4));");
                    Console.WriteLine("Rect result = r1.Intersection(r2);");
                    Console.WriteLine("result.X != 2 || result.Y != 2 || result.W != 1 || result.H != 1");
                    Console.WriteLine(result.X + " != 2 || " + result.Y + " != 2 || " + result.W + " != 1 || " + result.H + " != 1");
                    Wait();
                    return;
                }
            }

            // Don't close the program
            Console.WriteLine("All unit tests have passed.");
            Console.ReadLine();
        }
    }
}

