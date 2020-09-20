using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Azure.Cosmos;
using System;

namespace GottaBeGeek
{
    public class TodoManager : ITodoManager
    {
        private readonly TodoOptions options;
        private readonly CosmosClient cosmosClient;

        public TodoManager(IOptions<TodoOptions> options)
        {
            this.options = options?.Value;
            cosmosClient = new CosmosClient(this.options.TodoDbUrl, this.options.TodoDbKey);
        }

        public async Task<Guid> AddTodoAsync(Todo todo)
        {
            todo.Id = Guid.NewGuid().ToString();
            var container = cosmosClient.GetContainer(options.TodoDbId, options.TodoDbContainerId);
            var response = await container.CreateItemAsync<Todo>(todo, new PartitionKey(todo.Id)).ConfigureAwait(false);
            if (response.Value == null)
            {
                throw new Exception("Creation Failed");
            }
            return Guid.Parse(response.Value.Id);
        }

        public async Task<IEnumerable<Todo>> GetTodosAsync()
        {
            var container = cosmosClient.GetContainer(options.TodoDbId, options.TodoDbContainerId);

            var query = new QueryDefinition("SELECT * FROM c");

            var todos = new List<Todo>();
            await foreach (var todo in container.GetItemQueryIterator<Todo>(query))
            {
                todos.Add(todo);
            }
            return todos;
        }
    }
}