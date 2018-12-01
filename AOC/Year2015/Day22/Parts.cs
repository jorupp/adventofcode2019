using System;
using AoC.GraphSolver;

namespace AoC.Year2015.Day22
{
    public abstract class SharedPart : BasePart
    {
        protected void RunScenario(string name, GameNode initialState)
        {
            RunScenario($"{name} - simple", new SimpleSolver(), initialState);
            RunScenario($"{name} - real", new RealSolver(), initialState);
        }

        private void RunScenario(string name, ISolver solver, GameNode initialState)
        {
            RunScenario(name, () =>
            {
                var finalState = solver.Evaluate(initialState, initialState.Key);
                Console.WriteLine($"{name} - SpentMana: {finalState.SpentMana}");
                //var states = finalState.SelectDeep(s => s == null || s.Parent == null || s.Parent.Item1 == null ? new GameNode[0] : new[] { s.Parent.Item1 }).ToList();
                //var moves = states.Where(i => i.Parent != null && i.Parent.Item2 != null).Select(i => string.Format("{0} - {1} -> {2} - {3} {4} {5} - {6} {7}", i.Parent.Item2, i.PlayerMana, i.SpentMana, i.ShieldTurnsLeft, i.PoisonTurnsLeft, i.RechargeTurnsLeft, i.PlayerHP, i.BossHP)).Reverse().ToList();
                //moves.ForEach(i => Console.WriteLine(i));
            });
        }
    }

    public class Part1 : SharedPart
    {
        public override void Run()
        {
            RunScenario("Rupp part 1 (953)", new GameNode() { PlayerHP = 50, PlayerMana = 500, BossHP = 55, BossDmg = 8, IsPlayerTurn = false }); // Rupp puzzle, should be 953
        }
    }

    public class Part2 : SharedPart
    {
        public override void Run()
        {
            RunScenario("Rupp part 2 (1289)", new Part2GameNode() { PlayerHP = 50, PlayerMana = 500, BossHP = 55, BossDmg = 8, IsPlayerTurn = false }); // Rupp puzzle, should be 1289 - was getting 1295
            RunScenario("Sean part 2 (1309)", new Part2GameNode() { PlayerHP = 50, PlayerMana = 500, BossHP = 58, BossDmg = 9, IsPlayerTurn = false }); // Sean puzzle, should be 1309
        }
    }

}
