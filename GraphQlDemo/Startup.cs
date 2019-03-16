using GraphQL.Validation.Complexity;
using GraphQlDemo.Data;
using GraphQlDemo.Query.Data;
using GraphQlDemo.Query.GraphQlTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                    schema.SetQueryType<BooksQuery>();
                });
                // .AddDataLoader();

            // All graph types must be registered
            services.AddSingleton<BooksQuery>();
            services.AddSingleton<BookType>();
            services.AddSingleton<AuthorType>();
            services.AddSingleton<PublisherType>();

            services.AddSingleton<IBookRepository, BookRepository>();
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
            });

            app.UseMvc();
        }
    }
}
