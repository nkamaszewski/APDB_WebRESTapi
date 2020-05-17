using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using APDB_WebRESTapi.DAL;
using APDB_WebRESTapi.DTOs.Requests;
using APDB_WebRESTapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace APDB_WebRESTapi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentDBService _studentDBService;

        public StudentsController(IStudentDBService studentDBService)
        {
            _studentDBService = studentDBService;
        }

        [HttpGet]
        [Authorize]
        //[Authorize(Roles ="admin")]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_studentDBService.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            }
            else if (id == 2)
            {
                return Ok("Malewski");
            }
            else
            {
                return NotFound("Nie znaleziono studenta");
            }
        }

        [HttpGet("{studentIndexNumber}/enrollment")]
        public IActionResult GetStudentsEnrollments(string studentIndexNumber)
        {
            return Ok(_studentDBService.GetStudentEnrollment(studentIndexNumber));
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id)
        {
            return Ok($"student o {id} - Aktualizacja dokończona ");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok($"student o {id} - Usuwanie ukończone");
        }

    }
}