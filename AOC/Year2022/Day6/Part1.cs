using System;
using System.Linq;
using AoC;

namespace AOC.Year2022.Day6
{
    public class Part1 : BasePart
    {
        // 11:15:15 - 11:18:26, time: 3m11s, 100 @ 1m52s
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);
                var line = lines[0];

                var firstUniqueStart = Enumerable.Range(0, line.Length - 3)
                    .Select(i => new { i, v = line.Substring(i, 4) })
                    .First(i => i.v.Distinct().Count() == 4)
                    .i;

                Console.WriteLine(firstUniqueStart+4);
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
