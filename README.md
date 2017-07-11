[![Build status](https://ci.appveyor.com/api/projects/status/vxe22mwm1l2gw3b4/branch/master?svg=true)](https://ci.appveyor.com/project/JuergenGutsch/graphql-aspnetcore/branch/master)
# GraphQl.AspNetCore

The feedback about [my last blog post about the GraphQL end-point in ASP.NET Core](http://asp.net-hacker.rocks/2017/05/29/graphql-and-aspnetcore.html) was amazing. That post was mentioned on reddit, many times shared on twitter, lInked on http://asp.net and - I'm pretty glad about that - it was mentioned in the ASP.NET Community Standup.

Because of that and because GraphQL is really awesome, I decided to make the GraphQL MiddleWare available as a NuGet package. I did some small improvements to make this MiddleWare more configurable and more easy to use in the `Startup.cs`

## NuGet
Install that package via Package Manager Console:

~~~ powershell
PM> Install-Package GraphQl.AspNetCore -Pre
~~~

Install via dotnet CLI:

~~~ shell
dotnet add package GraphQl.AspNetCore --version 1.0.0-preview1
~~~

## Using the library

You still need to configure your GraphQL schema using the graphql-dotnet library, as [described in my last post](http://asp.net-hacker.rocks/2017/05/29/graphql-and-aspnetcore.html). If this is done open your `Startup.cs` and add an using to the GraphQl.AspNetCore library:

~~~ csharp
using GraphQl.AspNetCore;
~~~

You can use two different ways to register the GraphQlMiddleware:

```csharp
app.UseGraphQl(new GraphQlMiddlewareOptions
{
  GraphApiUrl = "/graph", // default
  RootGraphType = new BooksQuery(bookRepository),
  FormatOutput = true // default: false
});
app.UseGraphQl(options =>
{
  options.GraphApiUrl = "/graph-api";
  options.RootGraphType = new BooksQuery(bookRepository);
  options.FormatOutput = false; // default
});
```
Personally I prefer the second way, which is more readable in my opinion.

The root graph type needs to be passed to the GraphQlMiddlewareOptions object, depending on the implementation of your root graph type, you may need to inject the data repository or a EntityFramework DbContext, or whatever you want to use to access your data. In this case I reuse the IBookRepository of the last post and pass it to the BooksQuery which is my root graph type.

I registered the repository like this:

~~~ csharp
services.AddSingleton<IBookRepository, BookRepository>();
~~~

and needed to inject it to the Configure method:

~~~ csharp
public void Configure(
  IApplicationBuilder app,
  IHostingEnvironment env,
  ILoggerFactory loggerFactory,
  IBookRepository bookRepository)
{
  // ...
}
~~~

Another valid option is to also add the BooksQuery to the DependencyInjection container and inject it to the Configure method.

## Options

The GraphQlMiddlewareOptions are pretty simple. Currently there are only three properties to configure

* RootGraphType: This configures your GraphQL query schema and needs to be set. If this property is unset an ArgumentNullException will be thrown.
* GraphApiUrl: This property defines your GraphQL endpoint path. The default is set to /graph which means your endpoint is available under //yourdomain.tld/graph
* FormatOutput: This property defines whether the output is prettified and indented for debugging purposes. The default is set to false.

This should be enough for the first time. If needed it is possible to expose the Newtonsoft.JSON settings, which are used in GraphQL library later on.

## One more thing

I would be happy, if you try this library and get me some feedback about it. A demo application to quickly start playing around with it, is [available here on GitHub](https://github.com/JuergenGutsch/graphql-aspnetcore). Feel free to raise some issues and to create some PRs to improve this MiddleWare.
