using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace maze_generation_and_solving_csharp
{

    class Program
    {
        public static int delay = 0;
        static void Main(string[] args)
        {

            Console.ReadKey();
            Console.CursorVisible = false;

            while (true)
            {

                (int, int) mazeDimenations = (Console.WindowWidth- 8, Console.WindowHeight - 5);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("generating maze...");

                var mazes = new HashSet<HashSet<Point>>
                {
                    BoruvkasAlgorithm.generateMaze(mazeDimenations.Item1, mazeDimenations.Item2),
                    PrimsAlgorithm.generateMaze(mazeDimenations.Item1, mazeDimenations.Item2),
                    KruskalsAlgorithm.generateMaze(mazeDimenations.Item1, mazeDimenations.Item2),
                    RecursiveBacktrackerAlgorithm.generateMaze(mazeDimenations.Item1, mazeDimenations.Item2)

                };

                Console.Clear();

                foreach (HashSet<Point> maze in mazes)
                {
                    printMaze(maze);
                    Depth_first_Search.solveMaze(maze);
                    Thread.Sleep(400);
                    Console.ResetColor();
                    Console.Clear();
                }
            }

            Console.ReadKey();
        }

        public static void printMaze(HashSet<Point> mazeHashSet)
        {
            setBothColours(ConsoleColor.White);
            foreach (Point v in mazeHashSet)
            {
                printPoint(v);
                Thread.Sleep(delay);
            }
        }

        public static void findPath(Point goal, Point root, Dictionary<Point, Point> cameFrom)
        {
           setBothColours(ConsoleColor.Green);
           var mazePath = new List<Point>();
           var currentV = goal;
           while (!currentV.Equals(root))
           {
               mazePath.Add(currentV);
               currentV = cameFrom[currentV];
           }

           mazePath.Add(root);

           mazePath.Reverse();

           foreach (Point vertex in mazePath) 
           {
               Thread.Sleep(delay);
               printPoint(vertex);
           }
        }

        public static List<Point> returnAdjacentVertices(HashSet<Point> maze, Point v)
        {
            var adjacentPoints = new List<Point>();
            var editPoint = new Point(v.X + 2,v.Y);

            if (maze.Contains(editPoint)) { adjacentPoints.Add(editPoint); }

            editPoint = new Point(v.X - 2, v.Y);

            if (maze.Contains(editPoint)) { adjacentPoints.Add(editPoint); }

            editPoint = new Point(v.X, v.Y + 1);

            if (maze.Contains(editPoint)) { adjacentPoints.Add(editPoint); }

            editPoint = new Point(v.X, v.Y - 1);

            if (maze.Contains(editPoint)) { adjacentPoints.Add(editPoint); }

            return adjacentPoints;
        }

        public static void setBothColours(ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.BackgroundColor = colour;
        }

        public static void printPoint(Point inputPoint)
        {
            Console.SetCursorPosition(inputPoint.X,inputPoint.Y);
            Console.Write("XX");
        }

        public static List<Point> returnUnvisitedNeighbours(Dictionary<Point, bool> visited, Point currentV)
        {
            var returnList = new List<Point>();

            var editPoint = new Point(currentV.X + 4, currentV.Y);

            if (visited.ContainsKey(editPoint) && !visited[editPoint])
            {
                returnList.Add(editPoint);
            }

            editPoint = new Point(currentV.X - 4, currentV.Y);

            if (visited.ContainsKey(editPoint) && !visited[editPoint])
            {
                returnList.Add(editPoint);
            }

            editPoint = new Point(currentV.X, currentV.Y + 2);

            if (visited.ContainsKey(editPoint) && !visited[editPoint])
            {
                returnList.Add(editPoint);
            }

            editPoint = new Point(currentV.X, currentV.Y - 2);

            if (visited.ContainsKey(editPoint) && !visited[editPoint])
            {
                returnList.Add(editPoint);
            }

            return returnList;
        }
    }
}