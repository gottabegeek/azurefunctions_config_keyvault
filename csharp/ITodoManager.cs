using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GottaBeGeek
{
    public interface ITodoManager
    {
        Task<IEnumerable<Todo>> GetTodosAsync();
        Task<Guid> AddTodoAsync(Todo todo);
    }
}