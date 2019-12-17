<Query Kind="Statements" />

var input = "ZYZYXWZZYZYXWZZYZXXWZYXWZXXWZZYZXXWZ";
var substrings = Enumerable.Range(2,5)
	.SelectMany(i => Enumerable.Range(0, input.Length-i).Select(ii => input.Substring(ii, i)))
	.Distinct()
	.ToList();
//	substrings.Dump();
//Console.WriteLine(substrings.IndexOf("ZYZ"));
//Console.WriteLine(substrings.IndexOf("YXWZ"));
//Console.WriteLine(substrings.IndexOf("XXWZ"));

var goodRe = new Regex("^[ABC]*$");
for (var a = 0; a < substrings.Count; a++)
{
	var are = new Regex(substrings[a]);
	for (var b = a + 1; b < substrings.Count; b++)
	{
		var bre = new Regex(substrings[b]);
		for (var c = b + 1; c < substrings.Count; c++)
		{
			var cre = new Regex(substrings[c]);

			var output = cre.Replace(bre.Replace(are.Replace(input, "A"), "B"), "C");
			//			Console.WriteLine($"{a},{b},{c} -> {output}");
			if (goodRe.IsMatch(output))
			{
				Console.WriteLine(output);
				Console.WriteLine(substrings[a]);
				Console.WriteLine(substrings[b]);
				Console.WriteLine(substrings[c]);
			}
		}
	}
}
//var groups = substrings.ToDictionary(i => i, i =>
//{
//	var starts = new List<int>();
//	var ix = 0;
//	while (true)
//	{
//		ix = input.IndexOf(i, ix);
//		if (ix == -1) {
//			break;
//		}
//		starts.Add(ix);
//		ix++;
//	}
//	return starts;
//});
//groups.Dump();