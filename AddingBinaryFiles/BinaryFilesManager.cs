using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AddingBinaryFiles
{
    class BinaryFilesManager
    {
        public byte[] GetBinaryFiles(string fileName)
        {
            byte[] bytes = null;

            try
            {
                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    bytes = new byte[file.Length];
                    file.Read(bytes, 0, bytes.Length);
                    file.Close();
                }
            }
            catch (Exception e)
            {

            }

            return bytes;
        }

        public void InsertDataInBytes(byte[] bytes, string connectionString, int id)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE ActivityPoints SET locationImage=@li WHERE ID=@id";
                    command.Parameters.Add("@li", SqlDbType.VarBinary).Value = bytes;
                    command.Parameters.AddWithValue("@id",id); 
                    connection.Open();

                    int a = command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception e)
            {

            }
        }

    }
}
