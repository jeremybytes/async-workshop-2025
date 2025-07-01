using MazeGrid;

namespace Algorithms;

public class RecursiveBacktracker : IMazeAlgorithm
{
    public void CreateMaze(Grid grid)
    {
        Random rnd = new();
        Stack<Cell> stack = new();
        stack.Push(grid.RandomCell());

        while (stack.Any())
        {
            Cell current = stack.Peek();
            List<Cell> neighbors = current.Neighbors.Where(c => c.Links().Count == 0).ToList();

            if (neighbors.Count == 0)
            {
                stack.Pop();
            }
            else
            {
                int index = rnd.Next(neighbors.Count());
                var neighbor = neighbors[index];
                current.Link(neighbor);
                stack.Push(neighbor);
            }
        }
    }
}
