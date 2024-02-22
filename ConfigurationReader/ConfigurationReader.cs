using System.Configuration;

namespace Importer.ConfigurationReader
{
    public class ConfigurationReader : IConfigurationReader
    {
        //Configuration file is used for storing connectionString
        public string GetSection(string section) => new AppSettingsReader().GetValue(section, typeof(string)).ToString() ?? "";
    }
}
