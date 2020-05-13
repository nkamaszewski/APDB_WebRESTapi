using APDB_WebRESTapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.DAL
{
   public interface IStudentDBService
    {
        IEnumerable<Student> GetStudents();
        IEnumerable<Enrollment> GetStudentEnrollment(string studentIndexNumber);
        bool CheckIndex(string index);
    }
}
