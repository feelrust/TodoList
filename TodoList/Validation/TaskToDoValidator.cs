using System;
using FluentValidation;
using TodoList.Entity;

namespace TodoList.Validation
{
	public class TaskToDoValidator : AbstractValidator<TaskToDo>
    {
		public TaskToDoValidator()
		{
			RuleFor(t => t.Title).NotEmpty().MinimumLength(1).MaximumLength(15);
            RuleFor(t => t.Description).MaximumLength(25);
        }
	}
}

