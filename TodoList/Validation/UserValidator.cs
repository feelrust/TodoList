using System;
using FluentValidation;
using TodoList.Entity;

namespace TodoList.Validation
{
	public class UserValidator : AbstractValidator<User>
	{
		public UserValidator()
		{
			RuleFor(x => x.Username).EmailAddress().NotEmpty();
			RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
		}
	}
}

