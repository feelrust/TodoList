using System;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using TodoList.Entity;
using TodoList.Services;
using TodoList.Models;
using FluentValidation;

namespace TodoList.Endpoints
{
	public static class UserEndpoints
	{
        public static RouteGroupBuilder MapUserApiV1(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetUsers);
            group.MapPost("/", UpdateUser);
            return group;
        }

        public static async Task<Ok<IEnumerable<UserResponse>>> GetUsers(IUserService userService)
        {
            var result = await userService.GetAll();

            var response = result.Select(o => new UserResponse() { id = o.Id, Username = o.Username, Roles = o.Roles.Select(o => o.Name).ToList() });

            return TypedResults.Ok(response);
        }

        public static async Task<Results<Ok<User>, NotFound, BadRequest<Error>>> UpdateUser(int id, IValidator<User> validator, User userRequest, IUserService userService)
        {
            var validationResult = await validator.ValidateAsync(userRequest);

            if (!validationResult.IsValid)
            {
                var errors = new Error(validationResult.Errors.Select(o => o.ErrorMessage));
                return TypedResults.BadRequest(errors);
            }
            var user = await userService.Find(id);

            if (user is null) return TypedResults.NotFound();

            user.Password = userRequest.Password;

            await userService.Update(user);

            return TypedResults.Ok(user);
        }
    }
}

