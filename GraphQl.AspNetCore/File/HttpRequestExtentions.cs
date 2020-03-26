using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace GraphQl.AspNetCore.File
{
    public static class HttpRequestExtentions
    {
        public async static Task<string> ReadAsString(this HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, -1, true))
            {
                var body = await reader.ReadToEndAsync();
                return body;
            }
        }

        public async static Task<T> ReadAsAsync<T>(this HttpRequest request)
        {
            var json = await request.ReadAsString();
            T retValue = JsonConvert.DeserializeObject<T>(json);
            return retValue;
        }

        public static async Task<MultipartReader> ReadAsStream(this HttpRequest request, string boundary)
        {
            var reader = new MultipartReader(boundary, request.Body);
            
            return reader;
        }
    }
}
