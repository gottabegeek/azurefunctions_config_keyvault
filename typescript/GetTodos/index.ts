import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import createTodoManager from '../common';
import ITodoManager from "../common/itodo.manager";

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    const todoManager: ITodoManager = await createTodoManager();
    const todos: ITodo[] = await todoManager.getTodosAsync();

    context.res = {
        // status: 200, /* Defaults to 200 */
        headers: {'Content-Type': 'application/json'},
        body: todos
    };
};

export default httpTrigger;