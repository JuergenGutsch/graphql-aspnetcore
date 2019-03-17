using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace GraphQl.AspNetCore
{
    public static class MultipartRequestHelper
    {
        private const string BOUNDARY = "boundary";

        private const int BondaryLengthLimit = 1024;

        private const int BufferSize = 81920;

        public static string RemoveQuotes(string input)
        {
            if (!string.IsNullOrEmpty(input) && input.Length >= 2 && input[0] == '"' && input[input.Length - 1] == '"')
            {
                input = input.Substring(1, input.Length - 2);
            }

            return input;
        }

        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit = BondaryLengthLimit)
        {
            var boundary = RemoveQuotes(contentType.Boundary());
            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        public static Encoding GetEncoding(MultipartSection section)
        {
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(
                section.ContentType,
                out MediaTypeHeaderValue mediaType);

            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }

            return mediaType.Encoding;
        }

        public static string Boundary(this MediaTypeHeaderValue media)
        {
            foreach (var value in media.Parameters)
            {
                if (string.Equals(value.Name.Value, BOUNDARY, StringComparison.OrdinalIgnoreCase))
                {
                    return value.Value.Value;
                }
            }

            return null;
        }

        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && StringSegment.IsNullOrEmpty(contentDisposition.FileName)
                   && StringSegment.IsNullOrEmpty(contentDisposition.FileNameStar);
        }

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && (!StringSegment.IsNullOrEmpty(contentDisposition.FileName)
                       || !StringSegment.IsNullOrEmpty(contentDisposition.FileNameStar));
        }

        public static async Task<KeyValueAccumulator> AccumulateForm(
            KeyValueAccumulator formAccumulator,
            MultipartSection section,
            ContentDispositionHeaderValue contentDisposition)
        {
            var key = RemoveQuotes(contentDisposition.Name.Value);
            var encoding = GetEncoding(section);
            using (var streamReader = new StreamReader(
                section.Body,
                encoding,
                true,
                BufferSize,
                true))
            {
                var value = await streamReader.ReadToEndAsync();
                if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                {
                    value = string.Empty;
                }

                formAccumulator.Append(key, value);

                if (formAccumulator.ValueCount > FormReader.DefaultValueCountLimit)
                {
                    throw new InvalidDataException(
                        $"Form key count limit {FormReader.DefaultValueCountLimit} exceeded.");
                }
            }

            return formAccumulator;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
