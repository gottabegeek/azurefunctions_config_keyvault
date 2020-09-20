import { CosmosClient } from "@azure/cosmos";
import { Guid } from "guid-typescript";
import ITodoManager from './itodo.manager';

export default class TodoManager implements ITodoManager {
    private todoDbId: string = process.env.TodoDbId;
    private todoDbContainerId: string = process.env.TodoDbContainerId;

    public constructor(private readonly client: CosmosClient) {}

    public async addTodoAsync(todo: ITodo): Promise<string> {
        const container = this.client.database(this.todoDbId).container(this.todoDbContainerId);
        todo.id = Guid.create().toString();
        await container.items.create(todo);
        return todo.id;
    }

    public async getTodosAsync(): Promise<ITodo[]> {
        const container = this.client.database(this.todoDbId).container(this.todoDbContainerId);
        const { resources } = await container.items.query('SELECT * FROM c').fetchAll();
        return this.toTodos(resources);
    }

    private toTodos(resources: any[]): ITodo[] {
        const todos: ITodo[] = [];
        for (const item of resources) {
            todos.push({ id: item.id, value: item.value });
        }
        return todos;
    }
}