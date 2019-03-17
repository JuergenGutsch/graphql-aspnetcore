using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;

namespace GraphQl.AspNetCore
{
    public static class HttpRequestExtentions
    {
        public async static Task<string> ReadAsString(this HttpRequest request)
        {
            using (var stream = new MemoryStream())
            {
                request.EnableRewind();
                request.Body.Position = 0;
                request.Body.CopyTo(stream);
                stream.Position = 0;
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                var body = Encoding.UTF8.GetString(buffer);
                return body;
            }
        }

        public async static Task<T> ReadAsAsync<T>(this HttpRequest request)
        {
            var json = await request.ReadAsString();
            T retValue = JsonConvert.DeserializeObject<T>(json);
            return retValue;
        }

        public static Stream ReadAsStream(this HttpRequest request)
        {
            var stream = new MemoryStream();

            request.EnableRewind();
            request.Body.Position = 0;
            request.Body.CopyTo(stream);
            stream.Position = 0;
            return stream;
        }
    }
}
