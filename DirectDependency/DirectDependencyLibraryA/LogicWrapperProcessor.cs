using TransitiveDependency;

namespace DirectDependency;

public class LogicWrapperProcessor
{
    private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

    public ICalculationResult Compute(string input)
    {
        input += " - hello from direct dependency";
        return _someLogicProcessor.Calculate(input);
    }
}