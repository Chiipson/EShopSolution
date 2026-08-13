using EShopData.Common;
using EShopData.Data;
using EShopData.Data.Seed;
using EShopData.Menus;
using EShopData.Security;
using EShopData.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                {
                    options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection"));
                    options.LogTo(_ => { }, Microsoft.Extensions.Logging.LogLevel.None);
                });

                services.AddTransient<MainMenu>();
                services.AddTransient<UserMenu>();
                services.AddTransient<ProductMenu>();
                services.AddTransient<CartMenu>();
                services.AddTransient<OrderMenu>();

                services.AddTransient<ProductService>();
                services.AddTransient<CartService>();
                services.AddTransient<UserService>();
                services.AddTransient<CheckoutService>();
                services.AddTransient<OrderService>();
                services.AddTransient<CategoryService>();

                services.AddTransient<PasswordHasher>();
                services.AddSingleton<UserSession>();

                services.AddTransient<ConsoleHelper>();
            });

            hostBuilder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.None);
            });

            var host = hostBuilder.Build();

            //var dbContext = host.Services.GetRequiredService<EShopDbContext>();
            //DbSeder.Seed(dbContext);

            var mainMenu = host.Services.GetRequiredService<MainMenu>();
            mainMenu.Show();
        }
    }
}
