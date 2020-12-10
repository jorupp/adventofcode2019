using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AoC.Year2020.Day10
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                lines = new[] {0}.Concat(lines).Concat(new[] {lines.Max() + 3}).OrderBy(i => i).ToList();

                var start = lines[0];
                long combinations = 1;
                for(var i=0; i < lines.Count - 1; i++)
                {
                    var t = lines[i];
                    var n = lines[i + 1];
                    if (t + 3 == n)
                    {
                        var c = t - start + 1;
                        var newC = GetCombinations(c);
                        Console.WriteLine($"Processing {start} to {t} since next is {n}, {c} items arranged {newC} ways");
                        combinations *= newC;
                        start = n;
                    }
                }

                Console.WriteLine(combinations);
            });
        }

        //private int GetCombinations(int[] pairs)
        //{
        //    if (pairs.Length <= 2)
        //    {
        //        return 1;
        //    }

        //    // keep [1]
        //    var keep1 = GetCombinations(pairs.Skip(1).ToArray());
        //    var skip1 = GetCombinations(pairs.Skip(2).ToArray());
        //    var skip2 = GetCombinations(pairs.Skip(3).ToArray());

        //    return keep1 + skip1 + skip2;
        //}

        private int GetCombinations(int count)
        {
            switch (count)
            {
                case 1:
                    return 1;
                case 2:
                    return 1;
                case 3:
                    return 2;
                case 4:
                    return 4;
                case 5:
                    return 7;
            }
            throw new NotImplementedException($"Don't support {count}");


            //if (count <= 2)
            //{
            //    return 1;
            //}

            //var oCount = 1;
            //if (count > )


            //// keep [1]
            //var keep1 = GetCombinations(count-1);
            //var skip1 = GetCombinations(count - 2);
            //var skip2 = GetCombinations(count -3);

            //Console.WriteLine($" {count}:   {keep1} {skip1} {skip2}");

            //return keep1 + skip1 + skip2;
        }

        public override void Run()
        {
            RunScenario("initial", @"16
10
15
5
1
11
7
19
6
12
4");

            RunScenario("initial-2", @"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3");
            //return;
            RunScenario("part1", @"66
7
73
162
62
165
157
158
137
125
138
59
36
40
94
95
13
35
136
96
156
155
24
84
42
171
142
3
104
149
83
129
19
122
68
103
74
118
20
110
54
127
88
31
135
26
126
2
51
91
16
65
128
119
67
48
111
29
49
12
132
17
41
166
75
146
50
30
1
164
112
34
18
72
97
145
11
117
58
78
152
90
172
163
89
107
45
37
79
159
141
105
10
115
69
170
25
100
80
4
85
169
106
57
116
23");

        }
    }
}
