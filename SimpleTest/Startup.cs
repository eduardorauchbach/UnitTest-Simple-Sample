using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleTest
{
	public class Startup
	{
        #region Constants
        public const string DefaultPolityName = "DefaultPolicy";

        private const string ProductionEnvironment = "Prod";

        private const string SwaggerTitle = "Simple Test";
        private const string SwaggerDescription = "Aplicação Demo";
        private const string SwaggerVersion = "v1.0";

        #endregion

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddCors(options =>
            {
                options.AddPolicy(DefaultPolityName,
                builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            });

            _ = services.AddControllers();
            _ = services.AddResponseCompression();

            _ = services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerVersion, new OpenApiInfo
                {
                    Title = SwaggerTitle,
                    Description = SwaggerDescription,
                    Version = SwaggerVersion,
                    Contact = new OpenApiContact
                    {
                        Name = "Eduardo Rauchbach",
                        Email = "eduardo.rauchbach@gmail.com",
                    },
                });
                c.UseAllOfForInheritance();
                c.UseOneOfForPolymorphism();
            });

            _ = services.AddSingleton(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string targetValue;
            bool isProduction;

            string endpoint;
            string title;

            targetValue = Configuration.GetValue<string>("TargetEnvironment");
            isProduction = ProductionEnvironment.Equals(targetValue, StringComparison.OrdinalIgnoreCase);

            if (!isProduction)
            {
                endpoint = $"{SwaggerVersion}/swagger.json";
                title = $"{SwaggerTitle} {SwaggerVersion}";

                _ = app.UseDeveloperExceptionPage();

                _ = app.UseSwagger();
                _ = app.UseSwaggerUI(c => c.SwaggerEndpoint(endpoint, title));
            }

            _ = app.UseRouting();
            _ = app.UseCors(DefaultPolityName);
            _ = app.UseAuthorization();

            _ = app.UseStaticFiles();
            _ = app.UseResponseCompression();

            _ = app.UseAuthorization();
            _ = app.UseHttpsRedirection();
            _ = app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
