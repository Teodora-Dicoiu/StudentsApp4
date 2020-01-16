using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace StudentsAPI.Core.Entities
{
    public class StudentStatistics
    {
        private int lines;

        public long? Id { get; set; }
        public string FirstName { get; set; }
        public int Commits { get; set; }
        public int LinesModified { get; set; }

        public StudentStatistics(long? id, string name, int commits, int lines)
        {
            this.Id = id;
            this.FirstName = name;
            this.Commits = commits;
            this.LinesModified = lines;
        }

	}
}
