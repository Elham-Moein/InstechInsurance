using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceProject.Consumers;
using InsuranceProject.Context;
using InsuranceProject.Interfaces;
using InsuranceProject.Mapper;
using InsuranceProject.Repository;
using InsuranceProject.Senders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace InsuranceProject
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors(options =>
           {
               options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
               {
                   builder.WithOrigins("http://localhost:3000",
                       "http://www.contoso.com");
               }); 
           }); 
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InsuranceProject", Version = "v1" });
            });
            services.AddSingleton<ICosmosDbRepository>(InitializeCosmosClientInstanceAsync(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());
            services.AddTransient<IAuditMessageSender, AuditMessageSender>();
            services.AddSingleton<IAuditMessageConsumer, AuditMessageConsumer>();
            services.AddScoped<IClaimAuditRepository, ClaimAuditRepository>();
            
            services.AddDbContext<DbReadWriteContext>(options =>
                options.UseSqlServer(Configuration["InsuranceDb"]));
            services.AddAutoMapper(typeof(InsuranceMapper));
        }
        
        private static async Task<CosmosDbRepository> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            CosmosClient client = new CosmosClient(account, key);
            
            
            // var database = client.GetDatabase(databaseName);
            // DatabaseResponse databaseResourceResponse = await database.DeleteAsync();
            
            
            CosmosDbRepository cosmosDbService = new CosmosDbRepository(client, databaseName, containerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/name");

            return cosmosDbService;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InsuranceProject v1"));
                app.UseCors(
                    options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            }

            app.UseHttpsRedirection();



            app.UseRouting();

            
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            var bus = app.ApplicationServices.GetService<IAuditMessageConsumer>();
            bus.RegisterHandlers().GetAwaiter().GetResult();

        }
    }
}