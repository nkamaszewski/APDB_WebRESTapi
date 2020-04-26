using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.Models
{
    public class RegistrationNewStudentDTO
    {
        public string IndexNumber { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Studies { get; set; }

        public static bool isValid(RegistrationNewStudentDTO registrationNewStudent)
        {
            return !(String.IsNullOrEmpty(registrationNewStudent.IndexNumber) || String.IsNullOrEmpty(registrationNewStudent.FirstName) || String.IsNullOrEmpty(registrationNewStudent.LastName) || !registrationNewStudent.BirthDate.HasValue || String.IsNullOrEmpty(registrationNewStudent.Studies));
        }
    }
}
