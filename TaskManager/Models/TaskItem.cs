using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название задачи")]
        [StringLength(100, ErrorMessage ="Название не должно превышать 100 символов")]
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }

        //Привязка задачи к пользователю
        public string? UserId { get; set; }
    }
}
