using APDB_WebRESTapi.DTOs;
using APDB_WebRESTapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.DAL
{
    public interface IEnrollmentDBService
    {
        public RegisterStudentStatus RegisterStudent(RegistrationStudentRequest registrationStudentRequest);
    }
}
