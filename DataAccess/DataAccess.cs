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
                    //For performance it is better to submit a DataTable and process it on the DB rather than .NET code without the need of extra logic
                    connection.Open();
                    var cmd = new SqlCommand("dbo.ImportCSVFromDataTable", connection);
                    cmd.Parameters.Add(new SqlParameter("@csvDataTable", ToDataTable(ReadPersonCSVAsEnumerable(csvPath))));
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        //Converts CSV to Person object IEnumerable
        private IEnumerable<Person> ReadPersonCSVAsEnumerable(string path) => File.ReadAllLines(path).Select(v => Person.FromCsv(v));

        //Converts generic IEnumerable to a DataTable. Generics for reusability
        private static DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
