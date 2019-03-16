using GraphQL.Types;
using GraphQlDemo.GraphQl.Types;
using GraphQlDemo.Services;

namespace GraphQlDemo.GraphQl.RootTypes
{
    public class BookRootTypes : ObjectGraphType
    {
        public BookRootTypes(IBookService bookService)
        {
            // get all

            this.FieldAsync<ListGraphType<BookType>>(
                name: "books",
                resolve: async context => await bookService.GetBooksAsync()
            );

            // get by isbn

            var args = new QueryArguments(
                new QueryArgument<StringGraphType> { Name = "isbn" });

            this.FieldAsync<BookType>(
                name: "bookByIsbn",
                arguments: args,
                resolve: async context =>
                {
                    var isbn = context.GetArgument<string>("isbn");
                    return await bookService.GetBookByIsbnAsync(isbn);
                });

            // get by author id

            args = new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" });

            this.FieldAsync<ListGraphType<BookType>>(
                name: "booksByAuthorId",
                arguments: args,
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    return await bookService.GetBooksByAuthorIdAsync(id);
                });

            // get by publisher id

            args = new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" });

            this.FieldAsync<ListGraphType<BookType>>(
                name: "booksByPublisherId",
                arguments: args,
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    return await bookService.GetBooksByPublisherIdAsync(id);
                });
        }
    }
}
