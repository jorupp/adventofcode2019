﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2019.Day6
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var pairs = lines.Select(i => i.Split(")")).ToList();
                var nodes = pairs.SelectMany(i => i).Distinct().ToDictionary(i => i, i => new Node() { Name = i });
                foreach (var pair in pairs)
                {
                    nodes[pair[1]].Parent = nodes[pair[0]];
                }

                var you = nodes["YOU"].Parent;
                var san = nodes["SAN"].Parent;
                var count = 0;
                while (you != san)
                {
                    if (you.Count() >= san.Count())
                    {
                        you = you.Parent;
                    }
                    else
                    {
                        san = san.Parent;
                    }

                    count++;
                }

                Console.WriteLine(count);
            });
        }

        private class Node
        {
            public string Name { get; set; }
            public Node Parent { get; set; }
            public List<Node> Children { get; set; } = new List<Node>();

            public int Count()
            {
                if (null == this.Parent)
                {
                    return 0;
                }

                return 1 + this.Parent.Count();
            }
        }

        public override void Run()
        {
            RunScenario("initial", @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L
K)YOU
I)SAN");
            //return;
            RunScenario("part1", @"762)NDB
PYQ)MMW
5YP)67Y
HZ3)XJ2
J9B)M5S
54T)TK1
85G)4ND
R42)ZZB
J9B)PVZ
ZQ9)M2G
46Q)CC3
ZW6)4QQ
W56)8LS
VL4)12K
FS6)B3M
WF7)7XR
2XJ)12N
D3Z)CWT
N21)MMD
DH7)4WF
WG4)755
N8Z)9W9
54V)YST
KQS)N7N
XS8)7Y4
JN6)S69
3L4)B4B
DPC)2S1
R4M)BVB
DHK)L5L
YJ6)6SV
P2W)2FG
RZC)8WR
7BN)7BZ
QSD)PQV
WPW)L8D
QKH)2QV
NXP)W3M
8YF)4GJ
6J7)XJ1
KSD)CVG
S8G)FNK
XM6)HMK
QPM)J2B
SCV)SWP
Z6Z)9LB
Q93)FJT
JWL)S8C
36H)VM2
7C2)7R9
99T)HWM
NDB)BL2
MPN)DCG
PRD)C52
481)RTK
KBG)YGM
JM8)NYV
XFG)FKD
DGZ)4CN
5TQ)YKM
SS3)9MX
Q59)HTP
MZS)TP3
7Y4)FK4
2BR)C6L
GC7)HKG
F6W)9XT
M3H)Y6T
X1B)NDN
2P6)JBM
VZL)B3S
3BJ)KLX
5WB)1FC
2Y9)FKY
Y5R)7DZ
9VX)13W
5TK)D4T
HWM)5YP
6VB)DHR
BLB)4L3
7MD)Q27
WVM)K4D
B1G)VWD
WHV)165
T9L)C3Q
H7R)P5G
3W5)WZM
KRH)57D
C9K)3MM
6L5)12G
YRG)2P6
BXZ)6RJ
FZK)9XS
45V)PBR
PFZ)H53
1TY)GWZ
3T7)H57
H9H)W6G
ZX9)7J6
FTM)ZTD
V17)HB5
XJ2)BC6
KS8)9HZ
CPW)Q3K
R5S)B3R
WH6)X46
QCH)5VZ
4R6)WJ2
C77)69L
B5L)VCV
V64)JQ7
7CZ)2Y9
Y5F)CXH
KL6)6VB
5BH)8XG
1PV)BT1
9LB)L9F
227)LBK
B3M)D37
VWD)SKQ
9V9)R76
XZK)5Z6
G86)VMN
V5J)V9W
FCY)Y3P
CPD)LJK
85D)XS8
9HP)9TS
23G)S3J
28Y)523
Z35)4MH
1F5)Y2B
VQZ)VHX
PDH)GJK
C87)ZX1
N5X)NT4
X46)D3Z
5JR)CSN
RCY)RFD
6SV)79X
7BZ)XWG
CM6)JPT
1CZ)GBC
H2S)V5V
7H9)LNT
1HR)7G8
S45)PPW
QWL)ZPN
733)9XC
RGR)VH3
7XX)ZSD
NQ1)W6H
QBC)4VV
4DV)6GB
KJ6)JCQ
DGM)G6X
LDT)D9W
B3R)W36
9MX)S8N
7C7)BNS
7D7)Z21
XCR)1X7
QD9)8X5
3W5)5VJ
C55)FWQ
RPC)QBR
RVH)SSX
VGJ)FJ6
459)NC5
N87)3XP
MNJ)NRD
8QD)XQC
346)VSK
P23)1RW
BRR)5DR
M4L)MNQ
1BS)T5X
P8X)T32
N57)8BF
BF9)PGJ
3X2)32K
S5R)7CG
FVT)KQS
N86)1N3
C4W)VGJ
4K4)L2X
5BZ)NXS
N7N)Q1B
H2R)FVL
69L)8W5
6C5)XBZ
VXD)LDB
ZDN)8DM
783)4WG
WQM)6VD
LNF)BD3
2BB)JV9
PH4)P47
HTP)85G
11F)YFQ
CCW)ZTP
M3M)GC1
ZFM)2J6
3JK)VFP
P2V)HYK
X6Y)QG2
ZZB)N86
SQH)WWT
7JB)WST
TX5)GT9
VDL)H96
TV2)8CW
6K6)NNV
SNL)JJB
G9X)YYX
1W1)RGF
3MM)NKX
69L)FBR
DYH)PV9
5VJ)S2D
XKQ)YJG
C4Z)KVT
G5B)WJZ
KRD)HCP
ZPY)TPK
3FF)898
V2N)LCS
6FV)6GC
BZF)KQ2
J6R)QDB
X7M)Z7D
96C)6HZ
38Q)LNF
WPB)N6F
DZ6)VLQ
YXY)HS8
PSP)JWN
S92)X7H
54M)NT8
SPC)FF8
HS8)K44
C36)CPD
95H)2VF
FZK)9RR
B12)82B
WV8)8QD
NSF)FSZ
JYK)XH8
N6D)SNL
L3Q)9PX
2FG)KQ3
YJG)ZQ9
NWK)NFP
TSN)WL9
SWW)XV1
L2X)KDY
3FZ)93Z
NNV)BYQ
247)MGV
7G8)S82
ZBX)NXP
4N7)4MP
JM1)Y6L
5KQ)7HW
6VD)JXV
NLS)Z18
3SJ)DLM
L23)ZFM
FF8)R5S
5WM)M5Z
MMJ)XGY
HLC)4MX
796)GCZ
W31)3JC
T5N)WVN
KT9)P1D
W6J)T5C
91R)LDT
BK5)7LR
13W)BLB
3FY)KP7
4QW)BS2
5C5)6SC
NMP)L35
3NJ)WPB
18V)BDT
D3K)WF7
N74)K34
QRR)VKM
N6F)T5N
GDT)CPW
3PW)TGB
TLC)4K4
6BP)1ZG
291)W3V
V9W)6L5
8Y5)DL1
NGC)Q9C
WSL)S8G
H26)VMJ
5KP)ZRY
VKM)G2X
XGY)D6L
GLQ)398
HVJ)6J7
2QV)TTX
1JJ)297
7FJ)LK4
P28)D4P
68D)C3K
JR8)MX6
6X8)5WB
QMP)S5R
CBZ)5TK
LPJ)7F6
NMC)6JN
5BT)Q78
VQB)GLQ
JV9)M7Z
NKX)J6R
ZKR)QP1
CR4)949
297)WC7
J2V)HXD
PWT)7K3
TS7)1D9
J6V)B12
PWN)9Y7
Y6M)KBG
QPZ)9WJ
QG4)QM8
3XP)5YM
YG2)H7G
V17)Z7H
Q49)Y4L
346)HK5
LNT)FS9
595)BT8
LNV)NJS
ZJ7)Q59
VW9)DRP
8WS)ZZR
3GY)MKT
Y69)PR5
PBR)9WX
2T5)BKF
XKV)Y22
7W4)17Y
CSN)K59
CYM)FYQ
KPT)HFT
2P9)357
ND2)3FY
9XC)C1Q
XR8)DYD
1T4)YM4
6VC)DHK
1WB)4LW
26Q)WPW
QP1)HNW
THJ)1R1
JD1)GCK
WCR)Y94
SVL)RJN
V5V)477
HCP)1ST
RKW)SKP
JWT)RDR
2PM)BK5
K66)GFR
497)NZD
TWG)RJ7
XXR)5BV
S63)SN4
S42)HCY
LVM)R7R
HYK)LW8
FK7)4R6
5XR)KLQ
PXS)QWZ
QWH)RQK
NFP)9CF
153)1Q1
QG2)CKM
95M)KVC
WKS)RBK
T5X)WCG
CGX)ZTR
Y8Z)762
N3N)BH7
VLQ)F8B
D5T)VX5
XWG)HSX
R6D)9BM
BYT)RCY
LBS)Y5F
3RM)SCV
315)KGS
PS7)F5K
YMB)8QL
RVH)N1D
C1T)LX7
898)Q43
D37)H9H
WCG)D5T
94Y)5VR
PPK)3ZD
FXQ)134
3V6)115
L2Z)DGZ
MMP)2KJ
KZ1)HPB
WF7)F8D
PN9)N8K
MR4)WB2
9F4)67K
SSH)6QL
DHC)SK2
7YN)RZC
WQQ)KPS
XTC)V17
T1N)F9W
SGM)MSW
J4V)BHQ
XFV)2Q9
Q43)DZ6
RFD)PFZ
8XQ)FXQ
DCG)965
Z4P)L6W
XCP)QPM
8XG)HVJ
6M2)3RM
PR5)V8K
BG1)HCW
HKG)ZGL
12G)2L4
MSW)WWP
P1S)XL3
NXD)F76
ZK5)LKM
RDR)BFX
RCB)H9T
7ZB)V73
C7G)289
QTZ)24F
NTJ)Q8B
L9F)Y1L
Q8B)S6L
9BC)XSN
5YM)Q8P
R6B)2P9
F5K)3W6
D4P)C73
DQD)ZD8
FXR)DMD
X3G)3HL
GBH)QRC
9BC)BF9
S87)V3B
BF8)BHW
89B)PCX
5BV)87L
VHX)Y1X
BBL)B2D
WST)KTD
13N)5JR
VWK)Y5Z
PRD)KGB
7T5)NMP
LCS)9Z9
PGJ)XY2
PKX)CYM
965)PJH
GN1)7BW
J34)1DF
6T4)ZMX
L8D)YDF
YNN)9SM
PP2)KT9
JWN)YR1
8XJ)8MS
CBL)KVD
MBM)9CN
JHW)MG5
DNM)CJG
PJH)MXW
F8J)8YC
8MC)VL4
GHG)N5N
SLL)TYC
2QP)QXJ
JYK)613
ZGL)3TP
2PC)V47
B9G)WVM
5D7)5X4
HFT)XPK
WQC)FTM
VTB)MHZ
TTX)JB4
L5L)4TP
73H)Z43
M83)GRN
GZX)3T7
3HK)DR7
S7Z)KFC
KVC)R1B
MGC)YRG
N5X)7MD
FVL)4WX
H9T)Z6Z
YLR)28Y
7QC)PS7
WZ7)5KC
WCS)54V
MNQ)W27
36H)9ZT
FQ9)TSN
W97)MJK
GX8)9N4
9Z9)H9K
41H)2CF
8CM)K7L
32M)9S6
ZKF)WDZ
P4V)XXP
HP2)NXY
5KC)W1B
2L6)7G1
BR4)975
D9W)H1T
9ML)T29
9WJ)PD4
PV9)896
L8N)DDP
9CN)HB1
46W)S9B
ZSD)N14
FV7)8XJ
KN2)4G9
9PX)J15
9CB)CMT
1QX)2JZ
9RR)JYF
T97)QRR
KYP)G93
9BF)HJT
1M6)G9X
H57)NLS
3JK)WJJ
HZ3)VZF
Z74)LDM
5Z6)4D7
XMJ)521
N4M)VV7
91Q)36H
8ZH)PWZ
T32)8RW
6KQ)NB7
W27)5JJ
8YC)K6F
1DM)Z34
KPS)1BS
DG1)71Q
S69)KF4
ND7)XG4
WN3)MKP
PQV)YJ6
BHQ)DJ1
NT4)T9Z
Y9K)H7R
KT9)Z6B
66J)YRR
5X4)CQS
Y6L)1W1
XG4)DJ2
PS7)KBQ
7KJ)Z35
BTF)5Z4
P1C)8GF
4ND)1CZ
V64)WP7
NF5)2TZ
59F)247
8CK)WZ7
KSW)8WS
JVM)DCY
8ZX)M55
4M2)W2R
B9C)ZX9
YSV)RFY
9Y7)KSD
9S6)CH7
7J6)PPK
N5B)1KH
9LB)DTB
D4T)Z8Y
KH8)ZNM
RBK)YWN
ZNF)YZ3
521)L17
1ST)ZK5
V5D)7KJ
XCR)2X3
SMT)MPZ
QHZ)ZNH
772)733
VD1)4M2
61X)8ZX
6KD)W6J
Y1L)SAN
ZN3)JTF
SWP)8R2
CXX)6X8
4DS)Z24
ZQ8)QG4
1D9)C4Z
VC2)7FW
HVG)ZGM
1KH)2WG
DHS)HPK
VSJ)JWT
PJX)83H
RF3)HJH
B1S)8H3
G86)VZG
PHN)GPW
W3V)MMP
N1R)L5W
HDR)1ND
CJZ)281
5RZ)JN6
YKD)89B
WWP)DHS
LBX)5RZ
QFY)MMJ
WF6)7T5
BHW)KF2
NB7)68H
7DZ)WDP
PQJ)4NG
WZM)2TD
SN4)M4M
2MJ)M5V
HZR)TL9
ZJM)VXD
ZDS)3FZ
FWQ)C6Q
D4T)41Z
JB2)PP2
SSX)VWK
51M)Q6R
G3P)QTZ
FS9)9CB
W8J)L7J
3CK)TLC
HJT)ZXN
61V)TQC
F4S)LGT
5KB)T97
8RW)Q61
BPC)7WG
ZK2)MYT
821)QFY
8DZ)QD9
3V6)SNR
CZN)XCQ
VMJ)WJG
C8X)43B
BRR)PV1
K6F)N7Z
1N3)CBH
BJV)H87
KTD)MBD
V7Y)PWT
5Z4)SK6
L1Q)B5F
2FM)L7M
MV5)HSP
V73)ZPK
BWQ)PQJ
895)FQ9
2RT)26D
CCW)M3H
VX5)FBY
21K)SQ8
7YM)N6D
LGX)PTZ
FVP)7QX
1D6)JWP
VMN)5ZD
KF4)CZ7
DSJ)BTF
VYH)6K9
NXY)NF5
S3M)VHP
5Q4)SS3
2WG)3WY
XW1)6RM
XBX)K66
3FZ)YDS
3C5)6QQ
541)KRH
HB5)F6W
LN7)4QW
7R9)2FC
WBM)FLY
K44)18V
975)2XD
N44)CCW
KVD)XBC
BKF)6LT
281)5WM
YY3)ZD1
MG5)51M
D6B)T65
QFN)3HK
K3L)N5X
R9C)95P
LGT)TBD
HRN)THC
VSD)W8B
125)WQQ
B8P)6T4
PPW)R6D
3JS)2XJ
LDX)PK7
3H3)V31
QWZ)W28
8R8)9LM
CL9)WHD
GG4)JKS
3SJ)RNF
4JM)GW6
YCX)M83
CWT)KKV
WNP)D88
S8N)FGC
31D)ZJ7
WJJ)FWH
KPS)LNV
V63)9QG
Y4L)M3M
KQ3)JKF
SDP)GK8
5VR)YNV
D5T)6C5
NFZ)DRR
89G)PHN
4TG)BQH
CZN)HP2
Q6W)6WL
LFR)DY6
CKM)YC2
Q69)9R4
LW9)P23
X24)ZNS
D97)GHG
JKS)QKT
MMD)LDX
YGM)8CS
GJN)H2Z
DTB)XY3
VWK)H26
DR7)MZS
Q9C)SNP
KBQ)614
8L9)V63
DDK)1DM
1TC)315
ST4)R9C
GHG)LN7
N5N)821
WDS)941
81V)PN9
BB9)783
XW5)VY8
RTK)ZDS
Q2S)8MC
3K3)TGZ
QRJ)LVM
L7W)P67
TGB)H51
BYG)ZJT
HCY)ZKR
7LX)TWG
6SC)KZ1
KV2)95M
D88)FVN
289)CBZ
2T5)95F
HGB)3JS
VV7)4DM
NFK)J4V
5ZY)TV2
LB5)DY4
8X5)F8J
1X3)D6D
KVT)QKH
DHR)PLJ
B9N)7HH
TT3)6XZ
SK6)KV2
MBD)WHV
665)JR8
JZJ)VS3
7NG)XR8
2HS)GRM
KPN)DPP
BQH)HXQ
HY2)SC4
ZLV)5TQ
PD4)XXZ
9K5)NJB
XGW)L2S
B3R)61V
QDJ)772
9XC)KQD
613)DHC
4ZF)KBW
B81)Z1D
S82)V6R
2R7)X2M
GJK)8G1
JSG)7C2
S9B)W8J
H96)94Y
JCQ)6BP
5CV)QKD
PCX)PH4
BH7)8K2
KSX)THJ
YMX)9D3
JKD)YMX
XV1)NSF
DY4)S3P
TQ4)V48
GQ7)26Q
S2D)LW9
4XL)Z2H
9SM)ZY1
Z7H)TBN
B1H)17H
GG8)B1S
NDN)Y16
VDN)YXP
YXC)PSP
FBY)7X7
LNB)85D
FJ6)5LG
H87)3NW
WJZ)XW5
DCY)15W
V9R)23C
1QD)MPS
3S4)1WB
6JN)2LJ
7HH)FTN
8C3)ZJM
F76)Y4W
H4X)YOU
BXD)WSL
DMD)655
M2G)4HS
NJS)XCR
66Q)P1S
GCK)9Y2
TB2)2BB
VGW)9RZ
5DM)D1Z
YVN)QPZ
GM7)499
5BT)GM7
1W7)XKB
BHW)P28
BDV)P8X
SXS)JB2
7ZS)96C
RJN)ZW1
HNW)LXK
4QQ)V72
M6G)Y7V
L7M)WH6
2XD)54M
NRW)KYP
6RJ)4TG
J3N)PG8
5RF)BPC
SNL)L23
NLB)GMX
17Y)ND2
S3J)X24
SNP)84Y
Y22)95H
13W)LBS
B3S)LB9
2X3)L5V
1ND)JPQ
R1B)BJ3
SD5)N1R
4D5)C7G
6JH)VJ5
XSN)CML
7WX)7LX
Y7V)K1M
GC1)4DV
BG6)FNV
68H)T9L
ZXN)RKW
1DF)NDR
1YQ)QWH
4MP)914
FRK)HDR
NPD)HVG
64Q)N71
916)BQS
FKY)68D
W6D)FMD
YNN)T95
DDP)5KB
Q6Q)CHK
V6R)1T4
V8K)3X2
MPC)ZLV
SQB)DQD
MJW)VX9
FQ1)66J
LJK)WG4
N7Z)NLG
H57)ZTL
1ZG)X6Y
PV1)N87
YJB)GN1
9LM)7BX
95F)B9N
FVV)PDH
LDB)BYT
J88)4DS
9F5)3XR
64P)NX7
599)6VC
FNK)3K3
H53)FRK
K1M)R4M
R9Q)KPH
DDP)HLC
W6G)GX8
4WG)1TC
Q27)8CN
B1G)ZLB
VMM)XCP
9RZ)WQM
HLB)VCQ
VYN)9HP
112)M2K
Z84)CR7
KRN)WN2
5JJ)YXY
WN2)6TT
Y6T)XNS
H1T)MC7
VD4)3C5
NCQ)JZS
9W9)46W
HZ6)SQB
XZM)PZ5
YXZ)3CS
8WR)DVY
C1Q)B1H
NRX)WX9
YNV)MLP
37X)CCZ
PG8)ZK2
SFS)4D5
NPR)R42
5LG)5RF
BYG)YPK
WB2)8LY
XNS)11F
BL2)SSP
LJV)CB2
KP7)KRN
L5V)ZKF
PB5)S63
YKM)3L4
13V)CM6
Q61)C8X
87R)S45
631)4N7
MKP)F5C
BTK)5ZY
NDR)P4V
FLY)WYH
RV7)GND
DLM)N3N
WYH)1GL
W7J)XBX
MPZ)JHW
XBC)DNM
YNV)QBF
KBQ)T1N
LPP)PB5
VMJ)9BF
Z2B)DPC
CC3)F4S
Q78)BWQ
65B)PYQ
8CN)CGD
949)SC5
XH8)VQZ
ZMX)9L9
NQL)21D
Y89)8Y5
5VK)S3M
3W6)C4W
6LT)SRD
KTD)BM7
DRR)ST4
XRS)B9C
1CG)CGX
9R4)RCB
7HW)VDM
2S1)XZK
769)Q97
QMX)BJ8
2TY)2MJ
ZJT)62D
N68)L1Q
23G)5BH
8LY)1YQ
FGC)W7J
5V3)KPN
ZTD)NYY
N3K)QMP
GW6)G3Q
TF2)XSK
ZZR)JWL
7X1)GTK
TL9)H2S
TQC)SFS
RFY)JYK
FGY)NBT
KQ2)4X9
FTN)8BV
1HC)FWS
Z2H)5BZ
JQR)V5D
NJB)YNN
QDK)SR3
9L3)9BC
RDR)19V
SK2)497
LW8)FK7
RGV)HZ3
95P)SQH
F6W)DP6
JWL)NFZ
ZRY)Q83
BS2)R9S
JQ7)79D
9XT)NMC
P67)V9R
FWS)PWN
L31)RV7
59Q)LJZ
KS8)3DW
Z24)KTY
HK5)FXD
Q38)C77
Z21)N44
T23)N4M
8LS)1JR
7G1)RGV
KBW)7FJ
HMK)HG2
XBZ)SS9
GPW)9ML
914)SM6
QK3)3W5
SQ8)X83
TBD)41H
5DR)C3F
YV3)LZW
Z8W)7JS
Q2M)DYH
Y2B)7D3
SS9)59Q
Y55)J6V
BJ3)Z84
SC4)KBX
KLQ)PVB
XVN)N3K
17H)6D4
J2B)ZDN
S4C)VSJ
FBR)KH8
9R8)1FH
P1D)VYN
X2M)631
BJ8)KSW
5BH)9F4
FF9)J9B
YYX)VJ4
SQD)NPD
95K)N57
662)2G2
GTK)RGR
57G)M77
392)5V3
TK1)BRN
XJM)HRN
ZTL)XFG
VX9)L8N
FS9)SWW
K34)H49
NLG)RXN
67Y)RQ2
X17)7YN
YXP)18M
3X2)3CK
W8B)4HT
67K)WN3
3HL)M6G
LN1)YKD
DY6)L31
8X5)7BN
8H2)YSV
134)R6P
V72)SDP
KT1)YBY
6RM)ZGT
8DZ)5C5
614)DGM
KF2)V9M
X7H)TB2
L38)JD1
9XS)4QP
VZF)MNV
91M)CL9
WBR)N8Z
M7Z)VZL
T2R)XH6
CXH)J2V
79D)5DM
GMX)6M2
QLR)J3N
65B)Y92
9LQ)1HR
398)NYL
H51)GJN
B5F)Z79
DTB)31D
9ZT)XMX
JB4)74Y
J3L)VD4
P1R)WKS
BVB)JSQ
NC5)RXT
936)7YM
T95)D3K
9CJ)BBL
MKT)DDK
JWC)JVM
NYV)MV5
CBZ)2X4
BM7)Q38
8GF)XTC
F5C)GP7
9BM)QMX
6QB)BZF
ZTR)LGX
JBL)8CK
MMW)V3J
HB1)32M
2VF)SLL
Y3P)5WC
9CF)KR9
RJN)B5L
WHD)ZPR
RGV)BG6
H9V)ZW6
Z7H)Y8Z
H7G)7JB
734)KPT
RGF)2QP
NC4)K6T
D7P)Z74
JFW)Y89
TPK)SVL
XXP)6KD
J25)3GY
JPQ)L7W
XVK)QVH
L2T)C9Z
MX6)ND7
QM8)CXX
5TV)PXS
DRP)J34
T3P)Y6M
42S)LPJ
7FW)VMS
8QL)32B
9HZ)BTK
2J6)FV7
FM7)VQB
ZPK)3V6
HG2)42S
CMS)LJV
5ZD)BDC
CHK)2PC
LXK)MKB
KLX)8XQ
7KX)QWL
JD9)481
8H3)NXD
7FJ)V87
4D7)967
BB9)FVT
MPS)3VH
7BW)XVK
FSZ)2TY
CQS)S42
Y6T)Z2B
233)B1G
V3B)PRD
PD4)796
BN2)B81
8CK)S4C
S3P)VYH
RJ7)21K
HXD)S5B
DG1)91R
L8N)1X3
VDM)R28
WJ8)BG1
WRF)PLM
D6D)54T
RFW)W3K
25T)NLB
NZD)VGW
ZLB)81V
J4L)NWK
5VZ)GD7
1FH)MSZ
KTC)LQ8
G81)2R7
G2X)G5B
C3Q)NQL
4GJ)H4X
Y5Z)TF2
499)YCX
HLB)4DZ
R28)3DD
84Y)YLR
7D3)YG2
JNW)QDJ
SS9)6ZX
WWT)CZG
7G6)HSJ
SSP)YJB
DYD)VD1
X7M)JZJ
L6W)423
V87)C55
FCX)NZF
XCQ)TQ4
M8R)7C7
C6Q)Q6W
3CS)BWV
C87)W56
RXT)XXR
W3M)2LP
43B)FQ1
ZVP)S92
Q6R)9K5
PFM)64P
X83)65B
MJK)85W
DPP)D6B
LN7)4YD
6D4)PKX
7FS)5D7
4L3)3M8
H3K)JK2
FK4)J88
SK2)RH1
97Z)2RT
85W)K5D
JWZ)VG9
6HZ)936
NG3)ZVP
QW7)6JH
C77)KB1
9HS)WVY
9LM)PFM
165)25T
3JC)XKV
COM)7ZS
S5B)QFB
ZTP)R6B
3VJ)SGM
PLM)TT3
BYQ)X8Z
9L9)26F
Y5R)XVQ
Y94)GG4
TBN)VDN
4QP)1QX
WDL)199
VCV)JD9
YKM)Y9K
P1D)MPC
SF4)291
ZTY)DG1
BD3)59F
Q97)5Z1
PF7)3JK
JPT)BF8
RMJ)153
9WX)125
WCS)3NJ
PLJ)T3P
24F)1W7
Z6B)KRD
QKD)FFB
HYK)9L3
3DF)TYH
NYY)S5K
CML)L38
S8C)V64
4G9)HG7
T65)8PP
9RC)JQR
L23)5TV
DVY)6MK
ZNH)85B
TV2)Q6Q
KTY)VSD
8W5)HZ6
FFB)WQC
PVZ)2HS
7QX)3S4
4TP)NC4
3GY)5KQ
QRC)D34
JTF)WNP
2X4)BR4
XQC)R2L
7RF)K7B
WJ2)TN4
R9Q)9MB
VH3)JM8
SM6)N7H
PVB)W6D
LK4)XM6
ZQS)9R8
P5G)1TY
4DZ)6KQ
93Z)1TM
421)8SW
PK7)VYW
VJ5)7QC
BF8)665
MGV)91Q
7CG)346
15W)4ZR
XJ1)9HS
H4K)9LQ
3XR)QBC
F9W)C4K
LWG)29W
5WC)13V
4MH)WV8
41Z)1YZ
L8D)L2Z
JJB)1D6
L2S)GG8
Y4W)N84
8MS)JFW
H4C)JM1
GCZ)5XR
L35)Y9Z
4YD)QLR
HSP)9RC
7C7)WBM
V9M)2T5
B8L)97Z
B2D)6MH
FVL)P2V
CGD)37X
P2L)WF6
2TZ)S7Z
3ZD)NTJ
2TD)6K6
L17)FVV
ZDK)JWZ
T5C)66Q
QBF)XPB
HJH)XF7
STC)X17
12N)XRS
83H)7FS
YST)V5J
NT8)ZQ8
S4K)TS7
BWZ)NG3
LBK)M5D
9BF)VDL
CZG)47W
HSJ)1WS
ZNS)WFP
VZG)DSJ
3DD)V2N
M55)KTC
ZW1)5VK
BT1)LFR
1X7)MR4
1FC)ZBX
29W)2FM
6QL)KN2
YM4)Z8W
PPR)LJJ
ZGT)7CZ
BQS)STW
VYK)RPC
F8D)7W4
2CF)KJ6
YDS)ZNF
RVF)H3K
12N)SPC
NX7)73H
HGB)5KP
Y1X)895
GND)FVP
4VV)TX5
ZMX)9CJ
97F)QDK
CVG)541
K7L)H2R
ZPR)N74
8BF)WCR
C3F)PPR
FYQ)WDL
6GB)QW7
Q7H)227
3BV)N3F
F8B)WDS
NYL)GD3
CBH)QCH
8CS)HKL
VM2)C87
YWN)J3L
6MH)Y7P
2LP)JNW
26F)734
8T3)7KX
KBX)217
W6H)MJW
M2K)Z12
KR9)Q49
1WS)8CM
LJZ)D97
BXZ)X1B
VSD)MPN
G3Q)W74
BRN)KT1
1GL)CZN
MC7)K3L
QN2)4XL
MQ9)X3B
Q93)6FV
D6L)C9K
KDY)LYG
7XR)2BR
WDZ)F57
H9K)BJV
HCW)SSH
87L)NCQ
WH6)WMF
PWZ)9V9
LJJ)BDV
X3B)1VQ
BWV)662
9NW)JSG
WJG)QFN
T9Z)KL6
6WL)PC5
XH1)6PH
4X8)T2R
ZDK)HZR
HCY)NQ1
BFX)XVN
1R1)57G
S5K)VTB
523)5CV
HPK)S4K
1RT)CMX
YRR)ZTY
3F7)5H1
X9B)PF7
7BX)64Q
RQK)MNJ
BT8)Z4P
5H1)QHZ
DJ2)CR4
VN4)NRW
Z8Y)V7Y
JFS)P8C
GRN)3BK
Z3D)Q69
S87)23G
V47)QRM
JZS)4JM
3K3)QR8
3DW)6SR
N7H)1F5
BDT)NGC
Y9Z)LLN
VHP)GBH
32K)YV3
23C)Z3D
XF4)7WX
ZX1)JBL
GWZ)XV4
C52)CBL
4NG)8T3
3DW)XW1
KR9)GC7
QKT)WMX
RPC)595
186)FGY
GK8)8C3
YDF)599
59J)RMJ
X48)MWV
K6T)37J
1RW)YY3
KGB)ZLP
115)JBQ
6SR)BWZ
VCQ)BB9
Q3K)L4P
7CG)Y5R
JKD)X3G
2FW)9NW
XH6)HGB
SC7)M11
HPB)35J
7K3)N59
941)C36
YTC)LWG
STW)YXZ
K7B)3J6
7X7)YVN
H2Z)ZF5
V8G)JZ7
DLM)JWC
QFB)61X
3JS)XKQ
M11)45V
R9S)769
R2L)G81
M5V)4ZF
XV4)GWH
71Q)P2W
JK2)YNK
8DM)B8P
DJ1)N5B
F5K)VYK
N71)R9Q
1WS)186
TYC)7MP
SRD)1JJ
47J)STC
8PP)7D7
V3J)8H2
2L4)1T1
3J6)CMS
YBY)RFW
SKQ)X48
QBR)QN2
KRD)S87
F4S)1PV
8CW)3FF
CB2)FCY
JBQ)Y69
RQ2)XFV
1JR)XMJ
RFX)QSD
5Y6)BWT
1YZ)JFS
RFY)WBR
6PH)FW7
RNF)B9G
KPH)7G6
18M)95K
X7H)G4F
M5S)L2T
CH7)3VJ
NDR)D7P
MSZ)6HX
CZG)RVH
Q1B)VGV
VS3)BYG
KJ6)QK3
4M8)JKD
WDP)3SJ
C4K)4M8
XVQ)99T
Q83)LN1
YTC)ZPY
X4Y)GZX
17D)YMB
35J)KFX
8W5)WJ8
G6X)X7M
1VQ)C1T
GRM)SD5
BM7)YRJ
KFC)3BV
LNF)1CG
Y92)8R8
LQ8)5BT
755)Z5W
YRJ)FXR
B2D)NPR
1Q1)NFK
WVY)97F
KB1)VC2
LX7)D7L
DP6)3F7
JYF)Y55
4HT)G3P
GP7)CJZ
H2S)XGW
THC)392
Z1D)1RT
FXN)2L6
J7X)Y9J
CCZ)M8R
ZNM)3DF
4WF)Q7H
LDM)1HC
4QP)VW9
JZ7)89G
3WY)7H9
N8K)G86
XF7)Q93
XL3)64Y
KQD)FS6
6ZX)BXZ
W36)FF9
ZGM)MGC
5Z1)8L9
N3F)8ZH
FJT)ZN3
7LR)H4C
21D)47J
4X9)5Y6
2LJ)T23
XRS)SF4
WFP)L3Q
Z34)1M6
Y7P)MQ9
Z7D)KSX
47W)XJM
W3K)BRR
MNV)6QB
XKB)XF4
VZF)N68
W74)2FW
SC5)LNB
JBM)459
C73)B8L
8SW)HLB
7F6)PJX
VGV)X9B
D34)1KQ
3NW)J4L
W28)KFQ
XY2)D63
896)9F5
8R2)VMM
C9Z)1QD
4WX)DH7
9LQ)RFX
9N4)VN4
4HS)XZM
YPK)233
QR8)YXC
BDD)91M
KFQ)M4L
967)VKB
FXD)YTC
Q69)P1R
24F)FZK
XMX)V7S
F57)ZDK
7G8)HY2
8K2)QRJ
VYW)SC7
KFX)J25
FNV)SMT
MLP)P2L
V31)ZQS
Z18)FM7
MWV)7RF
G3Q)SXS
K3L)8YF
ZLP)H4K
Z5W)MBM
RH1)FCX
Y9J)GY3
26D)HZB
VG9)87R
GY3)4X8
W2R)8DZ
YNK)421
QVH)7ZB
ZY1)W31
XY3)916
QRM)Q2M
32B)7X1
2JZ)RVF
8G1)X4Y
Y16)112
CMT)BXD
MHZ)N21
G93)46Q
VKB)CD6
ZPN)H9V
L7J)17D
3M8)Q2S
62D)3BJ
4LW)SQD
1X7)5Q4
FWH)38Q
357)J7X
HMK)LPP
DSJ)BDD
D7L)KS8
SR3)GQ7
61X)LBX
JWP)7XX
WP7)13N
YZ3)WRF
5Z4)D6V
PBR)LB5
2TZ)WCS
MBD)3H3
2FC)59J
XXZ)7NG
7WG)3PW
9F4)2PM
4MX)XH1
JKF)GDT
3TP)BN2
K4D)P1C
46W)FXN
6GC)V8G
H49)9HG
WMF)RF3
VFP)PLT
217)NRX
N1D)W97
S4K)9VX
21D)PB1");

        }
    }
}
