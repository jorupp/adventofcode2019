using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace AoC.GraphSolver
{
    public class RealSolver
    {
        public TNode Evaluate<TNode, TKey, TCost>(TNode start, Func<TNode, TNode, bool> isBetter = null) where TNode : Node<TNode, TKey, TCost> where TCost : IComparable<TCost>
        {
            return Evaluate<TNode, TKey, TCost>(new[] {start}, isBetter);
        }

        public TNode Evaluate<TNode, TKey, TCost>(TNode[] startNodes, Func<TNode, TNode, bool> isBetter = null, Action<TNode> evaluateNode = null, Action<Dictionary<TKey, TNode>, SimplePriorityQueue<TKey, TCost>, HashSet<TKey>> whenDone = null) where TNode : Node<TNode, TKey, TCost> where TCost : IComparable<TCost>
        {
            if (null == isBetter)
            {
                isBetter = (i1, i2) => false;
            }
            if (null == evaluateNode)
            {
                evaluateNode = i => { };
            }
            if (null == whenDone)
            {
                whenDone = (a, b, c) => { };
            }

            TNode bestComplete = null;
            var bestNodes = new Dictionary<TKey, TNode>();
            var toEvaluate = new SimplePriorityQueue<TKey, TCost>();
            var evaluated = new HashSet<TKey>();

            foreach (var start in startNodes)
            {
                bestNodes[start.Key] = start;
                toEvaluate.Enqueue(start.Key, start.EstimatedCost);
            }

            while (true)
            {
                if (toEvaluate.Count == 0)
                {
                    //var x = bestNodes.Select(n => new { node = n.Value, next = n.Value.GetAdjacent().ToArray() })
                    //    .ToArray();
                    whenDone(bestNodes, toEvaluate, evaluated);
                    return bestComplete;
                    //if (null != bestComplete)
                    //{
                    //    return bestComplete;
                    //}
                    //var bestLeft = bestNodes.Values.OrderBy(i => i.CurrentCost).First();
                    //Console.WriteLine("Best of the rest....");
                    //return bestLeft;

                }
                var workKey = toEvaluate.Dequeue();
                var work = bestNodes[workKey];
                if (bestComplete != null && bestComplete.CurrentCost.CompareTo(work.EstimatedCost) < 0)
                {
                    whenDone(bestNodes, toEvaluate, evaluated);
                    return bestComplete;
                }
                evaluated.Add(work.Key);
                foreach (var next in work.GetAdjacent())
                {
                    if (!next.IsValid)
                    {
                        continue;
                    }

                    evaluateNode(next);
                    if (next.IsComplete)
                    {
                        if (null == bestComplete || next.CurrentCost.CompareTo(bestComplete.CurrentCost) < 0 || (next.CurrentCost.CompareTo(bestComplete.CurrentCost) == 0 && isBetter(next, bestComplete)))
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
                        var compare = next.CurrentCost.CompareTo(existing.CurrentCost);
                        if (compare < 0 || (compare == 0 && isBetter(next, existing)))
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
