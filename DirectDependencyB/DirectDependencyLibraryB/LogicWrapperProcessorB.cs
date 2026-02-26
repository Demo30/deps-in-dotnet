using TransitiveDependency;
using TransitiveDependency.NewNamespace;

namespace DirectDependency
{
    public class LogicWrapperProcessorB
    {
        private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

        public string Compute(string input)
        {
            input += " - Hello from Direct dependency B v2.0.0";
            // Using ICalculationResult from TransitiveDependency.NewNamespace (v2.0.0)
            ICalculationResult result = _someLogicProcessor.Calculate(input);
            return result.Result;
        }
    }
}