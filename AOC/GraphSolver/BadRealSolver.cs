using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace AoC.GraphSolver
{
    public class BadRealSolver
    {
        public TNode[] Evaluate<TNode, TKey>(TNode start, TKey key, Func<bool, TNode, TNode> chooseBetter) where TNode : Node<TNode, TKey>
        {
            var bestComplete = new List<TNode>();
            var bestNodes = new Dictionary<TKey, List<TNode>>();
            var toEvaluate = new SimplePriorityQueue<TKey, decimal>();
            var evaluated = new HashSet<TKey>();

            bestNodes[start.Key] = new List<TNode>() { start };
            toEvaluate.Enqueue(start.Key, start.EstimatedCost);

            while (true)
            {
                if (toEvaluate.Count == 0)
                {
                    //var x = bestNodes.Select(n => new { node = n.Value, next = n.Value.GetAdjacent().ToArray() })
                    //    .ToArray();
                    //var bestLeft = bestNodes.Values.OrderBy(i => i.CurrentCost).First();
                    //Console.WriteLine("Best of the rest....");
                    return bestComplete.ToArray();

                }
                var workKey = toEvaluate.Dequeue();
                var works = bestNodes[workKey];
                evaluated.Add(works[0].Key);
                foreach (var work in works)
                {
                    if (bestComplete.Any() && bestComplete[0].CurrentCost < work.EstimatedCost)
                    {
                        return bestComplete.ToArray();
                    }
                    foreach (var next in work.GetAdjacent())
                    {
                        if (!next.IsValid)
                        {
                            continue;
                        }
                        if (next.IsComplete)
                        {
                            if (!bestComplete.Any() || next.CurrentCost < bestComplete[0].CurrentCost)
                            {
                                // completely new best - remember it
                                bestComplete = new List<TNode>() { next };
                            } else if (next.CurrentCost == bestComplete[0].CurrentCost)
                            {
                                // new best - remember it
                                bestComplete.Add(next);
                            }
                            // no need to continue to evaluate complete nodes
                            continue;
                        }
                        if (bestNodes.TryGetValue(next.Key, out var existing))
                        {
                            // we've already seen this node - update the cost if better, but no need to process further
                            if (next.CurrentCost < existing[0].CurrentCost)
                            {
                                bestNodes[next.Key] = new List<TNode>() { next };
                                toEvaluate.TryUpdatePriority(next.Key, next.EstimatedCost);
                            }
                            else if(next.CurrentCost == existing[0].CurrentCost)
                            {
                                existing.Add(next);
                            }
                            continue;
                        }
                        // never seen this node before - track it and queue it up
                        bestNodes.Add(next.Key, new List<TNode>() { next });
                        toEvaluate.Enqueue(next.Key, next.EstimatedCost);
                    }
                }
            }
        }
    }
}
