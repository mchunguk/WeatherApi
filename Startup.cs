using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using WeatherApi.Data;
using WeatherApi.Swagger;

namespace WeatherApi
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

            // services.AddDbContext<WeatherContext>(opt => opt.UseSqlServer
            //    (Configuration.GetConnectionString("SqlServerConnection")));

            services.AddDbContext<WeatherContext>(opt => opt.UseSqlite
                 (Configuration.GetConnectionString("SqlLiteConnection")));
            
            services.AddControllers().AddNewtonsoftJson(s => s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            services.AddApiVersioning(options => 
                {
                    // Set to false or remove the line if route versioning is being used.
                    //options.AssumeDefaultVersionWhenUnspecified = true;
                    
                    // two ways of specifying 1.0 as default.
                    //options.DefaultApiVersion =  new ApiVersion(1, 0);
                    //options.DefaultApiVersion = ApiVersion.Default;

                    // where to read the version [1 TO 3] from header:
                    // Option 1 - Accept: application/json; version=2.0
                    //options.ApiVersionReader = new MediaTypeApiVersionReader("version");

                    // Option 2 - Just add header: x-version:2.0
                    //options.ApiVersionReader = new HeaderApiVersionReader("x-version");

                    // Option 3 - support multiples
                    // options.ApiVersionReader = ApiVersionReader.Combine (
                    //     new MediaTypeApiVersionReader("version"),
                    //     new HeaderApiVersionReader("x-version"));

                    // Option 4 - Route versioning done in controller.

                    // If you do not pick options [1]-[4]; at end of query string append ?api-version=1.0

                    // If you do not want to use attribute decoration on controllers you can do something like this
                    // using fluent API (need more time to figure this out):
                    // options.Conventions.Controller<WeatherForecastsController>()
                    //     .HasDeprecatedApiVersion(new ApiVersion(1,0))
                    //     .HasApiVersion(new ApiVersion(2,0))
                    //     .Action(typeof(WeatherForecastsController).GetMethod(nameof(WeatherForecastsController.GetAllForecastsV2))!).MapToAPiVersion(new ApiVersion(2,0));

                    // Response header will tell clients which API versions are available.
                    // api-supported-versions:1.0,2.0
                    options.ReportApiVersions = true;

                }
            );

            services.AddVersionedApiExplorer(options =>
            {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IWeatherRepo, SqlWeatherRepo>();
            services.AddScoped<IWeatherRepoAsync, SqlWeatherRepoAsync>();

            //services.AddControllers();
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherApi", Version = "v1" });
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    options =>
                    {
                        // build a swagger endpoint for each discovered API version
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                    });                
            }

            app.UseHttpsRedirection();

            // This will make sure each HTTP request is logged (https://jkdev.me/asp-net-core-serilog/).
            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
