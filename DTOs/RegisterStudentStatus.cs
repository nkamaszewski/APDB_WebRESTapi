using APDB_WebRESTapi.DTOs.Response;
using APDB_WebRESTapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.DTOs
{
    public class RegisterStudentStatus
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public RegistrationStudentEnrollmentResponse enrollment { get; set; }
    }
}
