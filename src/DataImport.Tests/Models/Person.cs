using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelReader.Tests.Models
{
    public class Person
    {
        public string Name { get; set; }

        public int FriendsCount { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender Sex { get; set; }

    }
}
