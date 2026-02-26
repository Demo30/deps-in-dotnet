using DirectDependency;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Simple binding redirect scenario:");
        Console.WriteLine("- DirectDependency compiled against TransitiveDependency v1.0.0");
        Console.WriteLine("- Binding redirect forces v2.0.0");
        Console.WriteLine("- v2.0.0 doesn't exist, v1.0.0 is on disk");
        Console.WriteLine("- Result: FileLoadException with inner exception!");
        Console.WriteLine();

        try
        {
            var worker = new Worker();
            var result = worker.DoWork("Test");
            Console.WriteLine($"SUCCESS: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"FAILED: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Message: {ex.InnerException.Message}");
            }
        }
    }
}
