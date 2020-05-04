using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APDB_WebRESTapi.DAL;
using APDB_WebRESTapi.DTOs;
using APDB_WebRESTapi.DTOs.Requests;
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
            EnrollmentStatus enrollmentStatus = _enrollmentDBService.RegisterStudent(registrationStudent);

            if(enrollmentStatus.Status == 400)
            {
                return BadRequest(enrollmentStatus.Message);
            }

            return Ok(enrollmentStatus.enrollment);
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudents(PromoteStudentsRequest promoteStudentsRequest)
        {
            EnrollmentStatus enrollmentStatus = _enrollmentDBService.PromoteStudents(promoteStudentsRequest);

            if (enrollmentStatus.Status == 400)
            {
                return BadRequest(enrollmentStatus.Message);
            }

            return Ok(enrollmentStatus.enrollment);
        }

    }
}