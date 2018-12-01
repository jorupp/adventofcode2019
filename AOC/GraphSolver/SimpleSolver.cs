using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.GraphSolver
{
    public class SimpleSolver : ISolver
    {
        public TNode Evaluate<TNode, TKey>(TNode start, TKey key) where TNode : Node<TNode, TKey>
        {
            var evaluated = new Dictionary<TKey, TNode>();
            var toEvaluate = new Dictionary<TKey, TNode>();
            toEvaluate[start.Key] = start;

            while (true)
            {
                var minCompleteCost = evaluated.Where(i => i.Value.IsComplete).Min(i => (int?)i.Value.CurrentCost);
                if (minCompleteCost.HasValue)
                {
                    if (!toEvaluate.Any(i => i.Value.EstimatedCost < minCompleteCost.Value))
                    {
                        // our smallest complete current cost is less than the cost of everything we haven't checked yet, so we're guaranteed optimal
                        break;
                    }
                }

                if (toEvaluate.Count == 0)
                {
                    // error - should never happen
                    throw new Exception("No solution found");
                }

                var work = toEvaluate.OrderBy(i => i.Value.EstimatedCost).First().Value;
                toEvaluate.Remove(work.Key);
                evaluated.Add(work.Key, work);
                if (!work.IsComplete)
                {
                    foreach (var next in work.GetAdjacent())
                    {
                        if (!next.IsValid)
                        {
                            continue;
                        }
                        if (evaluated.ContainsKey(next.Key))
                        {
                            if (evaluated[next.Key].CurrentCost > next.CurrentCost || evaluated[next.Key].EstimatedCost > next.EstimatedCost)
                            {
                                throw new ArgumentException();
                            }
                            continue;
                        }
                        if (toEvaluate.ContainsKey(next.Key))
                        {
                            if (toEvaluate[next.Key].CurrentCost > next.CurrentCost)
                            {
                                toEvaluate[next.Key] = next;
                            }
                            continue;
                        }
                        toEvaluate.Add(next.Key, next);
                    }
                }
            }

            return evaluated.Where(i => i.Value.IsComplete).OrderBy(i => i.Value.CurrentCost).First().Value;
        }
    }
}
