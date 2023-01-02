namespace Play.Inventory.Service.Controllers
{
    using System;
    using System.Net.Mime;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Common.Application;
    using Microsoft.AspNetCore.Http;

    internal static class BodyWriterExtensions
    {
        public static async ValueTask WriteResponseAsync(this Task<Response> response, HttpResponse httpResponse)
        {
            var waitResponse = await response;
            await waitResponse.WriteResponseAsync(httpResponse);
        }

        public static async ValueTask WriteResponseAsync(this Response response, HttpResponse httpResponse)
        {
            httpResponse.StatusCode = response.StatusCode;
            httpResponse.ContentType = MediaTypeNames.Application.Json;

            await httpResponse.StartAsync();

            if (response.IsFailure)
                await httpResponse.BodyWriter.WriteAsync(
                    new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(response.ErrorResponse)));
            else
            {
                if (response.Content.HasValue)
                {
                    await httpResponse.BodyWriter.WriteAsync(response.Content.ValueAsJsonUtf8Bytes);
                }
                else
                {
                    await httpResponse.BodyWriter.WriteAsync(new ReadOnlyMemory<byte>());
                }
            }

            await httpResponse.BodyWriter.CompleteAsync();
        }
    }
}