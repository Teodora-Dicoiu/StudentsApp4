using StudentsAPI.Core.Entities;
using StudentsAPI.V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentsAPIClient.Services
{
    public class StudentsAPIService
    {
        private readonly HttpClient client;
        private readonly List<StudentStatistics> studentStatistics=new List<StudentStatistics>();

        public StudentsAPIService(HttpClient client)
        {
            this.client = client;
            client.BaseAddress = new Uri("https://localhost:5001/");
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            client.DefaultRequestHeaders.Add("x-api-key", "123456");
        }

        public async Task<IEnumerable<Student>> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/students");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string message = string.Format(
                    "Error while using StudentsAPI (status code: {0}, reason: {1})",
                    response.StatusCode,
                    response.ReasonPhrase);

                throw new HttpRequestException(message);
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<Student>>(responseStream);
        }


        public async Task<IEnumerable<StudentStatistics>> GetStatistics( )
        {
            var students = new HttpRequestMessage(HttpMethod.Get, "api/students");

            var codeCommits = new HttpRequestMessage(HttpMethod.Get, "api/v2/CodeCommits");

            var responseFromStudents = await client.SendAsync(students);

            var responseFromCommits = await client.SendAsync(codeCommits);

            if (!responseFromStudents.IsSuccessStatusCode || !responseFromCommits.IsSuccessStatusCode)
            {
                string message = string.Format(
                    "Error while using StudentsAPI (status code: {0}, reason: {1})",
                    responseFromStudents.StatusCode,
                    responseFromStudents.ReasonPhrase);

                throw new HttpRequestException(message);
            }

            using var responseStreamStudents = await responseFromStudents.Content.ReadAsStreamAsync();
            var studentList = JsonSerializer.DeserializeAsync<IEnumerable<Student>>(responseStreamStudents).Result;

            using var responseStreamSCommits = await responseFromCommits.Content.ReadAsStreamAsync();
            var commitList = JsonSerializer.DeserializeAsync<IEnumerable<CodeCommit>>(responseStreamSCommits).Result;

            foreach (Student student in studentList)
            {
                int lines = 0;
                int commits = 0;
                foreach(CodeCommit commit in commitList)
                {
                    if (student.Id == commit.UserId)
                    {
                        lines = lines + (int)commit.LinesModified;
                        commits = commits + 1;
                    }
                }
                studentStatistics.Add(new StudentStatistics(student.Id, student.FirstName, commits, lines));
            }


            return studentStatistics.OrderBy(s => s.FirstName);
        }
    }
}
