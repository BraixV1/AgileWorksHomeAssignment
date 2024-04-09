using System;
using System.ComponentModel.DataAnnotations;
using Domain.HelperEnums;

namespace Domain
{
    public class ToDoTasks : BaseEntity
    {
        [MaxLength(256)]
        public string Description { get; set; } = default!;
    
        public DateTime CreatedAtDt { get; set; }
    
        public DateTime HasToBeDoneAtDt { get; set; }
    
        public DateTime? CompletedAtDt { get; set; }
    }
}