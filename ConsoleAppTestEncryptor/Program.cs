using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using EncryptLibrary;
using Microsoft.Extensions.Options;

namespace MyConsoleApp
{
    /// <summary>
    /// Open the EncodeDecode project and right click on the EncryptLibrary project and select Pack using VisualStudio 2022
    /// Import the package to this project. 
    /// If you are not able to pack you can just reference this project to that project for this code to run.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Set up the host to enable DI and Configuration
            using IHost host = CreateHostBuilder(args).Build();

            // Retrieve the service
            var app = host.Services.GetRequiredService<App>();

            // Run the application
            app.Run();
        }

        // Method to set up HostBuilder with DI and Configuration
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Add appsettings.json
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // Register services for DI
                    services.AddTransient<App>();
                   
                    services.Configure<AppSettings>(context.Configuration.GetSection("AppSettings"));
                    services.Configure<EncodeDecodeConfiguration>(context.Configuration.GetSection(EncodeDecodeConfiguration.Name));
                    services.AddScoped<IEncodeDecode, EncodeDecode>();
                });
    }

    public class App
    {
        private readonly AppSettings _settings;
        private readonly IEncodeDecode _encryptor;

        public App(IOptions<AppSettings> settings, IEncodeDecode encrypt)
        {
            _settings = settings.Value;
            _encryptor = encrypt;
        }

        public void Run()
        {
            // Use the configuration value from appsettings.json
            Console.WriteLine(_settings.Message);
            Console.WriteLine("Enter Text");
            var text = Console.ReadLine();
            var encrypted = _encryptor.Encrypt(text);
            var decrypted =_encryptor.Decrypt(encrypted);
            Console.WriteLine(encrypted);
            Console.WriteLine("****************");
            Console.WriteLine(decrypted);
        }
    }

    // Class to bind the configuration
    public class AppSettings
    {
        public string Message { get; set; }
    }
}
