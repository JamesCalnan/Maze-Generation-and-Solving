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
        public static int delay = 5;

        static void testsingle()
        {
            (int, int) mazeDimenations = (Console.WindowWidth - 8, Console.WindowHeight - 5);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("generating maze...");


            var maze = Hamiltonian_Graph.generateMaze(mazeDimenations.Item1, mazeDimenations.Item2);


            setBothColours(ConsoleColor.White);
            printMaze(maze, false);

            SPFA.solveMaze(maze);


            Thread.Sleep(800);
            resetWindow();
        }

        static void testAll()
        {
            (int, int) mazeDimenations = (Console.WindowWidth - 8, Console.WindowHeight - 5);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("generating maze...");

            var mazes = new HashSet<HashSet<Point>>();

            var thread = new Thread(
                () =>
                {
                    mazes.Add(BoruvkasAlgorithm.generateMaze(mazeDimenations.Item1, mazeDimenations.Item2));
                });
            var thread2 = new Thread(
                () =>
                {
                    mazes.Add(PrimsAlgorithm.generateMaze(mazeDimenations.Item1, mazeDimenations.Item2));
                });
            var thread3 = new Thread(
                () =>
                {
                    mazes.Add(KruskalsAlgorithm.generateMaze(mazeDimenations.Item1, mazeDimenations.Item2));
                });
            var thread4 = new Thread(
                () =>
                {
                    mazes.Add(RecursiveBacktrackerAlgorithm.generateMaze(mazeDimenations.Item1, mazeDimenations.Item2));
                });
            var thread5 = new Thread(
                () =>
                {
                    mazes.Add(HuntandKill.generateMaze(mazeDimenations.Item1, mazeDimenations.Item2));
                });

            var threadList = new List<Thread>
                {
                    thread,
                    thread2,
                    thread3,
                    thread4,
                    thread5
                };

            foreach (var t in threadList)
            {
                t.Start();
            };


            Console.Clear();

            if (thread.IsAlive | thread2.IsAlive | thread3.IsAlive | thread4.IsAlive)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("generating mazes...");
            }

            while (thread.IsAlive | thread2.IsAlive | thread3.IsAlive | thread4.IsAlive)
            {
                //if a maze is still being generated then don't go onto the code below
            }

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("finished generating mazes");

            foreach (HashSet<Point> maze in mazes)
            {
                setBothColours(ConsoleColor.White);
                printMaze(maze);
                Depth_first_Search.solveMaze(maze);
                Thread.Sleep(800);

                resetWindow();
            }
        }


        static void resetWindow()
        {
            Console.ResetColor();
            Console.Clear();
        }
        static void Main(string[] args)
        {

            Console.ReadKey();
            Console.CursorVisible = false;

            while (true)
            {

                testsingle();


            }

        }

        public static void printMaze(HashSet<Point> mazeHashSet, bool useDelay = true)
        {
            foreach (Point v in mazeHashSet)
            {
                printPoint(v);
                if (useDelay) { Thread.Sleep(delay); }
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