# GraphQl.AspNetCore

The feedback about [my last blog post about the GraphQL end-point in ASP.NET Core](http://asp.net-hacker.rocks/2017/05/29/graphql-and-aspnetcore.html) was amazing. That post was mentioned on reddit, many times shared on twitter, lInked on http://asp.net and - I'm pretty glad about that - it was mentioned in the ASP.NET Community Standup.

Because of that and because GraphQL is really awesome, I decided to make the GraphQL MiddleWare available as a NuGet package. I did some small improvements to make this MiddleWare more configurable and more easy to use in the `Startup.cs`

# Branches & contributing & testing

The **master** branch is the stable branch and I don't axcept PRs to that branch. To contribute, please create PRs based on the **develop** branch. To play around with the latest changes, please also use the **develop** branch. 

Changes on the **develop** branch ("next version" branch) will be pushed as preview releases to [MyGet](https://www.myget.org/feed/juergengutsch/package/nuget/GraphQl.AspNetCore). To see whether this branch is stable, follow the builds on AppVeyor:
[![Build status](https://ci.appveyor.com/api/projects/status/vxe22mwm1l2gw3b4/branch/develop?svg=true)](https://ci.appveyor.com/project/JuergenGutsch/graphql-aspnetcore/branch/develop)

Changes on the **master** branch ("current version" branch) will be pushed as releases to [NuGet](https://www.nuget.org/packages/GraphQl.AspNetCore). To see whether this branch is stable, follow the builds on AppVeyor:
[![Build status](https://ci.appveyor.com/api/projects/status/vxe22mwm1l2gw3b4/branch/master?svg=true)](https://ci.appveyor.com/project/JuergenGutsch/graphql-aspnetcore/branch/master)

# Usage and short documentation

## NuGet
Preview builds on [MyGet](https://www.myget.org/feed/juergengutsch/package/nuget/GraphQl.AspNetCore) and release builds on [NuGet](https://www.nuget.org/packages/GraphQl.AspNetCore).

Install that package via Package Manager Console:

~~~ powershell
PM> Install-Package GraphQl.AspNetCore
~~~

Install via dotnet CLI:

~~~ shell
dotnet add package GraphQl.AspNetCore
~~~

## Using the library

You still need to configure your GraphQL schema using the graphql-dotnet library, as [described in my last post](http://asp.net-hacker.rocks/2017/05/29/graphql-and-aspnetcore.html). 

First configure your schema(s) in the `ConfigureServices` method in `Startup.cs`. Make sure all referenced graph types are registered as well so they can be resolved from the container.

```csharp
// Configure the default schema
services.AddGraphQl(schema =>
{
    schema.SetQueryType<BooksQuery>();
    schema.SetMutationType<BooksMutation>();
});

// Also register all graph types
services.AddSingleton<BooksQuery>();
services.AddSingleton<BooksMutation>();
services.AddSingleton<BookType>();
services.AddSingleton<AuthorType>();
services.AddSingleton<PublisherType>();
```

In the `Configure` method, you add the GraphQL middleware like this:

You can use different ways to register the GraphQlMiddleware:

```csharp
// the simplest form to use GraphQl. defaults to '/graphql' with default options
app.UseGraphQl();

// or specify options only (default path)
app.UseGraphQl(new GraphQlMiddlewareOptions
{
    FormatOutput = true, // default
    ComplexityConfiguration = new ComplexityConfiguration()); //default
});

app.UseGraphQl(options =>
{
    options.EnableMetrics = true;
});

// or specify path and options

app.UseGraphQl("/graphql", new GraphQlMiddlewareOptions
{
    FormatOutput = true, // default
    ComplexityConfiguration = new ComplexityConfiguration()); //default
});

// or like this:

app.UseGraphQl("/graph-api", options =>
{
    options.SchemaName = "OtherSchema"; // only if additional schemas were registered in ConfigureServices
    //options.AuthorizationPolicy = "Authenticated"; // optional
});
```

Personally I prefer the second way, which is more readable in my opinion.

## Options

The `GraphQlMiddlewareOptions` are pretty simple.

* SchemaName: This specifies the registered schema name to use. Leave `null` for the default schema.
* AuthorizationPolicy: This configures the authorization policy name to apply to the GraphQL endpoint.
* FormatOutput: This property defines whether the output is prettified and indented for debugging purposes. The default is set to `true`.
* ComplexityConfiguration: This property is used to customize the complexity configuration.
* ExposeExceptions: This property controls whether exception details such as stack traces should be returned to clients. This defaults to `false` and should only be set to `true` in the Development environment.
* EnableMetrics: Enable metrics defaults to `false`. See [GraphQL .net client documentation](https://github.com/graphql-dotnet/graphql-dotnet/blob/master/docs/src/learn.md#metrics) how to create a stats report.

This should be enough for the first time. If needed it is possible to expose the Newtonsoft.JSON settings, which are used in GraphQL library later on.

# GraphQL.AspNetCore.Graphiql

This library provides a middleware to add a GraphiQL UI to your GraphQL endpoint. To learn more about it and the way I created it, read the blog post about it: [GraphiQL for ASP.NET Core](http://asp.net-hacker.rocks/2017/10/26/graphicl.html)

## NuGet

Preview builds on [MyGet](https://www.myget.org/feed/juergengutsch/package/nuget/GraphQl.AspNetCore.Graphiql) and release builds on [NuGet](https://www.nuget.org/packages/GraphQl.AspNetCore.Graphiql).

Install that package via Package Manager Console:

```powershell
PM> Install-Package GraphQl.AspNetCore.Graphiql
```

Install via dotnet CLI:

```shell
dotnet add package GraphQl.AspNetCore.Graphiql
```

## Using the library

Open your `Startup.cs` and configure the middleware in the `Configure` method.

You can use two different ways to register the GraphiqlMiddleware:

```csharp
app.UseGraphiql("/graphiql", new GraphQlMiddlewareOptions
{
    GraphQlEndpoint = "/graphql"
});


app.UseGraphiql("/graphiql", options =>
{
    options.GraphQlEndpoint = "/graphql";
});
```

Personally I prefer the second way, which is more readable in my opinion.

The GraphQlEndpoint needs to match the paht a GraphQL endpoint.

## Options

Currently the options just have two properties:

* GraphQlEndpoint: This is the path of your GraphQL end-point, configured with the GraphQlMiddleware. In theory it could be any possible path or URL that provides an GraphQL endpoint. Until now, I just tested it with the GraphQlMiddleware.

# One more thing

I would be happy, if you try this library and get me some feedback about it. A demo application to quickly start playing around with it, is [available here on GitHub](https://github.com/JuergenGutsch/graphql-aspnetcore/tree/develop/GraphQlDemo). Feel free to raise some issues and to create some PRs to improve this MiddleWare.
