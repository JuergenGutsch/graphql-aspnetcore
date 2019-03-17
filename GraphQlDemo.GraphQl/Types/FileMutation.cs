using System;
using GraphQl.AspNetCore.File;
using GraphQL.Types;

namespace GraphQlDemo.GraphQl.Types
{
    public class FileMutation : ObjectGraphType
    {
        public FileMutation()
        {
            Field<FileUploadType>(
                "createFileUpload",
                arguments:
                    new QueryArguments(
                    new QueryArgument<NonNullGraphType<FileUploadInputType>> { Name = "file" }),
                resolve: context =>
                {
                    try
                    {
                        var fileType = new object();
                        var files = FileStreamingHelperB.ParseRequestForm(context, fileType).Result;

                        // base 64 decode file content from MultipartFileInfo

                        return null;
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                });
        }
    }
}
