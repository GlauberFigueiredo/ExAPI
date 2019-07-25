using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RCN.API.Data;

namespace RCN.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                })
                .AddXmlSerializerFormatters();

            services.AddDbContext<ProdutoContexto>(opt =>
            opt.UseInMemoryDatabase(databaseName: "prodtuoInMemory")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));


            services.AddTransient<IProdutoRepository, ProdutoRepository>();

            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            }) ;

            services.AddApiVersioning();

            services.AddResponseCaching();
            services.AddResponseCompression(opt =>
            {
                //opt.Providers.Add<GzipCompressionProvider>();
                opt.Providers.Add<BrotliCompressionProvider>();
                opt.EnableForHttps = true;
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var item in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(item.GroupName, new OpenApiInfo
                    {
                        Title = $"API de Produtos {item.ApiVersion} ",
                        Version = item.ApiVersion.ToString()
                    });

                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseResponseCaching();
            app.UseResponseCompression();


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                foreach (var item in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.GroupName);

                }

                c.RoutePrefix = string.Empty;




            });



            app.UseMvc();
        }
    }
}
