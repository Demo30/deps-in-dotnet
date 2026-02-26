using DirectDependency;
using System;

class Program
{
    static void Main(string[] args)
    {
        var input = "Main app";
        var directDepA = new LogicWrapperProcessor();

        Console.WriteLine("=== Testing DirectDepA methods ===");

        try
        {
            var simpleResult = directDepA.ComputeSimple(input);
            Console.WriteLine($"Method 'ComputeSimple' was SUCCESS: {simpleResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Method 'ComputeSimple' was FAILURE: {ex.GetType().Name}");
        }

        try
        {
            var complexResult = directDepA.Compute(input);
            Console.WriteLine($"Method 'Compute' was SUCCESS: {complexResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Method 'Compute' was FAILURE: {ex.GetType().Name} - {ex.Message}");
        }
    }
}
