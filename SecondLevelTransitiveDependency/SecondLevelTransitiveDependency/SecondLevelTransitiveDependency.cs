namespace SecondLevelTransitiveDependency;

public class SecondLevelTransitiveDependency
{
    public MuchBetterStringReturnType Calculate(string input)
    {
        return new MuchBetterStringReturnType
        {
            ReturnValue = input + " - second level transitive dependency v2"
        };
    }
    
    public string Calculate2(string input)
    {
        return "hello from second level transitive dependency v2";
    }
}