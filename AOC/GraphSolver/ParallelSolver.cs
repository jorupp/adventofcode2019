using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace AoC.GraphSolver
{
    public class ParallelSolver : ISolver
    {
        private readonly int _maxThreads;

        public ParallelSolver(int maxThreads)
        {
            _maxThreads = maxThreads;
        }

        public TNode Evaluate<TNode, TKey>(TNode start, TKey key) where TNode : Node<TNode, TKey>
        {
            TNode bestComplete = null;
            var bestNodes = new Dictionary<TKey, TNode>();
            var toEvaluate = new SimplePriorityQueue<TKey, decimal>();
            var evaluated = new HashSet<TKey>();

            bestNodes[start.Key] = start;
            toEvaluate.Enqueue(start.Key, start.EstimatedCost);

            while (true)
            {
                var tasks = new List<Task<List<TNode>>>();
                if (toEvaluate.Count == 0)
                {
                    //var x = bestNodes.Select(n => new { node = n.Value, next = n.Value.GetAdjacent().ToArray() })
                    //    .ToArray();
                    var bestLeft = bestNodes.Values.OrderBy(i => i.CurrentCost).First();
                    Console.WriteLine("Best of the rest....");
                    return bestLeft;
                }
                for (var i = 0; i < _maxThreads && toEvaluate.Count > 0; i++)
                {
                    var workKey = toEvaluate.Dequeue();
                    var work = bestNodes[workKey];
                    if (bestComplete != null && i == 0 && bestComplete.CurrentCost <= work.EstimatedCost)
                    {
                        return bestComplete;
                    }
                    evaluated.Add(work.Key);
                    tasks.Add(Task.Run(() =>
                    {
                        var results = new List<TNode>();
                        foreach (var next in work.GetAdjacent())
                        {
                            if (!next.IsValid)
                            {
                                continue;
                            }
                            var x = next.Key; // make sure the key is built
                            results.Add(next);
                        }
                        return results;
                    }));
                }
                foreach (var task in tasks)
                {
                    foreach (var next in task.Result)
                    {
                        if (next.IsComplete)
                        {
                            if (null == bestComplete || next.CurrentCost < bestComplete.CurrentCost)
                            {
                                // new best - remember it
                                bestComplete = next;
                            }
                            // no need to continue to evaluate complete nodes
                            continue;
                        }
                        if (bestNodes.TryGetValue(next.Key, out var existing))
                        {
                            // we've already seen this node - update the cost if better, but no need to process further
                            if (next.CurrentCost < existing.CurrentCost)
                            {
                                bestNodes[next.Key] = next;
                                toEvaluate.TryUpdatePriority(next.Key, next.EstimatedCost);
                            }
                            continue;
                        }
                        // never seen this node before - track it and queue it up
                        bestNodes.Add(next.Key, next);
                        toEvaluate.Enqueue(next.Key, next.EstimatedCost);
                    }
                }
            }
        }
    }
}
