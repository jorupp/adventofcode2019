using System;
using System.Collections.Generic;
using System.Linq;
using AoC.GraphSolver;

namespace AoC.Year2015.Day22
{
    public class Part2GameNode : GameNode
    {
        public Part2GameNode()
        {
        }

        public Part2GameNode(GameNode parent, string change) : base(parent, change)
        {
            if (IsPlayerTurn)
            {
                this.PlayerHP--;
            }
        }

        public override GameNode CreateChild(GameNode parent, string change) => new Part2GameNode(parent, change);
    }

    public class GameNode : Node<GameNode>
    {
        public int PlayerMana;
        public int PlayerHP;
        public int BossHP;
        public int BossDmg;
        public int MoveCount;
        public int ShieldTurnsLeft;
        public int PoisonTurnsLeft;
        public int RechargeTurnsLeft;
        public int SpentMana;
        public bool ShieldActive;
        public bool PoisonActive;
        public bool RechargeActive;
        public bool IsPlayerTurn;
        public Tuple<GameNode, string> Parent;
        public GameNode() { }
        public GameNode(GameNode parent, string change)
        {
            PlayerMana = parent.PlayerMana;
            PlayerHP = parent.PlayerHP;
            BossHP = parent.BossHP;
            BossDmg = parent.BossDmg;
            MoveCount = parent.MoveCount + 1;
            SpentMana = parent.SpentMana;
            IsPlayerTurn = !parent.IsPlayerTurn;
            Parent = new Tuple<GameNode, string>(parent, change);
            if (parent.ShieldTurnsLeft >= 1)
            {
                this.ShieldTurnsLeft = parent.ShieldTurnsLeft - 1;
                this.ShieldActive = true;
            }
            if (parent.PoisonTurnsLeft >= 1)
            {
                this.PoisonTurnsLeft = parent.PoisonTurnsLeft - 1;
                this.BossHP -= 3;
            }
            if (parent.RechargeTurnsLeft >= 1)
            {
                this.RechargeTurnsLeft = parent.RechargeTurnsLeft - 1;
                this.PlayerMana += 101;
            }
        }

        public virtual GameNode CreateChild(GameNode parent, string change) => new GameNode(parent, change);

        public string LastAction { get { return Parent == null ? null : Parent.Item2; } }
        public override bool IsValid { get { return PlayerHP > 0 && PlayerMana >= 0; } }
        public override bool IsComplete { get { return BossHP <= 0; } }
        public override decimal CurrentCost { get { return SpentMana + (Parent != null && Parent.Item2 == "Do nothing" ? 1 : 0); } }
        public override decimal EstimatedCost { get { return CurrentCost + Math.Max(0, (BossHP * 9)); } } // most efficient damage to boss is 9 mana per HP
        public override object[] Keys { get { return new object[] { PlayerMana, PlayerHP, BossHP, BossDmg, CurrentCost, ShieldTurnsLeft, PoisonTurnsLeft, RechargeTurnsLeft, ShieldActive, PoisonActive, RechargeActive, IsPlayerTurn }; } }
        public override string Description
        {
            get
            {
                var states = this.SelectDeep(s => s == null || s.Parent == null || s.Parent.Item1 == null ? new GameNode[0] : new[] { s.Parent.Item1 }).ToList();
                var moves = states.Where(i => i.Parent != null && i.Parent.Item2 != null).Where(i => i.IsPlayerTurn).Select(i => i.Parent.Item2).Reverse().ToList();
                return string.Join(", ", moves);
            }
        }

        public override IEnumerable<GameNode> GetAdjacent()
        {
            if (this.IsPlayerTurn)
            {
                // now time for the boss to move
                var bossDead = this.CreateChild(this, "Boss dies before attacking");
                if (bossDead.IsComplete)
                {
                    yield return bossDead;
                }
                // boss didn't auto-die, let him attack
                var next = this.CreateChild(this, "Boss attacks");
                next.PlayerHP -= Math.Max(1, next.BossDmg - (next.ShieldActive ? 7 : 0));
                yield return next;
            }
            else
            {
                // ok, player move - we have 5 choices...
                {
                    var next = this.CreateChild(this, "Boss dies before player attacks");
                    if (next.IsValid && next.IsComplete)
                    {
                        // in case the boss died from poison before we acted
                        yield return next;
                    }
                }
                {
                    var next = this.CreateChild(this, "Magic missle");
                    next.PlayerMana -= 53;
                    next.SpentMana += 53;
                    next.BossHP -= 4;
                    yield return next;
                }
                {
                    var next = this.CreateChild(this, "Drain");
                    next.PlayerMana -= 73;
                    next.SpentMana += 73;
                    next.BossHP -= 2;
                    next.PlayerHP += 2;
                    yield return next;
                }
                if (this.ShieldTurnsLeft <= 1)
                {
                    var next = this.CreateChild(this, "Shield");
                    next.PlayerMana -= 113;
                    next.SpentMana += 113;
                    next.ShieldTurnsLeft = 6;
                    yield return next;
                }
                if (this.PoisonTurnsLeft <= 1)
                {
                    var next = this.CreateChild(this, "Poison");
                    next.PlayerMana -= 173;
                    next.SpentMana += 173;
                    next.PoisonTurnsLeft = 6;
                    yield return next;
                }
                if (this.RechargeTurnsLeft <= 1)
                {
                    var next = this.CreateChild(this, "Recharge");
                    next.PlayerMana -= 229;
                    next.SpentMana += 229;
                    next.RechargeTurnsLeft = 5;
                    yield return next;
                }
            }
        }
    }

}
