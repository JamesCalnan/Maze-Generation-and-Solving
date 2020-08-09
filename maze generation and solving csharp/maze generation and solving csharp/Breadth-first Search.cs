using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace maze_generation_and_solving_csharp
{
    class Breadth_first_Search
    {
        public static void solveMaze(HashSet<Point> maze)
        {
            var Q = new Queue<Point>();
            var discovered = maze.ToDictionary(vertex => vertex, vertex => false);
            var cameFrom = new Dictionary<Point, Point>();

            var root = maze.First();
            var goal = maze.Last();

            discovered[root] = true;
            Q.Enqueue(root);

            Program.setBothColours(ConsoleColor.Red);

            while (isNotEmpty(Q))
            {
                var V = Q.Dequeue();

                Program.printPoint(V);

                if (V.Equals(goal)) {
                    Program.findPath(goal, root, cameFrom);
                    break;
                }
                foreach (var w in Program.returnAdjacentVertices(maze, V).Where(w => !discovered[w]))
                {
                    discovered[w] = true;
                    cameFrom[w] = V;
                    Q.Enqueue(w);
                }
            }
        }

        public static bool isNotEmpty(Queue<Point> q) => q.Count > 0;
    }
}
