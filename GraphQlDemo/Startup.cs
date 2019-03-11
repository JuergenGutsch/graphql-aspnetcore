using GraphQL.Types;
using GraphQL.Validation.Complexity;
using GraphQlDemo.Data.Repositories;
using GraphQlDemo.GraphQl;
using GraphQlDemo.Services;
using GraphQlDemo.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using InMemory = GraphQlDemo.Data.InMemory.Repositories;

namespace GraphQlDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Authenticated", policy => policy
                    .RequireAuthenticatedUser()
                    .Build());
            });

            services.AddGraphQl(schema =>
            {
                schema.SetQueryType<RootQuery>();
            });
            // .AddDataLoader();

            // Repositories
            services.AddTransient<IBookRepository, InMemory.BookRepository>();

            // Services
            services.AddTransient<IBookService, BookService>();

            // GraphQl
            ConfigureGraphQlServices(services);

            // Initialize InMemory repositories
            InMemory.BookRepository.Initialize();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseGraphiql("/graphiql", options =>
                {
                    options.GraphQlEndpoint = "/graphql";
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseGraphQl("/graphql", options =>
            {
                //options.SchemaName = "SecondSchema"; // optional if only one schema is registered
                //options.AuthorizationPolicy = "Authenticated"; // optional
                options.FormatOutput = false; // Override default options registered in ConfigureServices
                options.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 }; //optional
                //options.EnableMetrics = true;
            });

            app.UseMvc();
        }

        // Dynamically resolve all GraphQl types in the same assembly as the root query
        // This prevents that we should add all the types manually
        private static void ConfigureGraphQlServices(IServiceCollection services)
        {
            var graphQlProject = Assembly.GetAssembly(typeof(RootQuery));
            var projectNamespace = graphQlProject.GetName().Name;
            var graphQlTypes = graphQlProject
                               .GetTypes()
                               .Where(t => t.IsClass
                                        && t.IsPublic
                                        && t.IsSubclassOf(typeof(GraphType))
                                        && t.Namespace.StartsWith(projectNamespace, StringComparison.InvariantCultureIgnoreCase))
                               .Select(x => x.GetTypeInfo())
                               .ToList();

            foreach (var type in graphQlTypes)
            {
                services.AddTransient(type.AsType());
            }
        }
    }
}
