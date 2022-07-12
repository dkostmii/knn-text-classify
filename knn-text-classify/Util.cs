internal static class Util
{
    public static IEnumerable<char> Vectorize(string text)
    {
        return text.ToCharArray();
    }

    public static IEnumerable<(Tag, string)> NormalizeDataset(IEnumerable<(Tag, string)> dataset, int length)
    {
        return dataset.Select(record =>
        {
            var (tag, text) = record;

            return (tag, NormalizeString(text, length));
        });
    }

    public static string NormalizeString(string text, int length)
    {
        if (length >= text.Length)
            return text.PadRight(length);

        throw new Exception($"Cannot normalize text with length {text.Length} to smaller length {length}");
    }

    public static IEnumerable<(Tag, string)> NormalizeDataset(IEnumerable<(Tag, string)> dataset)
    {
        var maxLength = int.MinValue;
        foreach (var record in dataset)
        {
            var (_, text) = record;

            if (text.Length > maxLength)
                maxLength = text.Length;
        }

        if (maxLength < 1)
            throw new Exception("Expected length to be > 0");

        return NormalizeDataset(dataset, maxLength);
    }

    public static IEnumerable<(Tag, IEnumerable<char>)> VectorizeDataset(
        IEnumerable<(Tag, string)> dataset)
    {
        return dataset.Select(record =>
        {
            var (tag, text) = record;

            return (tag, Vectorize(text));
        });
    }

    public static decimal Distance(string avector, string bvector)
    {
        var avec = Vectorize(avector);
        var bvec = Vectorize(bvector);

        var avecLength = avec.Count();
        var bvecLength = bvec.Count();

        if (avecLength != bvecLength)
            throw new Exception($"Expected vectors to have same length. Got: avector size - {avecLength}, bvector size - {bvecLength}");

        double squaredSum = 0;

        foreach (var record in avec.Zip(bvec))
        {
            var (a, b) = record;

            squaredSum += Math.Pow(b - a, 2);
        }

        return decimal.Round(new decimal(Math.Sqrt(squaredSum)), 6);
    }

    public static void DisplayResult(IDictionary<Tag, decimal> result)
    {
        foreach (var kv in result)
            Console.WriteLine($"Probability of {kv.Key} = {kv.Value * 100}%");
    }
}
