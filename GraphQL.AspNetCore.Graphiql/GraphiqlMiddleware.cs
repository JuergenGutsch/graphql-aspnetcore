using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GraphQL.AspNetCore.Graphiql
{
    public class GraphiqlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GraphiqlMiddlewareOptions _options;

        public GraphiqlMiddleware(
            RequestDelegate next,
            GraphiqlMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }


        public async Task Invoke(HttpContext httpContext)
        {
            var sent = false;
            if (httpContext.Request.Path.StartsWithSegments(_options.GraphiqlPath))
            {
                try
                {
                    var result = RenderGraphiqlUi();
                    await httpContext.Response.WriteAsync(result);
                    sent = true;
                }
                catch (System.Exception)
                {
                    throw;
                }
            }

            if (!sent)
            {
                await _next(httpContext);
            }
        }


        private string RenderGraphiqlUi()
        {
            return @"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"">
    <title>GraphiQL</title>
    <meta name=""robots"" content=""noindex"">
    <style>
        html, body {
            height: 100%;
            margin: 0;
            overflow: hidden;
            width: 100%;
        }
    </style>
    <link href=""//unpkg.com/graphiql@0.11.2/graphiql.css"" rel=""stylesheet"">
    <script src=""//unpkg.com/react@15.6.1/dist/react.min.js""></script>
    <script src=""//unpkg.com/react-dom@15.6.1/dist/react-dom.min.js""></script>
    <script src=""//unpkg.com/graphiql@0.11.2/graphiql.min.js""></script>
    <script src=""//cdn.jsdelivr.net/fetch/2.0.1/fetch.min.js""></script>
</head>
<body>
    <script>
        // Collect the URL parameters
        var parameters = {};
        window.location.search.substr(1).split('&').forEach(function (entry) {
            var eq = entry.indexOf('=');
            if (eq >= 0) {
                parameters[decodeURIComponent(entry.slice(0, eq))] =
                    decodeURIComponent(entry.slice(eq + 1));
            }
        });
        // Produce a Location query string from a parameter object.
        function locationQuery(params, location) {
            return (location ? location : '') + '?' + Object.keys(params).map(function (key) {
                return encodeURIComponent(key) + '=' +
                    encodeURIComponent(params[key]);
            }).join('&');
        }
        // Derive a fetch URL from the current URL, sans the GraphQL parameters.
        var graphqlParamNames = {
            query: true,
            variables: true,
            operationName: true
        };
        var otherParams = {};
        for (var k in parameters) {
            if (parameters.hasOwnProperty(k) && graphqlParamNames[k] !== true) {
                otherParams[k] = parameters[k];
            }
        }

        // We don't use safe-serialize for location, because it's not client input.
        var fetchURL = locationQuery(otherParams, '" + _options.GraphQlEndpoint + @"');

        // Defines a GraphQL fetcher using the fetch API.
        function graphQLHttpFetcher(graphQLParams) {
            return fetch(fetchURL, {
                method: 'post',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: graphQLParams.query,  //JSON.stringify(graphQLParams),
                credentials: 'same-origin',
            }).then(function (response) {
                return response.text();
            }).then(function (responseBody) {
                try {
                    return JSON.parse(responseBody);
                } catch (error) {
                    return responseBody;
                }
            });
        }

        var fetcher = graphQLHttpFetcher;

        // When the query and variables string is edited, update the URL bar so
        // that it can be easily shared.
        function onEditQuery(newQuery) {
            parameters.query = newQuery;
            updateURL();
        }
        function onEditVariables(newVariables) {
            parameters.variables = newVariables;
            updateURL();
        }
        function onEditOperationName(newOperationName) {
            parameters.operationName = newOperationName;
            updateURL();
        }
        function updateURL() {
            var cleanParams = Object.keys(parameters).filter(function (v) {
                return parameters[v] !== undefined;
            }).reduce(function (old, v) {
                old[v] = parameters[v];
                return old;
            }, {});

            history.replaceState(null, null, locationQuery(cleanParams) + window.location.hash);
        }
        // Render <GraphiQL /> into the body.
        ReactDOM.render(
            React.createElement(GraphiQL, {
                fetcher: fetcher,
                onEditQuery: onEditQuery,
                onEditVariables: onEditVariables,
                onEditOperationName: onEditOperationName,
                query: null,
                response: null,
                variables: null,
                operationName: null,
                editorTheme: null,
                websocketConnectionParams: null,
            }),
            document.body
        );
    </script>

</body>
</html>";
        }
    }
}
