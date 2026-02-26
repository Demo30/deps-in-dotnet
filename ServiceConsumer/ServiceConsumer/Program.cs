using DirectDependency;
using System;

class Program
{
    static void Main(string[] args)
    {
        var input = "Main app";

        // Using DirectDependencyLibraryA (which uses TransitiveDependency v1.0.0)
        var directDepA = new LogicWrapperProcessor();
        var resultA = directDepA.Compute(input);
        Console.WriteLine($"DirectDepA result: {resultA}");

        // Using DirectDependencyLibraryB (which uses TransitiveDependency v2.0.0)
        var directDepB = new LogicWrapperProcessorB();
        var resultB = directDepB.Compute(input);
        Console.WriteLine($"DirectDepB result: {resultB}");

        Console.WriteLine("\n=== Diamond Dependency Situation ===");
        Console.WriteLine("Main app -> DirectLibraryA (v1.0.0) -> TransitiveDependency v1.0.0");
        Console.WriteLine("Main app -> DirectLibraryB (v2.0.0) -> TransitiveDependency v2.0.0");
        Console.WriteLine("This creates a diamond dependency conflict!");
    }
}
