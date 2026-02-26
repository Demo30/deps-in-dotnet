using DirectDependency;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Diamond dependency scenario:");
        Console.WriteLine("- DirectDependencyA depends on TransitiveDependency v1.0.0");
        Console.WriteLine("- DirectDependencyB depends on TransitiveDependency v2.0.0");
        Console.WriteLine("- NuGet picked v2.0.0");
        Console.WriteLine("- App.config binding redirect forces v9.0.0 (doesn't exist!)");
        Console.WriteLine();

        try
        {
            var workerA = new Worker();
            var resultA = workerA.DoWork("TestA");
            Console.WriteLine($"WorkerA SUCCESS: {resultA}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WorkerA FAILED: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
        }

        Console.WriteLine();

        try
        {
            var workerB = new WorkerB();
            var resultB = workerB.DoWorkB("TestB");
            Console.WriteLine($"WorkerB SUCCESS: {resultB}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WorkerB FAILED: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
        }
    }
}
