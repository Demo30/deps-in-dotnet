namespace TransitiveDependency
{
    public class SomeLogicProcessor : ISomeLogicProcessor
    {
        public string Calculate(string input)
        {
            return input + " - processed by TransitiveDependency";
        }
    }
}
