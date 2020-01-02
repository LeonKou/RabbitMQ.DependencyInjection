using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQMiddleware;
using MQMiddleware.Configuration;
using System.Collections.Generic;

namespace MQ
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddRabbitMqClient(new RabbitMqClientOptions
            {
                HostName = "192.168.0.25",
                Port = 5672,
                Password = "111111",
                UserName = "leon",
                VirtualHost= "LeonTest",
            })
            //    .AddProductionExchange("LeonTest", new RabbitMqExchangeOptions
            //{
            //    DeadLetterExchange="DeadExchange",
            //    AutoDelete = false,
            //    Type = "direct",
            //    Durable = true,
            //    Queues = new List<RabbitMqQueueOptions> { new RabbitMqQueueOptions { AutoDelete = false, Exclusive = false, Durable = true, Name = "myqueue", RoutingKeys = new HashSet<string> { "mini" } } }
            //})
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
