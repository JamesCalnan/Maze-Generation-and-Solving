using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace maze_generation_and_solving_csharp
{
    class AStar
    {
        private static Point ExtractMinPointAStar(HashSet<Point> openSet, Dictionary<Point, float> fScore)
        {

            var sortedDictionary = from pair in fScore orderby pair.Value ascending select pair;

            foreach (KeyValuePair<Point, float> pair in sortedDictionary)
            {
                if (openSet.Contains(pair.Key)) { return pair.Key; }
            }

            return openSet.First();
        }

        public static void AStarAlgorithm(List<Point> maze)
        {
            var heuristic = 10;

            var openSet = new HashSet<Point>();
            var prev = new Dictionary<Point, Point>();

            var gScore = new Dictionary<Point, float>();
            var fScore = new Dictionary<Point, float>();

            foreach (Point V in maze)
            {
                gScore[V] = short.MaxValue;
                fScore[V] = short.MaxValue;
            }

            var root = maze.First();
            var goal = maze.Last();

            gScore[root] = 0;
            fScore[root] = h(root,goal, heuristic);

            openSet.Add(root);

            Program.setBothColours(ConsoleColor.Red);

            while (openSet.Count > 0)
            {
                var current = ExtractMinPointAStar(openSet,fScore);

                Program.printPoint(current);

                if (current.Equals(goal))
                {
                    Program.findPath(goal, root, prev);
                    break;
                }

                openSet.Remove(current);

                foreach (Point neighbour in Program.returnAdjacentVertices(maze, current))
                {
                    var tentative_gScore = gScore[current] + h(current, neighbour, heuristic);

                    if (tentative_gScore <= gScore[neighbour])
                    {
                        prev[neighbour] = current;
                        gScore[neighbour] = tentative_gScore;
                        fScore[neighbour] = gScore[neighbour] + h(neighbour, goal, heuristic);

                        if (!openSet.Contains(neighbour)) { openSet.Add(neighbour); }
                    }
                }
            }
        }

        private static float h(Point current, Point goal, double h) => (float) ((Math.Abs(current.X - goal.X) + Math.Abs(current.Y - goal.Y)) * h);

    }
}
