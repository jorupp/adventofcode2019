﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2020.Day8
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(i =>
                {
                    var p = i.Split(' ');
                    return (p[0], int.Parse(p[1]));
                }).ToList();

                for (var i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Item1 == "nop")
                    {
                        var x = TestIt(lines.Select((ii, ix) => ix == i ? (("jmp", lines[i].Item2)) : ii).ToList());
                        if (null != x)
                        {
                            Console.WriteLine(x);

                        }
                    }
                    else
                    if (lines[i].Item1 == "jmp")
                    {
                        var x = TestIt(lines.Select((ii, ix) => ix == i ? (("nop", lines[i].Item2)) : ii).ToList());
                        if (null != x)
                        {
                            Console.WriteLine(x);
                        }
                    }
                }

                Console.WriteLine("Done");
            });
        }

        private int? TestIt(List<(string, int)> lines)
        {
            var offset = 0;
            var seen = new HashSet<int>();
            var accum = 0;
            while (!seen.Contains(offset))
            {
                if (offset >= lines.Count)
                {
                    return accum;
                }
                seen.Add(offset);
                var i = lines[offset];
                if (i.Item1 == "acc")
                {
                    accum += i.Item2;
                }
                else if (i.Item1 == "jmp")
                {
                    offset += i.Item2 - 1;
                }

                offset++;
            }

            return null;
        }

        public override void Run()
        {
            RunScenario("initial", @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6");
            //return;
            RunScenario("part1", @"acc -15
jmp +164
nop +157
acc -12
acc -19
acc +41
jmp +177
acc +36
acc +37
nop +471
jmp +433
acc +24
acc +13
acc -12
jmp +556
jmp +1
acc -15
acc +33
jmp +299
jmp +344
acc -3
jmp +124
acc +10
nop +562
acc +45
jmp +386
acc -3
jmp +206
acc -19
acc +12
jmp +424
acc -18
acc +23
acc +12
acc +0
jmp +311
nop +327
jmp +301
acc +20
nop +375
jmp +25
acc -13
acc +49
acc +23
acc -3
jmp +346
acc +2
acc +3
jmp +123
acc -7
nop +183
jmp +165
acc +47
acc +34
jmp +1
jmp +359
acc +12
acc +16
acc -3
acc +0
jmp +556
acc +14
acc -3
jmp +559
jmp +192
jmp +495
nop +264
acc +3
acc +47
jmp +187
acc -18
jmp +1
acc -12
jmp -58
acc +49
nop +288
jmp +145
acc +46
jmp +294
acc +38
nop +400
jmp +373
acc +7
acc +31
jmp +492
acc +40
acc +5
acc +11
jmp +263
acc +29
acc +10
acc +21
acc +14
jmp +450
nop +458
acc +38
nop +432
acc +42
jmp +191
jmp +279
nop +71
acc -17
jmp -64
acc +17
jmp +1
acc +29
jmp +506
jmp +354
acc +42
acc +32
jmp -40
jmp +184
acc +41
acc -7
acc +10
acc +38
jmp +100
jmp +104
jmp +245
jmp +335
jmp +20
acc +3
jmp +490
jmp -62
acc +34
acc +34
acc -1
jmp +6
acc +5
acc -9
acc -19
jmp +397
jmp +253
acc +9
jmp +270
acc +8
acc -16
acc +32
acc +48
jmp +258
acc +4
acc +37
nop +319
jmp +318
jmp -4
acc -5
jmp +32
nop -86
jmp +306
acc -13
acc +50
acc -16
jmp -53
acc +31
jmp +52
acc -11
jmp +89
acc +21
jmp +126
acc +44
acc +49
nop +177
jmp +44
acc +8
jmp +166
acc +20
acc -8
acc +38
acc +10
jmp +311
jmp +21
acc -10
nop +84
acc -7
acc +13
jmp +78
jmp +1
jmp +366
acc -6
acc -12
jmp -142
nop +223
jmp +42
acc -6
nop +227
nop +193
acc +23
jmp +83
acc -10
acc +12
jmp +1
acc -8
acc +3
nop +28
jmp +301
acc +23
jmp -170
nop -79
acc +21
acc +37
jmp +138
acc +37
acc +24
nop +413
acc -9
jmp -179
acc -1
acc -10
nop +261
acc -19
jmp +168
acc -16
acc +19
acc +17
acc +21
jmp -9
jmp +46
acc +4
nop +398
acc +28
jmp +396
acc +11
jmp +384
jmp +375
acc +25
acc +30
acc -11
jmp +371
jmp +249
acc -10
acc -15
jmp -7
jmp +38
acc +29
acc +15
acc +46
jmp -77
acc +43
jmp -83
jmp -42
acc +30
acc +44
acc +33
acc +14
jmp +326
acc -3
nop +49
acc +12
jmp +63
acc -13
acc -19
acc -17
jmp +126
jmp +293
acc +16
jmp -185
acc -12
jmp -92
acc -13
acc +19
acc -1
jmp -138
acc +28
nop -243
nop +352
acc +43
jmp +249
acc -5
acc +36
jmp -217
nop +197
nop -106
acc +30
jmp +194
acc +7
acc -16
nop +128
jmp -239
jmp -258
acc +11
nop -74
acc +42
acc +40
jmp +72
jmp -207
nop +337
nop -240
nop -169
jmp -55
nop +165
acc +27
acc +4
jmp -169
acc -2
jmp +69
acc +0
jmp -250
acc +11
acc +45
acc +31
jmp +195
acc -10
acc -8
nop -283
acc -2
jmp +63
acc +17
acc +12
acc +0
nop +243
jmp +190
acc +17
acc -18
jmp +78
acc +7
acc +33
jmp +244
nop +29
acc +20
nop +150
acc +29
jmp -43
acc +45
nop -132
acc +16
acc +14
jmp -237
jmp -199
acc -4
jmp +179
acc +13
acc +15
acc +6
acc +46
jmp -222
acc -8
acc +15
jmp -26
acc +38
jmp +1
nop +266
jmp +44
acc -13
nop +209
acc +21
jmp +201
acc +8
acc +18
jmp +190
acc +35
jmp -238
jmp +69
acc -11
nop -182
jmp -221
acc -16
acc -5
acc +7
jmp +39
acc +26
acc +43
acc +20
jmp +92
acc +22
jmp +81
acc +32
acc -13
jmp +30
acc +1
jmp +201
acc +4
jmp -165
acc -17
jmp -84
acc -16
acc +2
acc +47
jmp +54
jmp -195
acc +33
acc -17
jmp -18
jmp +256
acc +1
jmp -244
acc +28
acc +35
jmp +189
nop +32
acc +9
jmp +24
acc +21
acc +14
acc +17
jmp -67
acc +21
jmp -297
acc +36
acc +14
acc -13
jmp +115
acc -2
acc -13
jmp -182
nop +119
acc -4
acc +44
acc -14
jmp +61
acc +41
jmp -13
nop -116
jmp -294
jmp +7
jmp +17
acc -14
acc +42
acc -6
acc +24
jmp +151
nop -374
nop -375
acc +4
jmp -268
nop -27
acc +16
acc +2
jmp -206
jmp -320
nop -196
jmp +168
nop +36
acc +34
jmp -402
acc +36
acc +38
acc -11
nop +17
jmp -182
acc +15
jmp -145
acc +43
jmp -79
jmp -391
jmp -155
nop -94
acc +0
acc +9
jmp -441
acc +3
acc +6
acc +50
nop -334
jmp +163
acc +18
acc -11
jmp +21
acc +10
acc +4
nop +132
jmp -348
acc +18
nop -1
acc -4
nop +148
jmp +165
jmp +146
jmp -460
jmp -14
acc +26
nop -388
nop -353
jmp +119
acc +26
acc -1
acc +9
jmp -285
acc +37
jmp -345
jmp -178
acc +7
acc +13
jmp -39
acc +29
nop -200
acc +50
acc +24
jmp -160
acc +18
jmp +63
acc -11
acc +1
acc -6
acc +33
jmp -90
acc -3
acc +11
acc +45
jmp -197
jmp -169
acc +7
acc -4
jmp -281
acc +48
nop +19
nop -25
nop +9
jmp -274
nop -126
acc +22
acc -4
jmp -408
acc +1
acc +0
jmp +98
acc +25
acc +12
acc -19
jmp -90
acc +44
acc +20
acc +21
jmp -192
acc -12
jmp -70
nop +3
acc +17
jmp -349
acc +20
acc -7
acc +6
nop -43
jmp +53
acc +34
acc +48
acc -4
acc +8
jmp -126
acc +23
acc +25
jmp -349
acc -4
jmp -272
jmp -129
nop -366
jmp -292
acc +29
nop -269
acc +50
nop -254
jmp -321
jmp -23
acc +11
nop -425
nop -150
acc -9
jmp -467
acc +18
acc +27
jmp -338
jmp +1
acc +21
acc +27
acc -11
jmp -160
acc +27
acc +15
acc +0
acc +41
jmp -386
acc -10
acc +14
jmp -217
nop -484
acc +47
jmp -529
acc -10
acc +48
acc +0
jmp -430
acc +45
acc -8
acc +3
nop -103
jmp -387
acc -16
acc +39
jmp +1
acc +17
jmp -350
jmp -328
acc +30
acc +28
jmp -309
nop -361
acc +1
nop -468
jmp -212
acc +29
acc -4
jmp -249
acc +45
acc +30
acc +40
acc -17
jmp -579
acc +25
jmp -525
nop -217
acc +17
acc +3
jmp -142
nop +18
jmp -493
jmp +1
jmp -495
jmp -360
acc +7
acc +30
acc -3
nop -449
jmp -326
acc -10
acc -8
jmp -371
acc +22
acc +48
acc +6
acc +18
jmp -59
acc +17
acc +14
jmp -250
acc +19
acc +25
acc -14
acc -17
jmp -517
acc +29
acc -4
acc +9
acc +17
jmp +1");

        }
    }
}
