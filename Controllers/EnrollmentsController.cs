using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APDB_WebRESTapi.DAL;
using APDB_WebRESTapi.DTOs;
using APDB_WebRESTapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APDB_WebRESTapi.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {

        private readonly IEnrollmentDBService _enrollmentDBService;

        public EnrollmentsController(IEnrollmentDBService enrollmentDBService)
        {
            _enrollmentDBService = enrollmentDBService;
        }

        [HttpPost]
        public IActionResult RegisterNewStudent(RegistrationStudentRequest registrationStudent)
        {
            RegisterStudentStatus registerStudentStatus = _enrollmentDBService.RegisterStudent(registrationStudent);

            if(registerStudentStatus.Status == 400)
            {
                return BadRequest(registerStudentStatus.Message);
            }

            return Ok(registerStudentStatus.enrollment);
        }
    }
}