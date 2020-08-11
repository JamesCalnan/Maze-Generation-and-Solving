using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace maze_generation_and_solving_csharp
{
    class SPFA
    {

        public static void solveMaze(HashSet<Point> maze)
        {
            var root = maze.First();
            var goal = maze.Last();

            var d = new Dictionary<Point, float>();
            var Q = new SQFAQueue<Point>();
            var prev = new Dictionary<Point, Point>();

            foreach (Point v in maze)
            {
                d.Add(v, Int32.MaxValue);
                prev.Add(v, Point.Empty);
            }

            d[root] = 0;
            Q.push(root);

            Program.setBothColours(ConsoleColor.Red);

            while (!Q.isEmpty())
            {

                var u = Q.poll();

                if (u.Equals(goal))
                {
                    Program.findPath(goal, root, prev);
                    break;
                }

                foreach (Point v in Program.returnAdjacentVertices(maze, u))
                {

                    if (d[u] + 1 < d[v])
                    {
                        d[v] = d[u] + 1;
                        if (!Q.contains(v))
                        {
                            Q.pushSLF(v,d);
                            prev[v] = u;
                        }
                    } 
                }
            }
        }

    }

    class SQFAQueue<T>
    {
        private Queue<T> items;

        public SQFAQueue()
        {
            this.items = new Queue<T>();
        }


        public void push(T item)
        {
            items.Enqueue(item);
        }
        public bool contains(T item)
        {
            return items.Contains(item);
        }
        public void pushLLF(T item, Dictionary<T, float> d)
        {
            items.Enqueue(item);
            var x = d.Values.Average();

            while (d[items.First()] > x)
            {
                var u = items.Dequeue();
                items.Reverse();
                items.Enqueue(u);
                items.Reverse();
            }

        }

        public void pushSLF(T item, Dictionary<T, float> d)
        {
            items.Enqueue(item);

            if (d[items.Last()] < d[items.First()])
            {
                var u = items.Dequeue();
                items.Reverse();
                items.Enqueue(u);
                items.Reverse();
            }
        }

        public T poll()
        {
            return items.Dequeue();
        }

        public bool isEmpty()
        {
            return items.Count == 0;
        }

    }


}
