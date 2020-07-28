using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace maze_generation_and_solving_csharp
{
    class Depth_first_Search
    {
        public static void DepthfirstSearch(List<Point> maze)
        {
            var S = new Stack<Point>();
            var discovered = maze.ToDictionary(vertex => vertex, vertex => false);
            var cameFrom = new Dictionary<Point, Point>();

            var root = maze.First();
            var goal = maze.Last();

            S.Push(root);

            Program.setBothColours(ConsoleColor.Red);

            while (S.Count > 0)
            {
                var V = S.Pop();

                if (V.Equals(goal))
                {
                    Program.findPath(goal, root, cameFrom);
                    break;
                }

                if (!discovered[V])
                {
                    discovered[V] = true;

                    foreach (var w in Program.returnAdjacentVertices(maze, V).Where(w => !discovered[w]))
                    {
                        S.Push(w);
                        cameFrom[w] = V;
                    }
                }
            }
        }
    }
}
