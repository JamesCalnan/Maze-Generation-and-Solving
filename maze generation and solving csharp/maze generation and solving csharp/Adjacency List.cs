using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace colour_function_csharp
{
    class Adjacency_List
    {
        private static bool isCornerOrJunction(List<Point> maze, Point v)
        {
            if (v.Equals(maze.First()) || v.Equals(maze.Last()))
            {
                return true;
            }

            if (maze.Contains(new Point(v.X + 2, v.Y)) && maze.Contains(new Point(v.X, v.Y + 1)))
            {
                return true; //right & down
            }
            if (maze.Contains(new Point(v.X + 2, v.Y)) && maze.Contains(new Point(v.X, v.Y - 1)))
            {
                return true; //right & up
            }

            if (maze.Contains(new Point(v.X - 2, v.Y)) && maze.Contains(new Point(v.X, v.Y - 1)))
            {
                return true; //left & up
            }

            if (maze.Contains(new Point(v.X - 2, v.Y)) && maze.Contains(new Point(v.X, v.Y + 1)))
            {
                return true; //left & down
            }

            var surroundingNodes = new List<Point>
            {
                new Point(v.X + 2, v.Y),
                new Point(v.X - 2, v.Y),
                new Point(v.X, v.Y + 1),
                new Point(v.X, v.Y - 1),
            };

            return surroundingNodes.Count(vertex => maze.Contains(vertex)) >= 3;
        }

        private static List<Point> returnCornersAndJunctions(List<Point> maze)
        {
            return maze.Where(V => isCornerOrJunction(maze, V)).ToList();
        }

        public static Dictionary<Point, List<Point>> returnAdjacencylist(List<Point> maze)
        {
            var returnDictList = new Dictionary<Point, List<Point>>();

            var necessaryVertices = returnCornersAndJunctions(maze);

            foreach (Point vertex in necessaryVertices)
            {
                //if point is a junction or a corner then it is needed
                var currentVertexXValue = vertex.X;
                var currentVertexYValue = vertex.Y;
                var adjacentVertices = new List<Point>();

                //go up
                adjacentVertices.Add(traverse(necessaryVertices, maze, currentVertexXValue, currentVertexYValue - 1, 0, -1));
                //go right
                adjacentVertices.Add(traverse(necessaryVertices, maze, currentVertexXValue + 2, currentVertexYValue, 2, 0));
                //go down
                adjacentVertices.Add(traverse(necessaryVertices, maze, currentVertexXValue, currentVertexYValue + 1, 0, 1));
                //go left
                adjacentVertices.Add(traverse(necessaryVertices, maze, currentVertexXValue - 2, currentVertexYValue, -2, 0));

                returnDictList.Add(vertex, adjacentVertices.Where(V => !V.IsEmpty).ToList());
            }
            return returnDictList;
        }

        private static Point traverse(List<Point> cornerJunctionList, List<Point> maze, int x, int y, int addX, int addY)
        {
            if (!maze.Contains(new Point(x, y))) { return Point.Empty; }

            if (cornerJunctionList.Contains(new Point(x,y))) return new Point(x,y);

            return traverse(cornerJunctionList, maze, x + addX, y + addY, addX, addY);

        }

        private static void retracePathUnconnectedVertices(Point goal, Point root, Dictionary<Point, Point> cameFrom)
        {
            Program.setBothColours(ConsoleColor.Green);
            var mazePath = new List<Point>();
            var currentV = goal;
            while (!currentV.Equals(root))
            {
                mazePath.Add(currentV);
                currentV = cameFrom[currentV];
            }

            mazePath.Add(root);

            mazePath.Reverse();


            for (int i = 0; i < mazePath.Count - 1; i++)
            {
                connectPoints(mazePath[i], mazePath[i+1]);
            }
        }

        private static void connectPoints(Point p1, Point p2)
        {
            if (p1.Y == p2.Y)
            {
                if (p1.X < p2.X)
                {
                    for (int x = p1.X; x <= p2.X; x += 2)
                    {
                        Program.printPoint(new Point(x, p1.Y));
                    }
                }
                else
                {
                    for (int x = p2.X; x <= p1.X; x += 2)
                    {
                        Program.printPoint(new Point(x, p1.Y));
                    }
                }
            }
            else
            {
                if (p1.Y > p2.Y)
                {
                    for (int y = p2.Y; y <= p1.Y; y++)
                    {
                        Program.printPoint(new Point(p1.X, y));
                    }
                }
                else
                {
                    for (int y = p2.Y; y >= p1.Y; y--)
                    {
                        Program.printPoint(new Point(p1.X, y));

                    }
                }
            }
        }
    }
}
