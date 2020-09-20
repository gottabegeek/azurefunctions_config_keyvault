using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GottaBeGeek.Function
{
    public class GetTodos
    {
        private readonly ITodoManager todoManager;

        public GetTodos(ITodoManager todoManager)
        {
            this.todoManager = todoManager;
        }

        [FunctionName(nameof(GetTodos))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var todos = await todoManager.GetTodosAsync().ConfigureAwait(false);
            return new OkObjectResult(todos);
        }
    }
}