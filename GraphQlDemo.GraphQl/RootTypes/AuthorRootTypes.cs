using GraphQL.Types;
using GraphQlDemo.GraphQl.Types;
using GraphQlDemo.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQlDemo.GraphQl.RootTypes
{
    public class AuthorRootTypes : ObjectGraphType
    {
        public AuthorRootTypes(IAuthorService authorService)
        {
            // get all

            this.FieldAsync<ListGraphType<AuthorType>>(
                name: "authors",
                resolve: async context => await authorService.GetAuthorsAsync()
            );

            // get by id

            var args = new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" });

            this.FieldAsync<AuthorType>(
                name: "authorById",
                arguments: args,
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    return await authorService.GetAuthorByIdAsync(id);
                });
        }
    }
}
