using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using TradingSystem.Service;
using TradingSystem.WebApi.Controllers;

namespace TradingSystem.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    //if (CurrentEnvironment.IsDevelopment())
                    //{
                    //    builder.WithOrigins("http://localhost:3000");
                    //}
                    //else
                    //{
                    //    // TBD
                    //    // builder.WithOrigins("");
                    //}

                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });

            services.AddControllers();

            services.AddSingleton(CurrentEnvironment.ContentRootFileProvider);
            services.AddSingleton(MarketGeneralService.Instance);
            services.AddSingleton(MarketProductsService.Instance);
            services.AddSingleton(MarketShoppingCartService.Instance);
            services.AddSingleton(MarketStoreGeneralService.Instance);
            services.AddSingleton(MarketStorePermissionsManagementService.Instance);
            services.AddSingleton(MarketUserService.Instance);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/login")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
                        var username = new byte[1024 * 4];
                        WebSocketReceiveResult result = await ws.ReceiveAsync(new ArraySegment<byte>(username), CancellationToken.None);
                        LoggedInController.Instance.addClient(BitConverter.ToString(username), ws);
                        //in logout
                        //await ws.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                        //LoggedInController.Instance.RemoveClient(BitConverter.ToString(username));
                    }
                    else { context.Response.StatusCode = 400; }
                }
                
                else
                {
                    await next();
                }
            });
        }
    }



    /*was WebSocketRecieveResult, changed to String
    private async Task RecieveMessage(WebSocket socket, Action<String, byte[]> action)
    {
        var buf = new byte[1024 * 4];
        while(socket.State == WebSocketState.Open)
        {
            
        }
    }*/
}
