using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using TodoList.Models;
using TodoList.Entity;
using TodoList.Services;
using System.Security.Claims;

namespace TodoList.Endpoints
{
    public static class TaskEndpoints
    {
        public static RouteGroupBuilder MapTaskApiV1(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetTasks);
            group.MapGet("/{id}", GetTasksById);
            group.MapPost("/", AddTask);
            group.MapPut("/{id}", UpdateTask);
            group.MapPatch("/{id}/complete", CompleteTask);
            group.MapPatch("/{id}/incomplete", IncompleteTask);
            group.MapDelete("/{id}", DeleteTask);
            return group;
        }

        public static async Task<Ok<TaskToDo>> GetTasksById(int id, ClaimsPrincipal user, ITaskService taskService)
        {
            var userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.Name)?.Value);

            var result = await taskService.Find(id, userId);

            return TypedResults.Ok(result);
        }


        public static async Task<Ok<List<TaskToDo>>> GetTasks(ClaimsPrincipal user, ITaskService taskService)
        {
            var userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.Name)?.Value);

            var result = await taskService.GetAll(userId);

            return TypedResults.Ok(result);
        }

        public static async Task<Results<Ok<TaskToDo>, BadRequest<Error>>> AddTask(IValidator<TaskToDo> validator, TaskToDo task, ClaimsPrincipal user, ITaskService taskService)
        {
            var validationResult = await validator.ValidateAsync(task);

            if (!validationResult.IsValid)
            {
                var errors = new Error(validationResult.Errors.Select(o => o.ErrorMessage));
                return TypedResults.BadRequest(errors);
            }

            var userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.Name)?.Value);

            task.UserId = userId;

            await taskService.Add(task);

            return TypedResults.Ok(task);
        }

        public static async Task<Results<Ok<TaskToDo>, NotFound, BadRequest<Error>>> UpdateTask(int id, IValidator<TaskToDo> validator, TaskToDo taskRequest, ClaimsPrincipal user, ITaskService taskService)
        {
            var validationResult = await validator.ValidateAsync(taskRequest);

            if (!validationResult.IsValid)
            {
                var errors = new Error(validationResult.Errors.Select(o => o.ErrorMessage));
                return TypedResults.BadRequest(errors);
            }

            var userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.Name)?.Value);
            var task = await taskService.Find(id, userId);

            if (task is null) return TypedResults.NotFound();

            task.Title = taskRequest.Title;
            task.Description = taskRequest.Description;

            await taskService.Update(task);

            return TypedResults.Ok(task);
        }

        public static async Task<Results<Ok<TaskToDo>, NotFound>> CompleteTask(int id, ClaimsPrincipal user, ITaskService taskService)
        {

            var userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.Name)?.Value);
            var task = await taskService.Find(id, userId);

            if (task is null) return TypedResults.NotFound();

            task.IsComplete = true;
            task.CompleteDate = DateTime.UtcNow;

            await taskService.Update(task);

            return TypedResults.Ok(task);
        }

        public static async Task<Results<Ok<TaskToDo>, NotFound>> IncompleteTask(int id, ClaimsPrincipal user, ITaskService taskService)
        {

            var userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.Name)?.Value);
            var task = await taskService.Find(id, userId);

            if (task is null) return TypedResults.NotFound();

            task.IsComplete = false;
            task.CompleteDate = null;

            await taskService.Update(task);

            return TypedResults.Ok(task);
        }

        public static async Task<Results<Ok<TaskToDo>, NotFound>> DeleteTask(int id, ClaimsPrincipal user, ITaskService taskService)
        {
            var userId = Convert.ToInt32(user?.FindFirst(ClaimTypes.Name)?.Value);
            var task = await taskService.Find(id, userId);

            if (task is null) return TypedResults.NotFound();

            await taskService.Remove(task);

            return TypedResults.Ok(task);
        }

    }
}

