using System;
using System.ComponentModel.DataAnnotations.Schema;
using Auditor.Helper;

namespace Auditor.Console.Entities
{
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "int")]
        public int Age { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string Address { get; set; }
    }
}
