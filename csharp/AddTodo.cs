using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace GottaBeGeek.Function
{
    public class AddTodo
    {
        private readonly ITodoManager todoManager;

        public AddTodo(ITodoManager todoManager)
        {
            this.todoManager = todoManager;
        }

        [FunctionName(nameof(AddTodo))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var todoString = await new StreamReader(req.Body).ReadToEndAsync().ConfigureAwait(false);
            var todo = JsonSerializer.Deserialize<Todo>(todoString);
            var id = await todoManager.AddTodoAsync(todo).ConfigureAwait(false);
            return new CreatedResult(string.Empty, id);
        }
    }
}