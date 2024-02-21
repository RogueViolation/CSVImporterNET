using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace ApplicationDataAccess.DTO
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public static Person FromCsv(string line, string delimiter = ",")
        {
            var values = line.Split(delimiter);
            return values.Length == typeof(Person).GetProperties(BindingFlags.Public | BindingFlags.Instance).Length ? 
                new Person() { FirstName = values[0], LastName = values[1], Age = int.Parse(values[2]), Email = values[3] } : 
                throw new FormatException("CSV file is invalid");
        }
    }
}
