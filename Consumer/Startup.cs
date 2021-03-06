using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumer.Data;
using Consumer.Entities;
using Consumer.Interfaces;
using Consumer.Messaging;
using Consumer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentsAPI.Messaging;

namespace Consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(o => o.UseInMemoryDatabase(databaseName: "PaymentsDb"));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.Configure<RabbitMqConfig>(Configuration.GetSection("RabbitMQ"));
            services.AddControllers();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICardPaymentService, CardPaymentService>();
            services.AddHostedService<CardPaymentReceiver>();
            services.AddHostedService<PurchaseOrderReceiver>();
            services.AddHostedService<DirectCardPaymentReceiver>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
