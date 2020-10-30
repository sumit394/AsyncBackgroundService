using Demo.BackgroundService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using StarterKit;
using System;
using System.Net.Http;

namespace Maersk.StarterKit.IntegrationTests
{
    public class SetupServer : IDisposable
    {
        public SetupServer()
        {

            HostServer = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer();
                    webBuilder.UseStartup<Startup>();
                }).UseStarterKit().Build();
            HostServer.Start();
            Client = HostServer.GetTestClient();
        }

        public HttpClient Client { get; }

        public IHost HostServer { get; }

        public void Dispose()
        {
            Client?.Dispose();
            HostServer.StopAsync().Wait();
        }
    }
}
