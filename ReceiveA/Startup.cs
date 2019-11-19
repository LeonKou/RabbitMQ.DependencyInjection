using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQMiddleware;
using MQMiddleware.Configuration;
using ReceiveA.Service;
using System.Collections.Generic;

namespace ReceiveA
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddRabbitMqClient(new RabbitMqClientOptions
            {
                HostName = "31044278.mq-amqp.cn-hangzhou-a.aliyuncs.com",
                Port = 5672,
                Password = "1ZtItBgQR3PINsCD9QCamekOdnihJk",
                UserName = "LTAI4FnXef37eRU7H8NEaVQ3",
                VirtualHost = "AcadsocAMQPTest",
            }).AddConsumptionExchange("AcadsocTest", new RabbitMqExchangeOptions
            {
                DeadLetterExchange = "DeadExchange",
                AutoDelete = false,
                Type = "direct",
                Durable = true,
                Queues = new List<RabbitMqQueueOptions> { new RabbitMqQueueOptions { AutoDelete = false, Exclusive = false, Durable = true, Name = "myqueue", RoutingKeys = new HashSet<string> { "mini" } } }
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

            app.UseMvc();
        }
    }
}
