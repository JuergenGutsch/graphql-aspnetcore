using GraphQL.Types;
using GraphQlDemo.Query.Data;

namespace GraphQlDemo.Query.GraphQlTypes
{
    public class BooksQuery : ObjectGraphType
    {
        public BooksQuery(IBookRepository bookRepository)
        {
            Field<BookType>("book",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType>() { Name = "isbn" }),
                resolve: context =>
                {
                    var id = context.GetArgument<string>("isbn");
                    return bookRepository.BookByIsbn(id);
                });

            Field<ListGraphType<BookType>>("books",
                resolve: context =>
                {
                    return bookRepository.AllBooks();
                });
        }
    }
}
