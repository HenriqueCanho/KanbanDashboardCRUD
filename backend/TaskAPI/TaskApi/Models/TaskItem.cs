using System;

namespace TaskApi.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }   // id padrão  || Guid é uma biblioteca que cria o id
        public string Title { get; set; } = null!;
        public string? Description { get; set; } // '?' significa que não é necessário

        public DateTimeOffset? DueDate { get; set; } // DueDate é um tipo de formatação de data

        public int Priority { get; set; }          // 1 = baixa, 2 = média, 3 = alta
        public string Status { get; set; } = "pending"; // "pending", "completed", etc. || padrão 'pending'

        // vínculo com User
        public Guid UserId { get; set; } // chave extrangeira
        //public User User { get; set; } = null!;
    }
}