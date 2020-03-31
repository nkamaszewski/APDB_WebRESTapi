using APDB_WebRESTapi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.DAL
{
    public class StudentDBService : IStudentDBService
    {
        private string SqlConn = "Data Source=db-mssql;Initial Catalog=s16456;Integrated Security=True";

        public IEnumerable<Student> GetStudents()
        {
            var output = new List<Student>();
            using (var client = new SqlConnection(SqlConn))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = "SELECT * FROM Student";

                    client.Open();
                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        output.Add(new Student
                        {
                            IndexNumber = dataReader["IndexNumber"].ToString(),
                            FirstName = dataReader["FirstName"].ToString(),
                            LastName = dataReader["LastName"].ToString(),
                        });
                    }
                }
            }

            return output;
        }
    }
}
