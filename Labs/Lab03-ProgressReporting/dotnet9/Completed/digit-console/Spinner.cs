namespace digit_console;

public static class Spinner
{
    private static bool Running = false;

    private static async void Spin()
    {
        await Task.Run(async () =>
        {
            int count = 0;
            while(Running)
            {
                char line = (count++ % 4) switch
                { 
                    0 => '|',
                    1 => '/',
                    2 => '-',
                    3 => '\\',
                    _ => ' ',
                };
                Console.SetCursorPosition(0, 1);
                Console.Write($"  {line}  {line}  {line}");
                Console.SetCursorPosition(0, 1);
                await Task.Delay(40);
            }
            Console.SetCursorPosition(0, 1);
        });
    }

    public static void StartSpinner()
    {
        Running = true;
        Spin();
    }

    public static void StopSpinner()
    {
        Running = false;
    }
}
