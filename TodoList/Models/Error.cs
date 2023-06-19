using System;
namespace TodoList.Models
{
    public class Error
    {
        public Error(IEnumerable<string> message)
        {
            this.Errors = message;
        }
        public IEnumerable<string> Errors { get; set; }
    }
}

