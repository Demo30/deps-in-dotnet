using DirectDependency;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Diamond dependency scenario with NATURAL FileLoadException:");
        Console.WriteLine("- DirectDependencyA depends on TransitiveDependency v1.0.0");
        Console.WriteLine("- DirectDependencyB depends on TransitiveDependency v2.0.0");
        Console.WriteLine("- v1 and v2 signed with DIFFERENT keys -> different PublicKeyTokens");
        Console.WriteLine("- NuGet picks v2.0.0 (highest version wins)");
        Console.WriteLine("- Result: DirectDependencyA will fail looking for v1 with specific PublicKeyToken!");
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
