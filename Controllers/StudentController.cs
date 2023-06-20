using Lesson01.API.DTO;
using Lesson01.API.Entities;
using Lesson01.API.Interfaces;
using MicroServices.Shared.Adapters;
using MicroServices.Shared.Controllers;
using MicroServices.Shared.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lesson01.API.Controllers
{
    public class StudentController : BaseApiController<StudentController>
    {
        private readonly IGenericRepository<View_Student> _studentViewRepo;
        private readonly ILogger<LessonController> _logger;

        public StudentController(IGenericRepository<View_Student> studentViewRepo,
                                 ILogger<LessonController> logger)
        {
            _studentViewRepo = studentViewRepo;
            _logger = logger;
        }

        /// <summary>
        /// Get student info
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="examCode"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<ExamScoreModel>>), 200)]
        public async Task<IActionResult> GetInfo(long id)
        {
            var student = await _studentViewRepo.GetByIdAsync(id, select: a => new StudentInfo
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                FullName = a.FullName,
                Age = a.Age,
                EmailAddress = a.EmailAddress,
                CreatedOn = a.CreatedOn
            });

            if(student == null)
            {
                return BadRequest(Result.Failure("Student not found! ID may be invalid, check and try again"));
            }

            return Ok(Result<StudentInfo>.Success(student, "Student data retrieved"));
        }
    }
}