using DirectLibrary;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Attempting to use DirectLibrary (which depends on MyLibrary v1.0.0)...");

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
                Console.WriteLine($"Inner: {ex.InnerException.Message}");
            }
        }
    }
}
