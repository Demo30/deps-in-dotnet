using TransitiveDependency;

namespace DirectDependency
{
    public class LogicWrapperProcessor
    {
        private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

        public string Compute(string input)
        {
            input += " - hello from direct dependency A";
            // Using ICalculationResult from TransitiveDependency namespace (v1.0.0)
            ICalculationResult explicitlyTypedResult = _someLogicProcessor.Calculate(input);
            return explicitlyTypedResult.Result;
        }
    }
}