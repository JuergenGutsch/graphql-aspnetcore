using GraphQL.Types;

namespace GraphQlDemo.GraphQl.Types
{
    public class FileUploadType : ObjectGraphType<File>
    {
        public FileUploadType()
        {
            Name = "FileUpload";
            Description = "FileUpload";

            Field(x => x.FileName).Description("The File Name.");
        }
    }

    public class FileUploadInputType : InputObjectGraphType<FileUploadType>
    {
        public FileUploadInputType()
        {
            Name = "FileUploadInputType";

            Field<NonNullGraphType<StringGraphType>>("FileName");
        }
    }

    public class File
    {
        public string FileName { get; set; }
    }
}
