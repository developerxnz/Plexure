using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Plexure.Sample
{

    /// <summary>
    /// Exercise 1:
    /// Demonstrate your knowledge of asynchronous programming by creating a method that downloads three resources and aggregates the content length of all 3 responses.The caller should be able to cancel the operation at any time.
    /// Notes / Assumptions:
    /// The method should be written as efficiently as possible.
    /// You can assume each resource you download is a string via a HTTP GET request.
    /// You can assume each resource exists. Eg.No error handling for HTTP responses.
    /// You can assume that each response returns all standard header you would expect from a normal HTTP rest request.
    /// using other httpclient setups for managing lifecycle
    /// </summary>

    public interface IDownloadClient
    {
        /// <summary>
        /// Returns the content length for 3 web responses
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> GetContentLength(CancellationToken cancellationToken);
    }

    public class DownloadClient : IDownloadClient
    {
        private HttpClient _client;
        private string[] urls = new[] { "RouteToStringContent1", "RouteToStringContent2", "RouteToStringContent3" };

        public DownloadClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<long> GetContentLength(CancellationToken cancellationToken)
        {
            List<Task<long>> contentLengthTasks = new List<Task<long>>();

            foreach (var url in urls)
            {
                contentLengthTasks.Add(GetResponseContentLength(cancellationToken, url));
            }

            long[] lengths = await Task.WhenAll(contentLengthTasks);

            return lengths.Sum();
        }

        private async Task<long> GetResponseContentLength(CancellationToken cancellationToken, string path)
        {
            var response = await _client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return response.Content.Headers.ContentLength ?? 0;
        }

    }
}