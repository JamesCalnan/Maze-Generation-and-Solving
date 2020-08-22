using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Text;

namespace maze_generation_and_solving_csharp
{
    class Hamiltonian_Graph
    {

        private static bool unvisitedVertices(Dictionary<Point, bool> visited) => visited.Any(v => !v.Value);

        /// <summary>
        /// generates a hamiltonian graph which contains every vertex in the maze
        /// pseudocode:
        ///     while there are unvisited positions:
        ///         if there are unvisited adjacent vertices
        ///             perform a non overlapping random walk
        ///         else
        ///             pick a random adjacent vertex that isn't connected to the trapped point
        ///             connect to the random vertex, named the target vertex
        ///             indentify the two adjacent vertices that are next to the target vertex
        ///             run depth first search from one of the two adjacent vertices until the stack is empty or until the trapped point is reached
        ///             if the trapped point is reached then remove the vertex that connected it
        ///             make the current vertex the vertex adjacent to the vertex that was just removed that isn't the target vertex
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        //todo add scoring to the random vertex walk in order to improve efficiency
        public static HashSet<Point> generateMaze(int width, int height)
        {
            var startV = new Point(5, 3);
            var endV = new Point(9, 3);
            var currentV = startV;

            var visited = RecursiveBacktrackerAlgorithm.ReturnDict(width, height);

            var maze = new HashSet<Point>();

            visited[startV] = true;
            maze.Add(startV);

            var r = new Random();

            Program.setBothColours(ConsoleColor.White);
            Program.printPoint(currentV);

            while (unvisitedVertices(visited))
            {

                var availableVertices = Program.returnUnvisitedNeighbours(visited, currentV);

                if (availableVertices.Count > 0)
                {
                    Program.setBothColours(ConsoleColor.White);

                    var temporaryV = availableVertices[r.Next(availableVertices.Count)];

                    visited[temporaryV] = true;

                    var wallV = RecursiveBacktrackerAlgorithm.pointMidPoint(currentV, temporaryV);

                    maze.Add(wallV);
                    maze.Add(temporaryV);

                    Program.printPoint(wallV);
                    Program.printPoint(temporaryV);

                    currentV = temporaryV;

                }
                else
                {
                    Program.setBothColours(ConsoleColor.White);
                    var endPoint = currentV;

                    Program.printPoint(endPoint);
                    //end point is selected as the current as it has no neighbours

                    //all surrounding points, doesnt take into account the maze at all
                    var neighbours = returnAdjacentVertices(endPoint);

                    //the valid available neighbouring vertices
                    var validChoices = neighbours.Where(potentialVertex =>
                        !isConnected(maze, endPoint, potentialVertex)
                        & visited.ContainsKey(potentialVertex)).ToList();

                    //chooses a random vertex from the available surrounding vertices
                    var targetVertex = validChoices[r.Next(validChoices.Count)];


                    //draws the edge and the target vertices in green

                    var wall = RecursiveBacktrackerAlgorithm.pointMidPoint(targetVertex, endPoint);
                    Program.printPoint(wall);

                    Program.printPoint(targetVertex);

                    (Point, Point) verticesToAdd = (wall, targetVertex);


                    //gets new surrounding vertices with the target vertex as the centre
                    neighbours = returnVertices(targetVertex);

                    //adjacent nodes from the target vertex; the edges that connect the target vertex to the adjacent vertices
                    var connectedVertices = neighbours.Where(potentialVertex => 
                        maze.Contains(potentialVertex)).ToList();

                    //run depth first search to see if there is a loop in the maze coming from one of the adjacent vertices
                    //sets the root equal to come of the adjacent vertices
                    var root = connectedVertices.First();


                    //create a temporary maze that doesnt include the target node so that the search is forced down a single path
                    var tempMaze = maze;

                    tempMaze.Remove(targetVertex);

                    //create a visited dictionary
                    var d = maze.ToDictionary(v => v, v => false);

                    //return the depth first search result
                    var isconnected = connectedVertex(tempMaze, root, endPoint, d);

                    var pointToBeRemoved = root;

                    //deciding which adjacent vertex to remove
                    if (!isconnected)
                    {
                        pointToBeRemoved = connectedVertices.Last();
                    }

                    //removing the vertex
                    Program.setBothColours(ConsoleColor.Black);
                    Program.printPoint(pointToBeRemoved);
                    maze.Remove(pointToBeRemoved);

                    //get the new point to begin the random walk from
                    neighbours = Program.returnAdjacentVertices(maze, pointToBeRemoved);
                    
                    foreach (var v in neighbours.Where(v => !v.Equals(targetVertex)))
                    {
                        currentV = v;
                        break;
                    }

                    visited[currentV] = true;

                    maze.Add(verticesToAdd.Item1);
                    maze.Add(verticesToAdd.Item2);


                }

            }

            var startAndEnd = returnStartAndGoal(maze);

            Program.setBothColours(ConsoleColor.Green);
            Program.printPoint(startAndEnd.Item1);
            Program.setBothColours(ConsoleColor.Red);
            Program.printPoint(startAndEnd.Item2);

            Console.ReadKey();

            return maze;

        }

        private static (Point, Point) returnStartAndGoal(HashSet<Point> maze)
        {
            (Point, Point) returnTuple = (Point.Empty, Point.Empty);


            foreach (Point v in maze)
            {
                if (DeadendSolver.isDeadend(maze, v))
                {
                    if (returnTuple.Item1.IsEmpty)
                    {
                        returnTuple.Item1 = v;
                    }
                    else
                    {
                        returnTuple.Item2 = v;
                    }
                }
            }


            return returnTuple;
        }

        private static bool connectedVertex(HashSet<Point> maze, Point v, Point target, Dictionary<Point, bool> visited)
        {
            visited[v] = true;
            if (v.Equals(target))
            {
                return true;
            }


            foreach (Point neighbour in Program.returnAdjacentVertices(maze, v))
            {
                if (!visited[neighbour])
                {
                    return connectedVertex(maze, neighbour, target, visited);
                }
            }

            return false;
        }


        private static List<Point> returnPotentialOpposites(Point current, Dictionary<Point, bool> visited)
        {
            var list =  new List<Point>
            {
                new Point(current.X + 4, current.Y),
                new Point(current.X - 4, current.Y),
                new Point(current.X, current.Y + 2),
                new Point(current.X, current.Y - 2)
            };

            var valid = new List<Point>();

            foreach (var v in list)
            {
                if (visited.ContainsKey(v))
                {
                    valid.Add(v);
                }
            }

            return valid;
        }

        private static List<Point> returnAdjacentVertices(Point current) =>
            new List<Point>
            {
                new Point(current.X+4,current.Y),
                new Point(current.X-4,current.Y),
                new Point(current.X,current.Y+2),
                new Point(current.X,current.Y-2)
            };
        private static List<Point> returnVertices(Point current) =>
            new List<Point>
            {
                new Point(current.X+2,current.Y),
                new Point(current.X-2,current.Y),
                new Point(current.X,current.Y+1),
                new Point(current.X,current.Y-1)
            };
        private static bool isConnected(HashSet<Point> maze, Point currentPoint, Point newPoint) => 
            maze.Contains(RecursiveBacktrackerAlgorithm.pointMidPoint(currentPoint, newPoint));
    }
}
