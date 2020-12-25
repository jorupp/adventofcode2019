using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2020.Day24
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);

                var directions = new Dictionary<string, (int, int, int)>
            {
                { "e", (1, -1, 0) },
                { "se", (0, -1, 1) },
                { "sw", (-1, 0, 1) },
                { "w", (-1, 1, 0) },
                { "nw", (0, 1, -1) },
                { "ne", (1, 0, -1) },
            };

                // false == white, true == black
                var status = new Dictionary<(int, int, int), bool>();
                var re = new Regex(@"e|se|sw|w|nw|ne");
                foreach (var line in lines)
                {
                    var coord = (0, 0, 0);
                    var ms = re.Matches(line);
                    foreach (Match m in ms)
                    {
                        var move = directions[m.Value];
                        coord.Item1 += move.Item1;
                        coord.Item2 += move.Item2;
                        coord.Item3 += move.Item3;
                    }

                    var current = status.TryGetValue(coord, out var x) ? x : false;
                    status[coord] = !current;
                }

                Console.WriteLine($"Flipped to black: {status.Values.Count(i => i)}");


                for (var i = 0; i < 100; i++)
                {
                    var newStatus = new Dictionary<(int, int, int), bool>();
                    var sx = status.Keys.Min(i => i.Item1) - 1;
                    var ex = status.Keys.Max(i => i.Item1) + 1;
                    var sy = status.Keys.Min(i => i.Item2) - 1;
                    var ey = status.Keys.Max(i => i.Item2) + 1;
                    var sz = status.Keys.Min(i => i.Item2) - 1;
                    var ez = status.Keys.Max(i => i.Item2) + 1;

                    // false == white, true == black
                    //Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white.
                    //Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black.

                    for (var x = sx; x <= ex; x++)
                    {
                        for (var y = sy; y <= ey; y++)
                        {
                            var z = 0 - x - y;

                            var current = status.TryGetValue((x, y, z), out var v) ? v : false;
                            var adjacent = 0;
                            foreach (var dir in directions.Values)
                            {
                                var key = (x + dir.Item1, y + dir.Item2, z + dir.Item3);
                                var av = status.TryGetValue(key, out var v2) ? v2 : false;
                                if (av)
                                {
                                    adjacent++;
                                }
                            }

                            if (current)
                            {
                                //Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white.
                                if(adjacent == 0 || adjacent > 2)
                                {

                                }
                                else
                                {
                                    newStatus[(x, y, z)] = true;
                                }
                            } else
                            {
                                //Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black.
                                if(adjacent == 2)
                                {
                                    newStatus[(x, y, z)] = true;
                                }
                            }
                        }
                    }

                    status = newStatus;

                    Console.WriteLine($"Day {i + 1}: {status.Values.Count(i => i)}");
                }

            });
        }

        public override void Run()
        {
            RunScenario("initial", @"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew");
            //return;
            RunScenario("part1", @"swswswswswnwswswswsweswsesw
wneswseseneswnweneswwswseswwnwseswswe
ewswswswwswnwnwsweswwwwwswwwswwsw
senwseseswseseswswesewseseseseseswsese
seswnwseseswseseswswseseswswseswesese
swnwnenwnenwnwnwswnwnwnwnwnwnwnwenwnwnw
seseeseneswnwseseneeeseeswsewseseese
senenwnwnewnwnwnwnenwnenwnwnwnenwswnenw
eneneneweeneneeneneneeneneenesew
nenesenewwenwseseseseswsesewneseweswse
swswweswswswswswswswseswnenwswswsweswsw
nwswnwnwnwnwnwnenenenwnwnw
seseeneseeesenesewseswsewseeeese
wwseswwnwneswsewswwswswswwswswswswne
wwewwnwwwswsenwnewnwwnwsewwww
wnwnwnwenwnwnwnenwnweswnwnwwswnwnwnw
neeneneswnenenenenwnenenwnenenenenewnwe
nenwsenwnwnwnwnwswswnwnwnwenwnwnwnwnwnw
swenwsenweenwswnwswseseeswswesenwne
ewseswsewswneewneneneeneseneeswnwsw
nwswnwswewswswswesesweswswse
swswswswenwswswswwswneswneswswswsesww
seswswseseswswsenwswse
wwwwwwwswsewnewwnwwwwwew
swwwswewswswswswswnwswswswswseswsww
sesewsewneseneseseseswswseeseseswnesw
wwswswwwwswwewwswwwsewnwnww
nwnenwnenenwnenwnwnwsenwnwne
neewwwneswseseeeneeseesesesewee
senwneeneneenewneneneneneneneswnenenene
swswswneswwswswwswswsewneswesw
nenenenenwnenesewnenenenenenenwnwnenwne
nwnwnwsesenwnwwsewnwnenwnewnwnwwwnwnw
nwnenwwnweswseseeswenenwweneneeswswsw
eeesweeeenweseseweeneeseeese
swswseseswseswnwseseswswswseseseneswswsese
swnwnwewnewwwwnwnwwnwwswnwse
sesenwnenenwnenwnwnwnenwnewnwnwnw
neswneneeneneenenenenewnwneneswnenwewne
nwnenwswnwenwnwnwnwnwnwnwswnwnwnwnwnwnwnw
nwwwewwneewswneswnwwwwwsewsew
wswswswswswweswwwwswseswswswneswswe
wnwwnwnwnewnwnwnwwnwwsenwwnwnwwnw
wswwwswswwwwnesew
nwnwneenenwnwswnwnwnwswnwsenwnwwenenenwnw
swsewseseswswseswneseseswseseswseseseese
swseswseswnwsewneweseseseeswsesesesew
sweswwwneswswwswesenenwsweswswswswse
wswseneeseweeeeweseesenwnwseew
wwnwnwwwnwnwnwnwwnwnwnwnwswwnwwe
swnwnwwseneneneneneneswswswnewenwsese
seseseseseesesenwseseneesewswseswnwnw
neswswsenwesenesenesenesesewwneswnwswnwne
nesesesenenwnwnwnenwnwswnwneenewnenwne
neneneneneeneenewseewnenesenenenenene
eseseseseneseeseseesenwseseseseseseesww
neeenenesweeeeeeweneeeeeene
enwwwwwsewwwnwnwwnwwwwwnwnwnw
neseeeeseseseeseeseneeseseseenwwsw
enenesewneenenenwseseneeweeswenwene
wneneseneneneenenenenenwnenenenenenene
wnwneeswewnwnwseswswsweswsenewswsw
eseeswsenwenweseseeseseseesenesesese
wswswseseseseneenesesesewsesenewsew
wnwwwwwwewwswwwwwwwswsww
swenewwwseswwnesesenewneswseneswswswsw
eswseseseseesewseseneseenwsewseee
swneneswneneneneeneeenwneeenenwwenene
neswnenesenenwnenenenenenenenenenenenwnene
sesesesesewsesesweseseseseseseesesenee
nwnwnwnwnwnwswnwnesewnwnwnwwnwnwnwnenw
eenesewwwwnwnwwwwnwwwsw
swnenwwswswswneseeseneswneseswwseswsw
neneenenwnenenwnwnwseswnwnwnenwnenenenenw
neenenwneneseneeneneeneneeeeneswnee
swswswswneseswwswswseswswweneenwnew
neseewsesesewswsese
enenweneeneneeeeneeswswneneeeneenw
seeeeneeeenwseewswenewnwseesew
sweeeneesenweswewsweseeseenenwee
seesesewswenenwnweswsewseneenwsesw
swwswwswwwneswwsww
wwwwwwwwwwwwewwwweww
neneneseneeneneeswneenesenwnewnenwe
nwnwnwnwnwnenwnwnwnwsenwnwnwwnwnwnwnwnw
eeeeeseseeenweeeesweeenee
nwnenwnwneneseneswnenwnenwwnwnwnenenwnwnw
sewswswwsewwneesewnewnewne
nwnwwnweswnwneswswnwnwnwnenwnweswenwe
nwneenenenenwneneneeneseeneneswnenenene
swwwwswswswswnewwwwwsw
neneneneeneeneeneneeewnwneswneneesene
nenenewnenenenenenenewnenwnwneesenene
swnwswswwwswswwweswnewswswwswnwswsee
neseseeseseeseseseeeseseewesewse
nwnenenesesenwnwswnwwswwnwnwnenwnwsew
seesesewseneseseseseseseswsesesenwswsese
nwsenwwnenwneseswwwnwwnewesewwswnw
sesesesenesesesesesewsewsesesesesesesesee
seswwnwseeswnwwneeneseweeseenee
nwseenwnwnwnewswnw
nwnwnenwsenwnwnwnwnww
wnwwwnwenwnwwsenewnwnwnwwsewwnenw
sesenwsenwseswseseseseeewsenesesesesese
seeneeeeseeenwnwsweeeeneeene
eeeeseeeeswneeneeneesweenew
swseneswswnwseswswseswswswswneswswswsesw
nesesweneseseneeweesenwnwseeswnwswse
senwswnenenwnwneeesw
nweeseeenweesweeeeeseeswese
sweneenwneeneeneesenwnwnwnesweswswnee
swswnwewswnwswwwwswwswwwewswswswe
swswswsenwswwswsweswswnwsweswswesesw
swswsesesenenwewseseswseeswseseswsesesw
seswnewewswnenwenwnenenenenenenwwene
wnwswswwwsewnwnwswweseswsenewwww
wswswweswwwnwsewwswswwswsw
swswseswsweswswswswswswswswswswnwswswsw
nenwnewnenenenwnenenwnwnenwneneneenese
seswswswswseneswsesw
neseneneenewsenwwnwnwwswwnwwee
swswswwswswswwnwnewwwneeneswwnwse
nwswwwwnwswesewsenwseswwswwwwwnew
wsenweeeseeeseeseeeeeeeseese
seseseseseseeswseesesesenesesewnesese
nenesesewsenwneseseseneseswseswwswsesese
sewseseewsenenweewswseseneeswsee
swseewnwsenwnenwswneswnwwswnesenwnewnw
neeeewseseneswnenwneneweneneseseew
nenwnenwswneneenenwnesesenewneneswnenwnw
nenewnenesweneneenesenenenenenenenwneee
wnwsenwnwsenenenesesesenesewnwnwnwnwnenw
nwswswswnewnwsweseswnesenweswenwesww
nwnwnenesenwnwwnwneswnenwnwnwnwnwenwnw
seswseseswswneswnwswseswsesesenwswswswswsw
nenwnwsenwnwnwnenwnenenenwsenwwnwnwnenwnwne
eswwswswwswswswswswswwsww
nenesenwenwsweesweeneenw
nenenesenenenenwnwnenenewnenwswnwnenenwne
eeseeeneseewneeeeseeweeeese
swswwseseswsenwseeswswswsesesw
nwnwsenwsenenenwswseswsewseswswwnwswnew
nwwnwnwnwenwnwwwnwwnwwwnwnwwwsw
wewnwwswswnwwnwnwnwnwewswnwesenwwne
nenenwnwnwnwenewenwsenwnenenwnwnewnwne
seseseenwseseseseseswwseswnwsee
swseswseseseswsesesesenwseseswsese
swswneseswseswwswswswseswsweswswswswswsw
seneneeseseswseseswsenwseeseseswsesee
nwneneseneneneneneenesenewseneewnene
nwwnenwnenenwnwsenwnwsenwnwnwnwnenenenw
wwnwwsewwwwwsewwwnwnwnwnwwnese
seseswnwseswseswsesesenwsesesese
wnweswsenwnwswnenwnwnwswwsewnenwnwwnw
wseswnwseswswseswswsenwswswsesene
neeeneenwneneeeeneeesweenenweesw
wwwnwnwwwwwwwwwnwwwwesew
neseeneneneneneneneneneneenwnenwneneswnene
neeeesenweeswenewne
nwswnwenwwwwnwswwwwwwnwnwenwnwe
wwwwswwwwwwswwwwwewsw
eeseeswswswneeeweewneneneenenene
swswswesesesenwswseswnese
sesewswseseseswseseseseseseseseseswnese
neeeeeneeweeeesweeenenwe
nwwnesenwneneswenenenenenwnwnenwnwwnene
neeenwneneswneneneswneeeneswneene
nwwewwnwnwnwswwswwwnwwwwwwnew
nenwnwnwwswnwwnwnenwesee
wswswswswwseswswswswnewswswnwswwswsww
neneneneswsenenenwne
neeeweeeeeeeseneneneneeesewe
nwswnwnwnwnwenwneeswnwnwwnwnw
swsesenwseswseseswwseneseneseseesesesese
nweeweeeeeseeseeswseeee
nwsesesesesewesenwnenwswneeseneswsesw
nwneneenenewsweeneneneneneneeewnene
swseeseneswwneswnwwswneewnesw
neseseswswseswseswseswsenwsweseswsesesese
seswswwswwwwewswswwnewwwswswnww
senwseesesesesesesesesesesesenwsenwsese
seswnwsesesewseseneseseseesenwsesesesewse
neeenwnesweenwneeeeeeeeeneswene
seenwsenweswnwswwnwnwnwnwnewnwswenw
swswswswswswswswswswwswswseswne
enwswswnwswneswsewnwswseneseenwwnwsw
swneswnenenwnwneeeneneneeswneswnenesew
nwswnwwswwseewseewsenenwnwwwnwwwse
eeeseesenesesewseeseswseneesesesese
swnewswseswwwnwwewew
eeeeeeseeeeeswnwe
nenenenenwnwneeneswwneeneneneswnenenesw
eswwswswswsweswnw
newnwsenwnwnenwnwenwnenenwnwneswnwwswnene
eeneeneneeeneeweeswneeneseeewne
nwwnenewenenwnwnwnwe
neneswwneswneneneswene
swwneneweneeswwneenesenwnwenenese
seesewsesesesesenesesesesewsesesesesese
nesenwesesesenwwswsesewesesesesesesesese
nwsenwneeweenweswnwnwnwwwenwwne
seswswswnwswseswswswseswenwswswswswnesw
swnwsesweswswseswwswswwseeswswsenesesw
ewseseneeseeeneeenwnweeesenwne
neswswswswswswswswseseswwenwswnwswswsw
nwswswwseeswswswswswswswswswswswswswne
nenwnenenenenwnenenenewnenenwneenwnw
senweeeenwseeseeenwseeseseeeese
seseseswseseswswseseseneneseseswswwseswswse
swswwswwswwswswswswwwwswwswwe
sesesewseswseseseseseseseesesesese
nwwnwnwnwnwenwwnwnwnwwnwnw
nwnwnwnwnewnenwnenwnenwnesenenwnenenwnw
nwwseneesenwwswwneseenwseswwwnwe
enwnwswsesenwswsesweeseseseseewnwswse
eeeeseseeesweneeeewewwse
ewneneeseeneseswswnwswwseswesesene
eeeneeeneeweeneeeeeeweese
nwnesenwnwnewnwnenwnenwnwneeswnwnwneswe
swseweeeseseneeeeseneseseeneeew
seewnenenwenesesewnwnwnwseswe
nesenewsesweseseswwsesewneswswswese
neenenesenwneeneeneswswneneenenenene
wneswsenenwsewseeswseswswnesewswnese
senwnwnenenewwwnenwenwnenwseenenenw
nwwwsewneswswwnenwswnewnenwseesenenwnw
eneneneeneseswweeneeewnweneee
seenenwswswswnwnwwseneneseeeeeswsw
nenenesenewnenenwneneneneneneenenenenene
wswwwwwwswwswwnwwswwswewwew
neenweneneseeneswwswnew
eeeenweeseneeneeeeeneeswee
nwsesewsesesewnwseswneseseesesesenwse
wnwnenenenenwwesweeneneswsenwnwswsene
swswwswswswswswswwsweneswnwwnwswesw
wwewwwwnwww
eeeeneswsweneenwnenwneneneeneene
sewseswswswswswseseswseewnwnenwsw
neeneseweeneeneesweeneenweese
sweeeeeeneeeeeeee
wnwnwwnenewwnwnwwsewswwwwnwnwwnww
seswwseseseswnwswseeswswswnwseswseswswse
eseswnenwweeswnwwneeswnese
sesesewnewwwwneneww
nweneesweneesenewnenw
eesesesenwseseeseseseseseese
neswswnewenenenenenenwenenenwne
swnenwnwnwsenwwnwwwneewwnwwwnenwwse
seswswsesewweneenenwsesewswsw
seeseeeswenwswnweeeswnweeeeee
sweswneswwwwswne
seseswseswswseneswswseswseswneswswseswsese
wnwwnwnwnwwwwwnwwwwnwenww
wswseswswswswsweseswswnwseswneswswswsw
enwwnwnwwseeesewnww
nwnwsweseswswswwswswseswswweswnwswe
swseswswswwwswswnewsw
wswnewnewnewenwsewwwseseswnwnwwse
eseneseseeesesweesesewseseesesewse
neewseeseeeswswsesesenenw
newneweenenesewseewwnwwsenesesee
wnenwwswwswswnwnweeenwneswseenwnwnw
swnwnwnwnwenwnenwnwnwnwnwnw
neneesewnweseeneeneseewsewnwswne
wwwwwenwwwwwwswwwwwnwsew
seesweneseeseweeseneseeseseeee
nwnwnwsewwnwwnwnwnwnwnwnwnwnwnwneswwse
newnwswnwwnwnwnwnwwwnwnwnwwwnwnwenw
seweeeseeeseeseeeeeseswnweseene
eseseeeeeswesesweeseneeesenesee
eseswwsesenwseseswseseseeswsenese
wnwnwnwnwnenwnwenenw
neneenesweswsenwseneneswnenwwwneswneww
wswneswewswseswswswswwsw
seseeneeseswsewneswnwseneswswneenwnee
nwwnesenwsenwnenwnwnesewnwswswnwneswwnw
enweneeeneeneneeneneswseneseenwne
nwnenwnenenwnwnwsenwnwnewwnwnenwene
wwnwneswwewnwweswwneseswwswnesew
enewneewsewenenenenesenenewene
nwsenwnwnwnenenenenenenenwnwnwnenw
nenenenenenwnwneneneswne
enwneseeneneneneswwneeneneswe
swswwswsewsewwwwneeeswwwswnwww
swnwseenenenwenwnenwnwwnwswneswnwnwnenw
wswswwswwwswswswswswswwswweswnwsw
seseswwneswwswswswswswswneswswswswswsww
seseswnwnwnwnwnwnwnwnwwewnwnw
wwnwnwwwnwnewnwsewwnwnwnwnwnwewsw
wseswnwwneswnenewwseewseswswwwne
wswwwwwwwwwnwewwwnw
senwswseneseseseswswswsweswswseswseswsw
wwwswwwswwwsewnwswwnewwswswswsw
swseswswwswseswswseswseseswseswne
ewswswsweswswnewswseneseswseswsenwnwe
wwwwwsenwwwwwwwwwwne
nwsewwwwsewnwwwnwne
swswswswswnewswwwwwswswswswswsw
nwwseenwsenenwwnwnwnwnwenwwnwnwwsw
eswseswswsenenenenwswseswswswseswsenwswse
wswwswwswwsewwwswswswwnwwnene
sesesesesesenewwswwseneseseeswsesesese
swnwnwnwnwnwnwnwnenwnwnwenenwnwnwwnwnwnw
wneenenwneenwwnenwnwnwneneneneeneswnw
swwwwwwwwwwwwwwwnewseww
senwneswseswsesenwsesee
neswwseseeswseseswswsesww
nwswseswseseswseneeseseneseseseswseswnesw
eneeeneneneewenenenenenweenesene
wnwwnenenwnenwswnwnenweenwnwnenese
wswseswswneswswswnweswswsweseseswsene
senwwseneseeesewnesewswsewsesenwnese
esenesesesesesesesesesesewsesesewsesw
neeneneseswnwnesenwwnwnwswnenwne
neenenwnenewneswnewneneneenenenenenwe
swseswswswswseswswnwswsweswsesesesesw
nwenwsewnwnenenwnwwnwnwnwswnwnwnwnwwnw
wsenwwnwswneweswwweewnewesene
swwnwwswwwnwnwewewewwwnwnwnww
wsewwnwswsweswswswswswswswswswswswswsw
seswwswwwewswewwwwnewwwwne
esesesesesewneseeseseseseeswseneesesese
nwnwnenenwneneeneweneneneseseswnenwnenw
neneneeneswseswwnenenenenenwneneneneenene
nwnwnwnwnwnenwsenwnwnwnenwnwwnwnenw
eeeeeeewnweeeseeeeeeee
wwwnwwnwewnwwnwwnwswnwwnwnewwnw
swenwswnwswesesenwswswswswnwwnweseesw
neeneneeneneswneneneswneneneneenenenene
nwsenwseeeesesese
wnwwnwnwwnweesewnwswnwswnwnwseswneww
nwseseswseseeneseneseseseseseswswesesee
neneeenenwswneeeswneneneenwnenenene
seswseeswwswswswswswsweswnwse
neeeneneeneswenenwnenwneeneweswnee
ewewwswnwneewwnwswsewswwenew
swwswswswsweswswswswswswswsenewswswsesw
nwsewnwswnwnwsenwnewwnenwnwnwwwnwewnw
nwseswwwwwswswswwswneswwswwwwsww
nenwnwnwnenenenwnenenenenenesenenenene
eseseswsweseswseswsenesesewwsesesesese
sesesesesesesesenewnwseseseseseseesese
nenenenesenewnesenewwenesenenwnenene
wewewswwswnwwwwwwwswwww
enwwnwnwnwwwswnewnwnwwnwseewnwsenew
swseseenwnweswseseswswnenenwnesesewsw
wwnwwwwsewwwwwwewwwwwww
nwewnwnwwenwswnwnwenwnwnwnwnwsw
wsesesenwsenwneeseseewseeswseseese
enweneeeswnewneswewneneeneeneee
seswwseswseseseswswesesesesesesesenwsese
swwnwswwnewsewesesenwwewnew
nweneeneseneneneenenee
newnenenenenenwnenesenenenenenene
wwwwwwwwswwnwswwwsewwwwwne
swnwswnwenesenwnenew
nwwwwwwwwwwsewwwwwnewww
swwsweswnwnenwnwnenwnenewewswsenwswnese
enenewneswneneneeeeneeseneneneneenee
nenwnenenenenwnenwwnenenenesesenenwnewne
swnenenenesweswnenwneswswnwswnwnwneswswne
wwwwsesewneswwwnewwnewwnewsew
seswseseseseseeseseswsesesesenenwsesesese
wwwswwnwnwewwnwwnwwww
seseseseswsesenwswsese
swnwswswswswswnwswsweswswswswswswswseswswsw
nwnenwswnenwnwnwnwsenenwnwnwenwnwnwnwnwne
seneneneneenwneneswwnwswneneswnesenenw
swswnewwnenwewswwswswswsesewswsww
neeswwnwnwnwnwswnewenenwnwnwnwnwesewne
nwswseswewnewswnwswwsweswseeneseswne
sesenwswesenweenwwnwesw
seeseseseswnwseseseswseseswsesesesesese
esewseeesweseseseseesenenweseese
nwswseenweeneeeeeseenwseenwsesesw
enweeeseeesenwsweee
nenwneneeeneneneeneeseeneeneneneew
swswseswneswseswswswse
enenenenwnwnwneswnenwnwnwnwneeneswnenenw
nenenwneswnenwnwnenwnweswnwnenwnwnenenwne
swseesewseeenwenwnwsenenwweseese
swseenenenenenenenenenwwnwnenwwenenene
nwnwnweneneewwnwsenenenenwswswnesene
seeswswesenwneeneeenweswesesenww
eeeeeneseeeeweeeeeesweee
enwswswneswswswwswswswswseswswswswswswsw
sewsesesesesesenesesesesesesesesenesesw
nesenesenenewnweneeenenenenewnee
swseseseswsweswsesesesweswnwswswseswsesenw
eeeeeeeweeeeeeweeeeee
nwnwnwnwnenwnenwnwesenwwnwnwnenwnwnwnw
wswwnwewnwwwswswwwswswswwswswe
eeenweeneeeeeeeweeeseseee
seswseseseswsesesesesenesesesese
wnwnwwsewswwwewwwnwwneweseswne
nwnwnwnweswnwnwnwnwwnewnwswnwnwnwnwnw
seseeseewseswsewsesesewneeseswsenene
seseneseswseesenwsesesenenwseseswwsesese
wswwnwenwwnwswnewwwwwnwnenwwnw
ewewwwwwwswwwwwewnwwsww
wwsewswwwwnewneswwwswwnenwwne
wsewnwnwnwwwwwnweeswnewnwwnwnw
nwnwwnwnwnwnwnwswnwenwewswnwwwwnwnw
newwwewswsenwnwnwwwsew
swswwseswnwnweswwsweswwwwswnwswnwse
eseseseenweewseseseseseeseeeenwe
eseseeeswesesewnwenwseenwseseesene
wwnwwnwseewnwnwnenwsesenwnwswnwsene
eseeseseewseseseeseese
nwenwneswweneeeswseeseenwneee
wweswwwswwseswwswswswwwswwnew
seenwwseswwseeneseenwsenwswe
nenenenwwneswnenenwnwsesewseneneneenwswnw
nwnenenenenwneneneswnwnenwnwenene
sewnwswesewsewneneswwwsenwne
wnwnwnwnwnenwnenwwswnwnwnwnwnwwwswnw
sweseneseseswswswswseswnwswseswwswsww
wenwwwsenwseswnwnwwwnenwnwnwnwnwww
seneswswswswneswswswswswswswwswswwsw
nwnwnwenwnwnwwnwnwnwnwnwnwnwnwnwnwswnw
nwnwesenenenwnenwnwnewnenewnenwnwswnwsenw
nwwnwwenwnwnwsenwnwnwwewwnwnwwnw
nwnwnwnwnwnwswnwnwnwnenwnwnenwswnenw
nwnwnwnenwnwnwnwnwnesenenenenwnwnwnww
swsenwnwwnesenwnwwnwnwsewnwseenwnwne
swswenwswseswswswwswswswseswswnwseswswse
wneneneweseneswswswsenww
nwneswswnwswnwenenwwneswnwnenenenesenwne
nenenwneseswnenenenenwneneeneenewee
nwwwewwnwnwnewsewswnewesewnww
nwswneswseseseswseseswseseswseseseseswse
swswwwwwswwwnwswsew
swsweswneswswswnweswwswswswswswwsww
swswswswswswswswenwwwswswwswneswswsw
seeeeseweswswenwneseeseenwnwswseww
enweeneeenweneeseseneeweseee
nwswnwenenwnwnwnwswnwnwnwenwsenwwwse
nweewwnwesenewswnwwswsenwwnwsww
enwnwswwseswnewsewneeeswseew
neneenesweeneeneneneneewnwenenese
swwneneseenesewsesesenewesesesesewsenw
wnwwnwwwwwewwwwsww
wnwswwnwsesewwwwwnewnenww
nwwnwnwnenwswnwneneenwnwswnenwnwnenenw
eswswewwswwnenwewneneswswewsesw
neswwswswwswswswwswswswwswwewswww
eseenwweseseeeeneeeseeenwese
neeeeweeeeeneenwesesweswneswse
nwwwnwwwewwnwswnwnwwenwsenwwwse
nwnwnwnwnwnenenwnwnwnwnwnwnwnenwnesenwsene
nwseswswswswseswswseswswswswswswswswneswsw
wnwnenwwenwnwwewwenwwsenwwswwswnw
seseswneswnwswwswwseseswneenenenesww
esesewseeneseswwsesesesesesesesesese
wwnwnwnewnwnwsenwnwwnwenwnwsenwsenw
swwwneswwwwwwwwwewswwwww
seneneeseneewswnwswswnewnenwwnenenwswne
swswseswwwswwwswwswwwwwwwnew
eeseseewseseseenwseeeeeseseesese
enenenenwnwswnenwnenwnenenwneswswnenw
nesewswsesesesesenesesenwseeseneseswswe
neeneesweswneeswneenwenweenenwsw
swnenwwsenwneswneneswnenenenenesweene
wnwewwnwwwwwnwnwsew
nwnwneswwnwnwswneenwnenwneneeenenwnw
neeeneseeewneseswnwseswnw
nwseenenewenwnewneswnenwenwswsenene
wnenwnewnwnwnwnwnwenwnesenwnwnenenwnwnwnw
newwwwswwnwwswwswswewsewweww
neeenenenwneneseneseseenenenwnewnene
wnweseswseseenwnw
senwswseswnweseseswseswswswswswswswenw
nwnwnwnwnwsenwswnwnwnwnenwwnwnwnenesenw
eneneneenesenesweeeeeeeeeewe
nwnenwneneswnwnwnwnwnenwnwnenenwewnwnee
neeenwseeeneeeeneeeeseenenew
wsweeswseswswswswweswewnwneswseswnw
swswnwseswswswnwswswswswswswwwswswenwse
wnweswwswswnwseswswneswswsewswswsw
nwnenenwnenwnwnenenwewnenwnwnwsenwnwnesw
wnenenesenwnenenenenesenenenenewnenenenene
swnwnewswswseseswseswwnwnwsweneswsew
neeeeneenenenenenwneneswweenenenene
nwewswswnenenwnwwswweweneeesenwne
sesewenesesewsewsesenesesenwnesesenwsw
nwewwnwwwwnwwwwswwwwe
eseseesewsesesesewneseneeseeseesesew
neneneeneneeeeeneenenesw
newewswsesewnenwwwswwwnwwsw
sewwnwewwsesenenwnwswswnenenw
nwnwnwsewwswesenee
ewnwseewswnwwswwwwsw
wwwenewwesewwwwwwwwwww
wnwseeswenenwww
neeseseeesesewwneeeeseswneeseswe
wwsewswwswwswwswwswnewswswswwsw
eeseeweseenweseseswsesenesesesese
senenwwnwwwnwswnwewwnwwswnwwnww
nwnwnwswnwnenwnwnwsesenenenwnwnenenenenw
wswnwswswewnwwwnwseneesenewswwew
nweeswesweeeeeeeeeeeenwe
eeneeenenwenweeeeseneesweew
seewswseswsweswwenwswswseswswnweswswsw
seswenewwswswnewnwwsenenewswswwwse
nenewweeneeseneeneeswnenenewnene
neweswnenwnwswneenwwsenwsenwenewnw
nwnwneseswnenweenenenenesewnenwsenenwwe
nesweswenesweneenweenewsenenenwnw
seeeseeeseseseenenweeseeenweseew
nenewneeswneneneeneneseneeeneneenenee
swswswswnwwseseseswnwsesenwseseeesesene
swnwsesenwneseenwsesesesewsesweseeee
nwneneneneseneneneneneneneew
wswwwwwwwsenwnwwnwnwwwwneww");

        }
    }
}
