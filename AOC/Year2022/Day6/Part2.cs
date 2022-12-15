using System;
using System.Linq;
using AoC;

namespace AOC.Year2022.Day6
{
    public class Part2 : BasePart
    {
        // 11:15:15 - 11:19:36, time: 4m21s, 100 @ 2m25s
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);
                var line = lines[0];

                var tl = 14;
                var firstUniqueStart = Enumerable.Range(0, line.Length - tl-1)
                    .Select(i => new { i, v = line.Substring(i, tl) })
                    .First(i => i.v.Distinct().Count() == tl)
                    .i;

                Console.WriteLine(firstUniqueStart + tl);
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"mjqjpqmgbljsphdztnvjfqwrcgsmlb");
            RunScenario("initial", @"bvwbjplbgvbhsrlpgdmjqwftvncz");
            RunScenario("initial", @"nppdvjthqldpwncqszvftbrmjlhg");
            RunScenario("initial", @"nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg");
            RunScenario("initial", @"zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw");
            //return;
            RunScenario("part1", @"rvnvzvhzzjgjgffclllnhhtltptgptgpttjhttsllmbbphbpbzpbpjjcwjwqwccnrrtvrtrfrwffnqffsggwzzhtzhthqhffmrrzsrrnrtrqqbllhrlrjrvvrvvgdgjgfjjtzznffrfvfggswgssccpcwpccstcsstwssgzssswsgwsgsnsshcsscsffcwcmmhmsmrsrddwhhszsfsjfsfccwssvdssmzzwrzzjfzzvzzfsfsvshschhvlhhvmvpvhhstsdtstgtrrtjjcssgfssnsnfsnscnsslvvqbbhthqtqzqmzmjjfmmbdmmzdmdpmpnnfjnnrwnwbwrrwjrrttfzzlwwfmmnhhqlhqqbnqnqznncmmhfmfftsfszssftstggppfddtztltctqcqjcqcmclcnccdgcggvmvvcsstztbzttsccmgcghcczfzszbzmbmjjggsqgghbhjjhhrrjljmllczlzzqssjfjqfjfmfdfgffczfzwwpfwpwbbftbffplphhfcftctlcclhlnlhnhlhnhqnhhjbjmmtpmmlsmllvhhjghhmzzwpzprrjrzzvrvcvqvssgssnccmgmppfhfwhhczhhprhrffpvvqrqvqccdhdvvvssvdvwdwwzggfvfnvvqvwwzwmzzvttgzzwjjfvvpvffqpqhqrrwccjbjppbzbrbrrphrhgrhhzbbhrrblbjjznnlccsffpcfpcfpfddfwddbffczztzsttzsstddrqdqqchqccrlccfcwwghwhnhbhjbhhzrrgprrftrffqlljlslggzdzvvrgvgcgnnngtnggdcgddhjhchqcqfqwfwjffqppblbddgmdgmddrnddjtdjttgzgpzggwvwjvwwhdwwbvvtftcfttpftpfpzfznnchnhrrdtdbtdbdppsjsgsttptsptssnwntwnnsshqqgnnqnddhjjtqqjhhmghmhmsstrrhmmwvvlssprsrbbswsddbnntmtbmtbmmtmrrffvjffdrffnttbffbccgchghjgjwwshwswdswdsdsrdsdjdvjjgbjbssptpddzbdddqdpdspswssjrjwwcbbvsspnspslpplzlqlccvlvfllntlntnqnrqnqpqlqflfrfsfgsfswwptwwgqggrhghzghhrthtzhzsswhhzffpsffhnnrvnrvvhmmlvvqsslddhcddsllpmpggrhrcrbbmnmjmqmlmpmrrqwqhqrqhrrgrqqsrrpjpdjppzhphthddlzdzhddjvdjdcjcncnvnzzvwzvzgzjggcllvpllgdgqqdvvwnvwvwqqvwvvrbbtqqwhdztqfzzcqrshjzwqnpdsshmpjzwqdptbvfqzmfnbtlgbbsjbqgnblhbbpsfdzvcmpzfwczcnbdndsjzccjcqnrdglwfrvwtnjwpvpvvgwtmnpzhbwnbqwznmdvdrsjnlsfrpcnhlbmlgmrcjbbvhqnvbrmlnfttjllstqnqqnhqrzrhfqjbfbwfhhjzwtwzjmszzhjnjbhrlbnnpfvdmlftjnfsfnvddqfhqqlljrthptvmhbqrdmdcmljwgbrqrjwcrtmvjgqtblnslgbjmdsrrpgfsctrhwlwnszpljhrfnsfpcsgczzltgsztclhgcqljrcmbbpdlztncnrnrmgrttplcnldfqddqhznmcczbmwvsztmwmcqnrzmlmqchnhnhrrhhfntjzqcbnttspptqwvphlbtpfcbhdqzbhsbvhmsqbsdntwpcrnzvmbgqsgttbqhhblfjpmvfcrzhfnzwrzbzgsdfqndzzhfdnrvsnvfbptthjnhgljhrvwwrlbnfpvvjdjchcgbhfrqvszhrhqvtzplwsptvdhqwlzhcjpclmmrlccvgvtgsfpnjhqhrbqglznpdhmsqwwsbmhmlsmmvvghsmplqjchrfctltmnqnddzqjfpljwljbdqjcqdqzwsbcclqsmsmlstvljwwtfmpnhqzqfjghjfchjccqrchsvngvrnwfwttsnvrdlfvwfptsjcpslvvpmjclfcpljqjszptsgsmntzrdjbgrzmgvzddqrlsndjzzqbznqnphbnwfhtjjlwjpsvffrdrbsbttpgrvmqrdndqvlgzcpfbttvqdgvrmtvfclcbcwllthmdzjcflwpnrsqzrjdzbvqgzsqvjjpjjpnjtcjqhcfbjdqndlcwzhcbjtgtlvtdwctdnqcbcgsmrcrmwjntdwbjdbzmshbvlspjfdbvmlrbdzlmlggthvphnrqlrcdsqpsgqcmpgmgdzvdqlmcldvztpsbmpwjgjfhswplwrvwpbwbsgsvlhdvmpzwnnbwjwmshwcnqfqjdpchfjjcbdnslqchhnznpqpnnctznbtccclgjhmnngdjlmqnzpsdptqqcrblmrlnnpgvrfrtmbjnmspfrbwpclhgtbsghndrjfbggsplvcjnhjjbqzsfdpfnvchzjgbhdqgddfgddvzdcrjlntnsmscqwmpptqgbnvtsvpjvmhcfpbfrpzqbpfhlbjrmbmvdvvnvfqsndglvhfrqcsbsbbprscrbfthzcwcdrprqrwjzwrpblfllpwzlhmqvltjgpcjzpzwbltgwsrgrrhzcqfpvhcprdhnzcfphqrwcvtpcbppjwzmmwjhbvwbblnbwvcqzvlfzjhgmlnhlhrbsplfctggfbhbgwpncznvtmdtqqmjsvsnrlqswzvflrfsncgpdcndlwrfrqwqnqtjmsphwsgzhdjpnsdgrbrfhbfdrntwvgvbwnvwnrmdbhqgrglbfwprflnrljrwsgwtpgtmfhvvghtzndvwlzjchhmlwcncmpvrslrglzjcnhfdqhcrljhgbzpssvdnmfwzstmvrztgpsscfswltnbwrrtcnvbswmmjbmnnnvqwjzhprfnvlbvzzdvbwlwchrvqnwwpbnttbhfdvjjvzsznhczjcncmcrmwtrlsvbwpsrcwqdvgcfjsbqnwmjmtcgpnmcbfbcqhzrjbtlpvwzhjqqprbdnbgzfwlprlcspwjwnfftldqzbcgqnjtglvbpqffdvjbpslqcdzwdnmncvcwfshdhsmssttqfrsbnjgmhfqzlgrbpdfqtfdwslsgphfzzgbzjssfbgnwztzmczplqwjmhtlflpvqqqmrvlllhngtfgsvbbnvhzqbcgpmnlsmpwqwgqfjpzplzjhrslwzrsrgjgpppjlnhrnggcdzvspsztnschnqftgffbtvrpzndzpqmtsfmwgnrvpmtgpvnmqfmwgcvlznwqnjnjwpgnqfjwtdhhmztlvlcvrlzmlpmjdvdnzwbfsshfsvbbhqsmphjhqtlnvlsmvwlqvfqpnjnzlgmdvwzzrllmcgwtqdwphhbwmlrqhqrfmdvtqswvsllqvwmfbbpllsbjbvgsmrcgqvfgsnszfrdlcjbtfgcbclhmzlmqlnhmslcmgrcvjjlbpsjjcznzqwmztcgdgbmrlgwzjzzjrpndfrdzztzzgmsrcnwrvqrdcczrbhdpfjwvqsmbrcvllvjrrbrsqnzldltdgzscjrssvdhzhnvltpgfdfcvfbtqmphdzhpzgjhjbwsmdlbqqgcrhjrwhgvfgdllmmlnpsbtvdrrzmltfwgrcsfrrsdbdmjtrwhnlgcrgjgmbzvzvqbflvbrsqcssjgsvpjmlhtcggzwbvddmwwtfdrlltjqcpwnthczzlzdszvtmrmhbpcgstvrsnmbdcmjzsvnncmgmlnrzzhfvmblgptwwwbrmtcczjwcqmvdrsvfjgqqvghnhbntqcdrppfmvdzbcjvztrnpdhmdpmnvsnzzldvdfqbdrwqqqqsmswthwpjwdrflszbspqhpfwztjjcbdsrftdsrsfdltnfztcslmbsghgrtcscrfmptqplwpmtqdzthgfjhbqdnsffrpjmgczmrlfvzcjttwtmtqfbtlqrttvjdwhfgcgcclrlswmzhzbfhjrggnhwtnffnqqcvldlttvvgrbcqbmqzvtflfmdblhdbzphrqtbshvp");

        }
    }
}
