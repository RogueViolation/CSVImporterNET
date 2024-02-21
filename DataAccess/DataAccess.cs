using ApplicationDataAccess.DTO;
using Importer.ConfigurationReader;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Importer.ApplicationDataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfigurationReader _config;

        public DataAccess(IConfigurationReader config)
        {
            _config = config;
        }



        public bool ImportPersonCSVToDB(string csvPath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetSection("connectionString")))
                {
                    connection.Open();
                    var cmd = new SqlCommand("dbo.ImportCSVFromDataTable", connection);
                    cmd.Parameters.Add(new SqlParameter("@csvDataTable", ToDataTable(ReadPersonCSVAsEnumerable(csvPath))));
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    connection.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private IEnumerable<Person> ReadPersonCSVAsEnumerable(string path) => File.ReadAllLines(path).Select(v => Person.FromCsv(v));

        private static DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
