using System;
namespace TodoList.Entity
{
	public class TaskToDo
	{
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public User? CreatedBy { get; set; }

    }
}

