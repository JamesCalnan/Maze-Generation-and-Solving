using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace maze_generation_and_solving_csharp
{
    class KruskalsAlgorithm
    {
        public static HashSet<Point> generateMaze(int width, int height)
        {
            var availableVertices = RecursiveBacktrackerAlgorithm.ReturnDict(width, height).Keys.ToList();

            var maze = new HashSet<Point>();

            var usedVertices = new HashSet<Point>();

            var weights = BoruvkasAlgorithm.returnEdgeWeights(width, height, availableVertices);
            var V = BoruvkasAlgorithm.assignInitialSetNumbers(availableVertices);

            var mazeEntry = new Point(5, 2);
            maze.Add(mazeEntry);

            //pick random edge/lowest weight edge
            //combine set of connected trees IF NOT IN THE SAME SET
            //remove edge
            //repeat

            while (!BoruvkasAlgorithm.singleSet(V))
            {

                foreach (Point v in availableVertices)
                {

                    var lowestWeightEdge = BoruvkasAlgorithm.returnLowestWeightEdge(weights);

                    if (lowestWeightEdge.IsEmpty)
                    {
                        continue;
                    }

                    var connectedVertices = BoruvkasAlgorithm.returnNodes(availableVertices, lowestWeightEdge);

                    var setNumber1 = V[connectedVertices.First()];
                    var setNumber2 = V[connectedVertices.Last()];


                    if (setNumber1 != setNumber2)
                    {
                        V = BoruvkasAlgorithm.assignSetNumber(V, setNumber1, setNumber2);

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
                    weights.Remove(lowestWeightEdge);

                }

            }

            var mazeExit = RecursiveBacktrackerAlgorithm.returnExit(width, height);

            maze.Add(mazeExit);

            return maze;
        }
    }
}
