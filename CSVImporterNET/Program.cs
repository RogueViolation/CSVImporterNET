﻿using Importer.ApplicationDataAccess;
using Importer.ConfigurationReader;
using Microsoft.Extensions.DependencyInjection;
//.NET 6 feature to omit "Console." for every Console operation
using static System.Console;
namespace CSVImporterNET
{
    public class Program
    {
        static void Main(string[] args)
        {
            //DI used for cleaner code, testability and no class init from Main
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
        //Main application
        public void RunApp()
        {
            WriteLine("CSVImporterNET..");
            Write("Please enter the path to CSV file. ");
            ForegroundColor = ConsoleColor.Yellow;
            Write("To use the default file press Enter. \n");
            ResetColor();
            var path = ReadLine();
            //Pass the entered value for import logic
            if (_dataAccess.ImportPersonCSVToDB(string.IsNullOrEmpty(path) ? "./file.csv" : path))
            {
                ForegroundColor = ConsoleColor.Green;
                WriteLine("CSV imported successfully!");
                ResetColor();
            }
            else
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("An error occured while importing!");
                ResetColor();
            }
            WriteLine("Press any key to continue. . .");
            ReadKey();
        }
    }
}
