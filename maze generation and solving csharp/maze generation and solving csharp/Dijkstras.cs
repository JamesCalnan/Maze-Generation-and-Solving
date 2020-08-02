using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace maze_generation_and_solving_csharp
{
    class Dijkstras
    {
        public static void solveMaze(HashSet<Point> maze)
        {

            var Q = new HashSet<Point>();
            var dist = new Dictionary<Point, Int32>();
            var prev = new Dictionary<Point, Point>();

            var root = maze.First();
            var goal = maze.Last();

            foreach (Point vertex in maze)
            {
                dist.Add(vertex, Int16.MaxValue);
                prev.Add(vertex, Point.Empty);
                Q.Add(vertex);
            }

            dist[root] = 0;

            while (Q.Count > 0)
            {
                var V = ExtractMinPoint(Q, dist);

                
                if (V.Equals(goal))
                {
                    Program.findPath(goal, root, prev);
                    break;
                }

                Q.Remove(V);

                foreach (Point w in Program.returnAdjacentVertices(maze, V))
                {
                    var alt = dist[V] + 1;
                    if (alt < dist[w])
                    {
                        dist[w] = alt;
                        prev[w] = V;
                    }
                }
            }
        }

        private static Point ExtractMinPoint(HashSet<Point> Q, Dictionary<Point, int> dist)
        {

            var sortedDictionary = from pair in dist orderby pair.Value ascending select pair;

            foreach (KeyValuePair<Point, int> pair in sortedDictionary)
            {
                if (Q.Contains(pair.Key)) { return pair.Key; }
            }

            return Q.First();
        }
    }
}
