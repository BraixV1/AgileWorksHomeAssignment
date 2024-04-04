using System;
using System.ComponentModel.DataAnnotations;
using Domain.HelperEnums;

namespace Domain
{
    public class ToDoTasks : BaseEntity
    {
        [MaxLength(256)]
        public string Description { get; set; } = default!;

        public EStatus Status { get; set; } = EStatus.NotCompleted;

        public DateTime CreatedAtDt { get; set; }

        public DateTime HasToBeDoneDt { get; set; } = DateTime.Now;
    }
}