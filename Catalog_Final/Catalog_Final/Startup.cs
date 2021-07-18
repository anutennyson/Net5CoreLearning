using Catalog_Final.Repositories;
using Catalog_Final.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog_Final
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //Registering all the services
        public void ConfigureServices(IServiceCollection services)
        {
            //Telling MongoDB client how to serialize some of the data
            //Here we are just concerened about GUid and datetime as they are the 
            //only one with special data values

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            //Ends
            var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            services.AddSingleton<IMongoClient>(ServiceProvider =>
            {
                return new MongoClient(mongoDbSettings.ConnectionString);
            }
            );
            // rather than explicit type of the depedency
            // the way we did below. Here we are going to
            //construct the inject

            //We are now going to use MongoDbRepository rather than in memeory
            //so comment below and add a new line
            //services.AddSingleton<IItemsRepository, InMemItemsRepository>();
            services.AddSingleton<IItemsRepository, MongoDBItemsRepository>();

            //Below line is modified to ignore the framework from removing async
            //at runtime from the functions
            //services.AddControllers();
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
                //Now Async suffix will not be removed
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog_Final", Version = "v1" });
            });

            //We are adding additional health checks to make sure that we are able to reach mongodb or not
            //we have to install a nuget package -- aspnetcore.healthchecks.mongodb
            //services.AddHealthChecks();
            //Below one is just to check if the mongo is working or not. I am adding additional code for
            //ready and live end points
            //services.AddHealthChecks()
            //    .AddMongoDb(mongoDbSettings.ConnectionString, name: "mongodb", timeout: TimeSpan.FromSeconds(3));

            services.AddHealthChecks()
                .AddMongoDb(mongoDbSettings.ConnectionString, name: "mongodb", timeout: TimeSpan.FromSeconds(3),
                tags:new[] { "ready"});

            //So the name is the name of the healthcheck and if in 3 secs we do not get a response then we will consider
            //that the check has failed
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //Adding all the middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog_Final v1"));
            }

            //When the app runs in the docker then it will switch from dvelopment to production
            //And we do not want the container to have the https redirection

            if(env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //Adding additional health checks for ready and live
                //endpoints.MapHealthChecks("/health");
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions { 
                    Predicate = (check) => check.Tags.Contains("ready"),
                    //Whole below code is for writing a different message - Starts
                    ResponseWriter= async(context, report) =>
                    {
                        var result = JsonSerializer.Serialize(
                            new
                            {
                                status = report.Status.ToString(),
                                checks = report.Entries.Select(entry => new
                                {
                                    name = entry.Key,
                                    status = entry.Value.Status.ToString(),
                                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                    duration = entry.Value.Duration.ToString()
                                })
                            });

                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                    //Whole below code is for writing a different message - ends
                });
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = (_) => false //come back as long as the service is alive
                });
            });
        }
    }
}
