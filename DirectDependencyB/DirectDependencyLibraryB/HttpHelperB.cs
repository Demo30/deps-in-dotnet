using System.Net.Http;
using System.Net.Http.Headers;

namespace DirectDependencyB
{
    /// <summary>
    /// Exposes HTTP types obtained via the System.Net.Http NuGet package.
    /// </summary>
    public class HttpHelperB
    {
        public string ProcessResponse(HttpResponseMessage response)
        {
            return $"Processed by DirectDependencyB (System.Net.Http): StatusCode={response.StatusCode}";
        }

        public HttpRequestHeaders CreateRequestHeaders()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client.DefaultRequestHeaders;
        }
    }
}
