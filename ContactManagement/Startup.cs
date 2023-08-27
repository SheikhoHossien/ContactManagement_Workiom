using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using ContactManagement.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ContactManagement.Models;
using System.IO;
using MongoDB.Bson;
using ContactManagement.Middlewares;
using ContactManagement.Exceptions;

namespace ContactManagement
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
            services.AddMvc();
            services.Configure<ContactManagementDatabaseSettings>(
                Configuration.GetSection(nameof(ContactManagementDatabaseSettings))
                );
            services.AddSingleton<IContactManagementDatabaseSettings>(
                sp=>sp.GetRequiredService<IOptions<ContactManagementDatabaseSettings>>().Value
                );


            //********Get Connection String
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var connectionString = configuration.GetConnectionString("ConnectionStrings");


            //*******
            /*services.AddSingleton<IMongoClient>(
                s => new MongoClient(Configuration.GetValue<string>("ContactManagementDatabaseSettings.ConnectionString"))
                ) ;*/
            services.AddSingleton<IMongoClient>(
              s => new MongoClient(connectionString)
              );
            services.AddScoped<IContactManagementRepository<Company>, CompanyRepository>();
            //services.AddScoped<IContactManagementRepository<Document>, CompanyRepository>();

            services.AddScoped<IContactManagementRepository<Contact>, ContactRepository>();
            
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddControllersWithViews().AddNewtonsoftJson(options=>
            options.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options=>options.SerializerSettings.ContractResolver
                =new DefaultContractResolver());
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContactManagement", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContactManagement v1"));
            }
            app.UseExceptionMiddleware();
            app.UseHttpsRedirection();

            app.UseRouting();
            //app.UseWhen(context => context.Request.Path.ToString().Contains("/api"),configuration);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
