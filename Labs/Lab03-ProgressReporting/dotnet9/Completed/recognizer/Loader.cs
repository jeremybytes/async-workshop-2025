namespace digits;

public class FileLoader
{
    public static (DigitImage[], DigitImage[]) GetData(string filename, int offset, int count)
    {
        var contents = File.ReadAllLines(filename)
                           .Where(s => s.Trim().Length > 0)
                           .Skip(1);
        List<DigitImage> data = [];
        foreach (string line in contents)
        {
            var split = SplitRawData(line);
            var digitImage = ParseDigitImage(split);
            data.Add(digitImage);
        }

        return SplitDataSets(data, offset, count);
    }

    private static List<int> SplitRawData(string data)
    {
        List<int> results = [];
        var items = data.Split(',');
        foreach (var item in items)
        {
            if (int.TryParse(item, out int i))
            {
                results.Add(i);
            }
        }
        return results;
    }

    private static DigitImage ParseDigitImage(List<int> data)
    {
        var value = data[0];
        List<int> image = data[1..];
        return new DigitImage(value, image.ToArray());
    }

    private static (DigitImage[], DigitImage[]) SplitDataSets(List<DigitImage> data, int offset, int count)
    {
        var training = data[..offset];
        training.AddRange(data[(offset+count)..]);
        var validation = data[offset..(offset+count)];
        return (training.ToArray(), validation.ToArray());
    }
}
