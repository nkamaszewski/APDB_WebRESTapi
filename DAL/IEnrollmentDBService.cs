using APDB_WebRESTapi.DTOs;
using APDB_WebRESTapi.DTOs.Requests;
using APDB_WebRESTapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.DAL
{
    public interface IEnrollmentDBService
    {
        public EnrollmentStatus RegisterStudent(RegistrationStudentRequest registrationStudentRequest);
        public EnrollmentStatus PromoteStudents(PromoteStudentsRequest promoteStudentsRequest);
    }
}
