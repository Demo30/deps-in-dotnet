using DirectDependency;
using DirectDependencyB;
using System;

class Program
{
    static void Main(string[] args)
    {
        var helperA = new HttpHelperA();
        var helperB = new HttpHelperB();

        // Test 1: DirectDependencyA (Microsoft.Net.Http) creates HttpResponseMessage
        //         DirectDependencyB (System.Net.Http) processes it
        //         → Are the types compatible across package boundaries?
        try
        {
            System.Net.Http.HttpResponseMessage response = helperA.CreateDefaultResponse();
            string processed = helperB.ProcessResponse(response);
            Console.WriteLine($"[A→B HttpResponseMessage] SUCCESS: {processed}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[A→B HttpResponseMessage] FAILURE: {ex.GetType().Name} - {ex.Message}");
        }

        // Test 2: DirectDependencyB (System.Net.Http) creates HttpRequestHeaders
        //         DirectDependencyA (Microsoft.Net.Http) reads them
        //         → Are the types compatible in the reverse direction?
        try
        {
            var headers = helperB.CreateRequestHeaders();
            string result = helperA.ReadAcceptHeader(headers);
            Console.WriteLine($"[B→A HttpRequestHeaders]  SUCCESS: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[B→A HttpRequestHeaders]  FAILURE: {ex.GetType().Name} - {ex.Message}");
        }

        // Test 3: Consumer creates HttpResponseMessage directly and passes to both
        //         → Is the consumer's own System.Net.Http compatible with both?
        try
        {
            var myResponse = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Created);
            myResponse.Content = new System.Net.Http.StringContent("Created by consumer");
            string processed = helperB.ProcessResponse(myResponse);
            Console.WriteLine($"[Consumer→B HttpResponseMessage] SUCCESS: {processed}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Consumer→B HttpResponseMessage] FAILURE: {ex.GetType().Name} - {ex.Message}");
        }
    }
}
