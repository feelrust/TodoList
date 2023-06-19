using System;
using TodoList.Entity;

namespace TodoList.Models
{
	public class TaskList
	{
        public TaskList(IEnumerable<TaskToDo> data)
        {
            this.Results = data;
        }
        public IEnumerable<TaskToDo> Results { get; set; }
	}
}

