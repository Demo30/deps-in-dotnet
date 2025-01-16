using TransitiveDependency;

namespace DirectDependency;

public class LogicWrapperProcessor
{
    private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

    public string Compute(string input)
    {
        input += " - hello from direct dependency";
        ICalculationResult explicitlyTypedResult = _someLogicProcessor.Calculate(input);
        return explicitlyTypedResult.Result;
    }
}