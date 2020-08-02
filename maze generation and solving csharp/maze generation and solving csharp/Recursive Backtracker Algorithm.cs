using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace maze_generation_and_solving_csharp
{
    class RecursiveBacktrackerAlgorithm
    {

        public static HashSet<Point> generateMaze(int width, int height)
        {
            var startV = new Point(5,3);
            var currentV = startV;

            var mazeStack = new Stack<Point>();
            var visited = ReturnDict(width, height);

            var maze = new HashSet<Point>();

            var mazeEntry = new Point(5,2);
            maze.Add(mazeEntry);

            visited[startV] = true;
            mazeStack.Push(currentV);
            maze.Add(startV);

            var r = new Random();


            while (true)
            {

                var availableVertices = Program.returnUnvisitedNeighbours(visited, currentV);

                if (availableVertices.Count > 0)
                {

                    var temporaryV = availableVertices[r.Next(availableVertices.Count)];

                    visited[temporaryV] = true;

                    mazeStack.Push(temporaryV);

                    var wallV = pointMidPoint(currentV, temporaryV);

                    maze.Add(wallV);
                    maze.Add(temporaryV);

                    currentV = temporaryV;

                } else if (mazeStack.Count > 0)
                {
                    currentV = mazeStack.Pop();
                }
                else
                {
                    break;
                }
            }


            var mazeExit = returnExit(width, height);

            maze.Add(mazeExit);

            return maze;

        }

        public static Point returnExit(int width, int height)
        {
            var endPointX = new List<int>();
            var endPointY = new List<int>();
            for (int x = 5; x <= width; x += 4)
            {
                endPointX.Add(x);
            }
            for (int y = 3; y <= height; y += 2)
            {
                endPointY.Add(y);
            }

            var mazeExit = new Point(endPointX.Last(), endPointY.Last()+1);

            return mazeExit;
        }


        public static Point pointMidPoint(Point p1, Point p2) => new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);

        public static Dictionary<Point, bool> ReturnDict(int xMax, int yMax)
        {
            Dictionary<Point,bool> visited = new Dictionary<Point, bool>();

            for (int x = 5; x <= xMax; x+=4)
            {
                for (int y = 3; y <= yMax; y+=2)
                {
                    visited.Add(new Point(x,y), false);
                }
            }

            return visited;
        }

    }
}
