using FichaDeMusicosCCB.Api;

namespace FichaDeMusicosCCB.Api
{
    public class Program
    {
        ///Versão Oficial .Net 5
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // public static IHostBuilder CreateHostBuilder(string[] args) =>
        //     Host.CreateDefaultBuilder(args)
        //         .ConfigureWebHostDefaults(webBuilder =>
        //         {
        //             webBuilder.UseStartup<Startup>();
        //         });

        ///Versão para rodar migrations - .Net Core 2.1
        //public static void Main(string[] args)
        //{
        //    BuildWebHost(args).Run();
        //}

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .Build();

        //Versão .Net 5 - Prod(Heroku)
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("http://*:" + Environment.GetEnvironmentVariable("PORT"));
            });

    }
}


