using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccessLayer
{
    public interface IDataAccess
    {
        void CreateDataViaStoredProcedure<T>(string storedProcName, Dictionary<string, object> parameters);
        List<T> ReadDataViaStoredProcedure<T>(string storedProcName, Dictionary<string, object> parameters);
        void UpdateDataViaStoredProcedure<T>(string storedProcName, Dictionary<string, object> parameters);
        void DeleteDataViaStoredProcedure<T>(string storedProcName, Dictionary<string, object> parameters);
    }
}
