using System.Linq;
using System.Collections.Generic;
using System.Text;

using WDict = System.Collections.Generic.Dictionary<string, uint>;
using TDict = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, uint>>;

public static class MarkovHelper	{
    public static TDict BuildTDict(string s, int size)
    {
        var buildTDict = new TDict();
        var previousKey = "";
        foreach (var key in Chunk(s, size))
        {
            if (buildTDict.ContainsKey(previousKey))
            {
                var w = buildTDict[previousKey];
                if (w.ContainsKey(key))
                    w[key] += 1;
                else
                    w.Add(key, 1);
            }
            else
            {
                buildTDict.Add(previousKey, new WDict {{key, 1}});
            }

            previousKey = key;
        }

        return buildTDict;
    }

    public static string[] Chunk(string s, int size)
    {
        var ls = s.Split(' ');
        var chunk = new List<string>();

        for (var i = 0; i < ls.Length - size; ++i)
        {
            var sb = new StringBuilder();
            sb.Append(ls.Skip(i).Take(size).Aggregate((w, k) => w + " " + k));
            chunk.Add(sb.ToString());
        }

        return chunk.ToArray();
    }

    public static string BuildString(TDict dictionaryT, int requiredLength, bool exactLength)
    {
        var ucStr = new List<string>();
        var stringBuilder = new StringBuilder();

        foreach (var word in dictionaryT.Keys.Skip(1))
        {
            if (char.IsUpper(word.First()))
                ucStr.Add(word);
        }

        if (ucStr.Count > 0)
            stringBuilder.Append(ucStr.ElementAt(MarkovMusic.R.Next(0, ucStr.Count)));

        var last = stringBuilder.ToString();
        stringBuilder.Append(" ");

        WDict w;

        for (uint i = 0; i < requiredLength; ++i)
        {
            if (!dictionaryT.ContainsKey(last))
                w = dictionaryT[""];
            else
                w = dictionaryT[last];

            last = Choose(w);
            stringBuilder.Append(last.Split(' ').Last()).Append(" ");
        }

        if (exactLength) return stringBuilder.ToString();
        while (last.Last() != '.')
        {
            if (!dictionaryT.ContainsKey(last))
                w = dictionaryT[""];
            else
                w = dictionaryT[last];

            last = Choose(w);
            stringBuilder.Append(last.Split(' ').Last()).Append(" ");
        }

        return stringBuilder.ToString();
    }

    private static string Choose(WDict w)
    {
        var total = w.Sum(t => t.Value);

        while (true)
        {
            var i = MarkovMusic.R.Next(0, w.Count);
            var c = MarkovMusic.R.NextDouble();
            var k = w.ElementAt(i);

            if (c < (double) k.Value / total)
                return k.Key;
        }
    }
}

