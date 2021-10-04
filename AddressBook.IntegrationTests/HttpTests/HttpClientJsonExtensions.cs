// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AddressBook.HttpTests
{
    public static partial class HttpClientJsonExtensions
    {
        public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, string? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            JsonContent content = JsonContent.Create(value, mediaType: null, options);
            return client.PatchAsync(requestUri, content, cancellationToken);
        }

        public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            JsonContent content = JsonContent.Create(value, mediaType: null, options);
            return client.PatchAsync(requestUri, content, cancellationToken);
        }

        public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, string? requestUri, TValue value, CancellationToken cancellationToken)
            => client.PatchAsJsonAsync(requestUri, value, options: null, cancellationToken);

        public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, Uri? requestUri, TValue value, CancellationToken cancellationToken)
            => client.PatchAsJsonAsync(requestUri, value, options: null, cancellationToken);
    }
}
