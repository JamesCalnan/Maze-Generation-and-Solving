using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace maze_generation_and_solving_csharp
{
    class Recursive_Solving_Method
    {
        public static void SolveMaze(List<Point> maze)
        {

            var discovered = new Dictionary<Point, bool>();
            var correctPath = new Dictionary<Point, bool>();
            
            foreach (Point vertex in maze)
            {
                discovered.Add(vertex, false);
                correctPath.Add(vertex, false);
            }
            
            
            var root = maze.First();
            
            
            bool solved = RecursiveSolve(maze, discovered, correctPath, root.X, root.Y);

            Program.setBothColours(ConsoleColor.Green);

            foreach (var vertex in correctPath.Where(vertex => vertex.Value))
            {
                Thread.Sleep(Program.delay);
                Program.printPoint(vertex.Key);
            }
        }

        private static bool RecursiveSolve(List<Point> maze, Dictionary<Point, bool> discovered, Dictionary<Point, bool> correctPath, int x, int y)
        {
            if (new Point(x, y).Equals(maze.Last())){
                correctPath[new Point(x, y)] = true;
                return true;
            }

            if (!maze.Contains(new Point(x, y)) || discovered[new Point(x, y)]) { return false; }

            discovered[new Point(x, y)] = true;

            if (RecursiveSolve(maze, discovered, correctPath, x, y - 1))
            {
                correctPath[new Point(x, y)] = true;
                return true;
            }
            if (RecursiveSolve(maze, discovered, correctPath, x - 2, y))
            {
                correctPath[new Point(x, y)] = true;
                return true;
            }
            if (RecursiveSolve(maze, discovered, correctPath, x + 2, y))
            {
                correctPath[new Point(x, y)] = true;
                return true;
            }
            if (RecursiveSolve(maze, discovered, correctPath, x, y + 1))
            {
                correctPath[new Point(x, y)] = true;
                return true;
            }

            return false;
        }

    }
}
