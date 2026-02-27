using DirectDependency;
using DirectDependencyB;
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
            var result = directDepA.Compute(input);
            Console.WriteLine($"[DirectDepA] Compute was SUCCESS: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DirectDepA] Compute was FAILURE: {ex.GetType().Name} - {ex.Message}");
        }

        try
        {
            var result = directDepB.Compute(input);
            Console.WriteLine($"[DirectDepB] Compute was SUCCESS: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DirectDepB] Compute was FAILURE: {ex.GetType().Name} - {ex.Message}");
        }
    }
}
