// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System.Net.Http;

namespace AddressBook.HttpTests
{
    public class JsonStringContent : StringContent
    {
        public JsonStringContent(string content) : base(content, encoding: null, mediaType: "application/json")
        {
        }
    }
}
