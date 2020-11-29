using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AoC.GraphSolver;

namespace AoC.Year2015.Day9
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var parsed = lines.Select(i => i.Split(' ')).ToList();
                var cities = parsed.SelectMany(i => new[] {i[0], i[2]}).Distinct().ToList();
                var edges = new Dictionary<string, Dictionary<string, int>>();

                void Add(string city1, string city2, int distance)
                {
                    if (!edges.ContainsKey(city1))
                    {
                        edges.Add(city1, new Dictionary<string, int>());
                    }

                    edges[city1][city2] = distance;
                }

                foreach (var p in parsed)
                {
                    Add(p[0], p[2], int.Parse(p[4]));
                    Add(p[2], p[0], int.Parse(p[4]));
                }

                var root = new GraphNode(null, new HashSet<string>(), cities, edges, 0);
                var solution = new RealSolver().Evaluate<GraphNode, string, int>(root);

                Console.WriteLine(solution.Description);
            });
        }

        public class GraphNode : Node<GraphNode, string, int>
        {
            private readonly string _currentCity;
            private readonly HashSet<string> _visited;
            private readonly List<string> _cities;
            private readonly Dictionary<string, Dictionary<string, int>> _edges;
            private readonly int _cost;

            public GraphNode(string currentCity, HashSet<string> visited, List<string> cities,
                Dictionary<string, Dictionary<string, int>> edges, int cost)
            {
                _currentCity = currentCity;
                _visited = visited;
                _cities = cities;
                _edges = edges;
                _cost = cost;
            }

            public override IEnumerable<GraphNode> GetAdjacent()
            {
                var newV = new HashSet<string>(_visited);
                if (_currentCity != null)
                {
                    newV.Add(_currentCity);
                }
                foreach (var city in _cities)
                {
                    if (city == _currentCity)
                        continue;
                    var cost = _cost;
                    if (_currentCity != null)
                    {
                        cost += _edges[_currentCity][city];
                    }

                    yield return new GraphNode(city, newV, _cities, _edges, cost);
                }
            }

            public override bool IsValid => !_visited.Contains(_currentCity);
            public override bool IsComplete => _visited.Count == _cities.Count - 1;
            // for part 2, we want the highest value, so we just invert our cost
            public override int CurrentCost => 10000 - _cost;
            public override int EstimatedCost => 10000 - _cost + _cities.Count - _visited.Count -1;
            protected override string GetKey()
            {
                return _currentCity + "-" + string.Join("-", _visited.OrderBy(i => i));
            }

            public override string Description => GetKey() + " => " + _cost;
        }

        public override void Run()
        {
            //return;
            RunScenario("example", @"London to Dublin = 464
London to Belfast = 518
Dublin to Belfast = 141");

            RunScenario("part1", @"AlphaCentauri to Snowdin = 66
AlphaCentauri to Tambi = 28
AlphaCentauri to Faerun = 60
AlphaCentauri to Norrath = 34
AlphaCentauri to Straylight = 34
AlphaCentauri to Tristram = 3
AlphaCentauri to Arbre = 108
Snowdin to Tambi = 22
Snowdin to Faerun = 12
Snowdin to Norrath = 91
Snowdin to Straylight = 121
Snowdin to Tristram = 111
Snowdin to Arbre = 71
Tambi to Faerun = 39
Tambi to Norrath = 113
Tambi to Straylight = 130
Tambi to Tristram = 35
Tambi to Arbre = 40
Faerun to Norrath = 63
Faerun to Straylight = 21
Faerun to Tristram = 57
Faerun to Arbre = 83
Norrath to Straylight = 9
Norrath to Tristram = 50
Norrath to Arbre = 60
Straylight to Tristram = 27
Straylight to Arbre = 81
Tristram to Arbre = 90");

        }
    }
}
