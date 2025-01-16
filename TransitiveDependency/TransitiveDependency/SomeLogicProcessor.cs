namespace TransitiveDependency;

public class SomeLogicProcessor : ISomeLogicProcessor
{
    public ICalculationResult Calculate(string input)
    {
        return new CalculationResult
        {
        
            Result = null
        };
    }
}