using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace maze_generation_and_solving_csharp
{
    class PrimsAlgorithm
    {

        public static HashSet<Point> generateMaze(int width, int height)
        {

            var V = new HashSet<Point>();
            var discovered = RecursiveBacktrackerAlgorithm.ReturnDict(width, height);
            var maze = new HashSet<Point>();

            var weights = returnWeights(discovered.Keys.ToList());

            var startV = new Point(5, 3);
            var currentV = startV;

            discovered[startV] = true;

            var r = new Random();

            var mazeEntry = new Point(5, 2);
            maze.Add(mazeEntry);
            maze.Add(currentV);

            while (true)
            {
                var availableVertices = Program.returnUnvisitedNeighbours(discovered, currentV);

                foreach (var vertex in availableVertices.Where(vertex => !V.Contains(vertex)))
                {
                    V.Add(vertex);
                }

                if (V.Count == 0) { break; }

                var lowestWeightVertex = ExtractMinV(V, weights);

                var visitedNeighbours = returnVisitedNeighbours(discovered, lowestWeightVertex);

                var newVertex = visitedNeighbours[r.Next(visitedNeighbours.Count)];

                var wallV = RecursiveBacktrackerAlgorithm.pointMidPoint(newVertex, lowestWeightVertex);

                maze.Add(wallV);
                maze.Add(lowestWeightVertex);
                
                currentV = lowestWeightVertex;
                
                V.Remove(currentV);

                discovered[currentV] = true;

            }

            var mazeExit = RecursiveBacktrackerAlgorithm.returnExit(width, height);

            maze.Add(mazeExit);

            return maze;
        }
        public static List<Point> returnVisitedNeighbours(Dictionary<Point, bool> discovered, Point cV)
        {
            var returnList = new List<Point>();

            if (discovered.ContainsKey(nP(cV.X + 4, cV.Y)) && discovered[nP(cV.X + 4, cV.Y)])
            {
                returnList.Add(nP(cV.X + 4, cV.Y));
            }

            if (discovered.ContainsKey(nP(cV.X - 4, cV.Y)) && discovered[nP(cV.X - 4, cV.Y)])
            {
                returnList.Add(nP(cV.X - 4, cV.Y));
            }

            if (discovered.ContainsKey(nP(cV.X, cV.Y + 2)) && discovered[nP(cV.X, cV.Y + 2)])
            {
                returnList.Add(nP(cV.X, cV.Y + 2));
            }

            if (discovered.ContainsKey(nP(cV.X, cV.Y - 2)) && discovered[nP(cV.X, cV.Y - 2)])
            {
                returnList.Add(nP(cV.X, cV.Y - 2));
            }

            return returnList;
        }

        private static Point nP(int x, int y)
        {
            return new Point(x,y);
        }

        /// <summary>
        /// gets the vertex with the lowest weight in the hashset V
        /// </summary>
        private static Point ExtractMinV(HashSet<Point> V, Dictionary<Point, int> dist)
        {

            var sortedDictionary = from pair in dist orderby pair.Value ascending select pair;

            foreach (KeyValuePair<Point, int> pair in sortedDictionary)
            {
                if (V.Contains(pair.Key)) { return pair.Key; }
            }

            return V.First();
        }


        public static Dictionary<Point, int> returnWeights(List<Point> availableVertices)
        {
            return availableVertices.ToDictionary(v => v, v => new Random().Next());
        }



    }
}
