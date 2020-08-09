using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace maze_generation_and_solving_csharp
{
    class HuntandKill
    {


        public static HashSet<Point> generateMaze(int width, int height)
        {
            var startV = new Point(5, 3);
            var currentV = startV;

            var visited = RecursiveBacktrackerAlgorithm.ReturnDict(width, height);

            var maze = new HashSet<Point>();

            var mazeEntry = new Point(5, 2);
            maze.Add(mazeEntry);

            visited[startV] = true;
            maze.Add(startV);

            var r = new Random();

            while (unvisitedVertices(visited))
            {

                var availableVertices = Program.returnUnvisitedNeighbours(visited, currentV);

                if (availableVertices.Count > 0)
                {

                    var temporaryV = availableVertices[r.Next(availableVertices.Count)];

                    visited[temporaryV] = true;

                    var wallV = RecursiveBacktrackerAlgorithm.pointMidPoint(currentV, temporaryV);

                    maze.Add(wallV);
                    maze.Add(temporaryV);

                    currentV = temporaryV;

                }
                else
                {
                    foreach (Point v in visited.Keys)
                    {
                        var visitedNeighbours = PrimsAlgorithm.returnVisitedNeighbours(visited, v);
                        if (visitedNeighbours.Count == 0 || visited[v])
                        {
                            //if there are no visited neighbours OR the current vertex is also visited
                            continue;
                        }
                        else
                        {

                            var newVertex = visitedNeighbours[r.Next(visitedNeighbours.Count)];

                            var wallV = RecursiveBacktrackerAlgorithm.pointMidPoint(newVertex, v);

                            maze.Add(wallV);
                            maze.Add(v);

                            currentV = v;

                            break;

                        }
                    }
                    visited[currentV] = true;
                }
            }


            var mazeExit = RecursiveBacktrackerAlgorithm.returnExit(width, height);

            maze.Add(mazeExit);

            return maze;
        }

        static bool unvisitedVertices(Dictionary<Point, bool> visited) => visited.Values.ToList().Contains(false);

        //public static List<Point> returnVistsedNeighbours(Dictionary<Point, bool> discovered, Point cV)
        //{
        //    var returnList = new List<Point>();

        //    if (discovered.ContainsKey(new Point(cV.X + 4, cV.Y)) && discovered[new Point(cV.X + 4, cV.Y)])
        //    {
        //        returnList.Add(new Point(cV.X + 4, cV.Y));
        //    }

        //    if (discovered.ContainsKey(new Point(cV.X - 4, cV.Y)) && discovered[new Point(cV.X - 4, cV.Y)])
        //    {
        //        returnList.Add(new Point(cV.X - 4, cV.Y));
        //    }

        //    if (discovered.ContainsKey(new Point(cV.X, cV.Y + 2)) && discovered[new Point(cV.X, cV.Y + 2)])
        //    {
        //        returnList.Add(new Point(cV.X, cV.Y + 2));
        //    }

        //    if (discovered.ContainsKey(new Point(cV.X, cV.Y - 2)) && discovered[new Point(cV.X, cV.Y - 2)])
        //    {
        //        returnList.Add(new Point(cV.X, cV.Y - 2));
        //    }

        //    return returnList;
        //}
    }
}
