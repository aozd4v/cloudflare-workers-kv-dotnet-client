using System.Linq;
using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Exceptions;
using CloudflareWorkersKv.Client.Models;
using Flurl.Http;
using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client.Extensions
{
    internal static class FlurlHttpExceptionExtensions
    {
        internal static async Task ThrowContextualException(this FlurlHttpException exception)
        {
            var deserializationError = exception.Message.Contains(Errors.JsonDeserialization);

            if (deserializationError)
            {
                throw new JsonDeserializationException(exception.InnerException);
            }

            var response = await exception.Call.Response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<CloudflareErrorResponse>(response);
            var authenticationError = errorResponse.Errors.FirstOrDefault(x => x.Code == 10000);

            if (authenticationError != null)
            {
                throw new UnauthorizedException();
            }

            var namespaceFormattingError = errorResponse.Errors.FirstOrDefault(x => x.Code == 10011);

            if (namespaceFormattingError != null)
            {
                throw new NamespaceFormattingException(namespaceFormattingError.Message);
            }

            var namespaceNotFoundError = errorResponse.Errors.FirstOrDefault(x => x.Code == 10013);

            if (namespaceNotFoundError != null)
            {
                throw new NamespaceNotFoundException();
            }

            var namespaceAlreadyExistsError = errorResponse.Errors.FirstOrDefault(x => x.Code == 10014);

            if (namespaceAlreadyExistsError != null)
            {
                throw new NamespaceAlreadyExistsException(exception);
            }
        }
    }
}
