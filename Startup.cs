using System;　
using System.Collections.Generic;　
using System.Diagnostics;
using System.IO;
using System.Linq;　
using System.Reflection;
using System.Text;
using System.Threading.Tasks;　
using Microsoft.AspNetCore.Builder;　
using Microsoft.AspNetCore.Hosting;　
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;　
using Microsoft.AspNetCore.Mvc;　
using Microsoft.Extensions.Configuration;　
using Microsoft.Extensions.DependencyInjection;　
using Microsoft.Extensions.Hosting;　
using Microsoft.Extensions.Logging;　
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
namespace employ {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddControllers ();
            //Swagger的DI
            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine (AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments (xmlPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

           
            app.UseSwagger (c => {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "My API V1");
            });

            //app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}