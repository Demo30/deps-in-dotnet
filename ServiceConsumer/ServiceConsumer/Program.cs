// See https://aka.ms/new-console-template for more information

using DirectDependency;
using TransitiveDependency;
using TransitiveDependency.NewNamespace;

var directDep = new LogicWrapperProcessor();
var input = "Main app";
ICalculationResult result = directDep.Compute(input).Result; // <<<< had to be specific about the type due to namespace issues
Console.WriteLine($"Using the direct dependency processor for input '{input}', result = {result}");

var directUseOfTransitiveDependency = new SomeLogicProcessor().Calculate(input).Result;
Console.WriteLine($"Using the transitive dependency directly for input '{input}'; result = {directUseOfTransitiveDependency}");