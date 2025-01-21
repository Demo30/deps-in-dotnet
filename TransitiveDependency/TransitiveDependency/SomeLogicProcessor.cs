namespace TransitiveDependency;

public class SomeLogicProcessor : ISomeLogicProcessor
{
    public ICalculationResult Calculate(string input)
    {
        if (input != "test_A")
        {
            return new CalculationResult
            {
                Result = input + " - Transitive dependency A v1"
            };
        }

        var val = new SecondLevelTransitiveDependency.SecondLevelTransitiveDependency()
            .Calculate(input)
            .ReturnValue;

        return new CalculationResult
        {
            Result = val
        };
    }
    
    public ICalculationResult Calculate2(string input)
    {
        var val = new SecondLevelTransitiveDependency.SecondLevelTransitiveDependency()
            .Calculate2(input);

        return new CalculationResult
        {
            Result = val
        };
    }
}