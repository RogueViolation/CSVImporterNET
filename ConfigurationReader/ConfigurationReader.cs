using System.Configuration;

namespace Importer.ConfigurationReader
{
    public class ConfigurationReader : IConfigurationReader
    {
        public string GetSection(string section) => new AppSettingsReader().GetValue(section, typeof(string)).ToString() ?? "";
    }
}
