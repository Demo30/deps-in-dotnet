using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DirectDependency
{
    /// <summary>
    /// Exposes HTTP types obtained via the Microsoft.Net.Http NuGet package.
    /// </summary>
    public class HttpHelperA
    {
        public HttpResponseMessage CreateDefaultResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("Hello from DirectDependencyA (Microsoft.Net.Http)");
            return response;
        }

        public string ReadAcceptHeader(HttpRequestHeaders headers)
        {
            if (headers.Accept.Count > 0)
                return string.Join(", ", headers.Accept.Select(a => a.MediaType));
            return "(no Accept header)";
        }
    }
}
