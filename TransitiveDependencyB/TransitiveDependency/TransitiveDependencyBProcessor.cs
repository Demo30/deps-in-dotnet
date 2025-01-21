namespace TransitiveDependencyB;

public class TransitiveDependencyBProcessor : ISomeLogicProcessor
{
    public string Calculate(string input)
    {
        if (input != "test")
        {
            return input + " - Transitive dependency B v2";
        }

        return new SecondLevelTransitiveDependency.SecondLevelTransitiveDependency()
            .Calculate(input)
            .ReturnValue;
    }
}