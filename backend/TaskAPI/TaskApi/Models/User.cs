using System;
using System.Collections.Generic;

namespace TaskApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;  
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>(); // array de dados tipo tasks vai ter um array de outra tabela a TaskItem
    } // ICollection = array
}
