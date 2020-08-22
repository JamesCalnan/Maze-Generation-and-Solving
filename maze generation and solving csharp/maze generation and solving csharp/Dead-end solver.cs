using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace maze_generation_and_solving_csharp
{
    class DeadendSolver
    {

        static bool isJunction(HashSet<Point> maze, Point v)
        {
            return new List<Point>
            {
                new Point(v.X + 2, v.Y),
                new Point(v.X - 2, v.Y),
                new Point(v.X, v.Y + 1),
                new Point(v.X, v.Y - 1),
            }.Count(vertex => maze.Contains(vertex)) >= 3;
        }

        public static bool isDeadend(HashSet<Point> maze, Point v)
        {
            return new List<Point>
            {
                new Point(v.X + 2, v.Y),
                new Point(v.X - 2, v.Y),
                new Point(v.X, v.Y + 1),
                new Point(v.X, v.Y - 1),
            }.Count(vertex => maze.Contains(vertex)) == 1;
        }

        static HashSet<Point> returnDeadends(HashSet<Point> maze)
        {
            return maze.Where(v => isDeadend(maze, v) & !v.Equals(maze.First()) & !v.Equals(maze.Last())).ToHashSet();
        }


        public static void solveMaze(HashSet<Point> maze)
        {
            var path = new List<Point>();

            path.AddRange(maze);

            var mazecopy = maze;

            var deadEnds = returnDeadends(maze);

            Program.setBothColours(ConsoleColor.DarkGray);

            foreach (Point deadend in deadEnds)
            {
                var currentVertex = deadend;
                var previousVertex = currentVertex;

                Program.printPoint(currentVertex);

                var q = new Queue<Point>();
                q.Enqueue(currentVertex);

                var visited = new HashSet<Point> { currentVertex };


                //****************************************************************
                //flood fill until i hit a junction that hasnt previously been hit
                //dont enter places that have already been filled
                //****************************************************************


                while (Breadth_first_Search.isNotEmpty(q))
                {
                    var v = q.Dequeue();

                    if (isJunction(mazecopy, v))
                    {
                        mazecopy.Remove(previousVertex);
                        break;
                    }
                    
                    path.Remove(v);
                    Program.printPoint(v);
                    Thread.Sleep(Program.delay);

                    foreach (var w in Program.returnAdjacentVertices(mazecopy, v).Where(w => !visited.Contains(w)))
                    {
                        visited.Add(w);
                        q.Enqueue(w);
                    }

                    previousVertex = v;
                }
            }

            Program.setBothColours(ConsoleColor.Green);
            Program.printMaze(path.ToHashSet());

        }
    }
}
