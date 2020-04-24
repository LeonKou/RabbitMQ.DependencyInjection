using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQMiddleware;
using MQMiddleware.Configuration;
using ReceiveB.Service;
using System.Collections.Generic;

namespace ReceiveB
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddRabbitMqClient(new RabbitMqClientOptions
            {
                HostName = "172.16.127.229",
                Port = 5672,
                Password = "guest",
                UserName = "guest",
                VirtualHost = "/",
            }).AddConsumptionExchange("LeonTest", new RabbitMqExchangeOptions
            {
                DeadLetterExchange = "DeadExchange",
                AutoDelete = false,
                Type = "fanout",
                Durable = true,
                Queues = new List<RabbitMqQueueOptions> { new RabbitMqQueueOptions { AutoDelete = false, Exclusive = false, Durable = true, Name = "myqueue", RoutingKeys = new HashSet<string> { "mini", "yang" } } }
            })
             .AddMessageHandlerSingleton<CustomMessageHandler>("mini");

            services.BuildServiceProvider().GetRequiredService<IQueueService>().StartConsuming();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseMvc();
        }
    }
}
