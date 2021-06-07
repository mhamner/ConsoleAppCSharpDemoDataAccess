using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataLibrary.DataAccessLayer
{
    class DataAccess : IDataAccess
    {
        private string _connectionString;

        public DataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateDataViaStoredProcedure<T>(string storedProcName, Dictionary<string, object> parameters)
        {

        }

        public List<T> ReadDataViaStoredProcedure<T>(string storedProcName, Dictionary<string, object> parameters)
        {
            List<T> dataList = new List<T>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(storedProcName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Loop through the key value pairs passed in and extract each pair, then add them to the sqlcommand as the parameters for the stored proc
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                        //cmd.Parameters.Add("@AuthorName", SqlDbType.VarChar);
                        //cmd.Parameters["@AuthorName"].Value = "Barbara Park";
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Get the value returned from our SQL read (we're reading one at a time here, so we can just always get the 0 index)
                            var value = reader.GetValue(0);
                            try
                            {
                                ////Because I'm setting my method up to handle any data type, I need to take whatever data type I get back 
                                /// from the DB and cast it to type of the list T being passed in - so if the caller passes in a string list, T will be string,
                                /// and this command will cast whatever I get back from the DB to a string
                                dataList.Add((T)Convert.ChangeType(value, typeof(T)));
                            }
                            catch (InvalidCastException)
                            {
                                dataList.Add(default(T));
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dataList;
        }

        public void UpdateDataViaStoredProcedure<T>(string storedProcName, Dictionary<string, object> parameters)
        {

        }

        public void DeleteDataViaStoredProcedure<T>(string storedProcName, Dictionary<string, object> parameters)
        {

        }

        public List<Object> PopulateObjectViaStoredProcedure(string storedProcName, Dictionary<string, object> parameters)
        {
            List<Object> dataList = new List<Object>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(storedProcName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Loop through the key value pairs passed in and extract each pair, then add them to the sqlcommand as the parameters for the stored proc
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Loop through each row returned and add the fields to an Object array
                        while (reader.Read())
                        {
                            //Since we're going to get back multiple fields, create an array the size of the fields we're getting back to store them
                            Object[] dataObject = new Object[reader.FieldCount];

                            //Now we call GetValues which will put the values from tue current read into our dataObject array
                            int _count = reader.GetValues(dataObject);

                            //Now we add our data object array to our list of objects
                            dataList.Add(dataObject);                          
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dataList;
        }

        public DataTable PopulateDataTableViaStoredProcedure(string storedProcName, Dictionary<string, object> parameters)
        {
            DataTable _dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(storedProcName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Loop through the key value pairs passed in and extract each pair, then add them to the sqlcommand as the parameters for the stored proc
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                    }

                    //Fill the datatable with the results returned by the stored procedure - the advantage of a datatable is you can use the column names!
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(_dataTable);                       
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _dataTable;
        }
    }
}
