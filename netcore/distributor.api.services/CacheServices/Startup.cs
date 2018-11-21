using System.Threading.Tasks;
using CacheServices.Services.Helpers;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace CacheServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _version = typeof(Startup).Assembly.GetName().Version.ToString();
        }

        public IConfiguration Configuration { get; }
        private string _version;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(
                configuration =>
                {
                    configuration.UseMemoryStorage();
                }
            );
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMetrics();

            services.AddSingleton(Configuration);

            services.RegisterCacheServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = _version,
                    Title = "CacheService",
                    Description = "ASP.NET Core web API for caching results from DB and autoupdate using Hangfire",
                    TermsOfService = "None"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<AddParamsToLogContextMiddleware>();
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Seq(Configuration["Seq:ServerUrl"])
                .Enrich.FromLogContext()
                .CreateLogger();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CacheService");
            });          
            
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new [] { new HangfireFilter() }
            });
            
            app.UseHangfireServer();

            Task.Run(() =>
            {
                var serviceProvider = app.ApplicationServices;
                var helper = serviceProvider.GetService<CacheHelper>();
                Log.Logger.Information("Jobs for updates will be created");
                helper.FillCaches();
            });
                      
            app.Run(async (context) =>
            {              
                await context.Response.WriteAsync($"CacheService - {_version}");
            });
            
            Log.Logger.Information($"CacheService version {_version} started");
        }
    }
}
