using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace GraphQl.AspNetCore.File
{
    public static class FileStreamingHelperB
    {
        private static readonly int BondaryLengthLimit = 1024;

        public static async Task<(T model, List<MultipartFileInfo> file)>
            ParseRequestForm<T>(
            this ResolveFieldContext<object> context, T model)
            where T : class
        {
            var httpContext = (HttpContext)context.RootValue;

            var files = await ParseRequest(httpContext.Request);
            return (model, files);
        }

        private static async Task<List<MultipartFileInfo>>
            ParseRequest(this HttpRequest request, Func<MultipartSection, Task> fileHandler = null)
        {
            var files = new List<MultipartFileInfo>();

            if (fileHandler == null)
            {
                fileHandler = HandleFileSection;
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(request.ContentType),
                BondaryLengthLimit);

            using (var sr = await request.ReadAsStream())
            {
                var reader = new MultipartReader(boundary, sr);

                var section = await reader.ReadNextSectionAsync();

                while (section != null)
                {
                    var hasContentDispositionHeader =
                        ContentDispositionHeaderValue.TryParse(
                            section.ContentDisposition,
                            out ContentDispositionHeaderValue contentDisposition);

                    if (hasContentDispositionHeader)
                    {
                        if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                        {
                            await fileHandler(section);
                        }
                    }

                    section = await reader.ReadNextSectionAsync();
                }
            }

            return files;

            async Task HandleFileSection(MultipartSection fileSection)
            {
                string fileContentBase64;
                using (StreamReader reader = new StreamReader(fileSection.Body))
                {
                    fileContentBase64 = MultipartRequestHelper.Base64Encode(reader.ReadToEnd());
                }

                if (fileSection.Body.Length == 0)
                {
                    throw new InvalidDataException("Trying to upload empty file");
                }

                var formFile = new MultipartFileInfo
                {
                    Name = fileSection.AsFileSection().FileName,
                    FileName = fileSection.AsFileSection().Name,
                    OriginalFileName = fileSection.AsFileSection().FileName,
                    ContentType = fileSection.ContentType,
                    Length = fileSection.Body.Length,
                    Base64 = fileContentBase64
                };

                files.Add(formFile);
            }
        }
    }

    public class MultipartFileInfo
    {
        public long Length { get; set; }

        public string FileName { get; set; }

        public string OriginalFileName { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public string Base64 { get; set; }
    }
}
