using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsAPI.Core.Entities;
using StudentsAPIClient.Services;

namespace StudentsAPIClient.Controllers
{
    [Route("clientapi/[controller]")]
    [ApiController]
    public class StudentsStatisticController : ControllerBase
    {
        private readonly StudentsAPIService studentsAPIservice;

        public StudentsStatisticController(StudentsAPIService studentsAPIService)
        {
            this.studentsAPIservice = studentsAPIService;
        }

        public async Task<IEnumerable<StudentStatistics>> Get()
        {
            return await studentsAPIservice.GetStatistics();
        }
    }
}