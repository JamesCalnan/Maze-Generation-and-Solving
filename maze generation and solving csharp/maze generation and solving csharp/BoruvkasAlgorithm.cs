using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace maze_generation_and_solving_csharp
{
    class BoruvkasAlgorithm
    {

        public static HashSet<Point> generateMaze(int width, int height)
        {
            var availableVertices = RecursiveBacktrackerAlgorithm.ReturnDict(width, height).Keys.ToList();

            var maze = new HashSet<Point>();

            var usedVertices = new HashSet<Point>();

            var weights = returnEdgeWeights(width, height, availableVertices);
            var V = assignInitialSetNumbers(availableVertices);

            var mazeEntry = new Point(5, 2);
            maze.Add(mazeEntry);

            //pick random edge/lowest weight edge
            //combine set of connected trees IF NOT IN THE SAME SET
            //remove edge
            //repeat

            while (!singleSet(V))
            {

                foreach (Point v in availableVertices)
                {

                    var lowestWeightEdge = returnLowestAdjacentWeightEdge(weights, v);

                    if (lowestWeightEdge.IsEmpty)
                    {
                        continue;
                    }

                    var connectedVertices = returnNodes(availableVertices, lowestWeightEdge);

                    var setNumber1 = V[connectedVertices.First()];
                    var setNumber2 = V[connectedVertices.Last()];


                    if (setNumber1 != setNumber2)
                    {
                        V = assignSetNumber(V, setNumber1, setNumber2);

                        weights.Remove(lowestWeightEdge);

                        weights.Remove(lowestWeightEdge);

                        if (!maze.Contains(connectedVertices.First()))
                        {
                            maze.Add(connectedVertices.First());
                        }
                        if (!maze.Contains(connectedVertices.Last()))
                        {
                            maze.Add(connectedVertices.Last());
                        }
                        maze.Add(lowestWeightEdge);
                    }
                    

                }
                
            }

            var mazeExit = RecursiveBacktrackerAlgorithm.returnExit(width, height);

            maze.Add(mazeExit);


            return maze;
        }

        public static Dictionary<Point, int> assignInitialSetNumbers(List<Point> availableVertices)
        {
            var i = 0;
            var returnDictionary = new Dictionary<Point, int>();

            foreach (var v in availableVertices)
            {
                returnDictionary.Add(v, i);
                i++;
            }


            return returnDictionary;
        }


        internal static Point nP(int x, int y)
        {
            return new Point(x, y);
        }

        public static List<Point> returnNodes(List<Point> availableVertices, Point edge)
        {

            

            var vertices = new List<Point>();

            var attemptVertex = nP(edge.X + 2, edge.Y);

            if (availableVertices.Contains(attemptVertex))
            {
                vertices.Add(attemptVertex);
            }

            attemptVertex = nP(edge.X - 2, edge.Y);
            if (availableVertices.Contains(attemptVertex))
            {

                vertices.Add(attemptVertex);
            }

            attemptVertex = nP(edge.X, edge.Y + 1);
            if (availableVertices.Contains(attemptVertex))
            {
                vertices.Add(attemptVertex);
            }

            attemptVertex = nP(edge.X, edge.Y - 1);
            if (availableVertices.Contains(attemptVertex))
            {
                vertices.Add(attemptVertex);
            }


            return vertices;
        }

        public static Dictionary<Point, int> assignSetNumber(Dictionary<Point, int> setDictionary, int setToBeChanged, int newSet)
        { 

            var returnDictionary = setDictionary;

            var cellToChangeSet = (from cV in setDictionary where cV.Value == setToBeChanged select cV.Key).ToList();

            foreach (var vPoint in cellToChangeSet)
            {
                returnDictionary[vPoint] = newSet;
            }

            return returnDictionary;
        }

        private static Point returnLowestAdjacentWeightEdge(Dictionary<Point, int> weights, Point cV)
        {
            var returnPoint = new Point();

            var availablePoints = new List<Point>
            {
                new Point(cV.X + 2,cV.Y),
                new Point(cV.X - 2,cV.Y),
                new Point(cV.X,cV.Y + 1),
                new Point(cV.X,cV.Y - 1)
            };

            var validEdges = availablePoints.Where(potentialV => weights.Keys.ToHashSet().Contains(potentialV)).ToDictionary(potentialV => potentialV, potentialV => weights[potentialV]);

            if (validEdges.Count == 0)
            {
                return Point.Empty;
            }

            var sortedDictionary = from pair in validEdges orderby pair.Value ascending select pair;

            returnPoint = sortedDictionary.First().Key;

            return returnPoint;
        }

        public static Point returnLowestWeightEdge(Dictionary<Point, int> weights)
        {
            var returnPoint = new Point();

            var sortedDictionary = from pair in weights orderby pair.Value ascending select pair;

            if (sortedDictionary.ToList().Count == 0)
            {
                return Point.Empty;
            }

            returnPoint = sortedDictionary.First().Key;

            return returnPoint;
        }

        public static Dictionary<Point, int> returnEdgeWeights(int width, int height, List<Point> availablePoints)
        {
            var r = new Random();

            var returnDictionary = new Dictionary<Point, int>();

            int greatestX = 0;
            int greatestY = 0;
            foreach (var v in availablePoints)
            {
                if (v.X > greatestX)
                {
                    greatestX = v.X;
                }
                if (v.Y > greatestY)
                {
                    greatestY = v.Y;
                }
            }

            for (int x = 5; x <= width; x += 4)
            {
                for (int y = 3; y < height + 1; y += 2)
                {
                    if (x < greatestX)
                    {
                        returnDictionary.Add(new Point(x + 2, y), r.Next());
                    }
                    if (y < greatestY)
                    {
                        returnDictionary.Add(new Point(x, y + 1), r.Next());
                    }
                }
            }

            return returnDictionary;
        }

        public static bool singleSet(Dictionary<Point, int> setDictionary)
        {

            var valueList = setDictionary.Values.ToList();


            return !valueList.Distinct().Skip(1).Any();
        }
    }
}