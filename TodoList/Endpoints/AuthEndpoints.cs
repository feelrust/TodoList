using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using TodoList.Models;
using TodoList.Entity;
using TodoList.Services;

namespace TodoList.Endpoints
{
    public static class AuthEndpoints
    {
        public static RouteGroupBuilder MapAuthApiV1(this RouteGroupBuilder group)
        {
            group.MapPost("/login", Login);
            group.MapPost("/register", Register);

            return group;
        }

        public static async Task<Results<Ok<User>, BadRequest<Error>>> Register(IValidator<User> validator, User userModel, IUserService userService)
        {
            var validationResult = await validator.ValidateAsync(userModel);

            if (!validationResult.IsValid)
            {
                var errors = new Error(validationResult.Errors.Select(o => o.ErrorMessage));
                return TypedResults.BadRequest(errors);
            }

            //DANI REMEMBER: should be add password hashing logic

            await userService.Add(userModel);

            return TypedResults.Ok(userModel);

        }

        public static async Task<Results<Ok<LoginResponse>, BadRequest<Error>, NotFound<Error>>> Login(TokenService tokenService, IValidator<User> validator, User userModel, IUserService userService)
        {
            var validationResult = await validator.ValidateAsync(userModel);

            if (!validationResult.IsValid)
            {
                var errors = new Error(validationResult.Errors.Select(o => o.ErrorMessage));
                return TypedResults.BadRequest(errors);
            }

            var user = await userService.FindByUserName(userModel.Username);

            //DANI REMEMBER:should be add password hash validation logic

            if (user is null)
                return TypedResults.NotFound(new Error(new List<string> { "Invalid username or password" }));

            var token = tokenService.GenerateToken(user);

            user.Password = string.Empty;

            return TypedResults.Ok(new LoginResponse(user.Username, token));

        }

    }

}