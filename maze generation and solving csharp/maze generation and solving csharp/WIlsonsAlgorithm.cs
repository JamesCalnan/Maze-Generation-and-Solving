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
            var startV = new Point(5, 3);

            var maze = new HashSet<Point>
            {
                new Point(5, 2),
            };

            var r = new Random();

            var visited = RecursiveBacktrackerAlgorithm.ReturnDict(width, height);

            var cameFrom = new Dictionary<Point, Point>();

            cameFrom = resetCameFrom(visited);


            //Choose any vertex at random and add it to the UST.
            //Select any vertex that is not already in the UST and perform a random walk until you encounter a vertex that is in the UST.
            //Add the vertices and edges touched in the random walk to the UST.
            //Repeat 2 and 3 until all vertices have been added to the UST.

            var UST = new HashSet<Point>();

            var currentV = visited.Keys.ToList()[r.Next(visited.Keys.Count)];
            var prev = currentV;
            visited[currentV] = true;

            Program.setBothColours(ConsoleColor.White);

            Program.printPoint(currentV);

            UST.Add(currentV);


            currentV = getUnvisitedVertex(visited);

            var firstCell = currentV;


            Program.setBothColours(ConsoleColor.Blue);

            Program.printPoint(currentV);

            Console.ReadKey();

            while (unvisitedVertices(visited))
            {

                var surroundingVertices = returnNeighbours(visited, currentV);

                var temporaryVertex = surroundingVertices[r.Next(surroundingVertices.Count)];

                Program.setBothColours(ConsoleColor.Blue);

                Program.printPoint(currentV);


                if (UST.Contains(temporaryVertex))
                {


                    cameFrom[currentV] = temporaryVertex;

                    //trace the currentnode to the end of the camefrom dict

                    var verticesToBeAdded = new List<Point>();

                    temporaryVertex = firstCell;

                    Program.setBothColours(ConsoleColor.Cyan);

                    Program.printPoint(temporaryVertex);

                    var prevV = temporaryVertex;

                    while (!UST.Contains(temporaryVertex))
                    {

                        if (!cameFrom.ContainsKey(temporaryVertex))
                        {
                            break;
                        }
                        
                        temporaryVertex = cameFrom[temporaryVertex];

                       // Program.printPoint(RecursiveBacktrackerAlgorithm.pointMidPoint(prevV, temporaryVertex));

                        prevV = temporaryVertex;

                       // Program.printPoint(temporaryVertex);

                        verticesToBeAdded.Add(temporaryVertex);

                       // Console.ReadKey();

                    }

                    Program.setBothColours(ConsoleColor.Cyan);

                    prevV = verticesToBeAdded.First();

                    if (verticesToBeAdded.Count == 1)
                    {

                        Program.printPoint(RecursiveBacktrackerAlgorithm.pointMidPoint(verticesToBeAdded.First(), verticesToBeAdded.Last()));

                    }

                    foreach (Point v in verticesToBeAdded)
                    {
                        visited[v] = true;
                        UST.Add(v);
                        Program.printPoint(v);
                        Program.printPoint(RecursiveBacktrackerAlgorithm.pointMidPoint(prevV, v));
                        prevV = v;
                    }

                    cameFrom = resetCameFrom(visited);

                    currentV = getUnvisitedVertex(visited);

                    firstCell = currentV;

                    Console.ReadKey();
                }
                else
                {

                    currentV = temporaryVertex;

                    cameFrom[prev] = currentV;

                    prev = currentV;

                }


            }



            return maze;
        }

        static Dictionary<Point, Point> resetCameFrom(Dictionary<Point, bool> visited)
        {
            var returncameFrom = new Dictionary<Point, Point>();

            foreach (Point v in visited.Keys)
            {
                returncameFrom.Add(v, Point.Empty);
            }

            return returncameFrom;
        }

        static Point getUnvisitedVertex(Dictionary<Point, bool> visited)
        {

            var possibleVertices = new List<Point>();
            var r = new Random();

            foreach (var v in visited)
            {
                if (!v.Value)
                {
                    possibleVertices.Add(v.Key);
                }
            }

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
