import { AzureFunction, Context, HttpRequest } from "@azure/functions";
import createTodoManager from '../common';
import ITodoManager from "../common/itodo.manager";

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    const todoManager: ITodoManager = await createTodoManager();
    const todoId: string = await todoManager.addTodoAsync(req.body as ITodo);

    context.res = {
        status: 201, /* Defaults to 200 */
        body: todoId
    };
};

export default httpTrigger;