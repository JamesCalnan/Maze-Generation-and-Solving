using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;

namespace colour_function_csharp
{

    class Program
    {
        public static int delay = 10;
        static void Main(string[] args)
        {

            Console.ReadKey();
            Console.CursorVisible = false;

            var maze = Prims_Algorithm.primsAlgorithm(Console.WindowWidth / 2, Console.WindowHeight - 5);

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            AStar.AStarAlgorithm(maze);

            Console.SetCursorPosition(50, 0);
            Console.ResetColor();
            Console.Write($"time taken to solve: {stopwatch.Elapsed.TotalSeconds}");

            Console.ReadKey();
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

        public static List<Point> returnAdjacentVertices(List<Point> maze, Point v)
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