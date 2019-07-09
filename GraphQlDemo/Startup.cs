using GraphQL.Types;
using GraphQL.Validation.Complexity;
using GraphQlDemo.Data.Repositories;
using GraphQlDemo.GraphQl;
using GraphQlDemo.GraphQl.Types;
using GraphQlDemo.Services;
using GraphQlDemo.Services.Implementations;
using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using InMemory = GraphQlDemo.Data.InMemory.Repositories;
using System.Reflection;
using GraphQlDemo.Data;
using GraphQL.AspNetCore.Data;
using GraphQlDemo.Models;

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
            services.AddDbContext<ApplicationDBContext>(options =>
            {
            });

            // Add framework services.
            services.AddControllers();
            services.AddRazorPages();

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Authenticated", policy => policy
                    .RequireAuthenticatedUser()
                    .Build());
            });

            services.AddGraphQl(schema =>
            {
                schema.SetQueryType<RootQuery>();
                schema.SetMutationType<FileMutation>();
            });

            // GraphQl
            ConfigureGraphQlTypes(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // app.UseGraphiQL();
                // app.UseGraphiQL("/graphiql");
                // app.UseGraphiQL("/graphiql", options =>
                // {
                //     options.GraphQlEndpoint = "/graphql";
                // });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                if (env.IsDevelopment())
                {
                    // routes.MapGraphiQl();
                    // routes.MapGraphiQl("/graphiql");
                    endpoints.MapGraphiQL("/graphiql", options =>
                    {
                        options.GraphQlEndpoint = "/graphql";
                    });
                }

                // The simplest form to use GraphQL defaults to /graphql with default options.
                // routes.MapGraphQl();
                // routes.MapGraphQl("/graphql");
                endpoints.MapGraphQl("/graphql", options =>
                {
                    //options.SchemaName = "Schema01"; // optional if only one schema is registered
                    //options.AuthorizationPolicy = "Authenticated"; // optional
                    options.FormatOutput = false; // Override default options registered in ConfigureServices
                    options.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 }; //optional
                    //options.EnableMetrics = true;
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

            // app.UseGraphQl();
            // app.UseGraphQl("/graphql");
            // app.UseGraphQl("/graphql", options =>
            // {
            //     //options.SchemaName = "Schema01"; // optional if only one schema is registered
            //     //options.AuthorizationPolicy = "Authenticated"; // optional
            //     options.FormatOutput = false; // Override default options registered in ConfigureServices
            //     options.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 }; //optional
            //     //options.EnableMetrics = true;
            // });
        }

        // Dynamically resolve all GraphQl types in the same assembly as the root query
        // This prevents that we should add all the types manually
        private static void ConfigureGraphQlTypes(IServiceCollection services)
        {
            var applicationDbContext = services.BuildServiceProvider()
                .GetService<ApplicationDBContext>();
            
            var allGraphTypes = new GraphBuilder(applicationDbContext.Model)
                .Define<Book>()
                .Define<Author>(o => o
                    .Field(otherClass => otherClass.Name, with => with.Description("test")))
                .BuildGraphTypes();

            foreach(var graphType in allGraphTypes)
            {
                services.AddTransient(s => graphType);
            }
        }
    }
}
