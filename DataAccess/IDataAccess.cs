using ApplicationDataAccess.DTO;
using System.Data;

namespace Importer.ApplicationDataAccess
{
    public interface IDataAccess
    {
        bool ImportPersonCSVToDB(string csvPath);
    }
}
