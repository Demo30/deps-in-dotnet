using DirectDependency;
using TransitiveDependency;

var directDep = new LogicWrapperProcessor();
var input = "Main app";
var result = directDep.Compute(input);
Console.WriteLine($"Using the direct dependency processor for input '{input}', result = {result}");

var directUseOfTransitiveDependency = new SomeLogicProcessor().Calculate(input).Result;
Console.WriteLine($"Using the transitive dependency directly for input '{input}'; result = {directUseOfTransitiveDependency}");

if (directUseOfTransitiveDependency != "Main app - Transitive dependency v1")
{
    throw new Exception("Phew, this almost went under the radar...");
}