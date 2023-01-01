﻿namespace Play.Catalog.Service.Controllers.v1
{
    using System;
    using System.Net.Mime;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Common.Application;
    using Microsoft.AspNetCore.Http;

    internal static class ResponseExtensions
    {
        public static async ValueTask WriteResponseAsync(this Response response, HttpResponse httpResponse)
        {
            httpResponse.StatusCode = response.StatusCode;
            httpResponse.ContentType = MediaTypeNames.Application.Json;

            await httpResponse.StartAsync();

            if (response.IsFailure)
                await httpResponse.BodyWriter.WriteAsync(
                    new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(response.ErrorResponse)));
            else
                await httpResponse.BodyWriter.WriteAsync(response.Content.ValueAsJsonUtf8Bytes);

            await httpResponse.BodyWriter.CompleteAsync();
        }
        
        public static async ValueTask WriteResponseAsync(this Task<Response> response, HttpResponse httpResponse)
        {
            var waitResponse = await response;
            
            httpResponse.StatusCode = waitResponse.StatusCode;
            httpResponse.ContentType = MediaTypeNames.Application.Json;

            await httpResponse.StartAsync();

            if (waitResponse.IsFailure)
                await httpResponse.BodyWriter.WriteAsync(
                    new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(waitResponse.ErrorResponse)));
            else
                await httpResponse.BodyWriter.WriteAsync(waitResponse.Content.ValueAsJsonUtf8Bytes);

            await httpResponse.BodyWriter.CompleteAsync();
        }
    }
}