using DirectDependency;
using System;

class Program
{
    static void Main(string[] args)
    {

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
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Message: {ex.InnerException.Message}");
            }
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
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.GetType().Name}");
                Console.WriteLine($"Inner Message: {ex.InnerException.Message}");
            }
        }
    }
}
