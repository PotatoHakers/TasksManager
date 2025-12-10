using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsDone { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
