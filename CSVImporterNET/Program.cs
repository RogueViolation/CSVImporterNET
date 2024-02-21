using Importer.ApplicationDataAccess;
using Importer.ConfigurationReader;
using Microsoft.Extensions.DependencyInjection;
namespace CSVImporterNET
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new();
            services.AddSingleton<IConfigurationReader, ConfigurationReader>()
                    .AddSingleton<IDataAccess, DataAccess>();

            var provider = services.BuildServiceProvider();

            new Application(provider.GetService<IDataAccess>()).RunApp();
        }
    }

    public class Application
    {
        private readonly IDataAccess _dataAccess;
        public Application(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public void RunApp()
        {
            Console.WriteLine("CSVImporterNET..");
            if (_dataAccess.ImportPersonCSVToDB("./file.csv"))
            {
                Console.WriteLine("CSV imported successfully!");
            }
            else
            {
                Console.WriteLine("An error occured while importing!");
            }
        }
    }
}
