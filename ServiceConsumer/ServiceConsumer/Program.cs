using DirectDependency;
using System;

class Program
{
    static void Main(string[] args)
    {
        var input = "Main app";
        var directDepA = new LogicWrapperProcessor();
        var directDepB = new LogicWrapperProcessorB();
        
        try
        {
            var simpleResult = directDepB.Compute(input);
            Console.WriteLine($"[DirectDepB] Method 'Compute' was SUCCESS: {simpleResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DirectDepB] Method 'Compute' was FAILURE: {ex.GetType().Name}");
        }
        
        try
        {
            var simpleResult = directDepA.ComputeSimple(input);
            Console.WriteLine($"[DirectDepA] Method 'ComputeSimple' was SUCCESS: {simpleResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DirectDepA] Method 'ComputeSimple' was FAILURE: {ex.GetType().Name}");
        }

        try
        {
            var complexResult = directDepA.Compute(input);
            Console.WriteLine($"[DirectDepA] Method 'Compute' was SUCCESS: {complexResult}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DirectDepA] Method 'Compute' was FAILURE: {ex.GetType().Name} - {ex.Message}");
        }
    }
}
