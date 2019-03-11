using GraphQL.Types;
using GraphQlDemo.GraphQl.Types;
using GraphQlDemo.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQlDemo.GraphQl.RootTypes
{
    public class PublisherRootTypes : ObjectGraphType
    {
        public PublisherRootTypes(IPublisherService publisherService)
        {
            // get all

            this.FieldAsync<ListGraphType<PublisherType>>(
                name: "publishers",
                resolve: async context => await publisherService.GetPublishersAsync()
            );

            // get by id

            var args = new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" });

            this.FieldAsync<PublisherType>(
                name: "publisherById",
                arguments: args,
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    return await publisherService.GetPublisherByIdAsync(id);
                });
        }
    }
}
