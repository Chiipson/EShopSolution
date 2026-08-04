using EShopData.Common;
using EShopData.Data;
using EShopData.Data.Seed;
using EShopData.Menus;
using EShopData.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EShopData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);

            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<EShopDbContext>(options =>
                    options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));

                services.AddTransient<MainMenu>();
                services.AddTransient<UserMenu>();
                services.AddTransient<ProductMenu>();
                services.AddTransient<CartMenu>();

                services.AddTransient<ProductService>();
                services.AddSingleton<CartService>();

                services.AddTransient<ConsoleHelper>();
            });

            var host = hostBuilder.Build();

            //var dbContext = host.Services.GetRequiredService<EShopDbContext>();
            //DbSeder.Seed(dbContext);

            var mainMenu = host.Services.GetRequiredService<MainMenu>();
            mainMenu.Show();
        }
    }
}
