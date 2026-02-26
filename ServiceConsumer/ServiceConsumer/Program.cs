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

        // Using DirectDependencyLibraryB
        var directDepB = new LogicWrapperProcessorB();
        var resultB = directDepB.Compute(input);
        Console.WriteLine($"DirectDepB result: {resultB}");
    }
}
