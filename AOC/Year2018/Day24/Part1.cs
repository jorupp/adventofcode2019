using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2018.Day24
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, Team[] teams)
        {
            RunScenario(title, () =>
            {
                foreach (var team in teams)
                {
                    for (var i = 0; i < team.Groups.Length; i++)
                    {
                        team.Groups[i].Name = $"{team.Name} group {i + 1}";
                    }
                }

                while (true)
                {
                    foreach (var team in teams)
                    {
                        Console.WriteLine(team.Name);
                        foreach (var group in team.Groups)
                        {
                            if (!group.IsAlive)
                                continue;
                            Console.WriteLine($"{group.Name} contains {group.Units} units");
                        }
                    }

                    // exit if one won
                    if (teams.Count(t => t.Groups.Count(g => g.IsAlive) > 0) < 2)
                        break;

                    // target selection
                    
                    // During the target selection phase, each group attempts to choose one target.
                    // In decreasing order of effective power, groups choose their targets;
                    // in a tie, the group with the higher initiative chooses first.
                    // The attacking group chooses to target the group in the enemy army to which it would deal the most damage
                    // (after accounting for weaknesses and immunities, but not accounting for whether the defending group has
                    // enough units to actually receive all of that damage).
                    // If an attacking group is considering two defending groups to which it would deal equal damage,
                    // it chooses to target the defending group with the largest effective power; if there is still a tie,
                    // it chooses the defending group with the highest initiative. If it cannot deal any defending groups damage,
                    // it does not choose a target. Defending groups can only be chosen as a target by one attacking group.

                    var targets = new Dictionary<Group, Group>();

                    void FindTargets(Team attacker, Team defender)
                    {
                        var attackingGroups = attacker.Groups.Where(i => i.IsAlive).OrderByDescending(i => i.EffectivePower)
                            .ThenByDescending(i => i.Initiative).ToArray();

                        foreach (var ag in attackingGroups)
                        {
                            var defenders = defender.Groups.Where(i => i.IsAlive && !targets.Values.Contains(i))
                                .Select(i => new {i, dmg = ag.GetDamageWouldDeal(i)})
                                .OrderByDescending(i => i.dmg).ThenByDescending(i => i.i.EffectivePower).ThenByDescending(i => i.i.Initiative)
                                .ToArray();

                            foreach (var d in defenders)
                            {
                                Console.WriteLine($"{ag.Name} would deal {d.i.Name} {d.dmg} damage");
                            }

                            var dg = defenders.FirstOrDefault(i => i.dmg > 0)?.i;
                            targets.Add(ag, dg);
                        }
                    }

                    FindTargets(teams[0], teams[1]);
                    FindTargets(teams[1], teams[0]);

                    // attacking
                    // During the attacking phase, each group deals damage to the target it selected, if any.
                    // Groups attack in decreasing order of initiative, regardless of whether they are part of the infection or the immune system.
                    // (If a group contains no units, it cannot attack.)

                    // maybe we delay kills?
                    //var delayedKills = new Dictionary<Group, int>();
                    foreach (var pair in targets.OrderByDescending(i => i.Key.Initiative))
                    {
                        if(!pair.Key.IsAlive)
                            continue;
                        if (pair.Value == null)
                            continue;
                        var dmg = pair.Key.GetDamageWouldDeal(pair.Value);
                        var kills = Math.Min(pair.Value.Units, dmg / pair.Value.HitPoints);
                        //delayedKills.Add(pair.Value, kills);
                        pair.Value.Units -= kills;

                        Console.WriteLine($"{pair.Key.Name} attacks {pair.Value.Name}, killing {kills} units");
                    }

                    //foreach (var dk in delayedKills)
                    //{
                    //    dk.Key.Units -= dk.Value;
                    //}

                }


                //var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine(teams.Sum(t => t.Groups.Where(g => g.IsAlive).Sum(g => g.Units)));
            });
        }

        public class Team
        {
            public string Name;
            public Group[] Groups;
        }

        public class Group
        {
            public int Units;
            public int HitPoints;
            public DamageType[] WeakTo;
            public DamageType[] ImmuneTo;
            public DamageType Type;
            public int Damage;
            public int Initiative;
            public int EffectivePower => Units * Damage;
            public bool IsAlive => Units > 0;
            public string Name;

            public int GetDamageWouldDeal(Group target)
            {
                if (target.ImmuneTo.Contains(this.Type))
                    return 0;
                if (target.WeakTo.Contains(this.Type))
                    return 2 * this.EffectivePower;
                return this.EffectivePower;
            }
        }

        public enum DamageType
        {
            radiation,
            bludgeoning,
            fire,
            slashing,
            cold,
        }

        public override void Run()
        {
            // yes, I didn't want to write a parser for this... so sue me
            RunScenario("initial", new []
            {  
                new Team
                {
                    Name = "Immune System",
                    Groups = new []
                    {
//17 units each with 5390 hit points (weak to radiation, bludgeoning) with
// an attack that does 4507 fire damage at initiative 2
                        new Group() { 
                            Units = 17, 
                            HitPoints = 5390, 
                            WeakTo = new []
                            {
                                DamageType.bludgeoning,
                                DamageType.radiation
                            },
                            ImmuneTo = new DamageType[0],
                            Type = DamageType.fire,
                            Damage = 4507,
                            Initiative = 2,
                        }, 
//989 units each with 1274 hit points (immune to fire; weak to bludgeoning,
// slashing) with an attack that does 25 slashing damage at initiative 3    
                        new Group() { 
                            Units = 989, 
                            HitPoints = 1274, 
                            WeakTo = new []
                            {
                                DamageType.bludgeoning,
                                DamageType.slashing
                            },
                            ImmuneTo = new []
                            {
                                DamageType.fire
                            },
                            Type = DamageType.slashing,
                            Damage = 25,
                            Initiative = 3,
                        }, 
                    }
                },
                
                new Team
                {
                    Name = "Infection",
                    Groups = new []
                    {
//801 units each with 4706 hit points (weak to radiation) with an attack
// that does 116 bludgeoning damage at initiative 1
                        new Group() { 
                            Units = 801, 
                            HitPoints = 4706, 
                            WeakTo = new []
                            {
                                DamageType.radiation
                            },
                            ImmuneTo = new DamageType[0],
                            Type = DamageType.bludgeoning,
                            Damage = 116,
                            Initiative = 1,
                        }, 
//4485 units each with 2961 hit points (immune to radiation; weak to fire,
// cold) with an attack that does 12 slashing damage at initiative 4
                        new Group() { 
                            Units = 4485, 
                            HitPoints = 2961, 
                            WeakTo = new []
                            {
                                DamageType.fire,
                                DamageType.cold
                            },
                            ImmuneTo = new []
                            {
                                DamageType.radiation
                            },
                            Type = DamageType.slashing,
                            Damage = 12,
                            Initiative = 4,
                        }, 
                    }
                },
            });

            //return;
            RunScenario("part1", new []
            {  
                new Team
                {
                    Name = "Immune System",
                    Groups = new []
                    {
//3916 units each with 3260 hit points with an attack that does 8 radiation damage at initiative 16
                        new Group() { 
                            Units = 3916, 
                            HitPoints = 3260, 
                            WeakTo = new DamageType[0],
                            ImmuneTo = new DamageType[0],
                            Damage = 8,
                            Type = DamageType.radiation,
                            Initiative = 16,
                        }, 
//4737 units each with 2664 hit points (immune to radiation, cold, bludgeoning) with an attack that does 5 slashing damage at initiative 13
                        new Group() { 
                            Units = 4737, 
                            HitPoints = 2664, 
                            WeakTo = new DamageType[0],
                            ImmuneTo = new []
                            {
                                DamageType.radiation,
                                DamageType.cold,
                                DamageType.bludgeoning,
                            },
                            Damage = 5,
                            Type = DamageType.slashing,
                            Initiative = 13,
                        }, 
//272 units each with 10137 hit points with an attack that does 331 slashing damage at initiative 10
                        new Group() { 
                            Units = 272, 
                            HitPoints = 10137, 
                            WeakTo = new DamageType[0],
                            ImmuneTo = new DamageType[0],
                            Damage = 331,
                            Type = DamageType.slashing,
                            Initiative = 10,
                        }, 
//92 units each with 2085 hit points (immune to fire) with an attack that does 223 bludgeoning damage at initiative 1
                        new Group() { 
                            Units = 92, 
                            HitPoints = 2085, 
                            WeakTo = new DamageType[0],
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.fire
                            },
                            Damage = 223,
                            Type = DamageType.bludgeoning,
                            Initiative = 1,
                        }, 
//126 units each with 11001 hit points (immune to bludgeoning; weak to cold, fire) with an attack that does 717 bludgeoning damage at initiative 8
                        new Group() { 
                            Units = 126, 
                            HitPoints = 11001, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.cold,
                                DamageType.fire,
                            },
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.bludgeoning
                            },
                            Damage = 717,
                            Type = DamageType.bludgeoning,
                            Initiative = 8,
                        }, 
//378 units each with 4669 hit points (immune to cold, slashing) with an attack that does 117 fire damage at initiative 17
                        new Group() { 
                            Units = 378, 
                            HitPoints = 4669, 
                            WeakTo = new DamageType[0],
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.cold,
                                DamageType.slashing
                            },
                            Damage = 117,
                            Type = DamageType.fire,
                            Initiative = 17,
                        }, 
//4408 units each with 11172 hit points (immune to slashing; weak to bludgeoning) with an attack that does 21 bludgeoning damage at initiative 5
                        new Group() { 
                            Units = 4408, 
                            HitPoints = 11172, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.bludgeoning
                            },
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.slashing,
                            },
                            Damage = 21,
                            Type = DamageType.bludgeoning,
                            Initiative = 5,
                        }, 
//905 units each with 11617 hit points (weak to fire) with an attack that does 100 fire damage at initiative 20
                        new Group() { 
                            Units = 905, 
                            HitPoints = 11617, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.fire
                            },
                            ImmuneTo = new DamageType[0],
                            Damage = 100,
                            Type = DamageType.fire,
                            Initiative = 20,
                        }, 
//3574 units each with 12385 hit points (weak to bludgeoning; immune to radiation) with an attack that does 27 radiation damage at initiative 19
                        new Group() { 
                            Units = 3574, 
                            HitPoints = 12385, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.bludgeoning
                            },
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.radiation
                            },
                            Damage = 27,
                            Type = DamageType.radiation,
                            Initiative = 19,
                        }, 
//8186 units each with 3139 hit points (immune to bludgeoning, fire) with an attack that does 3 bludgeoning damage at initiative 9
                        new Group() { 
                            Units = 8186, 
                            HitPoints = 3139, 
                            WeakTo = new DamageType[0],
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.bludgeoning,
                                DamageType.fire
                            },
                            Damage = 3,
                            Type = DamageType.bludgeoning,
                            Initiative = 9,
                        }, 
                    }
                },
                
                new Team
                {
                    Name = "Infection",
                    Groups = new []
                    {
//273 units each with 26361 hit points (weak to slashing; immune to radiation) with an attack that does 172 radiation damage at initiative 18
                        new Group() { 
                            Units = 273, 
                            HitPoints = 26361, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.slashing
                            },
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.radiation
                            },
                            Damage = 172,
                            Type = DamageType.radiation,
                            Initiative = 18,
                        }, 
//536 units each with 44206 hit points (weak to fire, cold) with an attack that does 130 bludgeoning damage at initiative 12
                        new Group() { 
                            Units = 536, 
                            HitPoints = 44206, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.fire,
                                DamageType.cold
                            },
                            ImmuneTo = new DamageType[0],
                            Damage = 130,
                            Type = DamageType.bludgeoning,
                            Initiative = 12,
                        }, 
//1005 units each with 12555 hit points (immune to fire, radiation, bludgeoning) with an attack that does 24 radiation damage at initiative 6
                        new Group() { 
                            Units = 1005, 
                            HitPoints = 12555, 
                            WeakTo = new DamageType[0],
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.fire,
                                DamageType.radiation,
                                DamageType.bludgeoning
                            },
                            Damage = 24,
                            Type = DamageType.radiation,
                            Initiative = 6,
                        }, 
//2381 units each with 29521 hit points (immune to bludgeoning, radiation) with an attack that does 23 slashing damage at initiative 4
                        new Group() { 
                            Units = 2381, 
                            HitPoints = 29521, 
                            WeakTo = new DamageType[0],
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.bludgeoning,
                                DamageType.radiation,
                            },
                            Damage = 23,
                            Type = DamageType.slashing,
                            Initiative = 4,
                        }, 
//5162 units each with 54111 hit points (weak to radiation) with an attack that does 19 fire damage at initiative 2
                        new Group() { 
                            Units = 5162, 
                            HitPoints = 54111, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.radiation
                            },
                            ImmuneTo = new DamageType[0],
                            Damage = 19,
                            Type = DamageType.fire,
                            Initiative = 2,
                        }, 
//469 units each with 45035 hit points (weak to fire, slashing) with an attack that does 163 radiation damage at initiative 15
                        new Group() { 
                            Units = 469, 
                            HitPoints = 45035, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.fire,
                                DamageType.slashing
                            },
                            ImmuneTo = new DamageType[0],
                            Damage = 163,
                            Type = DamageType.radiation,
                            Initiative = 15,
                        }, 
//281 units each with 23265 hit points (weak to slashing; immune to bludgeoning) with an attack that does 135 radiation damage at initiative 11
                        new Group() { 
                            Units = 281, 
                            HitPoints = 23265, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.slashing
                            },
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.bludgeoning
                            },
                            Damage = 135,
                            Type = DamageType.radiation,
                            Initiative = 11,
                        }, 
//4350 units each with 46138 hit points (weak to fire) with an attack that does 18 bludgeoning damage at initiative 14
                        new Group() { 
                            Units = 4350, 
                            HitPoints = 46138, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.fire
                            },
                            ImmuneTo = new DamageType[0],
                            Damage = 18,
                            Type = DamageType.bludgeoning,
                            Initiative = 14,
                        }, 
//3139 units each with 48062 hit points (immune to bludgeoning, slashing, fire; weak to cold) with an attack that does 28 bludgeoning damage at initiative 3
                        new Group() { 
                            Units = 3139, 
                            HitPoints = 48062, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.cold
                            },
                            ImmuneTo = new DamageType[]
                            {
                                DamageType.bludgeoning,
                                DamageType.slashing,
                                DamageType.fire
                            },
                            Damage = 28,
                            Type = DamageType.bludgeoning,
                            Initiative = 3,
                        }, 
//9326 units each with 41181 hit points (weak to fire, bludgeoning) with an attack that does 8 cold damage at initiative 7
                        new Group() { 
                            Units = 9326, 
                            HitPoints = 41181, 
                            WeakTo = new DamageType[]
                            {
                                DamageType.fire,
                                DamageType.bludgeoning
                            },
                            ImmuneTo = new DamageType[0],
                            Damage = 8,
                            Type = DamageType.cold,
                            Initiative = 7,
                        }, 
                    }
                },
            });
        }
    }
}
