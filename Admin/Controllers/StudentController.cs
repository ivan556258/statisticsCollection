using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WebApplication1.Admin.DTOs;
using WebApplication1.Admin.Repository.Query;
using WebApplication1.DatabaseManager;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Admin.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/v1/[controller]")]
public class StudentController : ControllerBase
{
    private readonly StudentQuery studentQuery;

    public StudentController()
    {
        studentQuery = new StudentQuery();
    }

    [HttpGet]
    [Route("/api/v1/Students")]
    [Produces("application/json")]
    public async Task<IActionResult> GetList(int tutorId, int offset, int limit) 
    {
        List<StudentDTO> students = await studentQuery.GetList(tutorId, offset, limit);
        
        var response = new
        {
            data = students,
        };
        return Ok(response); 
    }

    [HttpGet]
    [Route("/api/v1/StudentsCount")]
    [Produces("application/json")]
    public async Task<IActionResult> GetListCount(int tutorId)
    {
        var students = await studentQuery.GetListCount(tutorId);
        var response = new
        {
            length = students,
        };

        return Ok(response);
    }

    [HttpPost]
    [Route("/api/v1/Student/Create")]
    [Produces("application/json")]
    public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDTO student)
    {
        await studentQuery.CreateStudent(student);
        return Ok(new { data = new { result = true } });
    }
}