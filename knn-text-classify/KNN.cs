using static Util;

internal static class KNN
{
    public static (Tag, decimal) ArgMin(IEnumerable<(Tag, decimal)> vector, decimal argument)
    {
        return vector.Where(record =>
        {
            var (_, value) = record;

            return value > argument;
        }).MinBy(record =>
        {
            var (_, value) = record;

            return value;
        });
    }

    public static IDictionary<Tag, decimal> KNearestNeighbours(IEnumerable<(Tag, string)> dataset, string text, int k = 1)
    {
        var dsLength = dataset.Count();

        // K must not exceed dataset length
        if (k > dsLength)
            throw new Exception($"Expected k to not be greater than dataset length ({dsLength}). Got: {k}");

        var dsSelfNorm = NormalizeDataset(dataset);

        var (_, dsText) = dsSelfNorm.First();
        var dsMaxLength = dsText.Count();

        var dsNorm = dsSelfNorm;
        var textNorm = text;

        // Normalize both dataset vectors or target vector
        if (dsMaxLength < text.Length)
        {
            dsNorm = NormalizeDataset(dsSelfNorm, text.Length);
        }
        else if (text.Length < dsMaxLength)
        {
            textNorm = NormalizeString(text, dsMaxLength);
        }

        // Calculate distances to every dataset vector
        var distances = dsNorm.Select(record =>
        {
            var (recordTag, recordText) = record;

            var dist = Distance(textNorm, recordText);

            return (recordTag, dist);
        });

        // Prepare result dict
        var result = new Dictionary<Tag, decimal>
        {
            { Tag.Good, 0 },
            { Tag.Wrong, 0 }
        };

        decimal arg = decimal.MinValue;
        // Sum up the distances to the nearest neighbours
        for (var i = 0; i < k; i += 1)
        {
            var (tag, distance) = ArgMin(distances, arg);
            result[tag] += 1;
            arg = distance;
        }

        // Get the average of repeating keys
        foreach (var kv in result)
        {
            result[kv.Key] /= k;
            result[kv.Key] = decimal.Round(result[kv.Key], 2);
        }

        return result;
    }
}
