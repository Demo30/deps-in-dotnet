using TransitiveDependency.NewNamespace;

namespace TransitiveDependency
{
    public interface ISomeLogicProcessor
    {
        // Method with breaking change - returns object from namespace that moves in v2.0.0
        ICalculationResult Calculate(string input);

        // Method with NO breaking change - returns primitive type
        string CalculateSimple(string input);
    }
}