namespace TransitiveDependency;

public interface ISomeLogicProcessor
{
    public ICalculationResult Calculate(string input);

    public ICalculationResult Calculate2(string input);
}