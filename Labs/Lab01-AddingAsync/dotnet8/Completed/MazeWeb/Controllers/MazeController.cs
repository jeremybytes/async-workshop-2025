using Algorithms;
using MazeGeneration;
using MazeGrid;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using System.Reflection;

namespace MazeWeb.Controllers;

public class MazeController : Controller
{
    public async Task<IActionResult> Index(int size, string algo, MazeColor color)
    {
        int mazeSize = GetSize(size);
        IMazeAlgorithm algorithm = GetAlgorithm(algo);
        Image mazeImage = await GenerateMazeImage(mazeSize, algorithm, color);
        byte[] byteArray = await ConvertToByteArray(mazeImage);
        return File(byteArray, "image/png");
    }

    private int GetSize(int size)
    {
        int mazeSize = 15;
        if (size > 0)
        {
            mazeSize = size;
        }
        return mazeSize;
    }

    private IMazeAlgorithm GetAlgorithm(string algo)
    {
        IMazeAlgorithm? algorithm = new RecursiveBacktracker();
        if (!string.IsNullOrEmpty(algo))
        {
            Assembly? assembly = Assembly.GetAssembly(typeof(RecursiveBacktracker));
            Type? algoType = assembly?.GetType($"Algorithms.{algo}", false, true);
            if (algoType is not null)
            {
                algorithm = Activator.CreateInstance(algoType) as IMazeAlgorithm;
            }
        }
        return algorithm!;
    }

    private async Task<Image> GenerateMazeImage(int mazeSize, IMazeAlgorithm algorithm, MazeColor color)
    {
        IMazeGenerator generator =
            new MazeGenerator(
                new ColorGrid(mazeSize, mazeSize, color),
                algorithm);

        Image maze = await generator.GetGraphicalMaze(true);
        return maze;
    }

    private static async Task<byte[]> ConvertToByteArray(Image img)
    {
        using var stream = new MemoryStream();
        await img.SaveAsPngAsync(stream);
        return stream.ToArray();
    }
}
