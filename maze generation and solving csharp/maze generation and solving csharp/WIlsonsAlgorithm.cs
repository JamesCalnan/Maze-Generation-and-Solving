using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace maze_generation_and_solving_csharp
{
    class WIlsonsAlgorithm
    {

        public static HashSet<Point> generateMaze(int width, int height)
        {
            var r = new Random();

            var visited = RecursiveBacktrackerAlgorithm.ReturnDict(width, height);

            var cameFrom = resetCameFrom(visited);

            //Choose any vertex at random and add it to the UST.
            //Select any vertex that is not already in the UST and perform a random walk until you encounter a vertex that is in the UST.
            //Add the vertices and edges touched in the random walk to the UST.
            //Repeat 2 and 3 until all vertices have been added to the UST.

            var UST = new HashSet<Point>();

            var currentV = visited.Keys.ToList()[r.Next(visited.Keys.Count)];
            var prev = currentV;
            visited[currentV] = true;

            var maze = new HashSet<Point>
            {
                new Point(5, 2),
                currentV
            };

            UST.Add(currentV);


            currentV = getUnvisitedVertex(visited);

            var firstCell = currentV;


            while (unvisitedVertices(visited))
            {

                var surroundingVertices = returnNeighbours(visited, currentV);

                var temporaryVertex = surroundingVertices[r.Next(surroundingVertices.Count)];

                cameFrom[prev] = temporaryVertex;


                if (UST.Contains(temporaryVertex))
                {
                    temporaryVertex = firstCell;

                    var verticesToBeAdded = new List<Point>
                    {
                        temporaryVertex
                    };


                    do
                    {
                        if (!cameFrom.ContainsKey(temporaryVertex))
                        {
                            break;
                        }

                        temporaryVertex = cameFrom[temporaryVertex];
                        verticesToBeAdded.Add(temporaryVertex);
                    } while (!UST.Contains(temporaryVertex));


                    for (int i = 0; i < verticesToBeAdded.Count - 1; i++)
                    {
                        var v1 = verticesToBeAdded[i];
                        var v2 = verticesToBeAdded[i + 1];
                        var wall = RecursiveBacktrackerAlgorithm.pointMidPoint(v1, v2);

                        visited[v1] = true;

                        maze.Add(wall);
                        maze.Add(v1);

                        UST.Add(v1);
                    }

                    cameFrom = resetCameFrom(visited);

                    if (!unvisitedVertices(visited))
                    {
                        break;
                    }

                    currentV = getUnvisitedVertex(visited);

                    prev = currentV;

                    firstCell = currentV;

                }
                else
                {

                    currentV = temporaryVertex;

                    cameFrom[prev] = currentV;

                    prev = currentV;

                }


            }

            var mazeExit = RecursiveBacktrackerAlgorithm.returnExit(width, height);

            maze.Add(mazeExit);

            return maze;
        }

        static Dictionary<Point, Point> resetCameFrom(Dictionary<Point, bool> visited)
        {
            return visited.Keys.ToDictionary(v => v, v => Point.Empty);
        }

        static Point getUnvisitedVertex(Dictionary<Point, bool> visited)
        {
            var r = new Random();

            var possibleVertices = (from v in visited where !v.Value select v.Key).ToList();

            return possibleVertices[r.Next(possibleVertices.Count)];
        }

        static bool unvisitedVertices(Dictionary<Point, bool> visited) => visited.Values.ToList().Contains(false);

        static List<Point> returnNeighbours(Dictionary<Point, bool> visited, Point v) => (from Point vertex in new List<Point>
            {
                new Point(v.X + 4, v.Y),
                new Point(v.X - 4, v.Y),
                new Point(v.X, v.Y + 2),
                new Point(v.X, v.Y - 2),
            } where visited.ContainsKey(vertex) select vertex).ToList();
    }
}
