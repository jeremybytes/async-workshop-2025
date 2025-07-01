using SixLabors.ImageSharp;

namespace MazeGrid;

public enum MazeColor
{
    White = 0,
    Teal,
    Purple,
    Mustard,
    Rust,
    Green,
    Blue,
}

public class ColorGrid : Grid
{
    private int? maximum;

    public override Distances? distances { get; set; }
    public override Distances? path { get; set; }
    public MazeColor color { get; set; }

    public ColorGrid(int rows, int columns, MazeColor color = MazeColor.Purple)
        : base(rows, columns)
    {
        includeBackgrounds = true;
        this.color = color;
    }

    public override string ContentsOf(Cell cell)
    {
        if (path != null &&
            path.ContainsKey(cell))
        {
            return path[cell].ToString().Last().ToString();
        }
        {
            return " ";
        }
    }

    public override Color BackgroundColorFor(Cell cell)
    {
        maximum = distances?.Values.Max();
        if (maximum != null &&
            distances != null &&
            distances.ContainsKey(cell))
        {
            int distance = distances[cell];
            float intensity = ((float)maximum - (float)distance) / (float)maximum;
            byte dark = Convert.ToByte(255 * intensity);
            byte bright = Convert.ToByte(128 + Convert.ToByte(127 * intensity));


            return color switch
            {
                MazeColor.White => Color.White,
                MazeColor.Teal => Color.FromRgba(dark, bright, bright, 255),
                MazeColor.Purple => Color.FromRgba(bright, dark, bright, 255),
                MazeColor.Mustard => Color.FromRgba(bright, bright, dark, 255),
                MazeColor.Rust => Color.FromRgba(bright, dark, dark, 255),
                MazeColor.Green => Color.FromRgba(dark, bright, dark, 255),
                MazeColor.Blue => Color.FromRgba(dark, dark, bright, 255),
                _ => Color.FromRgba(dark, bright, bright, 255),
            };
        }
        else
        {
            return Color.White;
        }
    }

}
