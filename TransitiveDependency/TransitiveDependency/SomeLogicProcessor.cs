using TransitiveDependency.NewNamespace;

namespace TransitiveDependency;

public class SomeLogicProcessor : ISomeLogicProcessor
{
    public ICalculationResult Calculate(string input)
    {
        return new CalculationResult
        {
        
            Result = input + " - Transitive dependency v2"
        };
    }
}