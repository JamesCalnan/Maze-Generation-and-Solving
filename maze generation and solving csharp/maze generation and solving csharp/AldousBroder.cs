using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace maze_generation_and_solving_csharp
{
    class AldousBroder
    {


        public static HashSet<Point> generateMaze(int width, int height)
        {
            var startV = new Point(5, 3);
            var currentV = startV;
            var prev = startV;

            var maze = new HashSet<Point>
            {
                new Point(5, 2),
                currentV
            };

            var r = new Random();

            var visited = RecursiveBacktrackerAlgorithm.ReturnDict(width, height);

            Program.setBothColours(ConsoleColor.White);

            visited[currentV] = true;

            while (unvisitedVertices(visited))
            {
                var surroundingVertices = returnNeighbours(visited, currentV);

                var temporaryVertex = surroundingVertices[r.Next(surroundingVertices.Count)];

                if (!visited[temporaryVertex])
                {

                    var wallV = RecursiveBacktrackerAlgorithm.pointMidPoint(currentV, temporaryVertex);

                    visited[temporaryVertex] = true;

                    maze.Add(wallV);
                    maze.Add(temporaryVertex);
                }

                currentV = temporaryVertex;

            }


            var mazeExit = RecursiveBacktrackerAlgorithm.returnExit(width, height);
            maze.Add(mazeExit);


            return maze;
        }

        static List<Point> returnNeighbours(Dictionary<Point, bool> visited, Point v)
        {
            var surroundVertices = new List<Point>
            {
                new Point(v.X + 4, v.Y),
                new Point(v.X - 4, v.Y),
                new Point(v.X, v.Y + 2),
                new Point(v.X, v.Y - 2),
            };
            return (from Point vertex in surroundVertices where visited.ContainsKey(vertex) select vertex).ToList();
        }
        static bool unvisitedVertices(Dictionary<Point, bool> visited) => visited.Values.ToList().Contains(false);
    }
}
